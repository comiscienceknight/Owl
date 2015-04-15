using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Owl.Models
{
    class AzureStorage
    {
        public AzureStorage()
        {
        }

        public async Task UploadProfile(string fileName, StorageFile file)
        {
            var credentials = new StorageCredentials("owlbat", "cKblxr8aqs36pM0LpLsT9vAxdMn+V2ShD/EPkD55xQeNyXG2LFGDZgRNi1OL8tOIeeQblxQUDo+K1RT/aMkZ6Q==");
            var client = new CloudBlobClient(new Uri("http://owlbat.blob.core.windows.net/"), credentials);
            var container = client.GetContainerReference("profile");
            var blockBlob = container.GetBlockBlobReference(fileName);
            await blockBlob.UploadFromFileAsync(file);
            var getblob = blockBlob.Uri;
        }
    }
}
