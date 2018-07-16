using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CloudCantiere.PreventiviInCantiere.Models;
using CloudCantiere.Models;
using CloudCantiere.PreventiviInCantiere.Models.HomeViewModels;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Azure.ServiceBus;
using CloudCantiere.Models.Requests;
using CloudCantiere.DataAccess.Cantieri;
using CloudCantiere.DataAccess.Interventions;

namespace CloudCantiere.PreventiviInCantiere.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICantiereRepository _cantiereRepository;
        private readonly IInterventionRepository _interventionRepository;
        public HomeController(
            IConfiguration configuration,
            ICantiereRepository cantiereRepository,
            IInterventionRepository interventionRepository)
        {
            _configuration = configuration;
            _cantiereRepository = cantiereRepository;
            _interventionRepository = interventionRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] CantiereViewModel cantiere)
        {
            if (ModelState.IsValid)
            {
                var newCantiere = new Cantiere()
                {
                    Customer = cantiere.Customer,
                    Email = cantiere.Email,
                    Location = cantiere.Location
                };
                var idCantiere = _cantiereRepository.Insert(newCantiere);

                var storageAccount = CloudStorageAccount.Parse(_configuration["ConnectionStrings:StorageAccount"]);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var cantiereContainer = blobClient.GetContainerReference("cantiere");
                await cantiereContainer.CreateIfNotExistsAsync();
                foreach (var cantierePic in cantiere.Photos)
                {
                    if (cantierePic.Length > 0)
                    {
                        var id = Guid.NewGuid();
                        var fileExtension = Path.GetExtension(cantierePic.FileName);
                        var blobName = $"{idCantiere}/{id}{fileExtension}";
                        var blobRef = cantiereContainer.GetBlockBlobReference(blobName);
                        using (var stream = cantierePic.OpenReadStream())
                        {
                            await blobRef.UploadFromStreamAsync(stream);
                        }
                        string sas = blobRef.GetSharedAccessSignature(
                            new SharedAccessBlobPolicy()
                            {
                                Permissions = SharedAccessBlobPermissions.Read
                            });
                        var blobUri = $"{blobRef.Uri.AbsoluteUri}{sas}";

                        var message = new InsertPhotoCantiereRequest()
                        {
                            IdCantiere = idCantiere,
                            URI = blobUri
                        };
                        var queueCS = _configuration["ConnectionStrings:ServiceBusInternal"];
                        var queueName = _configuration["Resources:InternalQueue"];
                        await SendCommand(message, queueCS, queueName);
                    }
                }
                return RedirectToAction("Intervention", idCantiere);
            }
            return View();
        }

        public IActionResult Intervention(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Intervention(InterventionViewModel intervention)
        {
            if (ModelState.IsValid)
            {
                string queueCS, queueName;

                var newIntervention = new Intervention()
                {
                    IdType = intervention.IdType,
                    IdCantiere = intervention.IdCantiere,
                    Notes = intervention.Notes,
                    Price = (int)(intervention.Price*100)
                };
                var idIntervention = _interventionRepository.Insert(newIntervention);

                var storageAccount = CloudStorageAccount.Parse(_configuration["ConnectionStrings:StorageAccount"]);
                var blobClient = storageAccount.CreateCloudBlobClient();
                var interventionContainer = blobClient.GetContainerReference("intervention");
                await interventionContainer.CreateIfNotExistsAsync();
                foreach (var interventionPic in intervention.Photos)
                {
                    if (interventionPic.Length > 0)
                    {
                        var id = Guid.NewGuid();
                        var fileExtension = Path.GetExtension(interventionPic.FileName);
                        var blobName = $"{intervention.IdCantiere}/{id}{fileExtension}";
                        var blobRef = interventionContainer.GetBlockBlobReference(blobName);
                        using (var stream = interventionPic.OpenReadStream())
                        {
                            await blobRef.UploadFromStreamAsync(stream);
                        }
                        string sas = blobRef.GetSharedAccessSignature(
                            new SharedAccessBlobPolicy()
                            {
                                Permissions = SharedAccessBlobPermissions.Read
                            });
                        var blobUri = $"{blobRef.Uri.AbsoluteUri}{sas}";

                        var message = new InsertPhotoInterventionRequest()
                        {
                            IdIntervention = idIntervention,
                            URI = blobUri
                        };
                        queueCS = _configuration["ConnectionStrings:ServiceBusInternal"];
                        queueName = _configuration["Resources:InternalQueue"];
                        await SendCommand(message, queueCS, queueName);
                    }
                }

                var cantiere = _cantiereRepository.Get(intervention.IdCantiere);
                var newInterventionRequest = new InterventionRequest()
                {
                    Customer = cantiere.Customer,
                    Email = cantiere.Email,
                    Location = cantiere.Location,
                    Notes = intervention.Notes
                };
                queueCS = _configuration["ConnectionStrings:ServiceBusInternal"];
                queueName = _configuration["Resources:InternalQueue"];
                await SendCommand(newInterventionRequest, queueCS, queueName);
            }
            return RedirectToAction("Intervention", intervention.IdCantiere);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task SendCommand<TCommand>(TCommand command, string cs, string queueName)
        {
            IQueueClient queueClient = new QueueClient(cs, queueName);
            var messageBody = JsonConvert.SerializeObject(command);
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));
            await queueClient.SendAsync(message);
            await queueClient.CloseAsync();
        }
    }
}
