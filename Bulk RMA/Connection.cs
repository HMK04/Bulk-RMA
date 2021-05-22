using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Bulk_RMA
{
    class Connection
    {
        public static SqlCommand cmd;
        public static SqlDataAdapter da;
        public static SqlDataReader dr;
        public static string SqlCon;
        public static string sqL;
        public static SqlConnection con;
        public static string userid, LogonProfile;
        public static SYSPROWCFServicesClientLibrary40.SYSPROWCFServicesPrimitiveClient appClient;
        public static string operatorCode, operatorName, companyID,
                            companyName, WCFAdrress, Input, CustomerCode, Reference, SupplierCode, LedgerCode, Invoice,
                            TransactionValue, Instance, Parameters, TaxCode;
        public static double Qty;

        #region open database connection
        public static void openconnection()
        {
            string SettingsPath = Application.StartupPath + "\\Connection.xml";
            XmlDocument settings = new XmlDocument();
            settings.Load(SettingsPath);
            // Select a specific node
            XmlNode node = settings.SelectSingleNode("/Settings/SQLconnectionString/Value");
            // Get its value
            SqlCon = node.InnerText;
            con = new SqlConnection(SqlCon);
            con.Open();
        }
        #endregion

        #region close database connection
        public static void closeconnection()
        {
            string SettingsPath = Application.StartupPath + "\\Connection.xml";
            XmlDocument settings = new XmlDocument();
            settings.Load(SettingsPath);
            // Select a specific node
            XmlNode node = settings.SelectSingleNode("/Settings/SQLconnectionString/Value");
            // Get its value
            SqlCon = node.InnerText;
            SqlConnection con = new SqlConnection(SqlCon);
            con.Close();
        }
        #endregion

        #region Method to validate field to accept numbers only

        public static void DisAllowCharacters(KeyPressEventArgs e)
        {
            if ((Keys)e.KeyChar <= Keys.Z && (Keys)e.KeyChar >= Keys.A)
            {
                e.Handled = true;
                Connection con = new Connection();
                con.setMessageBox("This field allows Numbers only", 2);
            }
            else if ((Keys)e.KeyChar <= (Keys)'z' && (Keys)e.KeyChar >= (Keys)'a')
            {
                e.Handled = true;
                Connection con = new Connection();
                con.setMessageBox("This field allows Numbers only", 2);
            }
            else if (e.KeyChar == '~' || e.KeyChar == '!' || e.KeyChar == '@' || e.KeyChar == '#' ||
                e.KeyChar == '$' || e.KeyChar == '%' || e.KeyChar == '^' || e.KeyChar == '&' ||
                e.KeyChar == '*' || e.KeyChar == '(' || e.KeyChar == ')' || e.KeyChar == '-' || e.KeyChar == '_' ||
                e.KeyChar == '=' || e.KeyChar == '+' || e.KeyChar == '|' || e.KeyChar == '\"' || e.KeyChar == ':' ||
                e.KeyChar == '\\' || e.KeyChar == ';' || e.KeyChar == '"' || e.KeyChar == '\'' || e.KeyChar == ',' ||
                e.KeyChar == '+' || e.KeyChar == '/' || e.KeyChar == '<' || e.KeyChar == '>' || e.KeyChar == '?')
            {

                e.Handled = true;
                Connection con = new Connection();
                con.setMessageBox("This field allows Numbers only", 2);


            }

        }
        #endregion


        #region textboxes
        string sMessageBox = "Information";

        public void setMessageBox(string sMessage, int iWhich)
        {
            switch (iWhich)
            {
                case 1:
                    MessageBox.Show(sMessage, sMessageBox, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    break;
                case 2:
                    MessageBox.Show(sMessage, sMessageBox, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;
                case 3:
                    MessageBox.Show(sMessage, sMessageBox, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    break;
            }
        }
        #endregion



    }
}
