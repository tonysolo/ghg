namespace MvvmLight1.Model
{
    static class Settings
    {
        public static bool Registered { get; set; }
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