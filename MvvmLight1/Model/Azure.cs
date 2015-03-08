using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.Model
{
   public static class Azure
   {
       public static CloudStorageAccount GhgAccount =
                   CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));


       public static string[] CountryNames()
       {       
               var cbc = GhgAccount.CreateCloudBlobClient();
               var container = cbc.GetContainerReference("countries");
               var blobs = container.ListBlobs().ToArray();
               var sarr = new string[blobs.Length];
               for (var i = 0; i < sarr.Length; i++)
                   sarr[i] = blobs[i].Uri.Segments[2].Substring(0, 2).ToUpper();
               return sarr;        
       }


       public static string[] GetRegions(string countryname)
       {
           if (countryname == null) return null;
           var cbc = GhgAccount.CreateCloudBlobClient();
           var container = cbc.GetContainerReference("countries");
           //if (Selectedcountryindex < 0) return null;
          // var sb = new StringBuilder(CountryNames[Selectedcountryindex]);
          // sb.Append(".txt");
           var countryblobname = countryname+".txt";
           var blob = container.GetBlockBlobReference(countryblobname.ToLower());

           string[] sarr = null;
           if (blob == null) return null;
           var ms = new MemoryStream();
           blob.DownloadToStream(ms);
           var s = ms.GetBuffer();
           var str = Encoding.UTF8.GetString(s);
           str = str.Trim('\0');
           sarr = str.Split(',');
           for (var i = 0; i < sarr.Length; i++)
           {
               var carr = sarr[i].ToCharArray();
               carr = Array.FindAll<char>(carr, (c => (char.IsLetterOrDigit(c))));
               sarr[i] = new string(carr);
           }
           return sarr;

       }
 
     }
  }
