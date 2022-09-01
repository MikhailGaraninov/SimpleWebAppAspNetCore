using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SimpleWebApp.Pages.Clients
{
    public class EditModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet() //allows get data from table (here we have to read id of the client and fill object clientInfo that will be displayed on the page)
        {
            String id = Request.Query["id"];

            try
            {
                String connectionString = "Data Source=DESKTOP-SCP0OR9\\SQLEXPRESS;Initial Catalog=SimpleWebApp;Integrated Security=True"; //try to connect to database using connectionString

                using (SqlConnection connection = new SqlConnection(connectionString)) // creating SQL connection
                {
                    connection.Open(); //open the connection
                    String sql = "SELECT * FROM clients WHERE id=@id"; // selected client with the corresponding id what we have received from request
                    using (SqlCommand command = new SqlCommand(sql, connection)) // allows to execute SQL query
                    {
                        command.Parameters.AddWithValue("@id", id); //have to replace @id parameter with id what be get from request on code line 14
                        using (SqlDataReader reader = command.ExecuteReader()) // obtain sql data reader
                        {
                            if (reader.Read()) // read data from the table using loop
                            {
                                clientInfo.id = "" + reader.GetInt32(0); // id is type string but in database ist type integer so we need to have empty string to convert integer into a string
                                clientInfo.name = reader.GetString(1); // 0-4 have to fill clientInfo with data from the database
                                clientInfo.email = reader.GetString(2);
                                clientInfo.phone = reader.GetString(3);
                                clientInfo.address = reader.GetString(4);
                                clientInfo.citizen = reader.GetString(6);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString()); //show exception message
            }
        }


        public void OnPost() //allows edit data in table from form
        {
            clientInfo.id = Request.Form["id"];
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];
            clientInfo.citizen = Request.Form["citizen"];


            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
              clientInfo.phone.Length == 0 || clientInfo.address.Length == 0 || clientInfo.citizen.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            try
            {
                String connectionString = "Data Source=DESKTOP-SCP0OR9\\SQLEXPRESS;Initial Catalog=SimpleWebApp;Integrated Security=True"; //try to connect to database using connectionString

                using (SqlConnection connection = new SqlConnection(connectionString)) // creating SQL connection
                {
                    connection.Open(); //open the connection
                    String sql = "UPDATE clients " + 
                        "SET name=@name, email=@email, phone=@phone, address=@address, citizen=@citizen" + " WHERE id=@id"; // update clients using @parameters

                    using (SqlCommand command = new SqlCommand(sql, connection)) // allows to execute SQL query
                    {
                        command.Parameters.AddWithValue("@name", clientInfo.name); // replace @parameters with the data received from the clientInfo form
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);
                        command.Parameters.AddWithValue("@id", clientInfo.id);
                        command.Parameters.AddWithValue("@citizen", clientInfo.citizen);

                        command.ExecuteNonQuery(); //execute the sql query
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.ToString()); //show exception message
            }


            Response.Redirect("/Clients/Index"); //redirect to the clients list
        }

    }
}


