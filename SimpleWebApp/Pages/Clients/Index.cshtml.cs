using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SimpleWebApp.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ClientInfo> listClients = new List<ClientInfo>(); // to store the data of all the clients, PUBLIC is to read info from the page
        public void OnGet() // access to the database and read data from there
        {
            try 
            {
                String connectionString = "Data Source=DESKTOP-SCP0OR9\\SQLEXPRESS;Initial Catalog=SimpleWebApp;Integrated Security=True"; //try to connect to database using connectionString

                using (SqlConnection connection = new SqlConnection(connectionString)) // creating SQL connection
                {
                    connection.Open(); //open the connection
                    String sql = "SELECT * FROM clients"; // allows to read all arrows from the table clients
                    using (SqlCommand command = new SqlCommand(sql, connection)) // allows to execute SQL query
                    {
                        using (SqlDataReader reader = command.ExecuteReader()) // obtain sql data reader
                        {
                            while (reader.Read()) // read data from the table using loop
                            {
                                ClientInfo clientInfo = new ClientInfo(); //save this data to clientinfo object
                                clientInfo.id = "" + reader.GetInt32(0); // id is type string but in database ist type integer so we need to have empty string to convert integer into a string
                                clientInfo.name = "" + reader.GetString(1);
                                clientInfo.email = "" + reader.GetString(2);
                                clientInfo.phone = "" + reader.GetString(3);
                                clientInfo.address = "" + reader.GetString(4);
                                clientInfo.created_at = "" + reader.GetDateTime(5).ToString();
                                clientInfo.citizen = "" + reader.GetString(6);

                                listClients.Add(clientInfo); // add this object into list
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString()); //show exception message
            }
        }
    }

    public class ClientInfo // here will be shown information from database about one client
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
        public string citizen;


    }
}
