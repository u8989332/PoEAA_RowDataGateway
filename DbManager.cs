using System.Data.SQLite;

namespace PoEAA_RowDataGateway
{
    public static class DbManager
    {
        public static SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection("Data Source=poeaa_rowdatagateway.db");
        }
    }
}