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
       //public static SqlDataAdapter da;
        public static SqlDataReader dr;
        public static string SqlCon;
        //public static string sqL;
        public static SqlConnection con;
        public static string userid, LogonProfile;
        public static SYSPROWCFServicesClientLibrary40.SYSPROWCFServicesPrimitiveClient appClient;
        public static string operatorCode, operatorName, companyID,
                            companyName, WCFAdrress, Input,Inputt, CustomerCode, RMANumber, StockCode, ProblemCode,Parameters,
                            Instance;
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


        #region Read WCF Address
        public static void ReadWCFAddress()
        {
            string SettingsPath = Application.StartupPath + "\\WCFAddress.xml";
            XmlDocument settings = new XmlDocument();
            settings.Load(SettingsPath);
            // Select a specific node
            XmlNode WCF = settings.SelectSingleNode("/SQLconnectionString/Address");
            // Get its value
            WCFAdrress = WCF.InnerText;

        }
        #endregion

        #region read LoginURL
        public static void ReadSysproInstance()
        {
            string SettingsPath = Application.StartupPath + "\\SysproInstance.xml";
            XmlDocument settings = new XmlDocument();
            settings.Load(SettingsPath);
            // Select a specific node
            XmlNode WCF = settings.SelectSingleNode("/SQLconnectionString/Address");
            // Get its value
            Instance = WCF.InnerText;

        }
        #endregion

        #region RMAHeaderXMLIn Header

        public static string RMAHeaderXmlIn()
        {

            String XMLInString;
            XMLInString = "<RmaHeader>" + Environment.NewLine
                    + Connection.Input + Environment.NewLine
                    + "</RmaHeader>";
            return XMLInString.ToString();

        }

        #endregion

        #region RMAHeaderINString
        public static string RMAHeaderXmlInItem(string CustomerCode)
        {
            string RMAHeaderINString = "<Item>" + Environment.NewLine
                    + "<CreateUpdate>C</CreateUpdate>" + Environment.NewLine
                    + "<Customer>" + CustomerCode + "</Customer>" + Environment.NewLine
                    + "<AllowCustomerOnHold>N</AllowCustomerOnHold>" + Environment.NewLine                     
                    + "</Item>";
            return RMAHeaderINString.ToString();
        }
        #endregion


        #region RMAHeaderBOParameters
        public static string RMAHeaderBOParameters()
        {

            //Declaration
            StringBuilder Parameter = new StringBuilder();

            //Building Parameter content
            Parameter.Append("<?xml version=\"1.0\" encoding=\"Windows-1252\"?>");
            Parameter.Append("<!-- Copyright 1994-2010 SYSPRO Ltd.-->");
            Parameter.Append("<!--");
            Parameter.Append("This is an example XML instance to demonstrate");
            Parameter.Append("use of the Create and Update RMA Header Business Object");
            Parameter.Append("-->");
            Parameter.Append("<RmaHeader xmlns:xsd=\"http://www.w3.org/2001/XMLSchema-instance\" xsd:noNamespaceSchemaLocation=\"RMATRH.XSD\">");
            Parameter.Append("	<Parameters>");
            Parameter.Append("		<IgnoreWarnings>N</IgnoreWarnings>");
            Parameter.Append("		<ApplyIfEntireDocumentValid>Y</ApplyIfEntireDocumentValid>");
            Parameter.Append("		<ValidateOnly>N</ValidateOnly>");
            Parameter.Append("	</Parameters>");
            Parameter.Append("</RmaHeader>");
            return Parameter.ToString();


        }

        #endregion



        #region RMAAddLinesXMLIn

        public static string RMAAddLinesXmlIn()
        {

            String XMLInString;
            XMLInString = "<RmaLine>" + Environment.NewLine
                    + Connection.Input + Environment.NewLine
                    + "</RmaLine>";
            return XMLInString.ToString();

        }

        #endregion

        #region RMALinesINString
        public static string RMALinesXmlInItem(string RMANumber,string StockCode,double Qty,string ProblemCode)
        {
            string RMAAddLinesINString = "<Item>" + Environment.NewLine
                    + "<AddChange>A</AddChange>" + Environment.NewLine
                    + "<RmaNumber>" + RMANumber + "</RmaNumber>" + Environment.NewLine
                    + "<StockCode>"+StockCode+"</StockCode>" + Environment.NewLine
                    + "<Quantity>" + Qty + "</Quantity>" + Environment.NewLine
                    + "<UnitOfMeasure>EA</UnitOfMeasure>" + Environment.NewLine
                    + "<ProblemCode>"+ProblemCode+"</ProblemCode>" + Environment.NewLine
                     + "<RmaType>G</RmaType>" + Environment.NewLine
                    + "</Item>";
            return RMAAddLinesINString.ToString();
        }
        #endregion


        #region RMAHeaderBOParameters
        public static string RMALinesBOParameters()
        {

            //Declaration
            StringBuilder Parameter = new StringBuilder();

            //Building Parameter content
            Parameter.Append("<?xml version=\"1.0\" encoding=\"Windows-1252\"?>");
            Parameter.Append("<!-- Copyright 1994-2012 SYSPRO Ltd.-->");
            Parameter.Append("<!--");
            Parameter.Append("This is an example XML instance to demonstrate");
            Parameter.Append("use of the Add or Change RMA Line Business Object");
            Parameter.Append("-->");
            Parameter.Append("<RmaLine xmlns:xsd=\"http://www.w3.org/2001/XMLSchema-instance\" xsd:noNamespaceSchemaLocation=\"RMATRL.XSD\">");
            Parameter.Append("<Parameters>");
            Parameter.Append("<IgnoreWarnings>N</IgnoreWarnings>");
            Parameter.Append("<ApplyIfEntireDocumentValid>N</ApplyIfEntireDocumentValid>");
            Parameter.Append("<ValidateOnly>N</ValidateOnly>");
            Parameter.Append("<ShipFromDefaultBin>N</ShipFromDefaultBin>");
            Parameter.Append("</Parameters>");
            Parameter.Append("</RmaLine>");
            return Parameter.ToString();


        }

        #endregion

    }
}
