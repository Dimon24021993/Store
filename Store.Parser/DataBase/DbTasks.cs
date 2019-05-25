using Store.DAL;
using Store.Domain.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Store.Parser.DataBase
{
    public static class DbTasks
    {
        public static DataBaseContext Context { get; set; }



        public static void AddPictures(IEnumerable<Group> groups)
        {
            if (groups == null || !groups.Any()) return;

            lock (Context)
            {
                foreach (var group in groups)
                {
                    var basePicture = Context.Groups.FirstOrDefault(p => p.Name == group.Name);
                    if (basePicture != null) continue;
                    Context.Groups.Add(group);
                    Context.SaveChanges();
                }
            }
        }


    }
}