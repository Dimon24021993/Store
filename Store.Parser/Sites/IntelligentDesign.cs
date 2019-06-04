using AngleSharp.Dom;
using Microsoft.EntityFrameworkCore.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Store.Domain.Entities;
using Store.Domain.Enums;
using Store.Parser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Group = Store.Domain.Entities.Group;

namespace Store.Parser.Sites
{
    public static class IntelligentDesign
    {

        public static int Items = 0;
        public static void ParseIntelligentDesign()
        {
            var path = Environment.GetEnvironmentVariable("PATH");
            var cd = Environment.CurrentDirectory;
            if (!path.Contains(cd))
                Environment.SetEnvironmentVariable("PATH", path + ";" + cd);
            var client = new HttpClient() { BaseAddress = new Uri("https://intelligent-design.ru") };

            var groups = client.GetDocument("").QuerySelectorAll(".nm-main-menu li li a");
            var groupType = GroupType.ByType;
            Parallel.ForEach(groups, new ParallelOptions() { MaxDegreeOfParallelism = 4 },
           element =>
            {
                if (string.IsNullOrWhiteSpace(element.GetAttribute("href")))
                {
                    groupType = GroupType.ByStyle;
                }

                ParseGroup(client.BaseAddress, element, groupType, groups.IndexOf(element));
            });
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

                //-------------------------------group
                DataBase.DbTasks.AddGroup(ref group);
                //-------------------------------

                #endregion

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
                        //-------------------------------image
                        DataBase.DbTasks.AddPicture(ref groupImage);
                        //-------------------------------
                    }
                }

                #endregion

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

                    //-------------------------------criterias
                    DataBase.DbTasks.AddCriterias(ref criterias2Base);
                    //-------------------------------
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
                    //-------------------------------criteriasItems
                    DataBase.DbTasks.AddCriteriasItems(ref criteriasItems2Base);
                    //-------------------------------
                }



                #endregion

                //items = chrome.FindElements(By.CssSelector(""));
                //foreach (var item in items)
                //{
                //    ParseItems(ref client, item, group);
                //}
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //ignore
            }
            finally
            {
                chrome.Quit();
                chrome.Dispose();

            }


        }

        private static void ParseItems(ref HttpClient client, object item, Group @group)
        {
            throw new NotImplementedException();
        }


        private static void ParseIntelligentDesign(ref HttpClient client, string href)
        {
            try
            {
                var document = client.GetDocument(href);
                var name = document.QuerySelector("h1").TextContent;
                var results = document
                    .QuerySelectorAll("ul.movie_credentials li")
                    .ToDictionary(elem => elem.FirstElementChild.TextContent);


            }
            catch (Exception e)
            {
                // ignored
            }
        }


    }
}