using AngleSharp.Html.Parser;
using Store.DAL;
using Store.Parser.DataBase;
using Store.Parser.Sites;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Store.Parser
{
    internal static class Program
    {
        public static HtmlParser Parser { get; set; } = new HtmlParser();

        private static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();

            DbTasks.Context = new DataBaseContext(new DbContextOptionsBuilder<DataBaseContext>().UseSqlServer(configuration.GetConnectionString("DataBase")).Options);
            IntelligentDesign.ParseIntelligentDesign();

            Console.ReadKey();
        }
    }
}
