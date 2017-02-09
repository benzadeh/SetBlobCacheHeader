using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace AzureCDN
{
    class Program
    {
        static void Main()
        {
            var accountName = "cloudprograming";
            var accountKey = "primary-key";

            //set CacheControl to one hour expiration
            var cacheControl = "public, max-age=3600";

            try
            {
                Console.WriteLine($"Connecting to Azure Storage account {accountName} and creating client.");
                var account = CloudStorageAccount.Parse($"DefaultEndpointsProtocol=https;AccountName={accountName};AccountKey={accountKey}");
                var client = account.CreateCloudBlobClient();

                Console.WriteLine($"Iterating through all containers in account {accountName}");
                foreach (var container in client.ListContainers(null, ContainerListingDetails.All))
                {
                    Console.WriteLine($"Now inspecting container {container.Name}");
                    var permissions = container.GetPermissions();
                    if (permissions.PublicAccess == BlobContainerPublicAccessType.Blob || permissions.PublicAccess == BlobContainerPublicAccessType.Container)
                    {
                        Console.WriteLine($"Container {container.Name} is a public container or has public blobs, proceeding.");
                        //we want to get a flat listing of blobs, since we're going to iterate them all
                        foreach (var blobItem in container.ListBlobs(null, true))
                        {
                            //we only want to cache-control block blobs
                            if (blobItem is CloudBlockBlob)
                            {
                                //cast IListBlobItem to CloudBlockBlob
                                var blob = (CloudBlockBlob)blobItem;
                                if (blob.Exists())
                                {
                                    Console.WriteLine($"Blob {blob.Name} is a CloudBlockBlob and it exists. Setting CacheControl to {cacheControl}");
                                    blob.Properties.CacheControl = cacheControl;
                                    blob.SetProperties();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error encountered: {ex.Message}");
            }

            Console.Read();
        }
    }
}
