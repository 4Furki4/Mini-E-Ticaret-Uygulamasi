
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence
{
    static class Configuration
    {
        static public string ConnectionString
        {
            get
            {
                ConfigurationManager manager = new();
                manager.SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../../Presentation/ETicaretAPI.API"));
                manager.AddJsonFile("appsettings.json");
                manager.AddUserSecrets("40ab5350-6e49-4df7-ba1e-5612d95dd697");
                var conntectionString = manager.GetConnectionString("MySql");
                return manager.GetConnectionString("MySql");
            }
        }
    }
}
