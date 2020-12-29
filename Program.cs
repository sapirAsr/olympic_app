/**using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace olympic_app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
*/

using System;
using System.Collections.Generic;
using olympic_app.DB;

namespace olympic_app
{
    class Program
    {  
        static void Main(string[] args)
        {
            DBConnect dBConnect = new DBConnect();
            dBConnect.OpenConnection();
            string result =dBConnect.TheMostXAthlete("Basketball", "Height", "DESC");
            Console.WriteLine(result);
/**
            List< string >[] list = dBConnect.Select(); 
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j< 5; j++)
                {
                     Console.WriteLine(list[i][j]);        
                }          
            }
            // from the users filters we represent a query
            string table = "olympicapp.athletes";
            List<string> atributes = new List<string>();
            atributes.Add("Athlete_Id");
            atributes.Add("Name");
            atributes.Add("Team");
            List< string >[] filter_list = dBConnect.BasicFilter(table, atributes); 
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j< 5; j++)
                {
                     Console.WriteLine(filter_list[i][j]);        
                }   
            }
            string sport = "Basketball";
            string name = dBConnect.TheBestAthlete(sport);
            Console.WriteLine("The Best Athlete in the field of " + sport + " is: " +name);
            */
        }
        
    }
}
