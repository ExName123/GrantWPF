using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Grant2FW.AdditionalClasses
{/// <summary>
/// класс для проверки подключения к базе данных
/// </summary>
    static public class ClassCheckConnection
    {
        static public bool CheckDatabaseConnection()
        {
            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["grant_2Entities"].ConnectionString;
                
                if (connectionString.ToLower().StartsWith("metadata="))
                {
                    System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(connectionString);
                    connectionString = efBuilder.ProviderConnectionString;
                }

                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);

                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка подключения к базе данных: " + ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
