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
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using OwlBatAzureMobileService.Models;

namespace OwlBatAzureMobileService.Controllers
{
    [AuthorizeLevel(AuthorizationLevel.User)]
    public class CustomController : ApiController
    {
        public ApiServices Services { get; set; }

        [Route("get/getposts/{date}/{time}")]
        [HttpGet]
        public List<GetPostsResult> GetAllVailablePosts(string date = "2015-03-10", string time = "010101")
        {
            string updateAs = date + "T" + time.Insert(2, ":").Insert(5, ":");
            DateTimeOffset updateAsDto;
            if (DateTimeOffset.TryParse(updateAs, out updateAsDto))
            {
                using (var db = new Models.OwlDataClassesDataContext())
                {
                    return db.GetPosts(updateAsDto).ToList();
                }
            }
            return new List<GetPostsResult>();
        }

        [Route("get/getpost/{userid}")]
        [HttpGet]
        public GetPostResult GetUserPost(string userid)
        {
            using (var db = new Models.OwlDataClassesDataContext())
            {
                var results = db.GetPost(userid).ToList();
                if (results.Count > 0)
                    return results.Last();
            }
            return new GetPostResult();
        }

        [Route("get/ifuserexist/{userId}")]
        [HttpGet]
        public bool IsUserExist(string userId)
        {
            using (var db = new Models.OwlDataClassesDataContext())
            {
                return db.UserAndPosts.Any(p => p.UserId == userId);
            }
        }

        [Route("get/getuser/{userid}")]
        [HttpGet]
        public User GetUser(string userId)
        {
            using (var db = new Models.OwlDataClassesDataContext())
            {
                var results = db.Users.Where(p => p.UserId == userId).ToList();
                if (results.Count > 0)
                    return results.Last();
                else
                    return new User()
                    {
                        UserId = userId
                    };
            }
        }


        [Route("get/getvenuesbi/{filterName}")]
        [HttpGet]
        public List<Models.Venue> GetVenuesByName(string filterName)
        {
            using (var db = new Models.OwlDataClassesDataContext())
            {
                var result = db.Venues.Where(p => p.SearchName.Contains(filterName)).ToList();
                return result;
            }
        }

        [Route("post/createuser")]
        [HttpPost]
        public async Task<string> CreateUser()
        {
            try
            {
                HttpRequestMessage request = this.Request;
                string querystring = await request.Content.ReadAsStringAsync();
                NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);

                using (var db = new Models.OwlDataClassesDataContext())
                {
                    db.InsertAnUser(qscoll["userid"], qscoll["username"], qscoll["sexe"], qscoll["birthday"]);
                }
            }
            catch (SqlException exp)
            {
                return exp.Message;
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
            return "";
        }

        [Route("post/createpost")]
        [HttpPost]
        public async Task<string> CreateOrUpdatePost()
        {
            try
            {
                HttpRequestMessage request = this.Request;
                string querystring = await request.Content.ReadAsStringAsync();
                NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);

                using (var db = new Models.OwlDataClassesDataContext())
                {
                    if(db.Posts.Any(p=>p.UserId == qscoll["userid"]))
                    {
                        db.UpdateAnPost(qscoll["userid"], qscoll["outype"], qscoll["venueid"], qscoll["lookingfor"], qscoll["arrivaltime"],
                           Convert.ToInt32(qscoll["girlnumber"]), Convert.ToInt32(qscoll["boynumber"]), qscoll["profileurl"], qscoll["otherinfo"], qscoll["codedress"]);
                    }
                    else
                    {
                        db.InsertAnPost(qscoll["userid"], qscoll["outype"], qscoll["venueid"], qscoll["lookingfor"], qscoll["arrivaltime"], 
                            Convert.ToInt32(qscoll["girlnumber"]), Convert.ToInt32(qscoll["boynumber"]), qscoll["profileurl"], qscoll["otherinfo"], qscoll["codedress"]);
                    }
                }
            }
            catch (SqlException exp)
            {
                return exp.Message;
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
            return "";
        }

        [Route("post/addpost")]
        [HttpPost]
        public async Task<string> CreateUserAndPost()
        {
            try
            {
                HttpRequestMessage request = this.Request;
                string querystring = await request.Content.ReadAsStringAsync();
                NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);

                using (var db = new Models.OwlDataClassesDataContext())
                {
                    string description = "";
                    if (qscoll["description"] != null)
                        description = qscoll["description"];
                    if (description.Length > 249)
                        description = description.Substring(0, 249);
                    description.Replace("'", "''");
                    string username = (qscoll["username"] ?? "").Replace("'", "''");
                    string command =
                    "INSERT INTO [dbo].[UserAndPost]([UserId],[UserName] ,[UserProfileUrl1] ,[VenueId] ,[VenueName] ,[Time] ,[Sexe] ,[GirlNumber],[BoyNumber],[AgeRange],[Description])" +
                    "VALUES ('" + qscoll["userid"] + "' ,'" + username + "' ,'" + qscoll["profileurl"] + "' ,'" + qscoll["venueId"] + "' ,'" + qscoll["venuename"] + "' ,'" + qscoll["time"] + "' ,'" + qscoll["Sexe"] + "' ," + qscoll["girlsnumber"] + " ," + qscoll["guysnumber"] + ",'" + qscoll["agerange"] + "','" + description + "')";

                    db.ExecuteCommand(command);
                }
            }
            catch(SqlException exp)
            {
                return exp.Message;
            }
            catch(Exception exp)
            {
                return exp.Message;
            }

            return "";
        }




        [Route("post/updatepost")]
        [HttpPost]
        public async Task<string> UpdateUserAndPost()
        {
            try
            {
                HttpRequestMessage request = this.Request;
                string querystring = await request.Content.ReadAsStringAsync();
                NameValueCollection qscoll = HttpUtility.ParseQueryString(querystring);

                using (var db = new Models.OwlDataClassesDataContext())
                {
                    string description = "";
                    if (qscoll["description"] != null)
                        description = qscoll["description"];
                    if (description.Length > 150)
                        description = description.Substring(0, 150);
                    description.Replace("'", "''");
                    StringBuilder commandSb = 
                        new StringBuilder("UPDATE [dbo].[UserAndPost] SET");
                    commandSb.Append(" [UserName] = '" + qscoll["username"] + "'");
                    if (!string.IsNullOrWhiteSpace(qscoll["profileurl"]))
                        commandSb.Append(",[UserProfileUrl1] = '" + qscoll["profileurl"] + "'");
                    if (!string.IsNullOrWhiteSpace(qscoll["profileurl2"]))
                        commandSb.Append(",[UserProfileUrl2] = '" + qscoll["profileurl2"] + "'");
                    if (!string.IsNullOrWhiteSpace(qscoll["profileurl3"]))
                        commandSb.Append(",[UserProfileUrl3] = '" + qscoll["profileurl3"] + "'");
                    commandSb.Append(",[VenueId] = '" + qscoll["venueid"] + "'");
                    commandSb.Append(",[VenueName] = '" + qscoll["venuename"] + "'");
                    commandSb.Append(",[Time] = '" + qscoll["arrivaltime"] + "'");
                    commandSb.Append(",[Sexe] = '" + qscoll["Sexe"] + "'");
                    commandSb.Append(",[GirlNumber] = " + qscoll["girlsnumber"]);
                    commandSb.Append(",[BoyNumber] = " + qscoll["guysnumber"]);
                    commandSb.Append(",[AgeRange] = '" + qscoll["agerange"] + "'");
                    commandSb.Append(",[Description] = '" + qscoll["description"] + "'");
                    commandSb.Append(",[CodeDress] = '" + qscoll["codedress"] + "'");
                    commandSb.Append(" WHERE [UserId]= '" + qscoll["userid"] + "'");

                    db.ExecuteCommand(commandSb.ToString());
                }
            }
            catch (SqlException exp)
            {
                return exp.Message;
            }
            catch (Exception exp)
            {
                return exp.Message;
            }

            return "";
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
                    string file = provider.FileData.First().LocalFileName;
                    UploadImageToStorageBlob(file).Wait();
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
