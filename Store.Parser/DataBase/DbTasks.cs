using Store.DAL;
using Store.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Store.Parser.DataBase
{
    public static class DbTasks
    {
        public static DataBaseContext Context { get; set; }

        public static void AddGroup(ref Group group)
        {
            if (group == null) return;
            var name = group.Name;
            lock (Context)
            {
                var groupBase = Context.Groups.FirstOrDefault(x => x.Name == name);
                if (groupBase == null)
                {
                    Context.Groups.Add(group);
                    Context.SaveChanges();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(groupBase.Description) && !string.IsNullOrWhiteSpace(group.Description))
                        groupBase.Description = group.Description;
                    if (string.IsNullOrWhiteSpace(groupBase.ShortDescription) && !string.IsNullOrWhiteSpace(group.ShortDescription))
                        groupBase.ShortDescription = group.ShortDescription;

                    group = groupBase;
                    Context.Groups.Update(groupBase);
                    Context.SaveChanges();
                }
            }
        }

        public static void AddPicture(ref Picture groupImage)
        {
            if (groupImage == null) return;

            var picture = groupImage;

            lock (Context)
            {
                var pictureBase = Context.Pictures
                                 .FirstOrDefault(x =>
                                                     (x.GroupId == picture.GroupId || x.ItemId == picture.ItemId || x.ArticleId == picture.ArticleId)
                                                  && x.Type == picture.Type
                                                  && x.Name == picture.Name
                                                  && x.Href == picture.Href);
                if (pictureBase == null)
                {
                    Context.Pictures.Add(groupImage);
                    Context.SaveChanges();
                }
                else
                {
                    groupImage = pictureBase;
                }
            }
        }

        public static void AddCriterias(ref List<Criteria> criterias2Base)
        {
            if (criterias2Base == null || !criterias2Base.Any()) return;


            lock (Context)
            {
                foreach (var criteria in criterias2Base)
                {
                    var criteriaBase = Context.Criterias.FirstOrDefault(x => x.GroupId == criteria.GroupId
                                                                          && x.Name == criteria.Name);
                    if (criteriaBase == null)
                    {
                        Context.Criterias.Add(criteria);
                        Context.SaveChanges();
                    }
                    else
                    {
                        criteria.Id = criteriaBase.Id;
                    }
                }
            }
        }

        public static void AddCriteriasItems(ref List<CriteriaItem> criterias2Base)
        {
            if (criterias2Base == null || !criterias2Base.Any()) return;


            lock (Context)
            {
                foreach (var criteria in criterias2Base)
                {
                    var criteriaBase = Context.CriteriaItems.FirstOrDefault(x => x.CriteriaId == criteria.CriteriaId
                                                                              && x.Value == criteria.Value
                                                                              && x.ItemId == criteria.ItemId);
                    if (criteriaBase == null)
                    {
                        Context.CriteriaItems.Add(criteria);
                        Context.SaveChanges();
                    }
                    else
                    {
                        criteria.Id = criteriaBase.Id;
                    }
                }
            }
        }

        public static void AddItem(ref Item item)
        {
            if (item == null) return;
            var itemNo = item.ItemNo;
            lock (Context)
            {
                var itemBase = Context.Items.FirstOrDefault(x => x.ItemNo == itemNo
                                                                 //&& x.GroupId == groupId
                                                                 );
                if (itemBase == null)
                {
                    Context.Items.Add(item);
                    Context.SaveChanges();
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(itemBase.Description) && !string.IsNullOrWhiteSpace(item.Description))
                        itemBase.Description = item.Description;
                    if (string.IsNullOrWhiteSpace(itemBase.ShortDescription) && !string.IsNullOrWhiteSpace(item.ShortDescription))
                        itemBase.ShortDescription = item.ShortDescription;
                    if (itemBase.Cost == 0 && item.Cost > 0)
                        itemBase.Cost = item.Cost;

                    item = itemBase;
                    Context.Items.Update(itemBase);
                    Context.SaveChanges();
                }
            }
        }

        public static void AddItemGroup(ref ItemGroup item)
        {
            if (item == null) return;
            var itemloc = item;
            lock (Context)
            {
                var itemBase = Context.ItemGroups.FirstOrDefault(x => x.ItemId == itemloc.ItemId
                                                                      && x.GroupId == itemloc.GroupId);
                if (itemBase != null) return;
                Context.ItemGroups.Add(item);
                Context.SaveChanges();
            }

        }

        public static void AddItemGroupByGroupName(ref ItemGroup item)
        {
            if (item == null) return;
            var itemloc = item;
            lock (Context)
            {
                var groupBase = Context.Groups.FirstOrDefault(x => x.Name == itemloc.Group.Name);
                if (groupBase == null)
                {
                    groupBase = new Group() { Id = Guid.NewGuid(), Name = itemloc.Group.Name };
                    Context.Groups.Add(groupBase);
                }
                var itemBase = Context.ItemGroups.FirstOrDefault(x => x.ItemId == itemloc.ItemId
                                                                   && x.GroupId == groupBase.Id);
                if (itemBase != null) return;
                Context.ItemGroups.Add(item);
                Context.SaveChanges();
            }
        }
    }
}