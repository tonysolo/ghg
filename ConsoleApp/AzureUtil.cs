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

        // public static CloudStorageAccount Csa = CloudStorageAccount.DevelopmentStorageAccount;
        public static CloudStorageAccount Csa = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("GHGConnectionString"));

        public static CloudPageBlob Patientblob, Populationblob, Imageblob, Loaderblob, Epidemblob;

        //public static string Container { get; set; }



        public static void SetupGhGstorage(CloudStorageAccount account, string qnnee)
        {
            var container = account.CreateCloudBlobClient().GetContainerReference(qnnee);
            container.CreateIfNotExists();
            Patientblob = container.GetPageBlobReference("p");
            if (!Patientblob.Exists())
            {
                Patientblob.Create(0xa000); //for development need to increase to terabyte for prod        
                Patientblob.FetchAttributes();
                Patientblob.Metadata["nextindex"] = "0x00";
                Patientblob.Properties.ContentEncoding = "application/octet-stream";
                Patientblob.SetMetadata();
                Patientblob.SetProperties();
            }

            Populationblob = container.GetPageBlobReference("m");
            if (!Populationblob.Exists())
            {
                Populationblob.Create(0xa000); //for development need to increase to 0x800000 X number of patient blobs
                Populationblob.FetchAttributes();
                Populationblob.Metadata["nextindex"] = "0x00";
                Populationblob.Properties.ContentEncoding = "application/octet-stream";
                Populationblob.SetMetadata();
                Populationblob.SetProperties();
            }

            Imageblob = container.GetPageBlobReference("i");
            if (!Imageblob.Exists())
            {
                Imageblob.Create(0xa000); //for development need to increase to terabyte
                Imageblob.FetchAttributes();
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
            Loaderblob.FetchAttributes();
            var etag = Loaderblob.Properties.ETag;
            var p = Convert.ToInt32(Loaderblob.Metadata["nextindex"], 16);
            p += 1;
            ndx = String.Format("{0:x6}", p);
            Loaderblob.Metadata["nextindex"] = ndx;
            try
            {
                Loaderblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) ndx = "";
            }
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
        Populationblob.FetchAttributes();
        var etag = Populationblob.Properties.ETag;
        var p = Convert.ToInt32(Populationblob.Metadata["nextindex"], 16);
        p += 1;
        ndx = String.Format("{0:x6}", p);
        Populationblob.Metadata["nextindex"] = ndx;
        try
        {
            Populationblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
        }
        catch (StorageException ex)
        {
            if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) ndx = "";
        }
        return ndx;
        }

        //set epidemiology - this will go into ghg account - each qne container keeps a single e blob
        //metadata could be kept in container - concurrency not an issue because there is only 1 writer and scheduled
        public static string SetNextEpidemPage()
        {
            var ndx = "";
            Epidemblob.FetchAttributes();
            var etag = Epidemblob.Properties.ETag;
            var p = Convert.ToInt32(Epidemblob.Metadata["nextindex"], 16);
            p += 1;
            ndx = String.Format("{0:x6}", p);
            Epidemblob.Metadata["nextindex"] = ndx;
            try
            {
                Epidemblob.SetMetadata(accessCondition: AccessCondition.GenerateIfMatchCondition(etag));
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode == (int)HttpStatusCode.PreconditionFailed) ndx = "";
            }
            return ndx;  
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
            var grow = (512 - bytes.Length % 512);
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
            

           AzureStorage.SetupGhGstorage(AzureStorage.Csa, "21f29");

            for(int i=0;i<50;i++)
            Console.WriteLine(AzureStorage.SetNextLoaderIndex());

            Console.ReadLine();

            
            //const string str = "21f29,Tony Manicom,173 Blandford Rd, North Riding,etc";

            //var sarr = str.Split(',');

            //var json = Newtonsoft.Json.JsonConvert.SerializeObject(sarr);

            //var s = AzureStorage.RegisterNewLoader(json);
            //var t = AzureStorage.RegisterNewLoader(json);
            /*
            Console.WriteLine(QneUtils.Timezone("1aa0a"));
            Console.WriteLine(QneUtils.Timezone("1aa1a"));
            Console.WriteLine(QneUtils.Timezone("1aa20"));
            Console.WriteLine(QneUtils.Timezone("1aa31"));
             */ 

        }
    }
}






