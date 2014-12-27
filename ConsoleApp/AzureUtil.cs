using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Runtime.ExceptionServices;
using System.Runtime.Remoting.Messaging;
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

        //  private static CloudStorageAccount csa = CloudStorageAccount.DevelopmentStorageAccount;
        //  private static CloudStorageAccount csa = CloudStorageAccount.Parse("GHGConnectionString");

        public static CloudPageBlob Patientblob, Populationblob, Imageblob, Loaderblob, Epidemblob;

        //public static string Container { get; set; }



        public static void SetupGhGstorage(CloudStorageAccount account, string qnnee)
        {
            var container = account.CreateCloudBlobClient().GetContainerReference(qnnee);
            Patientblob = container.GetPageBlobReference("p");
            if (!Patientblob.Exists())
            {
                Patientblob.Create(0xa000); //for development need to increase to terabyte for prod        
                Patientblob.Metadata["nextindex"] = "0x00";
               // Patientblob.Metadata["nextpage"] = "0x00";
               // Patientblob.Metadata.Add("startoffset", "0x800000"); 0x00 - data starts at 0
                Patientblob.Properties.ContentEncoding = "application/octet-stream";
                Patientblob.SetMetadata();
                Patientblob.SetProperties();
            }
            Populationblob = container.GetPageBlobReference("m");
            if (!Populationblob.Exists())
            {
                Populationblob.Create(0xa000); //for development need to increase to 0x800000 X number of patient blobs
                Populationblob.Metadata["nextindex"] = "0x00";
                //Populationblob.Metadata["nextpage"] = "0x00";
               // Populationblob.Metadata.Add("startoffset", "0x800000"); //data starts at 0
                Populationblob.Properties.ContentEncoding = "application/octet-stream";
                Populationblob.SetMetadata();
                Populationblob.SetProperties();
            }
            Imageblob = container.GetPageBlobReference("i");
            if (!Imageblob.Exists())
            {
                Imageblob.Create(0xa000); //for development need to increase to terabyte
                Imageblob.Metadata["nextindex"] = "0x00"; //data starts at 0
                Imageblob.Properties.ContentEncoding = "application/octet-stream";
                Imageblob.SetMetadata();
                Imageblob.SetProperties();
            }
            Loaderblob = container.GetPageBlobReference("l");
            if (Loaderblob.Exists()) return;
            Loaderblob.Create(0xa000); //need to increase in production 2^32 4 gigs = 1 million * 4 pages
            Loaderblob.FetchAttributes();
            Loaderblob.Metadata.Add("nextindex", "0x00000");
            Loaderblob.Properties.ContentEncoding = "application/octet-stream";
            Loaderblob.SetMetadata();
            Loaderblob.SetProperties();
        }
/*
 * epidemiology needs users to upload to a storage queue for processing at midnight for the timezone.
 * It is shared data so its value depends on combining.
 * I might try to only update those users that provide epidemiology data.
 * a worker role does the processing and the user has readonly access to the data for his region
 * this will be updated daily and displayed when the app starts
 
            public static void SetupEpidemStorage (CloudStorageAccount ghgAccount, string qnnee)
            {
                //use global ghg account for epidemiology (global)
                var cname = qnnee.Substring(0, 4).Remove(2, 1);
                var container = ghgAccount.CreateCloudBlobClient().GetContainerReference(cname);
                container.CreateIfNotExists();
                Epidemblob = container.GetPageBlobReference("e"); //production epidem will go in ghg/epidem/qne
                if (Epidemblob.Exists()) return;
                Epidemblob.Create(0xa000); //for development need to increase this to fill whole blob
                Epidemblob.FetchAttributes();
                Epidemblob.Metadata.Add("nextindex", "0x00");
                Epidemblob.Metadata["startoffset"] = "0x120";
                //constant 100 years =36500 days = 0x120 pages 128 per page.
                //make epoch 1/1/2015
                Epidemblob.Properties.ContentEncoding = "application/octet-stream";
                Epidemblob.SetMetadata();
                Epidemblob.SetProperties();
            }
 */

        public static void SaveEpidemiology(CloudStorageAccount ghgAccount, string qnnee)
        {
            //var x = Convert.ToInt32((qnnee.Substring(0,1)+qnnee.Substring(2, 1)),16);
            //timezone(qnnee)
            //loadtoqueuetoqueue(qne)
            //setsecstomidnight
            //data offloaded / saved to ghg/qne/e (global biome blobs) 256 x 2 x (45) x 0.25
        }

//its only the writing metadata that needs concurrency protection
//all other reads and writes involve overlapping pages which allows concurrent read / write

/// <summary>
/// sets loader index string , loader record is fixed length 2048 bytes
/// </summary>
/// <returns>string</returns>
        public static string SetNextLoaderIndex()
        {
            var ndx = "";
   // var load = "";
           
               // load = Loaderblob.AcquireLease(TimeSpan.FromSeconds(15),null);
                Loaderblob.FetchAttributes();
                var etag = Loaderblob.Properties.ETag;
                var p = Convert.ToInt32(Loaderblob.Metadata["nextindex"], 16);
                p += 1;
                ndx = String.Format("{0:x5}", p);
                Loaderblob.Metadata["nextindex"] = ndx;
    try
    {
        Loaderblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
    }
    catch(StorageException ex)
    {
        if (ex.RequestInformation.HttpStatusCode == (int) HttpStatusCode.PreconditionFailed) ndx = "error";
    }
   // finally { Loaderblob.ReleaseLease(AccessCondition.GenerateLeaseCondition(load)); }

    return ndx;
        }

//population management blob and patient blob share the same index but much longer record in patient blob and 
//this will have higfher traffic, thererfore the smaller population blob is used for metadata and index allocation
//and patients blob left free for storage 
//patient is fixed length record 131072 bytes (128K) (very adequate) but in addition the system provides for storing scans and images
//for extra storage.
        public static string SetNextPatientIndex()
        {
            var ndx = "";
            try
            {
                Populationblob.AcquireLease(TimeSpan.FromSeconds(15), "load");
                Populationblob.FetchAttributes();
                var p = Convert.ToInt32(Populationblob.Metadata["nextindex"], 16);
                p += 1;
                ndx = String.Format("{0:x6}", p); //maximum ndx is 0x800000
                Populationblob.Metadata["nextindex"] = ndx;
                Populationblob.SetMetadata();
            }
            finally
            {
                Populationblob.ReleaseLease(AccessCondition.GenerateLeaseCondition("load"));
            }
            return ndx;
        }

        //set epidemiology - this will go into ghg account - each qne container keeps a single e blob
        //metadata could be kept in container - concurrency not an issue because there is only 1 writer and scheduled
        public static string SetNextEpidemPage()
        {
            var pg = "";
            try
            {
                Epidemblob.AcquireLease(TimeSpan.FromSeconds(15), "load");//?omit
                Epidemblob.FetchAttributes();
                var p = Convert.ToInt32(Epidemblob.Metadata["nextpage"], 16);
                p += 1;
                pg = String.Format("{0:x8}", p); //maximum page is 0x80000000
                Epidemblob.Metadata["nextpage"] = pg;
                Epidemblob.SetMetadata();
            }
            finally
            {
                Epidemblob.ReleaseLease(AccessCondition.GenerateLeaseCondition("load"));//?
            }
            return pg;
        }

        //set population management index / next patient index
        //search / list loaders
        //search / list patients
        //search epidemiolgy date range
//save image and return the image location to patient blob
        //save epidemiology data and record the location in epidemiology date list.



/// <summary>
/// registers a new loader
/// </summary>
/// <param name="json"></param>
/// <returns>string index</returns>
        public static string RegisterNewLoader(string json)
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            var grow = (512 - bytes.Length%512);
            Array.Resize(ref bytes, bytes.Length + grow);
            var s = SetNextLoaderIndex();
            if (s == "") return s;
            var p = Convert.ToInt32(s, 16);
            var ms = new MemoryStream(bytes);
            var start = p << 10;
            Loaderblob.WritePages(ms, start);
            return s;
        }
}


        public class Test
        {
            public static void Main()
            {

                //AzureStorage.SetupGhGstorage(CloudStorageAccount.DevelopmentStorageAccount, "21f29");
               
                //const string str = "21f29,Tony Manicom,173 Blandford Rd, North Riding,etc";

                //var sarr = str.Split(',');

                //var json = Newtonsoft.Json.JsonConvert.SerializeObject(sarr);

                //var s = AzureStorage.RegisterNewLoader(json);
                 //var t = AzureStorage.RegisterNewLoader(json);
                Console.WriteLine(QneUtils.Timezone("1aa0a"));
                Console.WriteLine(QneUtils.Timezone("1aa1a"));
                Console.WriteLine(QneUtils.Timezone("1aa20"));
                Console.WriteLine(QneUtils.Timezone("1aa31"));
                Console.WriteLine(QneUtils.Timezone("2aa42"));
                Console.WriteLine(QneUtils.Timezone("2aa56"));
                Console.WriteLine(QneUtils.Timezone("0aa66"));
                Console.WriteLine(QneUtils.Timezone("0aa76"));
                Console.WriteLine(QneUtils.Timezone("0aa86"));
                Console.WriteLine(QneUtils.Timezone("0aa96"));
                Console.WriteLine(QneUtils.Timezone("0aaa6"));
                Console.WriteLine(QneUtils.Timezone("0aab6"));
                Console.WriteLine(QneUtils.Timezone("0aac6"));
                Console.WriteLine(QneUtils.Timezone("0aad6"));
                Console.WriteLine(QneUtils.Timezone("0aae6"));
                Console.WriteLine(QneUtils.Timezone("0aaf6"));

                Console.WriteLine();
                Console.WriteLine(QneUtils.Timezone("0aab0ab"));
                Console.WriteLine(QneUtils.Timezone("0aab1ab"));
                Console.WriteLine(QneUtils.Timezone("0aab20b"));
                Console.WriteLine(QneUtils.Timezone("0aab31b"));
                Console.WriteLine(QneUtils.Timezone("0aab42b"));
                Console.WriteLine(QneUtils.Timezone("0aab56b"));
                Console.WriteLine(QneUtils.Timezone("0aab66b"));
                Console.WriteLine(QneUtils.Timezone("0aab76b"));
                Console.WriteLine(QneUtils.Timezone("0aab86b"));
                Console.WriteLine(QneUtils.Timezone("0aab96b"));
                Console.WriteLine(QneUtils.Timezone("0aaba6b"));
                Console.WriteLine(QneUtils.Timezone("0aabb6b"));
                Console.WriteLine(QneUtils.Timezone("0aabc6b"));
                Console.WriteLine(QneUtils.Timezone("0aabd6b"));
                Console.WriteLine(QneUtils.Timezone("0aabe6b"));
                Console.WriteLine(QneUtils.Timezone("0aabf6b"));
            




                Console.ReadLine();


            
           
          }

          
        }
    }






