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
    public partial class ReadUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ButtonUpdate.Enabled = false;
            }

            UpdateGridview();
        }

        public void UpdateGridview()
        {
            SqlConnection conn = new SqlConnection(@"data source = .\sqlexpress; integrated security = true; database = northwind");
            SqlCommand cmd = null;
            SqlDataReader rdr = null;

            try
            {
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Myselectallshippers";
                rdr = cmd.ExecuteReader();

                GridViewShippers.DataSource = rdr;
                GridViewShippers.DataBind();
            }
            catch (Exception ex)
            {
                LabelMessage.Text = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        protected void GridViewShippers_SelectedIndexChanged(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"data source = .\sqlexpress; integrated security = true; database = northwind");
            SqlCommand cmd = null;
            SqlDataReader rdr = null;

            try
            {
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Myselectoneshipper";

                SqlParameter in1 = cmd.Parameters.Add("@ShipperID", SqlDbType.Int);
                in1.Direction = ParameterDirection.Input;
                in1.Value = Convert.ToInt32(GridViewShippers.SelectedRow.Cells[1].Text);

                SqlParameter out1 = cmd.Parameters.Add("@Shippercount", SqlDbType.Int);
                out1.Direction = ParameterDirection.Output;

                rdr = cmd.ExecuteReader();

                rdr.Read();
                TextBoxCompanyname.Text = rdr.GetString(1);
                TextBoxPhone.Text = rdr.GetString(2);

                rdr.Close();  // close before getting output parameter

                LabelMessage.Text = "You have chosen ShipperID " + GridViewShippers.SelectedRow.Cells[1].Text + " from the " + cmd.Parameters["@Shippercount"].Value + " shippers";
                ButtonUpdate.Enabled = true;
            }
            catch (Exception ex)
            {
                LabelMessage.Text = ex.Message;
            }
            finally
            {
                conn.Close();
            }
        }

        protected void ButtonUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"data source = .\sqlexpress; integrated security = true; database = northwind");
            SqlCommand cmd = null;
            
            try
            {
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Myupdateshipper";
                
                SqlParameter in1 = cmd.Parameters.Add("@ShipperID", SqlDbType.Int);
                in1.Direction = ParameterDirection.Input;
                in1.Value = Convert.ToInt32(GridViewShippers.SelectedRow.Cells[1].Text);

                SqlParameter in2 = cmd.Parameters.Add("@CompanyName", SqlDbType.Text);
                in2.Direction = ParameterDirection.Input;
                in2.Value = TextBoxCompanyname.Text;

                SqlParameter in3 = cmd.Parameters.Add("@Phone", SqlDbType.Text);
                in3.Direction = ParameterDirection.Input;
                in3.Value = TextBoxPhone.Text;

                cmd.ExecuteNonQuery();

                LabelMessage.Text = "Shipper has been updated";
            }
            catch (Exception ex)
            {
                LabelMessage.Text = ex.Message;
            }
            finally
            {
                conn.Close();
            }

            UpdateGridview();
        }
    }
}