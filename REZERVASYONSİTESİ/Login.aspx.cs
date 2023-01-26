using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace OFOS
{
    public partial class Login : System.Web.UI.Page
    {
        string constr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\Source\Repos\OFOS\ofos.mdf;Integrated Security=True";

        protected void Page_Load(object sender, EventArgs e)
        {            
            status0.Text = Request.QueryString["msg"];
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            
            SqlConnection con = new SqlConnection(constr);
            string selectSQL = "select * from [dbo].[Customers] where Username=@username ";
            SqlCommand cmd = new SqlCommand(selectSQL, con);
            SqlDataReader reader;

            try
            {
                con.Open();
                cmd.Parameters.AddWithValue("@username", tb_user.Text);
                
                reader = cmd.ExecuteReader();
                if (reader.Read() == false)
                {
                    status.Text = "user name.";
                }
                else
                {
                    Session["customer_id"] = (int)reader["Cust_Id"];
                    Session["user"] = tb_user.Text;
                    Response.Redirect("~/FoodItems.aspx");
                }
            }
            catch (Exception err)
            {

            }
            finally
            {
                con.Close();
            }
        }

        protected void user_click(object sender, EventArgs e)
        {
            registered.Visible = true;
            guest.Visible = false;
        }

        protected void guest_click(object sender, EventArgs e)
        {
            registered.Visible = false;
            guest.Visible = true;
        }

        protected void Button1_register_Click(object sender, EventArgs e)
        {            
            SqlConnection con = new SqlConnection(constr);

            try
            {
                con.Open();
                string insertSQL = "INSERT INTO [dbo].[Customers] (";
                insertSQL += "Name, Username, Email, ";
                insertSQL += "VALUES (";
                insertSQL += "@Name,@username, @Email, ";
                

                SqlCommand cmd = new SqlCommand(insertSQL, con);

                cmd.Parameters.AddWithValue("@Name", tb_name.Text);
                cmd.Parameters.AddWithValue("@username", "Guest" + System.DateTime.Now.ToString());
                cmd.Parameters.AddWithValue("@Email", tb_email.Text);
                cmd.Parameters.AddWithValue("@Contact_No", tb_contact.Text);
                ;

                int added;
                added = cmd.ExecuteNonQuery();

                string select = "select MAX(Cust_Id) from [dbo].[Customers]";
                cmd = new SqlCommand(select, con);
                int x = (int)cmd.ExecuteScalar();
                Session["customer_id"] = x;
                Session["user"] = "Guest";
                Response.Redirect("~/FoodItems.aspx");
            }
            catch (Exception err)
            {
                status.Text = err.Message;
            }
            finally
            {
                con.Close();
            }

        }
    }
}