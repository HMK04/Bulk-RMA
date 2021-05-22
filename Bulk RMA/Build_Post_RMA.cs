using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Bulk_RMA
{
    public partial class Build_Post_RMA : Form
    {
        public Build_Post_RMA()
        {
            InitializeComponent();
        }

        private void radDock1_Click(object sender, EventArgs e)
        {

        }

        private void Build_Post_RMA_Load(object sender, EventArgs e)
        {
            this.dataGridView1.EnableHeadersVisualStyles = false;
            // this.dataGridView1.ColumnHeadersHeight = 60;

            LoadStocks();
            LoadRMAProblems();
        }

        void LoadRMAProblems()
        {
            Connection.openconnection();
            SqlCommand Com = new SqlCommand("SELECT distinct([ProblemCode]) from [RmaProblemCodes] order by [ProblemCode] Asc", Connection.con);
            SqlDataReader dr = Com.ExecuteReader();
            cboreasonCode.Items.Clear();
            while (dr.Read())
            {
                cboreasonCode.Items.Add(dr[0].ToString());
            }
            dr.Close();

            Connection.closeconnection();


        }
        void LoadStocks()
        {
            Connection.openconnection();
            SqlCommand Com = new SqlCommand("SELECT distinct([StockCode]) from [InvMaster] order by StockCode Asc", Connection.con);
            SqlDataReader dr = Com.ExecuteReader();
            cboStockCode.Items.Clear();
            while (dr.Read())
            {
                cboStockCode.Items.Add(dr[0].ToString());
            }
            dr.Close();

            Connection.closeconnection();
        }

        private void radTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void cboStockCode_Leave(object sender, EventArgs e)
        {
            Connection.openconnection();
            SqlCommand Com = new SqlCommand("SELECT distinct([Description]) from [InvMaster] where StockCode ='"+ cboStockCode.Text +"'", Connection.con);
            SqlDataReader dr = Com.ExecuteReader();
            dr.Read();
            if (dr.HasRows )
            {
                txtDescription.Text = dr[0].ToString();
            }
            dr.Close();

            Connection.closeconnection();
            //Check if stock code is correct

            if (txtDescription.Text == "")
            {
                Connection con = new Connection();
                con.setMessageBox("Stock code not found", 2);
                cboStockCode.Text = "";
                cboStockCode .Focus();
            }

        }

        private void cboStockCode_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            if (cboStockCode.Text == "")
            {
                txtDescription.Text = "";
            }
            else
            {
                Connection.openconnection();
                SqlCommand Com = new SqlCommand("SELECT distinct([Description]) from [InvMaster] where StockCode ='" + cboStockCode.Text + "'", Connection.con);
                SqlDataReader dr = Com.ExecuteReader();
                dr.Read();
                if (dr.HasRows)
                {
                    txtDescription.Text = dr[0].ToString();
                }
                dr.Close();

                Connection.closeconnection();
            }
        }

        private void radTextBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            Connection.DisAllowCharacters(e);
        }

        private void cboreasonCode_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            Connection.openconnection();
            SqlCommand Com = new SqlCommand("SELECT distinct([ProblemDesc]) from [RmaProblemCodes] where [ProblemCode] ='" + cboreasonCode.Text + "'", Connection.con);
            SqlDataReader dr = Com.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                txtproblemdescription.Text = dr[0].ToString();
            }
            dr.Close();

            Connection.closeconnection();
        }

        private void radTextBox1_TextChanged(object sender, EventArgs e)
        {
            Connection.openconnection();
            SqlCommand Com = new SqlCommand("SELECT distinct(Name) from [ArCustomer] where [Customer] like '%" + txtCustomercode.Text + "'", Connection.con);
            SqlDataReader dr = Com.ExecuteReader();
            dr.Read();
            if (dr.HasRows)
            {
                txtCustomername.Text = dr[0].ToString();
            }
            dr.Close();

            Connection.closeconnection();
        }

        private void cboreasonCode_Leave(object sender, EventArgs e)
        {
            if(txtproblemdescription.Text=="")
            {
                Connection con = new Connection();
                con.setMessageBox("Problem Code not found", 2);
                cboreasonCode.Text = "";
                cboreasonCode.Focus();
            }
        }

        void AddLine(string customer,string customername,string invoiceno,string stockcode,string stockdescription,)
        {
            Connection.openconnection();
            Connection.cmd = new SqlCommand("INSERT INTO [dbo].[BulkRMA] ([Customer],[CustomerName],[InvoiceNo],[StockCode],[Description],[Qty],[ReasonCode],[ReasonDescription],[Lot]) VALUES (@Customer,@CustomerName,@InvoiceNo,@StockCode,@Description,@Qty,@ReasonCode,@ReasonDescription,@Lot"),Connection.con);

            Connection.cmd.Parameters.AddWithValue("MemberNumber", Membernumber);
            Connection.cmd.Parameters.AddWithValue("FirstName", firstname);
            Connection.cmd.Parameters.AddWithValue("FullName", memberfullname);
            Connection.cmd.Parameters.AddWithValue("Gender", gender);
            Connection.cmd.Parameters.AddWithValue("PhoneNumber", phonenumber);
            Connection.cmd.Parameters.AddWithValue("Email", email);
            Connection.cmd.Parameters.AddWithValue("MemberGroup", membergroup);
            Connection.cmd.ExecuteNonQuery();

            con.setMessageBox("Record saved successfully", 1);



            Connection.closeconnection();
        }

        private void radButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
