using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SimpleWebApp.Pages.Clients
{
    public class CreateModel : PageModel
    {
        public ClientInfo clientInfo = new ClientInfo();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() //will be executed when send data of this form using submit button
        {
            clientInfo.name = Request.Form["name"];
            clientInfo.email = Request.Form["email"];
            clientInfo.phone = Request.Form["phone"];
            clientInfo.address = Request.Form["address"];

            if (clientInfo.name.Length == 0 || clientInfo.email.Length == 0 ||
               clientInfo.phone.Length == 0 || clientInfo.address.Length == 0)
            {
                errorMessage = "All fields are required";
                return;
            }

            // save the new client into database
            try
            {
                String connectionString = "Data Source=DESKTOP-SCP0OR9\\SQLEXPRESS;Initial Catalog=SimpleWebApp;Integrated Security=True"; //try to connect to database using connectionString

                using (SqlConnection connection = new SqlConnection(connectionString)) // creating SQL connection
                {
                    connection.Open(); //open the connection
                    String sql = "INSERT INTO clients " + "(name, email, phone,address) VALUES " + 
                        "(@name, @email, @phone, @address);"; // add new client in clients table + add name, ... address values + replace parameters from the form
                    using (SqlCommand command = new SqlCommand(sql, connection)) // allows to execute sql query
                    {

                        command.Parameters.AddWithValue("@name", clientInfo.name); // replace @parameters with the data received from the clientInfo form
                        command.Parameters.AddWithValue("@email", clientInfo.email);
                        command.Parameters.AddWithValue("@phone", clientInfo.phone);
                        command.Parameters.AddWithValue("@address", clientInfo.address);

                        command.ExecuteNonQuery(); //execute the sql query
                        // add this object into list

                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            clientInfo.name = ""; clientInfo.email = ""; clientInfo.phone = ""; clientInfo.address = "";
            successMessage = "new client added correctly";

            Response.Redirect("/Clients/Index"); //redirect to the clients list
        }
    }
}
