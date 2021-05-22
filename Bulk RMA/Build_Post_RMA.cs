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
        Connection con = new Connection();
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
            Loadgrid();
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
                //con.setMessageBox("Stock code not found", 2);cboStockCode.Focus();
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

        void AddLine(string customer,string customername,string invoiceno,string stockcode,string stockdescription,double Qty,string Reasoncode,string Reasondesciption,string Lot)
        {
            Connection.openconnection();
            Connection.cmd = new SqlCommand("INSERT INTO [dbo].[BulkRMA] ([Customer],[CustomerName],[InvoiceNo],[StockCode],[Description],[Qty],[ReasonCode],[ReasonDescription],[Lot]) VALUES (@Customer,@CustomerName,@InvoiceNo,@StockCode,@Description,@Qty,@ReasonCode,@ReasonDescription,@Lot)",Connection.con);

            Connection.cmd.Parameters.AddWithValue("Customer", customer);
            Connection.cmd.Parameters.AddWithValue("CustomerName", customername);
            Connection.cmd.Parameters.AddWithValue("InvoiceNo", invoiceno);
            Connection.cmd.Parameters.AddWithValue("StockCode", stockcode);
            Connection.cmd.Parameters.AddWithValue("Description", stockdescription);
            Connection.cmd.Parameters.AddWithValue("Qty", Qty);
            Connection.cmd.Parameters.AddWithValue("ReasonCode", Reasoncode);
            Connection.cmd.Parameters.AddWithValue("ReasonDescription", Reasondesciption);
            Connection.cmd.Parameters.AddWithValue("Lot", Lot);
            Connection.cmd.ExecuteNonQuery();

            con.setMessageBox("Line Added successfully", 1);

            Loadgrid();

            Connection.closeconnection();
        }
        void Loadgrid()
        {

            Connection.openconnection();

            

            Connection.cmd = new SqlCommand("SELECT StockCode,Description,Qty,ReasonCode,Customer,Lot,InvoiceNo FROM BulkRMA", Connection.con);
            Connection.dr = Connection.cmd.ExecuteReader(CommandBehavior.CloseConnection);
            dataGridView1.Rows.Clear();
            while (Connection.dr.Read() == true)
            {
                dataGridView1.Rows.Add(Connection.dr[0], Connection.dr[1], Connection.dr[2], Connection.dr[3], Connection.dr[4], Connection.dr[5], Connection.dr[6]);

            }
            Connection.dr.Close();





            Connection.closeconnection();

        }

        private void radButton2_Click(object sender, EventArgs e)

        {
            if (txtCustomercode.Text == "")
            {
                con.setMessageBox("Customer field can not be blank", 2);
                txtCustomercode.Focus();
            }
            else if (cboStockCode.Text == "")
            { con.setMessageBox("Stock code field can not be blank", 2); cboStockCode.Focus(); }
            else if (txtQty.Text == "")
            { con.setMessageBox("Qty field can not be blank", 2);txtQty.Focus(); }
            else if (cboreasonCode.Text == "")
            { con.setMessageBox("Reason field can not be blank", 2);cboreasonCode.Focus(); }
            else
            {
                DialogResult msg = MessageBox.Show("Ok to Add Line?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (msg == DialogResult.OK)
                {
                    AddLine(txtCustomercode.Text, txtCustomername.Text, txtInvoice.Text, cboStockCode.Text, txtDescription.Text, double.Parse(txtQty.Text), cboreasonCode.Text, txtproblemdescription.Text, txtLot.Text);

                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
