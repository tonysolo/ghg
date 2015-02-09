namespace MvvmLight1.Model
{
    static class Settings
    {
        public static bool Registered { get; set; }

        public const string Za = "2202,2203,2204,jjkjk,jhjkkik,jhjkj,ghjghjj,";
        

        public static string[] Securityquestions
        {
            get
            {
                
                string[] s = {
                    "Your oldest siblings's middle name ?",
                    "Your oldest friends's middle name ?",
                    "Your childhood nickname ?",
                    "Your maternal grandmother's maiden name ?",
                    "Where were you when you heard about 9/11 ?" };
                return s;
            }
        }
    }
}