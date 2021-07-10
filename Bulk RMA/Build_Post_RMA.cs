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
            this.Height = 700;
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

        void AddLine(string customer,string customername,string invoiceno,string stockcode,string stockdescription,double Qty,string Reasoncode,string Reasondesciption,string Lot,string Operator,string PRN,string PostedFlag )
        {
            Connection.openconnection();
            Connection.cmd = new SqlCommand("INSERT INTO [dbo].[BulkRMA] ([Customer],[CustomerName],[InvoiceNo],[StockCode],[Description],[Qty],[ReasonCode],[ReasonDescription],[Lot],[Operator],[PRN],[Posted]) VALUES (@Customer,@CustomerName,@InvoiceNo,@StockCode,@Description,@Qty,@ReasonCode,@ReasonDescription,@Lot,@Operator,@PRN,@Posted)", Connection.con);

            Connection.cmd.Parameters.AddWithValue("Customer", customer);
            Connection.cmd.Parameters.AddWithValue("CustomerName", customername);
            Connection.cmd.Parameters.AddWithValue("InvoiceNo", invoiceno);
            Connection.cmd.Parameters.AddWithValue("StockCode", stockcode);
            Connection.cmd.Parameters.AddWithValue("Description", stockdescription);
            Connection.cmd.Parameters.AddWithValue("Qty", Qty);
            Connection.cmd.Parameters.AddWithValue("ReasonCode", Reasoncode);
            Connection.cmd.Parameters.AddWithValue("ReasonDescription", Reasondesciption);
            Connection.cmd.Parameters.AddWithValue("Lot", Lot);
            Connection.cmd.Parameters.AddWithValue("Operator", Operator );
            Connection.cmd.Parameters.AddWithValue("PRN", PRN);
            Connection.cmd.Parameters.AddWithValue("Posted", PostedFlag);
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
            else if (txtCustomerQty.Text == "")
            { con.setMessageBox("Qty field can not be blank", 2);txtCustomerQty.Focus(); }
            else if (cboreasonCode.Text == "")
            { con.setMessageBox("Reason field can not be blank", 2);cboreasonCode.Focus(); }
            else
            {
                DialogResult msg = MessageBox.Show("Ok to Add Line?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (msg == DialogResult.OK)
                {
                    AddLine(txtCustomercode.Text, txtCustomername.Text, txtInvoice.Text, cboStockCode.Text, txtDescription.Text, double.Parse(txtCustomerQty.Text), cboreasonCode.Text, txtproblemdescription.Text, txtLot.Text,lblOperator.Text,txtPRN.Text,"N");

                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        void CreateRMAHeader()
        {
            Connection.CustomerCode = txtCustomercode.Text;
            Connection.Input = Connection.RMAHeaderXmlInItem(Connection.CustomerCode);
            //Connection.Parameters = Connection.RMAHeaderBOParameters();

            string OutPut = Connection.appClient.TransactionPost(Connection.userid, "RMATRH", Connection.RMAHeaderBOParameters(), Connection.RMAHeaderXmlIn());

            if (OutPut.Contains("<ErrorDescription>"))
            {
                string[] ErrorsArray = null;
                ErrorsArray = OutPut.ToString().Split(new string[] { "<ErrorDescription>" }, StringSplitOptions.None);
                string s = "";
                for (int j = 1; j < ErrorsArray.Length; j++)
                {

                    int indx = ErrorsArray[j].ToString().LastIndexOf("</ErrorDescription>");
                    s += "*" + ErrorsArray[j].ToString().Substring(0, indx) + Environment.NewLine;

                    if (j == (ErrorsArray.Length - 1))
                    {

                        //MessageBox.Show(s, "Posting Errors", MessageBoxButtons.OK, MessageBoxIcon.Error );

                    }



                }
                MessageBox.Show(s, "Posting Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                s = null;
            }
            else
            {


                Connection.Input = null;

                string[] ErrorsArray = null;
                ErrorsArray = OutPut.ToString().Split(new string[] { "<RmaNumberCreated>" }, StringSplitOptions.None);
                string s = "";
                for (int j = 1; j < ErrorsArray.Length; j++)
                {

                    int indx = ErrorsArray[j].ToString().LastIndexOf("</RmaNumberCreated>");
                    s = ErrorsArray[j].ToString().Substring(0, indx);

                    if (j == (ErrorsArray.Length - 1))
                    {

                        //MessageBox.Show(s, "Posting Errors", MessageBoxButtons.OK, MessageBoxIcon.Error );

                    }



                }
                //MessageBox.Show(s, "Posted Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtRMAnumber.Text = s;
                s = null;
                //con.setMessageBox("Credit Notes Transaction(s) posted successfully", 1);
                //FlagPostedCredits();
                //btnPost.Enabled = false;

            }




        }

       
        void CreateRMALines()
        {
            //Get XmlIn String Values
            string IntermidiateInputItemm = "";
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                Connection.RMANumber = txtRMAnumber.Text;
                Connection.StockCode = dataGridView1.Rows[i].Cells[0].Value.ToString();
                Connection.Qty = double.Parse(dataGridView1.Rows[i].Cells[2].Value.ToString());
                Connection.ProblemCode = dataGridView1.Rows[i].Cells[3].Value.ToString();



                IntermidiateInputItemm = Connection.RMALinesXmlInItem(Connection.RMANumber, Connection.StockCode, Connection.Qty, Connection.ProblemCode);
                Connection.Input += IntermidiateInputItemm;

            }




                string OutPut = Connection.appClient.TransactionPost(Connection.userid, "RMATRL", Connection.RMALinesBOParameters(), Connection.RMAAddLinesXmlIn());


                //Echo the Output
                if (OutPut.Contains("<ErrorDescription>"))
                {
                    Connection.Input = null;    
                    string[] ErrorsArray = null;
                    ErrorsArray = OutPut.ToString().Split(new string[] { "<ErrorDescription>" }, StringSplitOptions.None);
                    string s = "";
                    for (int j = 1; j < ErrorsArray.Length; j++)
                    {

                        int indx = ErrorsArray[j].ToString().LastIndexOf("</ErrorDescription>");
                        s += "*" + ErrorsArray[j].ToString().Substring(0, indx) + Environment.NewLine;

                        if (j == (ErrorsArray.Length - 1))
                        {

                            //MessageBox.Show(s, "Posting Errors", MessageBoxButtons.OK, MessageBoxIcon.Error );

                        }
                


                    }
                    MessageBox.Show(s, "Posting Errors", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    s = null;
                }
                
            

            con.setMessageBox("RMA Posted Successfully", 1);
        }
        private void radButton5_Click(object sender, EventArgs e)
        {
            Connection.Input="";
            CreateRMAHeader();
            CreateRMALines();
        }

        private void txtCustomerQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            Connection.DisAllowCharacters(e);
        }

        void QtyCredited()
        {
            double CustomerQty, QtyCounted, QtyRejected, QtyAccepted, QtyCredited;
            CustomerQty = double.Parse(txtCustomerQty.Text);
            QtyCounted = double.Parse(txtCountedQty.Text);
            QtyRejected = double.Parse(txtRejectedQty.Text);
            ////QtyAccepted = double.Parse(txtAcceptedQty.Text);
            QtyCredited = double.Parse(txtQty.Text );

            QtyAccepted = CustomerQty - QtyRejected;




        }
        private void txtRejectedQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void radButton3_Click(object sender, EventArgs e)
        {

        }
    }
}
