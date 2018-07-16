using CloudCantiere.DataAccess.Interventions;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CloudCantiere.ExternalWorker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(
                "appsettings.json",
                optional: true,
                reloadOnChange: true);
            var configuration = builder.Build();

            string sqlCS = configuration["ConnectionStrings:Sql"];

            IInterventionRepository interventionRepository = new InterventionRepository(sqlCS);
            var interventions = interventionRepository.Get();
        }

    }
}
