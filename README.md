# SetBlobCacheHeader
Azure CDN impoves performance by serving your static content from the edge rather than the origin. However, a low maxed-aged will cause a revalidation with the origin. You will see that in your Azure CDN usage pattern
https://docs.microsoft.com/en-us/azure/cdn/cdn-analyze-usage-patterns
This code sets the Azure blob cache header. The TTL is determined by the Cache-Control header in the HTTP response from Azure Storage. Setting high number can improve performance of CDN
To use this code:
1- Create a new C# Console Application 
2- Install the Windows Azure Storage NuGet package into your project.https://www.nuget.org/packages/WindowsAzure.Storage/
3. Add your storage account name and primary key to the code
4. Run the code.
