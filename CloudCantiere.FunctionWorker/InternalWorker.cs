using CloudCantiere.DataAccess.Interventions;
using CloudCantiere.DataAccess.PhotosCantiere;
using CloudCantiere.DataAccess.PhotosIntervention;
using CloudCantiere.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace CloudCantiere.FunctionWorker
{
    public static class InternalWorker
    {
        [FunctionName("InternalWorker")]
        public static void Run([ServiceBusTrigger("internal", Connection = "ServiceBusCS")]string request, TraceWriter log)
        {
            string cs = Environment.GetEnvironmentVariable("SqlServerCS");
            var message = JsonConvert.DeserializeObject<JObject>(request);
            var requestType = message.Value<string>("RequestType");
            switch (requestType)
            {
                case "InsertPhotoCantiereRequest":
                    {
                        IPhotoCantiereRepository photoCantiereRepository = new PhotoCantiereRepository(cs);
                        var photoData = new PhotoCantiere()
                        {
                            IdCantiere = message.Value<int>("IdCantiere"),
                            URI = message.Value<string>("URI")
                        };
                        var insertedId = photoCantiereRepository.Insert(photoData);
                        break;
                    }
                case "InsertPhotoInterventionRequest":
                    {
                        IPhotoInterventionRepository photoInterventionRepository = new PhotoInterventionRepository(cs);
                        var photoData = new PhotoIntervention()
                        {
                            IdIntervention = message.Value<int>("IdIntervention"),
                            URI = message.Value<string>("URI")
                        };
                        var insertedId = photoInterventionRepository.Insert(photoData);
                        break;
                    }
                case "InterventionRequest":
                    {
                        IInterventionRepository interventionRepository = new InterventionRepository(cs);
                        var intervention = new Intervention()
                        {
                            IdType = message.Value<int>("IdType"),
                            IdCantiere = message.Value<int>("IdCantiere"),
                            Notes = message.Value<string>("Notes"),
                            Price = message.Value<int>("Price")
                        };
                        var insertedId = interventionRepository.Insert(intervention);
                        break;
                    }
                default:
                    break;
            }
            log.Info($"C# ServiceBus queue trigger function processed message: {request}");
        }
    }
}
