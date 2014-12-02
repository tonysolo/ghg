using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        }

        // var blobname = qnnee.Substring(0, 3).Remove(2, 1);

          //  Epidemblob = container.GetPageBlobReference(blobname); //production epidem will go in ghg/epidem/qne
         //   if (Epidemblob.Exists()) return;
          //  Epidemblob.Create(0x40000000); //for development need to increase this to fill whole blob
          //  Epidemblob.FetchAttributes();
          //  Epidemblob.Metadata.Add("nextindex", "0x00");
         //   Epidemblob.Metadata["startoffset"] = "0x24000"; //constant 100 years =36500 days = 0x120 pages 128 per page.
          //  Epidemblob.Properties.ContentEncoding = "application/octet-stream";
          //  Epidemblob.SetMetadata();
         //   Epidemblob.SetProperties();
        

            public static void SetupEpidemStorage (CloudStorageAccount ghgAccount, string qnnee)
            {
                //use global ghg account for epidemiology (global)
                var cname = qnnee.Substring(0, 4).Remove(2, 1);
                var container = ghgAccount.CreateCloudBlobClient().GetContainerReference(cname);
                container.CreateIfNotExists();
                Epidemblob = container.GetPageBlobReference("e"); //production epidem will go in ghg/epidem/qne
                if (Epidemblob.Exists()) return;
                Epidemblob.Create(0x400000); //for development need to increase this to fill whole blob
                Epidemblob.FetchAttributes();
                Epidemblob.Metadata.Add("nextindex", "0x00");
                Epidemblob.Metadata["startoffset"] = "0x24000";
                    //constant 100 years =36500 days = 0x120 pages 128 per page.
                Epidemblob.Properties.ContentEncoding = "application/octet-stream";
                Epidemblob.SetMetadata();
                Epidemblob.SetProperties();
            }

        

        public static int RegisterNewLoader(string[] sarr)
        {
            // Loaderblob.
            return 0;
        }
}


        public class Test
        {
            public static void Main()
            {

                AzureStorage.SetupGhGstorage(CloudStorageAccount.DevelopmentStorageAccount, "21f29");
                AzureStorage.SetupEpidemStorage(CloudStorageAccount.DevelopmentStorageAccount,"21f29");
                const string str = "21f29,Tony Manicom,173 Blandford Rd, North Riding,etc";

                var sarr = str.Split(',');

                var s = AzureStorage.RegisterNewLoader(sarr);
                // var ret = AzureStorage.GetLoader("21f29", s);
                //  var x = AzureStorage.LoaderExists("21f29", "Tony Manicom");
                //  Console.WriteLine(ret);
                // Console.WriteLine(ret.Length);
                Console.ReadLine();


            
           }
        }
    }






