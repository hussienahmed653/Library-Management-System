namespace Library_Management_System.MyConnection
{
    public class Connection
    {
        static IConfiguration _configuration;
        public static string ReadMyCon()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            return _configuration["ConnectionStrings:DeafultConnection"] ?? "";
        }
    }
}
