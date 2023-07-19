namespace Api_Coppel.Repository
{
    public class ConexionSql
    {
        public string GetConnectionString()
        {
            var configFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(configFilePath, optional: true, reloadOnChange: true)
                .Build();

            string connectionString = configuration.GetConnectionString("cadenaSQl");


            return connectionString;
        }
    }
}
