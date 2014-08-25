using GalaSoft.MvvmLight;
using System;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MvvmLight1.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class LoaderVM : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MvvmViewModel1 class.
        /// </summary>
        public LoaderVM()
        {
            Model.settings.registered = true;
            string s = Model.AzureStorage.devListContainers();
            CloudStorageAccount account = Microsoft.WindowsAzure.Storage.CloudStorageAccount.DevelopmentStorageAccount;

            CloudBlobContainer container = account.CreateCloudBlobClient().GetContainerReference("2aabb");
            CloudPageBlob loader = container.GetPageBlobReference("l");
            loader.Create(1024);
            string s1 = "Tony Manicom/n173 blandford road/n north riding, Randburg/n";
            byte[] sb = Encoding.UTF8.GetBytes(s1);

            // byte[] ba = new byte[512];
            int grow = 512 - ((sb.Length) % 512);
            Array.Resize(ref sb, sb.Length + grow);
            //  for (int i = 0; i < sb.Length; i++) ba[i] = sb[i];      
            //ba.
            // byte[] x = new byte[512];
            // for (int i = 0; i < 512; i++) x[i] = (byte)i;
            loader.UploadFromByteArray(sb, 0, 512);

            CloudPageBlob readerblob = container.GetPageBlobReference("l");
            System.IO.Stream stream = readerblob.OpenRead();
            byte[] buffer = new byte[512];
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            stream.Read(buffer, 0, 512);
            //Model.AzureStorage.devListContainers();
        }

        public string[] Qualifications { get { return Enum.GetNames(typeof(Model.qualification)); } }

        public int SecurityChoice { get; set; }

        public string[] SecurityQuestions { get { return Model.settings.securityquestions; } }

        public string SecurityAnswer { get; set; }

        public bool Registered
        {
            get { return Model.settings.registered; }
            set { Model.settings.registered = value; }
        }
    }
}