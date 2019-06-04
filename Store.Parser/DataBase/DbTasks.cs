using Store.DAL;
using Store.Domain.Entities;
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
                                                     x.GroupId == picture.GroupId
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
                    var criteriaBase =
                        Context.Criterias.FirstOrDefault(x => x.GroupId == criteria.GroupId && x.Name == criteria.Name);
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
                    var criteriaBase =
                        Context.CriteriaItems.FirstOrDefault(x => x.CriteriaId == criteria.CriteriaId &&
                                                                  x.Value == criteria.Value);
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
    }
}