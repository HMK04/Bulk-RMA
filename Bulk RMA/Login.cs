using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Bulk_RMA
{
    public partial class Login : Form
    {
        Connection con = new Connection();
        public Login()
        {
            InitializeComponent();
        }

        void SysproLogin()
        {
            if (txtUserName.Text == "")
            {
                con.setMessageBox("User name field is blank", 2);
                txtUserName.Focus();
            }
            else if (txtCompanyID.Text == "")
            {
                con.setMessageBox("Company ID is blank", 2);
                txtCompanyID.Focus();
            }
            else
            {
                try
                {
                Connection.ReadWCFAddress();
                Connection.ReadSysproInstance();
                Connection.appClient = new SYSPROWCFServicesClientLibrary40.SYSPROWCFServicesPrimitiveClient(Connection.WCFAdrress, SYSPROWCFServicesClientLibrary40.SYSPROWCFBinding.NetTcp);
                Connection.userid = Connection.appClient.Logon(txtUserName.Text, txtUserPassword.Text, txtCompanyID.Text, txtCompanyPaswd.Text, "5", "0", Connection.Instance, "");


                //Get Profile Details
                XmlDocument xmldoc = new XmlDocument();
                Connection.LogonProfile = Connection.appClient.GetLogonProfile(Connection.userid);


                xmldoc.LoadXml(Connection.appClient.GetLogonProfile(Connection.userid));

                Connection.operatorCode = xmldoc.SelectSingleNode("//UserInfo/OperatorCode").InnerText;
                Connection.operatorName = xmldoc.SelectSingleNode("//UserInfo/OperatorName").InnerText;
                Connection.companyID = xmldoc.SelectSingleNode("//UserInfo/CompanyId").InnerText;
                Connection.companyName = xmldoc.SelectSingleNode("//UserInfo/CompanyName").InnerText;


                Build_Post_RMA RMA = new Build_Post_RMA();
                RMA.lblOperator.Text = Connection.operatorCode.ToString();
                RMA.lblOpName.Text = Connection.operatorName.ToString();
                RMA.lblCompID.Text = Connection.companyID.ToString();
                RMA.lblCompanyName.Text = Connection.companyName.ToString();
                RMA.ShowDialog();
                this.Close();
                }
                catch (Exception e)
                {

                    con.setMessageBox(e.Message.ToString(), 2);
                }

            }
        }


        private void radButton1_Click(object sender, EventArgs e)
        {
            //Build_Post_RMA BuildRMA = new Build_Post_RMA();
            //BuildRMA.ShowDialog();
            SysproLogin();
        }
    }
}
