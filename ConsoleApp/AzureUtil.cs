using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Security.Permissions;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

//http://gauravmantri.com/
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace ConsoleApp
{

    public static class AzureStorage
    {

        private static CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
        //private static CloudStorageAccount csa = CloudStorageAccount.Parse("GHGConnectionString");
        public static CloudPageBlob Patientblob, PopulationBlob, Imageblob, Loaderblob, Epidemblob;

        //public static string Container { get; set; }

        public static void SetupGhGdevstorage(string qnnee)
        {
            var account = csa;
            var container = account.CreateCloudBlobClient().GetContainerReference(qnnee);
             Patientblob = container.GetPageBlobReference("p");
            if (!Patientblob.Exists())
            {
                Patientblob.Create(0x40000000); //for development need to increase for prod        
                Patientblob.Metadata["nextindex"] = "0x00";
                Patientblob.Metadata["nextpage"] = "0x00";
                Patientblob.Metadata.Add("startoffset", "0x800000"); //constant
                Patientblob.Properties.ContentEncoding = "application/octet-stream";
                Patientblob.SetMetadata();
                Patientblob.SetProperties();
            }
             PopulationBlob = container.GetPageBlobReference("m");
            if (!PopulationBlob.Exists())
            {
                PopulationBlob.Create(0x40000000); //for development need to increase
                PopulationBlob.Metadata["nextindex"] = "0x00";
                PopulationBlob.Metadata["nextpage"] = "0x00";
                PopulationBlob.Metadata.Add("startoffset", "0x800000"); //constant
                PopulationBlob.Properties.ContentEncoding = "application/octet-stream";
                PopulationBlob.SetMetadata();
                PopulationBlob.SetProperties();
            }
             Imageblob = container.GetPageBlobReference("i");
             if (!Imageblob.Exists())
            {
                Imageblob.Create(0x40000000); //for development need to increase
                Imageblob.Metadata["nextindex"] = "0x00";
                Imageblob.Properties.ContentEncoding = "application/octet-stream";
                Imageblob.SetMetadata();
                Imageblob.SetProperties();
            }
             Loaderblob = container.GetPageBlobReference("l");
            if (!Loaderblob.Exists())
            {
                Loaderblob.Create(0x400000); //need to increase in production 2^32 4 gigs = 1 million * 4 pages
                Loaderblob.FetchAttributes();
                Loaderblob.Metadata.Add("nextindex", "0x00000");
                Loaderblob.Properties.ContentEncoding = "application/octet-stream";
                Loaderblob.SetMetadata();
                Loaderblob.SetProperties();
            }
               Epidemblob = container.GetPageBlobReference("e");
            if (!Epidemblob.Exists())
            {
                Epidemblob.Create(0x40000000); //for development need to increase this to fill whole blob
                Epidemblob.FetchAttributes();
                Epidemblob.Metadata.Add("nextindex", "0x00");
                Epidemblob.Metadata["startoffset"] = "0x24000"; //constant 100 years =36500 days = 0x120 pages 128 per page.
                Epidemblob.Properties.ContentEncoding = "application/octet-stream";
                Epidemblob.SetMetadata();
                Epidemblob.SetProperties();
            }
        }


/*
        public static void SetupPatientBlob(string qnnee)
        {
            //fixed length records - index/size = 8 bytes in index table
            //concurrent write access to blob
            //CloudStorageAccount.Parse();
            //Settings.Registered = true;
            var account = csa;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            var patient = cont.GetPageBlobReference("p");
            patient.Create(0x40000000); //for development need to increase this to fill whole blob 2^40
            //0 to 2^32-1 for PHM at base and the rest 2^32 to 2^40-1 to patient records
            //patient.Metadata.Add("nextindex", "0");
            patient.Metadata["nextindex"] = "0x00";
            patient.Metadata["nextpage"] = "0x00";
            patient.Metadata.Add("startoffset", "0x800000"); //constant
            patient.Properties.ContentEncoding = "application/octet-stream";
            patient.SetMetadata();
            patient.SetProperties();


        }



        public static void SetupPopulationManagementBlob(string qnnee)
        {

            var account = csa;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            var phm = cont.GetPageBlobReference("m");
            phm.Create(0x40000000); //for development need to increase this to fill whole blob 2^40
            //0 to 2^32-1 for PHM at base and the rest 2^32 to 2^40-1 to patient records
            //patient.Metadata.Add("nextindex", "0");
            phm.Metadata["nextindex"] = "0x00";
            phm.Metadata["nextpage"] = "0x00";
            phm.Metadata.Add("startoffset", "0x800000"); //constant
            phm.Properties.ContentEncoding = "application/octet-stream";
            phm.SetMetadata();
            phm.SetProperties();
        }


        public static void SetupImageBlob(string qnnee)
        {
            //variable length records - index/size = 8 bytes in index table
            //concurrent write access to blob - store nextindex matadata
            //and setup overlapping pages for write access
            //CloudStorageAccount.Parse();
            //Settings.Registered = true;
            var account = csa;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            var imageblob = cont.GetPageBlobReference("i");
            if (imageblob.Exists()) return;
            imageblob.Create(0x40000000); //for development need to increase this to fill whole blob 2^40
            //image.Metadata.Add("nextindex", "0");
            //"blobindex" = "0;
            imageblob.Metadata["nextindex"] = "0x00";
            //image.Metadata["nextpage"] = "0x00";
            //image.Metadata.Add("startoffset", "0x800000"); //constant 0x4000000 for azure
            imageblob.Properties.ContentEncoding = "application/octet-stream";
            imageblob.SetMetadata();
            imageblob.SetProperties();

            var epidemblob = cont.GetPageBlobReference("e");
            if (epidemblob.Exists()) return;
            epidemblob.Create(0x40000000);
            // epidem.Create(0xffff); //for development need to increase this to fill whole blob
            // epidem.FetchAttributes();
            //epidem.Metadata.Add("nextindex", "0");
            epidemblob.Metadata["nextindex"] = "0x00";
            //epidem.Metadata["nextpage"] = "0x120";
            epidemblob.Metadata["startoffset"] = "0x24000"; //constant 100 years =36500 days = 0x120 pages 128 per page.
            epidemblob.Properties.ContentEncoding = "application/octet-stream";
        }


        public static void SetupEpidemiologyBlob(string qnnee)
        {
            //no write concurrency - only one writer
            //variable length records - index/size = 8 bytes in index table
            //CloudStorageAccount.Parse();
            //Settings.Registered = true;
            var account = csa;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            cont.CreateIfNotExists();

            var epidemblob = cont.GetPageBlobReference("e");
            if (epidemblob.Exists()) return;
            epidemblob.Create(0x40000000);
            // epidem.Create(0xffff); //for development need to increase this to fill whole blob
            // epidem.FetchAttributes();
            //epidem.Metadata.Add("nextindex", "0");
            epidemblob.Metadata["nextindex"] = "0x00";
            //epidem.Metadata["nextpage"] = "0x120";
            epidemblob.Metadata["startoffset"] = "0x24000"; //constant 100 years =36500 days = 0x120 pages 128 per page.
            epidemblob.Properties.ContentEncoding = "application/octet-stream";
            // epidem.SetMetadata();
            // epidem.SetProperties();
        }

        public static void SetupLoaderBlob(string qnnee)
        {
            // fixed length records
            //concurrent write access to blob
            var account = csa; //CloudStorageAccount.DevelopmentStorageAccount;
            // var en = account.CreateCloudBlobClient().ListContainers();
            var cont = account.CreateCloudBlobClient().GetContainerReference(qnnee); //"2aabb"
            var loaderblob = cont.GetPageBlobReference("l");
            if (loaderblob.Exists()) return;
            loaderblob.Create(0x400000); //need to increase in production 2^32 4 gigs = 1 million * 4 pages
            loaderblob.FetchAttributes();
            loaderblob.Metadata.Add("nextindex", "0x00000");
            //loader.Metadata["nextpage"] = "0x00";
            // loader.Metadata.Add("dataoffset", "5120"); /\no index - fixed length record +-2 kilobytes for prefs
            loaderblob.Properties.ContentEncoding = "application/octet-stream";
            loaderblob.SetMetadata();
            loaderblob.SetProperties();
        }


*/

        public static int RegisterNewLoader(string[] ldr) //registers a loader and returns the loader id
        {
            //fixed length records concurrent write access
            //store nextindex metadata and overlapping pages for         
            //write access
            if (ldr == null) throw new ArgumentNullException("ldr");

            //fetchattributes
            //set nextindex metadata to allow concurrent access for other users to update their data
            //then update this users data

            var json = JsonConvert.SerializeObject(ldr);
            var account = csa; //CloudStorageAccount.DevelopmentStorageAccount;          
            var cont = account.CreateCloudBlobClient().GetContainerReference(ldr[0]); //"2aabb"
            var loaderblob = cont.GetPageBlobReference("l");

            var bytes = Encoding.UTF8.GetBytes(json);
            var grow = 2048 - (bytes.Length % 2048);
            Array.Resize(ref bytes, bytes.Length + grow);
            var ms = new MemoryStream(bytes);
            loaderblob.FetchAttributes();
            var ndx = Convert.ToInt16(loaderblob.Metadata["nextindex"], 16);
            var ofs = ndx * 2048; // loader uses 4 pages = 2048 bytes
            loaderblob.WritePages(ms, ofs);
            loaderblob.Metadata["nextindex"] = String.Format("{0:x4}", ndx + 1);
            loaderblob.SetMetadata();
            return ndx;
        }


        public static string GetLoader(string region, int index)
        {
            //retieves readonly loader details from region and loader id
            if (region == null) throw new ArgumentNullException("region");
            var account = csa;
            index = index << 11; //2048 bytes
            var cont = account.CreateCloudBlobClient().GetContainerReference(region);
            var loader = cont.GetPageBlobReference("l");
            var stm = loader.OpenRead();
            var buffer = new byte[2048];
            stm.Seek(index, SeekOrigin.Begin);
            stm.Read(buffer, 0, 2048);
            return Encoding.UTF8.GetString(buffer).Trim('\0'); //returns json string        
        }

        public static bool LoaderExists(string region, string loadername)
        {
            if (region == null) throw new ArgumentNullException("region");
            var account = csa;
            var cont = account.CreateCloudBlobClient().GetContainerReference(region);
            var loader = cont.GetPageBlobReference("l");
            if (loader == null) return false;

            loader.FetchAttributes();
            var ndx = Convert.ToInt16(loader.Metadata["nextindex"], 16);
            var bytes = new byte[4096];
            loader.DownloadToByteArray(bytes, 0);
            var bstring = bytes.ToString(); //can shorten string - the name is always at the start  
            return bstring.Contains(loadername);
        }

    }


    public class Test
    {
        public static void Main()
        {

            AzureStorage.SetupLoaderBlob("21f29");
            const string str = "21f29,Tony Manicom,173 Blandford Rd, North Riding,etc";

            var sarr = str.Split(',');
            //if (AzureStorage.LoaderExists("21f29", "Tony Manicom")) return;
            var s = AzureStorage.RegisterNewLoader(sarr);
            var ret = AzureStorage.GetLoader("21f29", s);
            var x = AzureStorage.LoaderExists("21f29", "Tony Manicom");
            Console.WriteLine(ret);
            Console.WriteLine(ret.Length);
            Console.ReadLine();

            // AzureStorage.SetupEpidemiologyBlob("2aabb");
        }
    }
}



//   public staticvoid Main()
//{
//}
/*
                
            // var s = qnnee.Name;
          
            const string s1 = "Tony Manicom\n173 blandford road\n north riding, Randburg\n";
            var sb = Encoding.UTF8.GetBytes(s1);

            // byte[] ba = new byte[512];
            var grow = 512 - (sb.Length) % 512;
            Array.Resize(ref sb, sb.Length + grow);
            //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
            //ba.
            // byte[] x = new byte[512];
            // for (int i = 0; i < 512; i++) x[i] = (byte)i;
         //   loader.UploadFromByteArray(sb, 0, 512);
        }

       // var readerblob = qnnee.GetPageBlobReference("l");
      // var stream = readerblob.OpenRead();
       // var buffer = new byte[512];
       // stream.Seek(0, System.IO.SeekOrigin.Begin);
      //  stream.Read(buffer, 0, 512);
      //  var s = Encoding.UTF8.GetString(buffer).Trim('\0');
       // var v = "";
        //Model.AzureStorage.devListContainers();     
    }


   // public static IEnumerable<CloudBlobContainer> DevelopmentContainers()
   // {
       // var dev = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageConnectionString"));
       // var blobClient = dev.CreateCloudBlobClient();
       // return blobClient.ListContainers();
        //ToArray<
        //CloudBlobContainer qnnee = blobClient.GetContainerReference("2aabb");
        // IEnumerable<CloudBlobContainer> cbc = blobClient.ListContainers();
        //string s = qnnee.Name;        
        //return s; 
   // }


    //   static UInt32 LoaderCount(string qnnee) //devstor for dev
    //   { 
    // CloudStorageAccount.
    //  }
    



//CloudStorageAccount storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
//    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient(); 
//    CloudBlobContainer qnnee = cloudBlobClient.GetContainerReference(containerName); 
//    string filePath = "<Full File Path e.g. C:\temp\myblob.txt>"; 
//    string blobName = "<Blob Name e.g. myblob.txt>"; 
//   CloudBlockBlob blob = qnnee.GetBlockBlobReference(blobName); 

    

    /// <summary>
    /// Gets all the name names for the GHG management account
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<IListBlobItem> DownloadCountryNames()
    {
        var storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("GHGConnectionString"));
        var blobClient = storageAccount.CreateCloudBlobClient();
        var qnnee = blobClient.GetContainerReference("countries");
        return qnnee.ListBlobs().AsEnumerable();

    }


    /// <summary>
    /// Downloads Regions from the GHG management account
    /// </summary>
    /// <param name="name">eg "za.txt"</param>,
    /// <returns>CSV string</returns>
  //  public static string DownloadNames(string name)
   // {
        name = name.ToLower() + ".txt";
        var storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("GHGConnectionString"));
        var blobClient = storageAccount.CreateCloudBlobClient();
        var qnnee = blobClient.GetContainerReference("countries");
        var blockBlob = qnnee.GetBlockBlobReference(name);
        string text;
        using (var memoryStream = new MemoryStream())
        {
            blockBlob.DownloadToStream(memoryStream);
            text = Encoding.UTF8.GetString(memoryStream.ToArray());
        }
        return text;
    }

    public static string[] CountryNames()
    {
        var storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("GHGConnectionString"));
        var blobClient = storageAccount.CreateCloudBlobClient();
        var qnnee = blobClient.GetContainerReference("countries");
        return qnnee.GetBlockBlobReference("countries.txt").DownloadText().ToUpper().Split(',');
    }

    

   
 */
//}



