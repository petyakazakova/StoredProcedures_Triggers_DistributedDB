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
    public partial class Delete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                UpdateGridview();
                ButtonDelete.Enabled = false;
            }

            DropDownListShippers.AutoPostBack = true;
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

                rdr.Close();
                rdr = cmd.ExecuteReader();

                DropDownListShippers.DataSource = rdr;
                DropDownListShippers.DataTextField = "CompanyName";
                DropDownListShippers.DataValueField = "ShipperID";
                DropDownListShippers.DataBind();
                DropDownListShippers.Items.Insert(0, "Select a shipper");
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

        protected void DropDownListShippers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DropDownListShippers.SelectedIndex != 0)
            {
                LabelMessage.Text = "You chose ShipperID " + DropDownListShippers.SelectedValue;
                ButtonDelete.Enabled = true;
            }
            else
            {
                LabelMessage.Text = "You chose none";
                ButtonDelete.Enabled = false;
            }
        }

        protected void ButtonDelete_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection(@"data source = .\sqlexpress; integrated security = true; database = northwind");
            SqlCommand cmd = null;

            try
            {
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Mydeleteshipper";

                SqlParameter in1 = cmd.Parameters.Add("@ShipperID", SqlDbType.Int);
                in1.Direction = ParameterDirection.Input;
                in1.Value = Convert.ToInt32(DropDownListShippers.SelectedValue);

                cmd.ExecuteNonQuery();

                ButtonDelete.Enabled = false;
                LabelMessage.Text = "Shipper " + DropDownListShippers.SelectedValue + " has been deleted";
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