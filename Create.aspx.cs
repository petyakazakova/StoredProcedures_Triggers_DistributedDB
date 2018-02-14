using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;

namespace NorthwindSP
{
    public partial class Create : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ButtonCreate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"data source = .\sqlexpress; integrated security = true; database = northwind");
            SqlCommand cmd = null;

            try
            {
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Myinsertshipper";

                SqlParameter in1 = cmd.Parameters.Add("@Companyname", SqlDbType.Text);
                in1.Direction = ParameterDirection.Input;
                in1.Value = TextBoxCompanyname.Text;
                
                SqlParameter in2 = cmd.Parameters.Add("@Phone", SqlDbType.Text);
                in2.Direction = ParameterDirection.Input;
                in2.Value = TextBoxPhone.Text;

                cmd.ExecuteNonQuery();

                LabelMesage.Text = "Shipper added";
            }
            catch (Exception ex)
            {
                LabelMesage.Text = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}