using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;
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

        public static CloudPageBlob Patientblob, PopulationBlob, Imageblob, Loaderblob, Epidemblob;

        //public static string Container { get; set; }



        public static void SetupGhGstorage(CloudStorageAccount account, string qnnee)
        {
            var container = account.CreateCloudBlobClient().GetContainerReference(qnnee);
            Patientblob = container.GetPageBlobReference("p");
            if (!Patientblob.Exists())
            {
                Patientblob.Create(0xa000); //for development need to increase terabyte for prod        
                Patientblob.Metadata["nextindex"] = "0x00";
               // Patientblob.Metadata["nextpage"] = "0x00";
               // Patientblob.Metadata.Add("startoffset", "0x800000"); //constant
                Patientblob.Properties.ContentEncoding = "application/octet-stream";
                Patientblob.SetMetadata();
                Patientblob.SetProperties();
            }
            PopulationBlob = container.GetPageBlobReference("m");
            if (!PopulationBlob.Exists())
            {
                PopulationBlob.Create(0xa000); //for development need to increase
               // PopulationBlob.Metadata["nextindex"] = "0x00";
                PopulationBlob.Metadata["nextpage"] = "0x00";
               // PopulationBlob.Metadata.Add("startoffset", "0x800000"); //constant
                PopulationBlob.Properties.ContentEncoding = "application/octet-stream";
                PopulationBlob.SetMetadata();
                PopulationBlob.SetProperties();
            }
            Imageblob = container.GetPageBlobReference("i");
            if (!Imageblob.Exists())
            {
                Imageblob.Create(0xa000); //for development need to increase
                Imageblob.Metadata["nextindex"] = "0x00";
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
                //Epidemblob.Metadata["startoffset"] = "0x24000";
                    //constant 100 years =36500 days = 0x120 pages 128 per page.
                Epidemblob.Properties.ContentEncoding = "application/octet-stream";
                Epidemblob.SetMetadata();
                Epidemblob.SetProperties();
            }



        public static bool RegisterNewLoader(string json)
        {

            var bytes = Encoding.UTF8.GetBytes(json);
            var grow = (512 - bytes.Length%512);
            Array.Resize(ref bytes, bytes.Length + grow);
            try
            {
              // Loaderblob.AcquireLease(TimeSpan.FromSeconds(30), "load");
                Loaderblob.FetchAttributes();
                var p = Convert.ToInt32(Loaderblob.Metadata["nextindex"], 16);
                var ms = new MemoryStream(bytes);
                var start = p*1024;
                Loaderblob.WritePages(ms,start);
                p += 1;
                Loaderblob.Metadata["nextindex"] = String.Format("{0:x}", p);
                Loaderblob.SetMetadata();
            }
            finally
            {
             // Loaderblob.ReleaseLease(AccessCondition.GenerateLeaseCondition("load"));
            }
            return true;
        }
}


        public class Test
        {
            public static void Main()
            {

                AzureStorage.SetupGhGstorage(CloudStorageAccount.DevelopmentStorageAccount, "21f29");
               
                const string str = "21f29,Tony Manicom,173 Blandford Rd, North Riding,etc";

                var sarr = str.Split(',');

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(sarr);

                var s = AzureStorage.RegisterNewLoader(json);
                 //var t = AzureStorage.RegisterNewLoader(json);
               
                Console.ReadLine();


            
           
          }

          
        }
    }






