using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using System.Threading.Tasks;
using OwlBatAzureMobileService.DataObjects;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Microsoft.WindowsAzure.Mobile.Service.Security;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class CustomController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/Custom
        public async Task<Place> Get()
        {
            using(var context = new Models.MobileServiceContext())
            {
                var dataBase = context.Database;
                var result = await dataBase.
                    SqlQuery<Place>("exec OwlBatAzureMobileService.GetCustomData @placeName = 'sample string 1'").
                    FirstOrDefaultAsync();
                return result;
            }
            //Services.Log.Info("Hello from custom controller!");
        }

        // Get GetCustom
        [Route("GetCustom")]
        [HttpGet]
        public string GetCustomMethod()
        {
            return "You got a custom method";
        }

        [Route("UploadPicture")]
        [HttpPost]
        public Task<HttpResponseMessage> PostFile()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            string root = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/uploads");
            var provider = new MultipartFormDataStreamProvider(root);

            var task = request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(o => 
                {
                    string file1 = provider.FileData.First().LocalFileName;
                    UploadImageToStorageBlob(file1).Wait();
                    // this is the file name on the server where the file was saved 
                    return new HttpResponseMessage()
                    {
                        Content = new StringContent("File uploaded.")
                    };
                }
            );

            return task;
        }

        private static async Task UploadImageToStorageBlob(string imageToUpload)
        {
            CloudStorageAccount storageAccount = CreateStorageAccountFromConnectionString(CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create a blob client for interacting with the blob service.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Create a container for organizing blobs within the storage account.
            CloudBlobContainer container = blobClient.GetContainerReference("profile");
            try
            {
                await container.CreateIfNotExistsAsync();
            }
            catch (StorageException)
            {
                throw;
            }

            // To view the uploaded blob in a browser, you have two options. The first option is to use a Shared Access Signature (SAS) token to delegate 
            // access to the resource. See the documentation links at the top for more information on SAS. The second approach is to set permissions 
            // to allow public access to blobs in this container. Uncomment the line below to use this approach. Then you can view the image 
            // using: https://[InsertYourStorageAccountNameHere].blob.core.windows.net/democontainer/HelloWorld.png
            // await container.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // Upload a BlockBlob to the newly created container
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageToUpload);
            await blockBlob.UploadFromFileAsync(imageToUpload, FileMode.Open);

            File.Delete(imageToUpload);
        }

        private static CloudStorageAccount CreateStorageAccountFromConnectionString(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Invalid storage account information provided. Please confirm the AccountName and AccountKey are valid in the app.config file - then restart the sample.");
                Console.ReadLine();
                throw;
            }

            return storageAccount;
        }
    }
}
