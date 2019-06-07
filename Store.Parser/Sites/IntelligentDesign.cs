using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Parser.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Group = Store.Domain.Entities.Group;

namespace Store.Parser.Sites
{
    public static class IntelligentDesign
    {

        public static int Items = 0;

        private static List<(Group group, string itemHref)> items { get; set; } = new List<(Group group, string itemHref)>();

        public static void ParseIntelligentDesign()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            var cd = Environment.CurrentDirectory;
            if (!path.Contains(cd))
                Environment.SetEnvironmentVariable("PATH", path + ";" + cd);
            var client = new HttpClient() { BaseAddress = new Uri("https://intelligent-design.ru") };

            var groups = client.GetDocument("").QuerySelectorAll(".nm-main-menu li li a");
            var groupType = GroupType.ByType;
            var options = new ParallelOptions() { MaxDegreeOfParallelism = 8 };
            var groupsParallel = Parallel.ForEach(groups, options, element =>
            {
                if (string.IsNullOrWhiteSpace(element.GetAttribute("href"))) groupType = GroupType.ByStyle;
                ParseGroup(client.BaseAddress, element, groupType, groups.IndexOf(element));
            });

            while (!groupsParallel.IsCompleted)
            {
                Thread.Sleep(1000);
            }

            //items = JsonConvert.DeserializeObject("");
            var itemsParallel = Parallel.ForEach(items, options, (item, state, index) => ParseItems(client.BaseAddress, item.group, item.itemHref, index));
            try
            {
                var a = Newtonsoft.Json.JsonConvert.SerializeObject(items, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
                File.AppendAllText("items.json", a);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            while (!itemsParallel.IsCompleted)
            {
                Thread.Sleep(1000);
            }

            Console.WriteLine("All done");


            //foreach (var element in groups)
            //{
            //    if (string.IsNullOrWhiteSpace(element.GetAttribute("href")))
            //    {
            //        groupType = GroupType.ByStyle;
            //    }

            //    ParseGroup(client.BaseAddress, element, groupType, groups.IndexOf(element));
            //}

            Console.WriteLine($"Added {Items} items");
            Console.WriteLine("Done Parse IntelligentDesign!");
        }

        private static void ParseGroup(Uri baseAddress, IElement element, GroupType groupType, int sort)
        {
            var href = element.GetAttribute("href");
            if (string.IsNullOrWhiteSpace(href)) return;

            href = href.StartsWith("http") ? href : baseAddress + href;

            var client = new HttpClient() { BaseAddress = baseAddress };
            var chrome = new ChromeDriver();
            var document = client.GetDocument(href);
            //ReadOnlyCollection<IWebElement> items;

            try
            {
                chrome.Url = href;

                #region Group

                var group = new Group()
                {
                    Name = element.TextContent,
                    Active = false,
                    GroupType = groupType,
                    Id = Guid.NewGuid(),
                    ParentId = null,
                    Sort = sort,
                    Href = href,
                    Description = document.QuerySelector(".nm-shop-taxonomy-header-inner p")?.TextContent

                };

                #endregion
                //-------------------------------group
                DataBase.DbTasks.AddGroup(ref group);
                //-------------------------------

                #region GroupPicture

                var a = chrome.FindElement(By.CssSelector(".nm-shop-taxonomy-header-inner"));
                if (a != null)
                {
                    var groupImageHref = Regex.Replace(a.GetCssValue("background-image"), @"[ \w]+\((.+)\)", "$1").Trim('"');
                    if (groupImageHref.Contains('.'))
                    {
                        var groupImage = new Picture()
                        {
                            Name = groupImageHref.Substring(groupImageHref.LastIndexOf('/')),
                            GroupId = group.Id,
                            Id = Guid.NewGuid(),
                            Href = groupImageHref.StartsWith("http") ? groupImageHref : baseAddress + groupImageHref,
                            Sort = 1,
                            SourceType = SourceType.Parser,
                            Type = PictureType.BackGround
                        };
                        #endregion
                        //-------------------------------image
                        //   DataBase.DbTasks.AddPicture(ref groupImage);
                        //-------------------------------
                    }
                }


                #region Criterias

                var criterias2Base = new List<Criteria>();
                var criteriasItems2Base = new List<CriteriaItem>();
                var criteriasGroups = document.QuerySelectorAll(".nm-shop-sidebar .widget_layered_nav.woocommerce-widget-layered-nav");
                //var criteriasGroups = chrome.FindElements(By.CssSelector(".nm-shop-sidebar .widget_layered_nav.woocommerce-widget-layered-nav"));
                if (criteriasGroups != null && criteriasGroups.Any())
                {
                    criterias2Base.AddRange(criteriasGroups.Select((x, i) => new Criteria()
                    {
                        GroupId = group.Id,
                        //Name = x.Text,
                        Name = x.QuerySelector("h3")?.TextContent,
                        Id = Guid.NewGuid(),
                        Sort = i + 1
                    }));

                    #endregion

                    //-------------------------------criterias
                    // DataBase.DbTasks.AddCriterias(ref criterias2Base);
                    //-------------------------------

                    #region CriteriaItem
                    for (var i = 0; i < criteriasGroups.Length; i++)
                    {
                        var criterias = criteriasGroups[i].QuerySelectorAll("a");
                        if (criterias != null && criterias.Any())
                            criteriasItems2Base.AddRange(criterias.Select((x, index) => new CriteriaItem()
                            {
                                CriteriaId = criterias2Base[i].Id,
                                Id = Guid.NewGuid(),
                                Value = x.TextContent
                            }));
                    }

                    #endregion
                    //-------------------------------criteriasItems
                    //  DataBase.DbTasks.AddCriteriasItems(ref criteriasItems2Base);
                    //-------------------------------
                }

                var doc = Program.Parser.ParseDocument(chrome.PageSource);

                var trycount = 0;
                while (doc.QuerySelector(".all-products-loaded .nm-infload-to-top") == null)
                {
                    var load = doc.QuerySelector(".nm-infload-btn");
                    if (load != null)
                    {
                        Thread.Sleep(2000);
                        try
                        {
                            chrome.FindElement(By.CssSelector(".nm-infload-btn")).Click();
                        }
                        catch (Exception e)
                        {
                            if (trycount > 5)
                            {
                                doc = Program.Parser.ParseDocument(chrome.PageSource);
                                break;
                            }
                            trycount++;
                            Thread.Sleep(2000);
                            continue;
                        }
                        Thread.Sleep(2000);
                        doc = Program.Parser.ParseDocument(chrome.PageSource);
                    }
                    else break;
                }

                var itemHrefs = chrome.FindElements(By.CssSelector(".product h3 a")).Select(x => x.GetAttribute("href")).ToList();
                if (itemHrefs.Any())
                    items.AddRange(itemHrefs.Select(x => (group, x)));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                //ignore
            }
            finally
            {
                chrome.Quit();
                chrome.Dispose();
            }
        }

        private static void ParseItems(Uri baseAddress, Group @group, string itemHref, long index)
        {
            if (string.IsNullOrWhiteSpace(itemHref)) return;

            itemHref = itemHref.StartsWith("http") ? itemHref : baseAddress + itemHref;

            var client = new HttpClient() { BaseAddress = baseAddress };
            var document = client.GetDocument(itemHref);

            try
            {
                //chrome.Url = itemHref;

                #region Item

                var summ = document.QuerySelector(".summary .woocommerce-Price-amount.amount")?.TextContent ?? "0";
                var cost = Convert.ToDecimal(Regex.Replace(summ, @"[\D ]+", ""));

                var item = new Item()
                {
                    ItemNo = document.QuerySelector("#nm-product-meta span.sku")?.TextContent ?? "",
                    Name = document.QuerySelector("h1")?.TextContent ?? "",
                    Description = document.QuerySelector(".nm-tabs-panel-inner.entry-content")?.TextContent ?? "",
                    ShortDescription = document.QuerySelector(".woocommerce-product-details__short-description.entry-content")?.TextContent ?? "",
                    Id = Guid.NewGuid(),
                    Active = false,
                    Cost = cost,
                    Sort = Convert.ToInt32(index)
                };

                #endregion
                //-------------------------------Item
                DataBase.DbTasks.AddItem(ref item);
                //-------------------------------

                #region ItemGroups

                var ig = new ItemGroup()
                {
                    Id = Guid.NewGuid(),
                    GroupId = group.Id,
                    ItemId = item.Id
                };

                #endregion
                //-------------------------------Item
                DataBase.DbTasks.AddItemGroup(ref ig);
                //-------------------------------

                #region ItemPictures

                var pictures = document.QuerySelectorAll(".woocommerce-product-gallery__image a").Select((x, i) =>
                {
                    var href = x.GetAttribute("href");
                    return new Picture()
                    {
                        Href = href,
                        Name = href.Split('/').Last(),
                        ItemId = item.Id,
                        Id = Guid.NewGuid(),
                        Sort = i,
                        SourceType = SourceType.Parser,
                        Type = PictureType.Preview

                    };
                }).ToArray();

                for (var i = 0; i < pictures.Count(); i++)
                {

                    #endregion
                    //-------------------------------image
                    DataBase.DbTasks.AddPicture(ref pictures[i]);
                    //-------------------------------
                }

                #region Criterias

                var criteriasWithItems = document.QuerySelectorAll(".nm-tabs-panel-inner .nm-additional-information-inner tr");
                var criterias2Base = criteriasWithItems.Select((x, i) => new Criteria()
                {
                    Id = Guid.NewGuid(),
                    Name = x.Children[0].TextContent,
                    Sort = i
                }).ToList();

                #endregion
                //-------------------------------criterias
                DataBase.DbTasks.AddCriterias(ref criterias2Base);
                //-------------------------------

                #region CriteriaItem

                var criteriasItems2Base = criteriasWithItems.Select((x, i) => new CriteriaItem()
                {
                    Id = Guid.NewGuid(),
                    Value = x.Children[1].TextContent,
                    CriteriaId = criterias2Base[i].Id,
                    ItemId = item.Id
                }).ToList();



                #endregion
                //-------------------------------criteriasItems
                DataBase.DbTasks.AddCriteriasItems(ref criteriasItems2Base);
                //-------------------------------
                #region ItemGroups

                var categoryWithItems = document.QuerySelector(".posted_in").TextContent;

                var igs = categoryWithItems.Split(':').Last().Split(',').Select(x => new ItemGroup()
                {
                    Id = Guid.NewGuid(),
                    ItemId = item.Id,
                    Group = new Group()
                    {
                        Name = x.Trim()
                    }
                }).ToArray();

                for (int i = 0; i < igs.Length; i++)
                {
                    #endregion
                    //-------------------------------ItemGroups
                    DataBase.DbTasks.AddItemGroupByGroupName(ref igs[i]);
                    //-------------------------------
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}