using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Principal;

namespace Gen2Server2
{
    public partial class gen2server2 : Form
    {

        /* [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
         private static extern IntPtr CreateRoundRectRgn
         (
             int nLeftRect, // x-coordinate of upper-left corner
             int nTopRect, // y-coordinate of upper-left corner
             int nRightRect, // x-coordinate of lower-right corner
             int nBottomRect, // y-coordinate of lower-right corner
             int nWidthEllipse, // height of ellipse
             int nHeightEllipse // width of ellipse
          );*/
        string connetionstringgen2 = "Data Source=10.207.40.200;Initial Catalog=Gen2;Persist Security Info=True;User ID=GEN2;Password=1234";
        string connetionString = "Data Source=10.207.1.56;Initial Catalog=pts;Persist Security Info=True;User ID=pts;Password=Qwer1234";
        string op_id = "";
        string globalusername = "";
        string globalpassword = "";


        public gen2server2()
        {
            InitializeComponent();
            //Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 5, 5));
            MaximizeBox = false;

        }




        private void gen2server2_Load(object sender, EventArgs e)
        {
            CultureInfo USlanguage = CultureInfo.CreateSpecificCulture("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = USlanguage;
            InputLanguage l = InputLanguage.FromCulture(USlanguage);
            InputLanguage.CurrentInputLanguage = l;

            szeriaszam_checkBox1.Checked = true;
            szeriaszam_textBox1.Enabled = true;

            datum_checkBox2.Checked = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            

            muvelet_checkBox3.Checked = false;
            muvelet_comboBox1.Enabled = false;

            eredmeny_checkBox4.Checked = false;
            eredmeny_comboBox2.Enabled = false;

            tipus_checkBox5.Checked = false;
            tipus_comboBox3.Enabled = false;

            muvelet_listbox.Enabled = false;
            muvelet_listbox_checkbox.Checked = false;

            eredmeny_comboBox2.Text = "OK";
            tipus_comboBox3.Text = "VOLVO";
            muvelet_comboBox1.Text = "4005 - Label sticked";

            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy HH:mm:ss";

            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "MM/dd/yyyy HH:mm:ss";

            dateTimePicker3.Format = DateTimePickerFormat.Custom;
            dateTimePicker3.CustomFormat = "MM/dd/yyyy HH:mm:ss";

            dateTimePicker4.Format = DateTimePickerFormat.Custom;
            dateTimePicker4.CustomFormat = "MM/dd/yyyy HH:mm:ss";

            dsp_id_checkBox1.Checked = true;
            bscan_dsp_ID_textBox1.Enabled = true;

            bscan_datum_checkBox2.Checked = false;
            dateTimePicker3.Enabled = false;
            dateTimePicker4.Enabled = false;

            bscan_eredmeny_checkBox3.Checked = false;
            bscan_eredmeny_comboBox1.Enabled = false;

            gw_radioButton1.Checked = true;
            firewall_value_label.BackColor = System.Drawing.Color.Yellow;


            muvelet_listbox.HorizontalScrollbar = true;

            try
            {
                string query1 = "Select * from [dbo].[Operations]";

                SqlConnection cnn = new SqlConnection(connetionstringgen2);
                cnn.Open();
                SqlCommand objcmdm = new SqlCommand(query1, cnn);
                objcmdm.ExecuteNonQuery();
                SqlDataAdapter adpm = new SqlDataAdapter(objcmdm);
                DataTable dtm = new DataTable();
                adpm.Fill(dtm);
                eredmenyek_seged_datagridview.DataSource = dtm;
                eredmenyek_seged_datagridview.Refresh();
                // pictureBox1.Visible = false;
                cnn.Close();


                if (eredmenyek_seged_datagridview.RowCount > 1)
                {
                    for (int i = 0; i < eredmenyek_seged_datagridview.RowCount - 1; i++)
                    {

                        muvelet_listbox.Items.Add(eredmenyek_seged_datagridview.Rows[i].Cells["Operation_ID"].Value.ToString() + " - " + eredmenyek_seged_datagridview.Rows[i].Cells["Operation_Msg"].Value.ToString());
                        muvelet_comboBox1.Items.Add(eredmenyek_seged_datagridview.Rows[i].Cells["Operation_ID"].Value.ToString() + " - " + eredmenyek_seged_datagridview.Rows[i].Cells["Operation_Msg"].Value.ToString());

                    }


                }

            }
            catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka:\n" + ex.Message); }

            //"yyyy-MM-dd HH:mm:ss")



        }

        private void exportal_Click(object sender, EventArgs e)
        {

            if (dataGridView1.RowCount > 1)
            {
                try
                {
                    System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                    saveDlg.InitialDirectory = @"C:\";
                    saveDlg.Filter = "CSV files (*.csv)|*.csv";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export csv File To";



                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string CsvFpath = saveDlg.FileName;
                        // MessageBox.Show(CsvFpath);
                        string columnHeaderText = "";
                        System.IO.StreamWriter csvFileWriter = new StreamWriter(CsvFpath, false);



                        int countColumn = dataGridView1.ColumnCount - 1;


                        if (countColumn >= 0)
                        {
                            columnHeaderText = dataGridView1.Columns[0].HeaderText;
                        }

                        for (int i = 1; i <= countColumn; i++)
                        {
                            columnHeaderText = columnHeaderText + ';' + dataGridView1.Columns[i].HeaderText;
                        }


                        csvFileWriter.WriteLine(columnHeaderText);

                        foreach (DataGridViewRow dataRowObject in dataGridView1.Rows)
                        {
                            if (!dataRowObject.IsNewRow)
                            {
                                string dataFromGrid = "";

                                dataFromGrid = dataRowObject.Cells[0].Value.ToString();

                                for (int i = 1; i <= countColumn; i++)
                                {
                                    dataFromGrid = dataFromGrid + ';' + dataRowObject.Cells[i].Value.ToString();


                                }
                                csvFileWriter.WriteLine(dataFromGrid);
                            }
                        }

                        csvFileWriter.Flush();
                        csvFileWriter.Close();
                    }
                }
                catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka: \n" + ex.Message); }
            }
            else { MessageBox.Show("Nincs adat a táblázatban!"); }
        }

        private void exportal2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.RowCount > 1)
            {
                try
                {
                    System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                    saveDlg.InitialDirectory = @"C:\";
                    saveDlg.Filter = "CSV files (*.csv)|*.csv";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export csv File To";



                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string CsvFpath = saveDlg.FileName;
                        // MessageBox.Show(CsvFpath);
                        string columnHeaderText = "";
                        System.IO.StreamWriter csvFileWriter = new StreamWriter(CsvFpath, false);



                        int countColumn = dataGridView2.ColumnCount - 1;


                        if (countColumn >= 0)
                        {
                            columnHeaderText = dataGridView2.Columns[0].HeaderText;
                        }

                        for (int i = 1; i <= countColumn; i++)
                        {
                            columnHeaderText = columnHeaderText + ';' + dataGridView2.Columns[i].HeaderText;
                        }


                        csvFileWriter.WriteLine(columnHeaderText);

                        foreach (DataGridViewRow dataRowObject in dataGridView2.Rows)
                        {
                            if (!dataRowObject.IsNewRow)
                            {
                                string dataFromGrid = "";

                                dataFromGrid = dataRowObject.Cells[0].Value.ToString();

                                for (int i = 1; i <= countColumn; i++)
                                {
                                    dataFromGrid = dataFromGrid + ';' + dataRowObject.Cells[i].Value.ToString();


                                }
                                csvFileWriter.WriteLine(dataFromGrid);
                            }
                        }


                        csvFileWriter.Flush();
                        csvFileWriter.Close();

                    }
                }
                catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka: \n" + ex.Message); }
            }
            else { MessageBox.Show("Nincs adat a táblázatban!"); }
        }

        private void lekerdez_Click(object sender, EventArgs e)
        {


            int selectedrowsinlistbox = 0;
            int osszesokburnin = 0;
            int hosszuokburin = 0;
            int rovidokburnin = 0;
            string szeriaszam = "";
            string listboxrow = "";
            rovidteszt_label.Visible = false;
            hosszuteszt_label.Visible = false;
            osszteszt_label.Visible = false;
            //  pictureBox1.Visible = true;

            /****************************************************Fejlesztett lekérdezés************************************************/
            // string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
            try
            {
                string heatsingquery =
                  "Select HeatSinkData.HeatSink_ID as ID "
                  + ", HeatSinkData.Workstation"
                  + ", HeatSinkData.Operation_ID"
                  + ", Operations.Operation_Msg"
                  + ", HeatSinkData.Result"
                  + ", HeatSinkData.Value"
                  + ", HeatSinkData.Limit_Min"
                  + ", HeatSinkData.Limit_Max"
                  + ", HeatSinkData.Date"
                  + ", HeatSinkData.Operator"
                  + ", HeatSinkData.Note"
                  + " from [dbo].[HeatSinkData] JOIN [dbo].[Operations]"
                  + " ON HeatSinkData.Operation_ID=Operations.Operation_ID"
                  + " INNER JOIN Gen2.dbo.Main ON HeatSinkData.HeatSink_ID=main.HeatSink_ID"
                  + " WHERE";

                //  Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                

                string housingquery =
                    "Select HousingData.Housing_ID as ID "
                    + ", HousingData.Workstation"
                    + ", HousingData.Operation_ID"
                    + ", Operations.Operation_Msg"
                    + ", HousingData.Result"
                    + ", HousingData.Value"
                    + ", HousingData.Limit_Min"
                    + ", HousingData.Limit_Max"
                    + ", HousingData.Date"
                    + ", HousingData.Operator"
                    + ", HousingData.Note"
                    + " from [dbo].[HousingData] JOIN [dbo].[Operations]"
                    + " ON HousingData.Operation_ID=Operations.Operation_ID"
                    + " INNER JOIN Gen2.dbo.Main ON HousingData.Housing_ID=main.Housing_ID"
                    + " WHERE";

                if (szeriaszam_checkBox1.Checked == true)
                {
                    szeriaszam = Convert.ToString(szeriaszam_textBox1.Text);
                    if (szeriaszam_textBox1.Text != "")
                    {
                        szeriaszam = szeriaszam.Replace("*", "%");
                        heatsingquery += "AND HeatSinkData.HeatSink_ID LIKE '" + szeriaszam + "'";
                        housingquery += "AND HousingData.Housing_ID LIKE '" + szeriaszam + "' ";
                    }
                    else
                    {
                        heatsingquery += "AND HeatSinkData.HeatSink_ID = NULL ";
                        housingquery += "AND HousingData.Housing_ID = NULL ";
                    }

                }

                if (datum_checkBox2.Checked == true)
                {

                    heatsingquery += "AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                    housingquery += "AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";

                }

                if (muvelet_checkBox3.Checked == true)
                {
                    string muvelet = muvelet_comboBox1.Text;
                    char[] separator = { ' ', '-' };
                    string[] word = muvelet.Split(separator);
                    op_id = word[0];


                    heatsingquery += "AND HeatSinkData.Operation_ID = '" + op_id + "'";
                    housingquery += "AND HousingData.Operation_ID = '" + op_id + "'";


                }

                if (eredmeny_checkBox4.Checked == true)
                {

                    heatsingquery += "AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "'";
                    housingquery += "AND HousingData.Result = '" + eredmeny_comboBox2.Text + "'";

                }
                if (tipus_checkBox5.Checked == true)
                {
                    heatsingquery += "AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "'";
                    housingquery += "AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "'";

                }
                if (muvelet_listbox_checkbox.Checked == true)
                {
                    int index;
                    int muvelet_item_selected = muvelet_listbox.SelectedItems.Count;

                    if (muvelet_item_selected > 0)
                    {
                        heatsingquery += "AND (HeatSinkData.Operation_ID =";
                        housingquery += "AND (HousingData.Operation_ID =";



                        foreach (int i in muvelet_listbox.SelectedIndices)
                        {
                            //  MessageBox.Show(Convert.ToString(i));
                            listboxrow = muvelet_listbox.Items[i].ToString();
                            string muvelet = listboxrow;
                            char[] separator = { ' ', '-' };
                            string[] word = muvelet.Split(separator);
                            op_id = word[0];

                            heatsingquery += " '" + op_id + "' OR HeatSinkData.Operation_ID =";
                            housingquery += "'" + op_id + "' OR HousingData.Operation_ID =";


                        }
                        heatsingquery += ")";
                        housingquery += ")";

                        heatsingquery = heatsingquery.Replace("OR HeatSinkData.Operation_ID =)", ")");
                        housingquery = housingquery.Replace("OR HousingData.Operation_ID =)", ")");

                    }

                }

                heatsingquery = heatsingquery.Replace("WHEREAND", " WHERE");
                housingquery = housingquery.Replace("WHEREAND", " WHERE");

                string unionquery = heatsingquery + " UNION " + housingquery;
                eredmenyek_lekerdezese1(unionquery,dataGridView1);

                //MessageBox.Show(unionquery);


                if (dataGridView1.RowCount <= 1)
                {
                    DialogResult dialogResult = MessageBox.Show("Nincs eredmény megpróbálja folytatni egy másik lekérdezéssel?", "Nincs találat", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {

                        /**************************************************************************************************************************/

                        /******************************************Lekérdezések*************************************************/


                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            MessageBox.Show("Kérem szűkítse a lekérdezést!");

                        }

                        if (muvelet_checkBox3.Checked == true)
                        {
                            string muvelet = muvelet_comboBox1.Text;
                            char[] separator = { ' ', '-' };
                            string[] word = muvelet.Split(separator);
                            op_id = word[0];

                            /* MessageBox.Show(op_id);
                             MessageBox.Show(Convert.ToString(op_id.Length));*/

                        }




                        /*******************************simpla lekérdezések*************************************/

                        /**********Ha csak a dátum van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("Csak a dátumot választottad ki!");

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";


                            eredmenyek_lekerdezese1(query1,dataGridView1);




                        }
                        /****Csak a szériaszám van kipipálva****/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            //  MessageBox.Show("Csak a szériaszámot választottad ki!");
                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {

                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "'";

                                    eredmenyek_lekerdezese1(query1,dataGridView1);

                                }
                                else
                                {

                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }


                        }
                        /****Csak a művelet van kipipálva****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            //   MessageBox.Show("Csak a műveletet választottad ki!");

                            /* string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "'";
                             eredmenyek_lekerdezese(query1);

                             if (dataGridView1.RowCount <= 1)
                             {
                                 string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Operation_ID = '" + Convert.ToString(op_id) + "'";
                                 eredmenyek_lekerdezese(query2);
                
                             }*/
                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Operation_ID = '" + op_id + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Operation_ID = '" + op_id + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);



                        }
                        /****Csak az eredmény van kipipálva****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {

                            //MessageBox.Show("Csak az eredményt választottad ki!");
                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Result = '" + eredmeny_comboBox2.Text + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                        }
                        /****Csak a típus van kipipálva****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            MessageBox.Show("Kérem szűkítse a lekérdezést (túl sok adat)");

                            /* string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE Main.Type = '" + tipus_comboBox3.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN Gen2.dbo.Main ON HousingData.Housing_ID=main.Housing_ID WHERE Main.Type = '" + tipus_comboBox3.Text + "'";


                             eredmenyek_lekerdezese(query1);*/

                        }

                        /***********************************Dupla lekérdezések***************************************/

                        /**********Ha a szériaszám és dátum van kipipálva*********/

                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("Szériaszámot és dátumot választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }


                        }

                        /**********Ha a szériaszám és művelet van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("Szériaszámot és műveletet választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Operation_ID = '" + op_id + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Operation_ID = '" + op_id + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }


                        }

                        /**********Ha a szériaszám és eredmény van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("Szériaszámot és eredményt választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Result = '" + Convert.ToString(eredmeny_comboBox2.Text) + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND Housingdata.Result = '" + Convert.ToString(eredmeny_comboBox2.Text) + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }

                        /**********Ha a szériaszám és típus van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Szériaszámot és típust választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }

                        /**********Ha a dátum és művelet van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("Dátum és művelet választottad ki!");

                            /* string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                             eredmenyek_lekerdezese(query1);

                             if (dataGridView1.RowCount <= 1)
                             {
                                 string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                 eredmenyek_lekerdezese(query2);

                             }*/

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HeatSinkData.Operation_ID = '" + op_id + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HousingData.Operation_ID = '" + op_id + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);


                        }

                        /**********Ha a dátum és eredmény van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {

                            //MessageBox.Show("Dátum és eredményt választottad ki!");

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Result = '" + eredmeny_comboBox2.Text + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                        }

                        /**********Ha a dátum és típus van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            //  MessageBox.Show("Dátum és típust választottad ki!");

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "'";
                            //eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);


                        }

                        /**********Ha a művelet és eredmény van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("Művelet és eredmény választottad ki!");
                            //MessageBox.Show(Convert.ToString(op_id));
                            /*  string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.Heatsink_ID<>'' AND HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HeatSinkData.Result = '"+eredmeny_comboBox2.Text+"'";
                              eredmenyek_lekerdezese(query1);

                              if (dataGridView1.RowCount <= 1)
                              {
                                  string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID<>'' AND HousingData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HousingData.Result = '" + eredmeny_comboBox2.Text + "'";
                                  eredmenyek_lekerdezese(query2);

                              }*/

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Operation_ID = '" + op_id + "'  AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Operation_ID = '" + op_id + "' AND HousingData.Result = '" + eredmeny_comboBox2.Text + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);



                        }

                        /**********Ha a művelet és típus van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Művelet és típus választottad ki!");

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Heatsink_ID<>'' AND HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "' AND Main.Type = '" + tipus_comboBox3.Text + "'";
                            //eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                            if (dataGridView1.RowCount <= 1)
                            {
                                string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID<>'' AND HousingData.Operation_ID = '" + Convert.ToString(op_id) + "' AND Main.Type = '" + tipus_comboBox3.Text + "'";
                               // eredmenyek_lekerdezese(query2);
                                eredmenyek_lekerdezese1(query1, dataGridView1);
                            }
                        }

                        /**********Ha az eredmény és típus van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            MessageBox.Show("Szűkítsd a lekérdezést! (Túl sok adat)");

                            /*string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Result = '" +eredmeny_comboBox2.Text+ "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Result = '" +eredmeny_comboBox2.Text + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "'";
                            eredmenyek_lekerdezese(query1);*/
                            //  pictureBox1.Visible = false;



                        }

                        /*******************************tripla lekérdezések*************************************/

                        /**********Ha a szériaszám, dátum,művelet van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("szériaszám, dátum, műveletet választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HeatSinkData.Operation_ID = '" + op_id + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HousingData.Operation_ID = '" + op_id + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }




                        }
                        /**********Ha a szériaszám, dátum, eredmény van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {

                            //  MessageBox.Show("szériaszám, dátum, ereményt választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HeatSinkData.Result='" + eredmeny_comboBox2.Text + "'";
                                  //  eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HousingData.Result='" + eredmeny_comboBox2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }
                        /**********Ha a szériaszám, dátum, típus van kipipálva*********/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("szériaszám, dátum, típus választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }
                        /********************************************************Ellenőrizni innen*********************************************************/
                        /**********Ha a szériaszám, művelet, eredmény van kipipálva**ok*******/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("szériaszám, művelet, eredmény választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Operation_ID = '" + op_id + "' AND HeatSinkData.Result='" + eredmeny_comboBox2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Operation_ID = '" + op_id + "' AND HousingData.Result= '" + eredmeny_comboBox2.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }
                        /**********Ha a szériaszám, művelet, típus van kipipálva****ok*****/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            //  MessageBox.Show("szériaszám, művelet, típus választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Operation_ID = '" + op_id + "' AND Main.Type='" + tipus_comboBox3.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Operation_ID = '" + op_id + "' AND Main.Type= '" + tipus_comboBox3.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }


                        }
                        /**********Ha a szériaszám, típus, eredmény van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("szériaszám, típus, eredmény választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' AND Main.Type='" + tipus_comboBox3.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Result = '" + eredmeny_comboBox2.Text + "' AND Main.Type= '" + tipus_comboBox3.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }

                        /**********Ha a dátum, művelet eredmény van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {
                            // MessageBox.Show("Dátum, művelet, eredmény választottad ki!");

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                          //  eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                            if (dataGridView1.RowCount <= 1)
                            {
                                string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HousingData.Result='" + eredmeny_comboBox2.Text + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                //eredmenyek_lekerdezese(query2);
                                eredmenyek_lekerdezese1(query1, dataGridView1);

                            }


                        }

                        /**********Ha a dátum, művelet típus van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Dátum, művelet, típus választottad ki!");

                            /* string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '"+tipus_comboBox3.Text+"'";
                             eredmenyek_lekerdezese(query1);

                             if (dataGridView1.RowCount <= 1)
                             {
                                 string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HousingData.Result='" + eredmeny_comboBox2.Text + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '"+tipus_comboBox3.Text+"'";
                                 eredmenyek_lekerdezese(query2);

                             }*/

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' AND HeatSinkData.Operation_ID='" + op_id + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' AND HousingData.Operation_ID = '" + op_id + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                        }

                        /**********Ha a dátum, eredmény típus van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            //  MessageBox.Show("Dátum, eredmény, típus választottad ki!");

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' AND HeatSinkData.Result='" + eredmeny_comboBox2.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + Convert.ToString(tipus_comboBox3.Text) + "' AND HousingData.Result = '" + eredmeny_comboBox2.Text + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                        }

                        /**********Ha a művelet, eredmény típus van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            //MessageBox.Show("Művelet, eredmény, típus választottad ki!");

                            /*   string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Heatsink_ID<>'' AND HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "' AND Main.Type = '" + tipus_comboBox3.Text + "' AND HeatSinkData.Result ='"+eredmeny_comboBox2.Text+"'";
                               eredmenyek_lekerdezese(query1);

                               if (dataGridView1.RowCount <= 1)
                               {
                                   string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID<>'' AND HousingData.Operation_ID = '" + Convert.ToString(op_id) + "' AND Main.Type = '" + tipus_comboBox3.Text + "' AND HousingData.Result = '"+eredmeny_comboBox2.Text+"'";
                                   eredmenyek_lekerdezese(query2);
                    
                               }*/
                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Operation_ID = '" + op_id + "'  AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' AND Main.Type='" + tipus_comboBox3.Text + "' UNION Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Operation_ID = '" + op_id + "' AND HousingData.Result = '" + eredmeny_comboBox2.Text + "' AND Main.Type='" + tipus_comboBox3.Text + "'";
                           // eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                        }

                        /*******************************Négyes lekérdezések*************************************/

                        /**********Ha a szériaszám, dátum, művelet, eredmény van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == false)
                        {

                            // MessageBox.Show("Szériaszám, dátum, művelet eredményt választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HeatSinkData.Operation_ID = '" + op_id + "' AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HousingData.Operation_ID = '" + op_id + "' AND HeatSinkData.Result = '" + eredmeny_comboBox2 + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }



                        }
                        /**********Ha a szériaszám, dátum, művelet, típus van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == false && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Szériaszám, dátum, művelet típust választottad ki!");
                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Operation_ID = '" + op_id + "' AND Main.Type='" + tipus_comboBox3.Text + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Operation_ID = '" + op_id + "' AND Main.Type= '" + tipus_comboBox3.Text + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }
                        /**********Ha a szériaszám, dátum, eredmény, típus van kipipálva***ok******/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == false && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Szériaszám, dátum, eredmény, típust választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' AND Main.Type='" + tipus_comboBox3.Text + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Result = '" + eredmeny_comboBox2.Text + "' AND Main.Type= '" + tipus_comboBox3.Text + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }
                        /**********Ha a szériaszám, művelet, eredmény, típus van kipipálva****ok*****/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == false && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Szériaszám, művelet, eredmény, típust választottad ki!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Operation_ID = '" + op_id + "' AND Main.Type='" + tipus_comboBox3.Text + "' AND HeatSinkData.Result='" + eredmeny_comboBox2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Operation_ID = '" + op_id + "' AND Main.Type= '" + tipus_comboBox3.Text + "' AND HousingData.Result='" + eredmeny_comboBox2.Text + "'";
                                    //eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }

                        }
                        /**********Ha a dátum művelet, eredmény, típus van kipipálva*****ok****/
                        if (szeriaszam_checkBox1.Checked == false && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Dátum, művelet, eredmény, típust választottad ki!");

                            string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HeatSinkData.Result = '" + eredmeny_comboBox2.Text + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + tipus_comboBox3.Text + "' AND HeatSinkData.Result='" + eredmeny_comboBox2.Text + "'";
                            //eredmenyek_lekerdezese(query1);
                            eredmenyek_lekerdezese1(query1, dataGridView1);

                            if (dataGridView1.RowCount <= 1)
                            {
                                string query2 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Housing_ID=main.Housing_ID WHERE HousingData.Operation_ID = '" + Convert.ToString(op_id) + "' AND HousingData.Result='" + eredmeny_comboBox2.Text + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND Main.Type = '" + tipus_comboBox3.Text + "' AND HousingData.Result='" + eredmeny_comboBox2.Text + "'";
                               // eredmenyek_lekerdezese(query2);
                                eredmenyek_lekerdezese1(query1, dataGridView1);

                            }


                        }

                        /**************************ötös lekérdezés***********************************/
                        /**********Ha az összes ki van pipálva*********/
                        if (szeriaszam_checkBox1.Checked == true && datum_checkBox2.Checked == true && muvelet_checkBox3.Checked == true && eredmeny_checkBox4.Checked == true && tipus_checkBox5.Checked == true)
                        {

                            // MessageBox.Show("Összeset kiválasztottad!");

                            if (szeriaszam_textBox1.Text.Length >= 3 && szeriaszam_textBox1.Text != "" && szeriaszam_textBox1.Text.Length == 12)
                            {
                                if (Convert.ToString(szeriaszam_textBox1.Text.Substring(0, 3)) == "511")
                                {
                                    string query1 = "Select HeatSinkData.HeatSink_ID as ID, HeatSinkData.Workstation, HeatSinkData.Operation_ID, Operations.Operation_Msg, HeatSinkData.Result, HeatSinkData.Value, HeatSinkData.Limit_Min, HeatSinkData.Limit_Max, HeatSinkData.Date, HeatSinkData.Operator, HeatSinkData.Note  from [dbo].[HeatSinkData] JOIN [dbo].[Operations] ON HeatSinkData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HeatSinkData.HeatSink_ID=main.HeatSink_ID WHERE HeatSinkData.HeatSink_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HeatSinkData.Operation_ID = '" + op_id + "' AND Main.Type='" + tipus_comboBox3.Text + "' AND HeatSinkData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HeatSinkData.Result='" + eredmeny_comboBox2.Text + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);

                                }
                                else
                                {
                                    string query1 = "Select HousingData.Housing_ID as ID, HousingData.Workstation, HousingData.Operation_ID, Operations.Operation_Msg, HousingData.Result, HousingData.Value, HousingData.Limit_Min, HousingData.Limit_Max, HousingData.Date, HousingData.Operator, HousingData.Note  from [dbo].[HousingData] JOIN [dbo].[Operations] ON HousingData.Operation_ID=Operations.Operation_ID INNER JOIN [dbo].[Main] ON HousingData.Hosugin_ID=main.Housing_ID WHERE HousingData.Housing_ID = '" + Convert.ToString(szeriaszam_textBox1.Text) + "' AND HousingData.Operation_ID = '" + op_id + "' AND Main.Type= '" + tipus_comboBox3.Text + "' AND HousingData.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' AND HousingData.Result = '" + eredmeny_comboBox2 + "'";
                                   // eredmenyek_lekerdezese(query1);
                                    eredmenyek_lekerdezese1(query1, dataGridView1);
                                }
                            }
                            else { MessageBox.Show("Helytelen ID!"); }
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        //do something else
                    }


                }

                // MessageBox.Show(dataGridView1.Columns[0].Name);
            }
            catch (Exception ex)
            {

                // MessageBox.Show("Nem várt hiba a hiba oka:\n" + ex.Message);


            } // catch vége

            /****************************************Lekérdezések vége****************************************************/

            if (dataGridView1.RowCount <= 1)
            {
                darab.Text = "0";
                // pictureBox1.Visible = false;
            }
            else
            {
                darab.Text = Convert.ToString(dataGridView1.RowCount - 1);
                //  pictureBox1.Visible = false;
            }
            //  pictureBox1.Visible = false;

            if (dataGridView1.RowCount > 1 && muvelet_checkBox3.Checked == true && op_id == "10990" && szeriaszam_checkBox1.Checked == false)
            {
                hosszuteszt_label.Visible = true;
                rovidteszt_label.Visible = true;
                osszteszt_label.Visible = true;


                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    //TestType: 0
                    if (Convert.ToString(dataGridView1.Rows[i].Cells["Result"].Value) == "OK")
                    {
                        osszesokburnin = osszesokburnin + 1;
                    }

                    if (dataGridView1.Rows[i].Cells["Result"].Value.ToString() == "OK" && dataGridView1.Rows[i].Cells["Note"].Value.ToString().Contains("Type: 0")) //hosszúteszt 
                    {

                        hosszuokburin = hosszuokburin + 1;
                    }
                    if (dataGridView1.Rows[i].Cells["Result"].Value.ToString() == "OK" && dataGridView1.Rows[i].Cells["Note"].Value.ToString().Contains("Type: 1")) //rövidteszt
                    {

                        rovidokburnin = rovidokburnin + 1;
                    }
                }
                //(int)Math.Round((double)(100 * complete) / total);
                int hosszszazalek = (int)Math.Round((double)(hosszuokburin * 100) / osszesokburnin);
                int rovidszazalek = (int)Math.Round((double)(rovidokburnin * 100) / osszesokburnin);

                osszteszt_label.Text = "Összteszt:" + Convert.ToString(osszesokburnin);
                hosszuteszt_label.Text = "Hosszúteszt:" + Convert.ToString(hosszuokburin) + "-" + Convert.ToString(hosszszazalek) + "%";
                rovidteszt_label.Text = "Rövidteszt:" + Convert.ToString(rovidokburnin) + "-" + Convert.ToString(rovidszazalek) + "%";

            }
            else
            {
                hosszuteszt_label.Visible = false;
                rovidteszt_label.Visible = false;
                osszteszt_label.Visible = false;
            }

        }

        private void lekerdez2_Click(object sender, EventArgs e)
        {

            // pictureBox2.Visible = true;
            if (mainboard_textBox1.Text != "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE MainBoard_ID LIKE '%" + Convert.ToString(mainboard_textBox1.Text) + "%'";
                //termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text != "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE HeatSink_ID LIKE '%" + Convert.ToString(heatsink_textBox2.Text) + "%'";
                //termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text != "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE DSP1_ID LIKE '%" + Convert.ToString(dsp1_textBox3.Text) + "%'";
               // termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text != "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE DSP1_ID LIKE '%" + Convert.ToString(dsp1_textBox3.Text) + "%'";
               // termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text != "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE DSP2_ID LIKE '%" + Convert.ToString(dsp2_textBox4.Text) + "%'";
                //termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text != "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE Gateway_ID LIKE '%" + Convert.ToString(gateway_textBox5.Text) + "%'";
                //termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text != "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE Housing_ID LIKE '%" + Convert.ToString(housing_textBox6.Text) + "%'";
                //termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text != "" && galia_textBox8.Text == "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE Customer_ID LIKE '%" + Convert.ToString(customer_textBox7.Text) + "%'";
               // termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text != "" && type_textBox9.Text == "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE Galia_ID LIKE '%" + Convert.ToString(galia_textBox8.Text) + "%'";
               // termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text == "" && type_textBox9.Text != "" && partnumber_textBox10.Text == "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE Type = '%" + Convert.ToString(type_textBox9.Text) + "%'";
                //termek_adatok_lekerdezese(query1);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else if (mainboard_textBox1.Text == "" && heatsink_textBox2.Text == "" && dsp1_textBox3.Text == "" && dsp2_textBox4.Text == "" && gateway_textBox5.Text == "" && housing_textBox6.Text == "" && customer_textBox7.Text == "" && galia_textBox8.Text != "" && type_textBox9.Text == "" && partnumber_textBox10.Text != "")
            {
                string query1 = "Select * from [dbo].[Main] WHERE PartNumber LIKE '%" + Convert.ToString(partnumber_textBox10.Text) + "%'";
               // termek_adatok_lekerdezese(query1);
               // eredmenyek_lekerdezese1(query1, dataGridView2);
                adatok_lekerdezese(query1, dataGridView2);
            }
            else
            {
                MessageBox.Show("Egy mezőt töltsön ki!");
            }
            //  pictureBox2.Visible = false;

        }

        private void szeriaszam_checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (szeriaszam_checkBox1.Checked == true)
            {
                szeriaszam_textBox1.Enabled = true;
                szeriaszam_checkBox1.BackColor = System.Drawing.Color.Lime;


            }
            if (szeriaszam_checkBox1.Checked == false)
            {
                szeriaszam_textBox1.Enabled = false;
                szeriaszam_checkBox1.BackColor = System.Drawing.Color.White;
            }

        }

        private void datum_checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (datum_checkBox2.Checked == true)
            {
                dateTimePicker1.Enabled = true;
                dateTimePicker2.Enabled = true;
                datum_checkBox2.BackColor = System.Drawing.Color.Lime;
            }
            if (datum_checkBox2.Checked == false)
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                datum_checkBox2.BackColor = System.Drawing.Color.White;
            }

        }

        private void muvelet_checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (muvelet_checkBox3.Checked == true)
            {
                muvelet_comboBox1.Enabled = true;
                muvelet_checkBox3.BackColor = System.Drawing.Color.Lime;
                muvelet_listbox_checkbox.Enabled = false;
            }
            if (muvelet_checkBox3.Checked == false)
            {
                muvelet_comboBox1.Enabled = false;
                muvelet_checkBox3.BackColor = System.Drawing.Color.White;
                muvelet_listbox_checkbox.Enabled = true;
            }

        }

        private void eredmeny_checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (eredmeny_checkBox4.Checked == true)
            {
                eredmeny_comboBox2.Enabled = true;
                eredmeny_checkBox4.BackColor = System.Drawing.Color.Lime;
            }
            if (eredmeny_checkBox4.Checked == false)
            {
                eredmeny_comboBox2.Enabled = false;
                eredmeny_checkBox4.BackColor = System.Drawing.Color.White;
            }
        }

        private void tipus_checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (tipus_checkBox5.Checked == true)
            {
                tipus_comboBox3.Enabled = true;
                tipus_checkBox5.BackColor = System.Drawing.Color.Lime;
            }
            if (tipus_checkBox5.Checked == false)
            {
                tipus_comboBox3.Enabled = false;
                tipus_checkBox5.BackColor = System.Drawing.Color.White;
            }
        }

        
        private void adatok_lekerdezese(string lekerdezes2, DataGridView datagridview)
        {
            // pictureBox2.Visible = true;
            string query2 = lekerdezes2;

            SqlConnection cnn = new SqlConnection(connetionstringgen2);
            cnn.Open();
            SqlCommand objcmdm = new SqlCommand(query2, cnn);
            objcmdm.ExecuteNonQuery();
            SqlDataAdapter adpm = new SqlDataAdapter(objcmdm);
            DataTable dtm = new DataTable();
            adpm.Fill(dtm);
            datagridview.DataSource = dtm;
            datagridview.Refresh();
            //  pictureBox2.Visible = false;
            cnn.Close();

        }

        private void eredmenyek_lekerdezese1(string lekerdezes1, DataGridView datagridview)
        {
            //pictureBox1.Visible = true;
            try
            {
                string query1 = lekerdezes1;
                MessageBox.Show(query1);
                SqlConnection cnn = new SqlConnection(connetionstringgen2);
                cnn.Open();
                SqlCommand objcmdm = new SqlCommand(query1, cnn);
                objcmdm.ExecuteNonQuery();
                SqlDataAdapter adpm = new SqlDataAdapter(objcmdm);
                DataTable dtm = new DataTable();
                adpm.Fill(dtm);
                datagridview.DataSource = dtm;              
                datagridview.Sort(datagridview.Columns["Date"], ListSortDirection.Ascending);
                datagridview.Refresh();
               
                cnn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka, a következő:\n" + ex.Message); }

        }


        /*private void eredmenyek_lekerdezese(string lekerdezes1)
        {
            //pictureBox1.Visible = true;
            try
            {
                string query1 = lekerdezes1;

                SqlConnection cnn = new SqlConnection(connetionstringgen2);
                cnn.Open();
                SqlCommand objcmdm = new SqlCommand(query1, cnn);
                objcmdm.ExecuteNonQuery();
                SqlDataAdapter adpm = new SqlDataAdapter(objcmdm);
                DataTable dtm = new DataTable();
                adpm.Fill(dtm);
                dataGridView1.DataSource = dtm;
                this.dataGridView1.Sort(this.dataGridView1.Columns["Date"], ListSortDirection.Ascending);

                dataGridView1.Refresh();
                // pictureBox1.Visible = false;
                cnn.Close();
            }
            catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka, a következő:\n" + ex.Message); }

        }*/

        private void lekerdez3_Click(object sender, EventArgs e)
        {
            //cast(Serial_NMBR as varchar)= '" + textBox1.Text + "' ORDER BY Date_Time DESC";
            string lekerdezes3 = "Select * from [test_boundary_scan].[BMW_DSP_HEAD_temp] WHERE ";

            if (dsp_id_checkBox1.Checked == false && bscan_datum_checkBox2.Checked == false && bscan_eredmeny_checkBox3.Checked == false)
            {
                MessageBox.Show("Szűkítsd a lekérdezést");
            }

            else
            {


                if (dsp_id_checkBox1.Checked == true)
                {
                    if (bscan_dsp_ID_textBox1.Text != "")
                    {
                        lekerdezes3 += "AND Serial_NMBR = '" + bscan_dsp_ID_textBox1.Text + "'";
                    }
                    else { MessageBox.Show("Töltse ki a DSP_ID mezőt"); }

                }

                if (bscan_datum_checkBox2.Checked == true)
                {
                    lekerdezes3 += "AND Date_Time BETWEEN '" + dateTimePicker3.Text + "' AND '" + dateTimePicker4.Text + "'";

                }

                if (bscan_eredmeny_checkBox3.Checked == true)
                {

                    lekerdezes3 += "AND Result = '" + bscan_eredmeny_comboBox1.Text + "'";

                }

                lekerdezes3 += "ORDER BY Date_Time ASC";

                string replacedlekerdezes3 = lekerdezes3.Replace("WHERE AND", "WHERE");
                //   MessageBox.Show(replacedlekerdezes3);
                lekerdezes3_bscan(replacedlekerdezes3);
            }







        }
        private void lekerdezes3_bscan(string lekerdezes3)
        {

            try
            {
                SqlConnection cnn = new SqlConnection(connetionString);
                string StrQuery = lekerdezes3;

                cnn.Open();
                // StrQuery = "Select * from [test_boundary_scan].[BMW_DSP_HEAD_temp] WHERE cast(Serial_NMBR as varchar)= '" + textBox1.Text + "' ORDER BY Date_Time DESC";

                SqlCommand objcmd = new SqlCommand(StrQuery, cnn);
                objcmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(objcmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dataGridView3.DataSource = dt;
                dataGridView3.Refresh();


                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nem várt hiba a hiba oka:" + ex.Message);
            }





        }


        private void lekerdezes4_ict(string lekerdezes4)
        {

            try
            {
                SqlConnection cnn = new SqlConnection(connetionString);
                string StrQuery = lekerdezes4;

                cnn.Open();
                // StrQuery = "Select * from [test_boundary_scan].[BMW_DSP_HEAD_temp] WHERE cast(Serial_NMBR as varchar)= '" + textBox1.Text + "' ORDER BY Date_Time DESC";

                SqlCommand objcmd = new SqlCommand(StrQuery, cnn);
                objcmd.ExecuteNonQuery();
                SqlDataAdapter adp = new SqlDataAdapter(objcmd);
                DataTable dt = new DataTable();
                adp.Fill(dt);
                dataGridView4.DataSource = dt;
                dataGridView4.Refresh();


                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nem várt hiba a hiba oka:" + ex.Message);
            }

        }



        private void exportal3_Click(object sender, EventArgs e)
        {
            if (dataGridView3.RowCount > 1)
            {
                try
                {
                    System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                    saveDlg.InitialDirectory = @"C:\";
                    saveDlg.Filter = "CSV files (*.csv)|*.csv";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export csv File To";



                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string CsvFpath = saveDlg.FileName;
                        // MessageBox.Show(CsvFpath);
                        string columnHeaderText = "";
                        System.IO.StreamWriter csvFileWriter = new StreamWriter(CsvFpath, false);



                        int countColumn = dataGridView3.ColumnCount - 1;


                        if (countColumn >= 0)
                        {
                            columnHeaderText = dataGridView3.Columns[0].HeaderText;
                        }

                        for (int i = 1; i <= countColumn; i++)
                        {
                            columnHeaderText = columnHeaderText + ';' + dataGridView3.Columns[i].HeaderText;
                        }


                        csvFileWriter.WriteLine(columnHeaderText);

                        foreach (DataGridViewRow dataRowObject in dataGridView3.Rows)
                        {
                            if (!dataRowObject.IsNewRow)
                            {
                                string dataFromGrid = "";

                                dataFromGrid = dataRowObject.Cells[0].Value.ToString();

                                for (int i = 1; i <= countColumn; i++)
                                {
                                    dataFromGrid = dataFromGrid + ';' + dataRowObject.Cells[i].Value.ToString();


                                }
                                csvFileWriter.WriteLine(dataFromGrid);
                            }
                        }


                        csvFileWriter.Flush();
                        csvFileWriter.Close();

                    }
                }
                catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka: \n" + ex.Message); }
            }
            else { MessageBox.Show("Nincs adat a táblázatban!"); }
        }

        private void dsp_id_checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (dsp_id_checkBox1.Checked == true)
            {
                bscan_dsp_ID_textBox1.Enabled = true;
            }
            if (dsp_id_checkBox1.Checked == false)
            {
                bscan_dsp_ID_textBox1.Enabled = false;
            }
        }

        private void bscan_datum_checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (bscan_datum_checkBox2.Checked == true)
            {
                dateTimePicker3.Enabled = true;
                dateTimePicker4.Enabled = true;

            }

            if (bscan_datum_checkBox2.Checked == false)
            {
                dateTimePicker3.Enabled = false;
                dateTimePicker4.Enabled = false;

            }
        }

        private void bscan_eredmeny_checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (bscan_eredmeny_checkBox3.Checked == true)
            {
                bscan_eredmeny_comboBox1.Enabled = true;
            }
            if (bscan_eredmeny_checkBox3.Checked == false)
            {
                bscan_eredmeny_comboBox1.Enabled = false;
            }
        }

        private void ict_lekerdez_Click(object sender, EventArgs e)
        {

            if (gw_radioButton1.Checked == true)
            {

                string query_gw = "SELECT * FROM test_ict.BMW_GW_HEAD_temp WHERE Serial_NMBR = '" + ict_id_textbox + "' ORDER BY Date_Time DESC";
                lekerdezes4_ict(query_gw);


            }
            if (mb_radioButton2.Checked == true)
            {
                string query_mb = "SELECT * FROM test_ict.BMW_MB_HEAD_temp WHERE Serial_NMBR = '" + ict_id_textbox + "' ORDER BY Date_Time DESC";
                lekerdezes4_ict(query_mb);
            }
        }

        private void ict_exportal_Click(object sender, EventArgs e)
        {
            if (dataGridView4.RowCount > 1)
            {
                try
                {
                    System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                    saveDlg.InitialDirectory = @"C:\";
                    saveDlg.Filter = "CSV files (*.csv)|*.csv";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export csv File To";



                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string CsvFpath = saveDlg.FileName;
                        // MessageBox.Show(CsvFpath);
                        string columnHeaderText = "";
                        System.IO.StreamWriter csvFileWriter = new StreamWriter(CsvFpath, false);



                        int countColumn = dataGridView4.ColumnCount - 1;


                        if (countColumn >= 0)
                        {
                            columnHeaderText = dataGridView4.Columns[0].HeaderText;
                        }

                        for (int i = 1; i <= countColumn; i++)
                        {
                            columnHeaderText = columnHeaderText + ';' + dataGridView4.Columns[i].HeaderText;
                        }


                        csvFileWriter.WriteLine(columnHeaderText);

                        foreach (DataGridViewRow dataRowObject in dataGridView3.Rows)
                        {
                            if (!dataRowObject.IsNewRow)
                            {
                                string dataFromGrid = "";

                                dataFromGrid = dataRowObject.Cells[0].Value.ToString();

                                for (int i = 1; i <= countColumn; i++)
                                {

                                    dataFromGrid = dataFromGrid + ';' + dataRowObject.Cells[i].Value.ToString();


                                }
                                csvFileWriter.WriteLine(dataFromGrid);
                            }
                        }


                        csvFileWriter.Flush();
                        csvFileWriter.Close();

                    }
                }
                catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka: \n" + ex.Message); }
            }
            else { MessageBox.Show("Nincs adat a táblázatban!"); }
        }

        private void takaya_lekerdez_Click(object sender, EventArgs e)
        {
            if (takaya_id_textbox.Text != "")
            {
                SqlConnection cnnm = new SqlConnection(connetionString);

                cnnm.Open();
                string StrQuery1m = "Select * from [dbo].[takayaresults] WHERE Serial_NMBR LIKE '%" + takaya_id_textbox.Text + "%' AND Side LIKE 'TOP' ORDER BY Date_Time DESC";

                SqlCommand objcmdm = new SqlCommand(StrQuery1m, cnnm);
                objcmdm.ExecuteNonQuery();
                SqlDataAdapter adpm = new SqlDataAdapter(objcmdm);
                DataTable dtm = new DataTable();
                adpm.Fill(dtm);
                dataGridView5.DataSource = dtm;
                dataGridView5.Refresh();

                string StrQuery1m1 = "Select * from [dbo].[takayaresults] WHERE Serial_NMBR LIKE '%" + takaya_id_textbox.Text + "%' AND Side LIKE 'BOT' ORDER BY Date_Time DESC";

                SqlCommand objcmdm1 = new SqlCommand(StrQuery1m1, cnnm);
                objcmdm1.ExecuteNonQuery();
                SqlDataAdapter adpm1 = new SqlDataAdapter(objcmdm1);
                DataTable dtm1 = new DataTable();
                adpm1.Fill(dtm1);
                dataGridView6.DataSource = dtm1;
                dataGridView6.Refresh();

                cnnm.Close();


            }


        }

        private void fw_lekerdez_button_Click(object sender, EventArgs e)
        {
            firewall_value_label.Text = "?";
            fw_housing_id_label.Text = "?";
            partnumber_label.Text = "?";
            firewall_swname_label2.Text = "?";
            Thread.Sleep(200);
            //T581618300050
            try
            {

                if (fw_textbox.Text != "" && fw_textbox.Text.Length == 13)
                {
                    SqlConnection cnnm = new SqlConnection(connetionstringgen2);
                    cnnm.Open();

                    string seged_lekerdezes = "Select * from [dbo].[Main] WHERE Customer_ID = '" + fw_textbox.Text + "' ";

                    adatok_lekerdezese(seged_lekerdezes, seged_datagridview);
                               
                    string housingid = "";

                    //V29057660-5298
                    string partnumber = "";
                    string subpartnumber = "";
                    if (seged_datagridview.RowCount > 1)
                    {

                        housingid = Convert.ToString(seged_datagridview.Rows[0].Cells["Housing_ID"].Value);
                        partnumber = Convert.ToString(seged_datagridview.Rows[0].Cells["PartNumber"].Value);
                        subpartnumber = partnumber.Substring(12, 2);
                        
                    }
                    else { MessageBox.Show("Nincs ilyen adat az adatbázisban"); }

                   /*******************************************50445 állomás lekérdezése*****************************************/
                    string fw_lekerdezes = "SELECT * from [dbo].[TesterData] WHERE Housing_ID='" + housingid + "' AND Operation_ID='50445' ORDER BY Date DESC";
                    adatok_lekerdezese(fw_lekerdezes, firewall_datagridview);                    
                    fw_housing_id_label.Text = housingid;
                    if (firewall_datagridview.Rows[0].Cells["Value"].Value != null)
                    {
                        firewall_swname_label.Text = Convert.ToString(firewall_datagridview.Rows[0].Cells["Value"].Value);
                    }
                    else
                    {
                        MessageBox.Show("Az 50445 állomás értéke hiányzik!");
                    }
                    /************************************************************************************/

                    /*******************************************50446 állomás lekérdezése*****************************************/
                    string fw_lekerdezes2 = "SELECT * from [dbo].[TesterData] WHERE Housing_ID='" + housingid + "' AND Operation_ID='50446' ORDER BY Date DESC";
                    adatok_lekerdezese(fw_lekerdezes, firewall_datagridview);
                    //fw_housing_id_label.Text = housingid;
                    if (firewall_datagridview.Rows[0].Cells["Value"].Value != null)
                    {
                        firewall_swname_label2.Text = Convert.ToString(firewall_datagridview.Rows[0].Cells["Value"].Value);
                    }
                    else
                    {
                        MessageBox.Show("Az 50446 állomás értéke hiányzik!");
                    }
                    /************************************************************************************/



                    /*if (firewall_swname_label.Text == "31473703 AA" && subpartnumber == "99")
                    {
                        firewall_value_label.BackColor = System.Drawing.Color.Red;
                        firewall_value_label.Text = "L1";
                        partnumber_label.Text = subpartnumber;
                        L1warning MSLform = new L1warning();
                        MSLform.Show();
                        MSLform.Activate();
                    }*/
                   /* else if (firewall_swname_label.Text == "31473703 AA" && subpartnumber != "99")
                    {
                        firewall_value_label.BackColor = System.Drawing.Color.Red;
                        firewall_value_label.Text = "L1";
                        partnumber_label.Text = subpartnumber;
                        MessageBox.Show("Hiba!\n Nem L1 típusú házba szerelték az L1-es szoftverrel rendelkező MainBoard-t!\n (Hibás PartNumber)");

                    }*/
                    if (firewall_swname_label.Text == "31450232AB" && subpartnumber == "98" && firewall_swname_label2.Text == "31394566AE")
                    {
                        firewall_value_label.BackColor = System.Drawing.Color.Yellow;
                        firewall_value_label.Text = "L2";
                        partnumber_label.Text = subpartnumber;

                    }
                  /*  else if (firewall_swname_label.Text == "31491859 AA" && subpartnumber != "98")
                    {
                        firewall_value_label.BackColor = System.Drawing.Color.Yellow;
                        firewall_value_label.Text = "L2";
                        partnumber_label.Text = subpartnumber;

                        MessageBox.Show("Hiba!\n Nem L2 típusú házba szerelték az L2-es szoftverrel rendelkező MainBoard-t!\n (Hibás PartNumber)");
                    }*/

                    else if (firewall_swname_label.Text == "31491552AA" /*&& subpartnumber == "99"*/ && firewall_swname_label2.Text == "31491553AA")
                    {

                        firewall_value_label.BackColor = System.Drawing.Color.Red;
                        firewall_value_label.Text = "516H";
                        partnumber_label.Text = subpartnumber;
                        MessageBox.Show("Hiba!\n Régi típusú 516H!!!");
                    
                    }
                    else if (firewall_swname_label.Text == "31450232AB" && subpartnumber == "99" && firewall_swname_label2.Text == "31491553AA")
                    {

                        firewall_value_label.BackColor = System.Drawing.Color.Yellow;
                        firewall_value_label.Text = "516HV2";
                        partnumber_label.Text = subpartnumber;
                     
                    }
                   /* else if (firewall_swname_label.Text == "31491859 AA" && subpartnumber != "99")
                    {
                        firewall_value_label.BackColor = System.Drawing.Color.Yellow;
                        firewall_value_label.Text = "516H";
                       // subpartnumber.FontColor=Color.Red
                        partnumber_label.Text = subpartnumber;

                        MessageBox.Show("Hiba!\n Nem 516H típusú házba szerelték az 516H-s szoftverrel rendelkező MainBoard-t!\n (Hibás PartNumber)");
                    
                    }*/
                    else
                    {
                        MessageBox.Show("Valamelyik adat nem stimmel, kérem értesítsen egy mérnököt!");
                        firewall_value_label.BackColor = System.Drawing.Color.Red;
                        firewall_value_label.Text = "Ismeretlen";
                        // firewall_value_label.Text = "?";
                        partnumber_label.Text = "?";


                    }

                    /******SMT adatbázisba logolunk******/

                    /*/* SqlConnection cnn2 = new SqlConnection(connetionString);
                     // Sqlconnection cnn = new Sqlconnection();
                     cnn2.Open();
                     string addDB = "INSERT INTO [dbo].[Gen2FWCheck] ([Housing_ID], [Customer_ID], [Scan_Date], [SW_Value], [SW_Name]) VALUES ('" + Convert.ToString(housingid) + "', '" + Convert.ToString(fw_textbox.Text) + "','" + DateTime.Now.ToString("s") + "','" + Convert.ToString(firewall_swname_label.Text) + "','" + Convert.ToString(firewall_value_label.Text) + "')";
                     SqlCommand createcommand = new SqlCommand(addDB, cnn2);
                     createcommand.ExecuteNonQuery();*/

                    /************************************/

                    /*********GEN2 adatbázisba logolunk***********/
                    SqlConnection cnn3 = new SqlConnection(connetionstringgen2);
                    // Sqlconnection cnn = new Sqlconnection();
                    cnn3.Open();
                    string addDB3 = "INSERT INTO [dbo].[Gen2FWCheck] ([Housing_ID], [Customer_ID], [Scan_Date], [SW_Value], [SW_Name]) VALUES ('" + Convert.ToString(housingid) + "', '" + Convert.ToString(fw_textbox.Text) + "','" + DateTime.Now.ToString("s") + "','" + Convert.ToString(firewall_swname_label.Text) + "','" + Convert.ToString(firewall_value_label.Text) + "')";
                    SqlCommand createcommand3 = new SqlCommand(addDB3, cnn3);
                    createcommand3.ExecuteNonQuery();
                    /*************************************/


                    fw_textbox.Select();
                    fw_textbox.SelectAll();
                    precustomer_label.Text = fw_textbox.Text;


                   // cnn2.Close();
                    cnnm.Close();
                }
             
                else
                {
                    try
                    {

                        firewall_datagridview.Rows.Clear();
                        firewall_datagridview.Columns.Clear();
                        firewall_datagridview.Refresh();

                    }
                    catch { }



                    MessageBox.Show("Nem megfelelő Customer_ID");
                    fw_textbox.Select();
                    fw_textbox.SelectAll();
                    precustomer_label.Text = fw_textbox.Text;
                    firewall_value_label.Text = "?";
                    partnumber_label.Text = "?";
                    fw_housing_id_label.Text = "?";
                    firewall_swname_label.Text = "?";


                }

            }


            catch (Exception ex) { MessageBox.Show("Nem várt hiba, a hiba oka: \n" + ex.Message); }


        }

        private void muvelet_listbox_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (muvelet_listbox_checkbox.Checked == true)
            {

                muvelet_listbox.Enabled = true;
                muvelet_listbox_checkbox.BackColor = System.Drawing.Color.Lime;
                muvelet_checkBox3.Enabled = false;


            }
            if (muvelet_listbox_checkbox.Checked == false)
            {

                muvelet_listbox.Enabled = false;
                muvelet_listbox_checkbox.BackColor = System.Drawing.Color.White;
                muvelet_checkBox3.Enabled = true;

            }
        }

        private void fpy_button_Click(object sender, EventArgs e)
        {
          

            this.dataGridView1.Sort(this.dataGridView1.Columns["Date"], ListSortDirection.Ascending);

           

          /*
            string fileName = @"C:\temp\FPY16.txt";
            FileStream fs = new FileStream(fileName, FileMode.Create);
            // Create the writer for data.
            // BinaryWriter w = new BinaryWriter(fs);
            StreamWriter w = new StreamWriter(fs);

            w.Write(szoveg);
            w.Close();
            fs.Close();
           */


            try
            {

                fpy_datagridview.Rows.Clear();
                fpy_datagridview.Refresh();

            }
            catch { }

            if (dataGridView1.RowCount > 1)
            {               
             //   file.WriteLine(lines);
                if (muvelet_checkBox3.Checked == true)
                {                  
                    /**********************************************/
                    string operation = muvelet_comboBox1.Text.ToString();
                    char[] separator = { '-',' ' };
                    string[] word = operation.Split(separator);
                    string  op_id1 = word[0];

                    // MessageBox.Show(op_id1);

                        ismetlodeskiszurese();
/************************************************ez a sima egyszeres lekérdezésre*****************************************************************/


                        fpyellenorzese(op_id1);
                        /*for (int i = 0; i < fpy_datagridview.RowCount - 1; i++)
                        {
                            if (fpy_datagridview.Rows[i].Cells["Result"].Value.ToString() == "OK")
                            {
                                indokcount++;
                            }
                            else if (fpy_datagridview.Rows[i].Cells["Result"].Value.ToString() == "NOK")
                            {
                                indnokcount++;
                            }
                        }


                        sum = indokcount + indnokcount;              
                        indtlr = (double)Math.Round(((double)(indnokcount * 100) / sum), 2);
                        indfpy = 100 - indtlr;
                     
                        MessageBox.Show("Összes teszt (újratesztel együtt): " + Convert.ToString(darab.Text) + "\nLetesztelt termékek száma: " + Convert.ToString(sum) + "\nNOK termékek száma: " + Convert.ToString(indnokcount) + "\nOk termékek száma: " + Convert.ToString(indokcount) + "\nFPY: " + Convert.ToString(indfpy) + "%\nTLR: " + Convert.ToString(indtlr) + "%");

                        string fileName = @"C:\temp\FPY16.txt";
                        FileStream fs = new FileStream(fileName, FileMode.Create);
                        // Create the writer for data.
                        // BinaryWriter w = new BinaryWriter(fs);
                        StreamWriter w = new StreamWriter(fs);
                        string szoveg =
                          "Összes teszt (újratesztel együtt):" + Convert.ToString(darab.Text) +"\r\n"
                          + "Letesztelt termékek száma: " + Convert.ToString(sum) + "\r\n"
                          + "NOK termékek száma: " + Convert.ToString(indnokcount) + "\r\n"
                          + "Ok termékek száma: " + Convert.ToString(indokcount) + "\r\n"
                          + "FPY: " + Convert.ToString(indfpy) + "%\r\n"
                          + "TLR: " + Convert.ToString(indtlr) + "%\r\n";


                        w.Write(szoveg);
                        w.Close();
                        fs.Close();*/

                  

  /************************************************************************************************************************************************/
                                      
                }//ha ki van jelölve a checkbox

                if (datum_checkBox2.Checked == true && szeriaszam_checkBox1.Checked == false && muvelet_checkBox3.Checked == false)
                {

                  //  MessageBox.Show("Ok");

                    ismetlodeskiszurese();      
                    fpyellenorzese("11000");
                    fpyellenorzese("10990");
                    fpyellenorzese("10985");
                    fpyellenorzese("10980");
                    fpyellenorzese("10615");
                    fpyellenorzese("10613");
                    fpyellenorzese("10606");

                }               
            
            }
            else
            {
                MessageBox.Show("Nincs feldolgozható adat!\nKérem előbb kérdezze le az adatokat!");
            }

        }
        private void ismetlodeskiszurese()
        {
            try
            {
                //System.IO.StreamWriter file = new System.IO.StreamWriter("c:\\temp\\fpy.txt");
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {

                    string cell1 = dataGridView1.Rows[i].Cells["ID"].Value.ToString();
                    string cell2 = dataGridView1.Rows[i].Cells["Operation_Msg"].Value.ToString();

                    if (find(cell1, cell2))
                    {
                        //MessageBox.Show("talalat");
                    }
                    else
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        row = (DataGridViewRow)dataGridView1.Rows[i].Clone();
                        int intColIndex = 0;
                        foreach (DataGridViewCell cell in dataGridView1.Rows[i].Cells)
                        {

                            row.Cells[intColIndex].Value = cell.Value;
                            intColIndex++;

                        }
                        fpy_datagridview.Rows.Add(row);

                    }

                }

            } // try blokk vége
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        
        
        
        }

        private void fpyellenorzese(string muvelet)
        {
            int sum = 0;
            double indtlr = 0;
            double indfpy = 0;
            int indokcount = 0;
            int indnokcount = 0;
            int osszdarab = 0;
            try
            {

                string root = @"C:\temp";

                System.IO.FileInfo file = new System.IO.FileInfo(root);
                file.Directory.Create();

                if (!System.IO.Directory.Exists(root))
                {
                    System.IO.Directory.CreateDirectory(root);
                }

                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells["Operation_ID"].Value.ToString() == muvelet)
                    {
                        osszdarab++;
                    }
                
                }

                    for (int i = 0; i < fpy_datagridview.RowCount - 1; i++)
                    {
                        if (fpy_datagridview.Rows[i].Cells["Operation_ID"].Value.ToString() == muvelet)
                        {
                            // osszdarab++;
                            if (fpy_datagridview.Rows[i].Cells["Result"].Value.ToString() == "OK")
                            {
                                indokcount++;
                            }
                            else if (fpy_datagridview.Rows[i].Cells["Result"].Value.ToString() == "NOK")
                            {
                                indnokcount++;
                            }
                        }
                    }


                sum = indokcount + indnokcount;
                indtlr = (double)Math.Round(((double)(indnokcount * 100) / sum), 2);
                indfpy = 100 - indtlr;

                MessageBox.Show("Operation_ID:" + Convert.ToString(muvelet) + "\nÖsszes teszt (újratesztel együtt): " + Convert.ToString(osszdarab) + "\nLetesztelt termékek száma: " + Convert.ToString(sum) + "\nNOK termékek száma: " + Convert.ToString(indnokcount) + "\nOk termékek száma: " + Convert.ToString(indokcount) + "\nFPY: " + Convert.ToString(indfpy) + "%\nTLR: " + Convert.ToString(indtlr) + "%");

                string fileName = @"C:\temp\FPY.txt";
                FileStream fs = new FileStream(fileName, FileMode.Append);
                // Create the writer for data.
                // BinaryWriter w = new BinaryWriter(fs);
                StreamWriter w = new StreamWriter(fs);
                string szoveg =
                 "" + Convert.ToString(dateTimePicker1.Text) + " -- " + Convert.ToString(dateTimePicker2.Text) + "\r\n"
                  + "Operation_ID:" + Convert.ToString(muvelet) + "\r\n"
                  + "Összes teszt (újratesztel együtt):" + Convert.ToString(osszdarab) + "\r\n"
                  + "Letesztelt termékek száma: " + Convert.ToString(sum) + "\r\n"
                  + "NOK termékek száma: " + Convert.ToString(indnokcount) + "\r\n"
                  + "Ok termékek száma: " + Convert.ToString(indokcount) + "\r\n"
                  + "FPY: " + Convert.ToString(indfpy) + "%\r\n"
                  + "TLR: " + Convert.ToString(indtlr) + "%\r\n"
                  + "\r\n";


                w.Write(szoveg);
                w.Close();
                fs.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        
        
        }

        private bool find(string cell1, string cell2)
        {
            bool eredmeny = false;
            try
            {
                if (fpy_datagridview.RowCount > 1)
                {
                    for (int i = 0; i < fpy_datagridview.RowCount - 1; i++)
                    {
                        if ((fpy_datagridview.Rows[i].Cells["ID"].Value.ToString() == cell1) && (fpy_datagridview.Rows[i].Cells["Operation_Msg"].Value.ToString() == cell2))
                        {
                            eredmeny = true;
                            break;
                        }
                        else
                        {
                            eredmeny = false;
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }

            return eredmeny;


        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (fpy_datagridview.RowCount > 1)
            {
                try
                {
                    System.Windows.Forms.SaveFileDialog saveDlg = new System.Windows.Forms.SaveFileDialog();
                    saveDlg.InitialDirectory = @"C:\";
                    saveDlg.Filter = "CSV files (*.csv)|*.csv";
                    saveDlg.FilterIndex = 0;
                    saveDlg.RestoreDirectory = true;
                    saveDlg.Title = "Export csv File To";

                    if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string CsvFpath = saveDlg.FileName;
                        // MessageBox.Show(CsvFpath);
                        string columnHeaderText = "";
                        System.IO.StreamWriter csvFileWriter = new StreamWriter(CsvFpath, false);



                        int countColumn = fpy_datagridview.ColumnCount - 1;


                        if (countColumn >= 0)
                        {
                            columnHeaderText = fpy_datagridview.Columns[0].HeaderText;
                        }

                        for (int i = 1; i <= countColumn; i++)
                        {
                            columnHeaderText = columnHeaderText + ';' + fpy_datagridview.Columns[i].HeaderText;
                        }


                        csvFileWriter.WriteLine(columnHeaderText);

                        foreach (DataGridViewRow dataRowObject in fpy_datagridview.Rows)
                        {
                            if (!dataRowObject.IsNewRow)
                            {
                                string dataFromGrid = "";

                                dataFromGrid = dataRowObject.Cells[0].Value.ToString();

                                for (int i = 1; i <= countColumn; i++)
                                {
                                    dataFromGrid = dataFromGrid + ';' + dataRowObject.Cells[i].Value.ToString();
                                }
                                csvFileWriter.WriteLine(dataFromGrid);
                            }
                        }

                        csvFileWriter.Flush();
                        csvFileWriter.Close();

                    }
                }
                catch (Exception ex) { MessageBox.Show("Nem várt hiba a hiba oka: \n" + ex.Message); }
            }
            else { MessageBox.Show("Nincs adat a táblázatban!"); }
        }

        private void bmw_fw_button_Click(object sender, EventArgs e)
        {
            DataTable house_id_note = new DataTable();
            DataTable house_id_acoustic = new DataTable();

            

            if (globalusername != "")
            {

            if (bmw_fw_textbox.Text != "" && bmw_fw_textbox.Text.Length == 31 && subcustomerid_textbox.Text!="" && bmwfw_housingid_textbox.Text!="")
                {                

                    bmwazonositas_label.BackColor = System.Drawing.Color.White;
                    bmwazonositas_label.Text = "?";
                    bmwtipus_label.Text = "?";
                    string[] splitted_HousingID = bmwfw_housingid_textbox.Text.Split('*'); /////////////////////////////////////csillag karakterrel delimitálom
                    string beolvasott_housingid = splitted_HousingID[1]; ////////////////operátor által beolvasott housind id kiszedve a csillag utáni rész
                    string number1 = "";
                    string number2 = "";
                    string subnumber2 = "";
                  
                    string type = "";
                    string type_group = "";
                    string sw = "";
                    string subsw = "";
                    string acoustic_result = "";
                    string canframe_result = "";
                   
                    Thread.Sleep(200);
                    
                    try 
                    {

                        if (bmw_fw_textbox.Text.Substring(11,12)==subcustomerid_textbox.Text) ///////////////////////////////////////////////////////////////Customerid vizsgálat
                        {

                        string seged_lekerdezes = "Select * from [dbo].[Main] WHERE Customer_ID = '" + bmw_fw_textbox.Text + "'";
                        adatok_lekerdezese(seged_lekerdezes, seged_datagridview);
                                       
                        string housingid = "";
                        string partnumber = "";
                        string subpartnumber = "";

                        if (seged_datagridview.RowCount > 1)
                        {
                        
                        housingid = Convert.ToString(seged_datagridview.Rows[0].Cells["Housing_ID"].Value);
                        partnumber = Convert.ToString(seged_datagridview.Rows[0].Cells["PartNumber"].Value);
                        subpartnumber = partnumber.Substring(12, 2);
                        string bmw_tipus = bmw_fw_textbox.Text.Substring(2, 9);

                        if (housingid == beolvasott_housingid) ////////////////////////////////////////////megnézem hogy a beolvasott és a rögzített housing id azonos-e
                        {
                            DataTable isfirewallcontains = new DataTable();
                            readtodatatable("SELECT * from [dbo].[BMW_Firewall_Check] WHERE customer_id='" + bmw_fw_textbox.Text + "' and housing_id <> '"+beolvasott_housingid+"'", isfirewallcontains);

                            if (isfirewallcontains.Rows.Count == 0) /////////////////////////megvizsgálom hogy más housingid-val van-e adat rögzítve ha nincs a sorok száma 0
                            {
                                string azonosito_lekerdezes = "SELECT * from [dbo].[product_identify] WHERE subnumber='" + bmw_tipus + "'";

                                adatok_lekerdezese(azonosito_lekerdezes, seged_datagridview);

                                if (seged_datagridview.RowCount > 1)
                                {

                                    type = Convert.ToString(seged_datagridview.Rows[0].Cells["type"].Value);
                                    type_group = Convert.ToString(seged_datagridview.Rows[0].Cells["type_group"].Value);
                                    sw = Convert.ToString(seged_datagridview.Rows[0].Cells["sw"].Value);
                                    subsw = Convert.ToString(seged_datagridview.Rows[0].Cells["sub_sw"].Value);

                                }


                                string fw_lekerdezes = "SELECT * from [dbo].[TesterData] WHERE Housing_ID='" + housingid + "' AND Operation_ID='150560' ORDER BY Date DESC";
                                adatok_lekerdezese(fw_lekerdezes, bmw_fw_datagridview);

                                number1 = Convert.ToString(bmw_fw_datagridview.Rows[0].Cells["Value"].Value);


                                /************************canfram**********************/
                                string fw_lekerdezes1 = "SELECT Result from [dbo].[Burnin_Time_Check] WHERE Housing_ID='" + housingid + "' AND Step='2' ORDER BY Date DESC";
                                adatok_lekerdezese(fw_lekerdezes1, bmw_fw_datagridview);

                                if (bmw_fw_datagridview.Rows.Count > 1)
                                {
                                    canframe_result = Convert.ToString(bmw_fw_datagridview.Rows[0].Cells["Result"].Value);
                                }
                                else
                                {
                                    canframe_result = "NOTEST";
                                }

                                /*********************************************/

                                string fw_lekerdezes2 = "SELECT * from [dbo].[TesterData] WHERE Housing_ID='" + housingid + "' AND Operation_ID='150570' ORDER BY Date DESC";
                                adatok_lekerdezese(fw_lekerdezes2, bmw_fw_datagridview);


                                number2 = Convert.ToString(bmw_fw_datagridview.Rows[0].Cells["Value"].Value);

                                subnumber2 = number2.Replace(".", "");

                                int dayoftheyear = Convert.ToUInt16(bmw_fw_textbox.Text.Substring(14, 3));

                                int year = Convert.ToUInt16("20"+bmw_fw_textbox.Text.Substring(11, 2));
                                        DateTime theDate = new DateTime(year, 1, 1).AddDays(dayoftheyear - 1);
                                string datum = theDate.ToString("yy.MM.dd");

                                // MessageBox.Show(housingid);
                                readtodatatable("Select Note from HousingData WHERE Housing_ID='" + housingid + "' and Operation_ID='10401' order by Date DESC", house_id_note);
                                // eredmenyek_lekerdezese1("Select Note from HousingData WHERE Housing_ID='" + housingid + "' and Operation_ID='10401' order by Date DESC", dataGridView8);
                                if (house_id_note.Rows.Count > 0)
                                {
                                    string acoustic_house_id = house_id_note.Rows[0]["Note"].ToString();
                                    readtodatatable("Select House_ID, Result from Acoustic_Test_Results WHERE House_ID='" + acoustic_house_id + "' order by Date desc", house_id_acoustic);
                                    adatok_lekerdezese("Select House_ID, Result from Acoustic_Test_Results WHERE House_ID='" + acoustic_house_id + "' order by Date desc", dataGridView8);


                                }
                                else
                                {
                                    messagebox("Nincs a gen2 adatbázisban adat!");
                                }


                                if (house_id_acoustic.Rows.Count > 0)
                                {
                                    acoustic_result = house_id_acoustic.Rows[0]["Result"].ToString();
                                }


                                acoustic_result = acoustic_result.Replace(" ", "");
                                //MessageBox.Show(acoustic_result.Length.ToString());
                                if (acoustic_result == "OK")
                                {
                                    if (canframe_result.Substring(0, 2) == "OK")
                                    {
                                        if ((type != "" && type_group != "") && type_group == "F30")
                                        {
                                            // if (acoustic_result == "OK") // insidix vizsgálat
                                            // {
                                            if (datum == number2) //szériaszám és adatbázisból kiolvasott dátum egyezik-e
                                            {
                                                if (bmw_fw_textbox.Text.Substring(19, 4) == number1.Substring(6, 4)) // szériaszámban logolt szám és az adatbázisban logolt szám egyezik e
                                                {

                                                    if (bmw_fw_textbox.Text.Substring(23, 8) == "16129510") //a szériaszám vége mindig ugyan az
                                                    {

                                                        if (bmw_fw_textbox.Text.Substring(0, 2) == "1X") //szériaszám eleje mindig ugyan az
                                                        {

                                                                   
                                                            if (subnumber2 == number1.Substring(0, subnumber2.Length)) //adatbázisban a két dátum ugyan az-e
                                                            {
                                                                bmwtipus_label.Text = type + "-" + type_group + "-" + sw + "-" + subsw;
                                                                bmwazonositas_label.BackColor = System.Drawing.Color.Lime;
                                                                bmwazonositas_label.Text = "OK";

                                                            }
                                                            else
                                                            {

                                                                bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                                bmwazonositas_label.Text = "NOK";
                                                                datumwarning datumwarning1 = new datumwarning();
                                                                datumwarning1.Show();
                                                                // MessageBox.Show("Az adatbázisban szereplő két dátum nem egyezik!\n" + subnumber2 + "\n" + number1.Substring(0, subnumber2.Length));

                                                            }

                                                        }
                                                        else
                                                        {
                                                            bmwazonositas_label.BackColor = System.Drawing.Color.Red;

                                                            bmwazonositas_label.Text = "NOK";
                                                            MessageBox.Show("Nem stimmel a szériaszám eleje (1X)");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                        bmwazonositas_label.Text = "NOK";
                                                        MessageBox.Show("Nem stimmel a szériaszám vége (16129510)");
                                                    }
                                                }
                                                else
                                                {

                                                    bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                    bmwazonositas_label.Text = "NOK";
                                                    MessageBox.Show("Nem stimmel a szériaszám és az adatbázisban logolt adat:\n" + bmw_fw_textbox.Text.Substring(19, 4) + "\n" + number1.Substring(6, 4));

                                                }
                                            }//if datum==number2
                                            else
                                            {
                                                       MessageBox.Show(datum + "\n" + number2);
                                                        bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                bmwazonositas_label.Text = "NOK";
                                                datumwarning datumwarning1 = new datumwarning();
                                                datumwarning1.Show();

                                            }
                                            SqlConnection cnn3 = new SqlConnection(connetionstringgen2);
                                            // Sqlconnection cnn = new Sqlconnection();
                                            cnn3.Open();

                                            string addDB3 = "INSERT INTO [dbo].[BMW_Firewall_Check] ([customer_id], [housing_id], [subnumber], [result], [dayofyear], [date1], [date2], [scandate], [type], [type_group], [sw], [subsw], [username]) VALUES ('" + Convert.ToString(bmw_fw_textbox.Text) + "', '" + Convert.ToString(housingid) + "','" + Convert.ToString(bmw_tipus) + "','" + Convert.ToString(bmwazonositas_label.Text) + "','" + Convert.ToString(dayoftheyear) + "','" + Convert.ToString(subnumber2) + "','" + Convert.ToString(number1.Substring(0, subnumber2.Length)) + "','" + DateTime.Now.ToString("s") + "','" + Convert.ToString(type) + "','" + Convert.ToString(type_group) + "','" + Convert.ToString(sw) + "','" + Convert.ToString(subsw) + "','" + Convert.ToString(globalusername) + "')";
                                            SqlCommand createcommand3 = new SqlCommand(addDB3, cnn3);
                                            createcommand3.ExecuteNonQuery();
                                        }//if f30 vége

                                        else if ((type != "" && type_group != "") && type_group == "35UP")
                                        {
                                            string bracket_eredmeny = "";
                                            string bracket_lekerdezes = "SELECT * from [dbo].[HousingData] WHERE Housing_ID='" + housingid + "' AND Operation_ID = '11015' ORDER BY Date DESC ";

                                            adatok_lekerdezese(bracket_lekerdezes, seged_datagridview);

                                            bracket_eredmeny = Convert.ToString(seged_datagridview.Rows[0].Cells["Result"].Value);

                                            if (acoustic_result == "OK")
                                            {
                                                if (datum == number2) //szériaszám és adatbázisból kiolvasott dátum egyezik-e
                                                {
                                                    if (bmw_fw_textbox.Text.Substring(19, 4) == number1.Substring(6, 4)) // szériaszámban logolt szám és az adatbázisban logolt szám egyezik e
                                                    {

                                                        if (bmw_fw_textbox.Text.Substring(23, 8) == "16129510") //a szériaszám vége mindig ugyan az
                                                        {

                                                            if (bmw_fw_textbox.Text.Substring(0, 2) == "1X") //szériaszám eleje mindig ugyan az
                                                            {
                                                                if (bracket_eredmeny == "OK")
                                                                {
                                                                    if (subnumber2 == number1.Substring(0, subnumber2.Length)) //adatbázisban a két dátum ugyan az-e
                                                                    {
                                                                        bmwtipus_label.Text = type + "-" + type_group + "-" + sw + "-" + subsw;
                                                                        bmwazonositas_label.BackColor = System.Drawing.Color.Lime;
                                                                        bmwazonositas_label.Text = "OK";
                                                                    }
                                                                    else
                                                                    {
                                                                        bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                                        bmwazonositas_label.Text = "NOK";
                                                                        datumwarning datumwarning1 = new datumwarning();
                                                                        datumwarning1.Show();
                                                                        // MessageBox.Show("Az adatbázisban szereplő két dátum nem egyezik!\n" + subnumber2 + "\n" + number1.Substring(0, subnumber2.Length));
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    MessageBox.Show("A Bracket eredmény NOK vagy nincs eredmény!");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                                bmwazonositas_label.Text = "NOK";
                                                                MessageBox.Show("Nem stimmel a szériaszám eleje (1X)");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                            bmwazonositas_label.Text = "NOK";
                                                            MessageBox.Show("Nem stimmel a szériaszám vége (16129510)");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                        bmwazonositas_label.Text = "NOK";
                                                        MessageBox.Show("Nem stimmel a szériaszám és az adatbázisban logolt adat:\n" + bmw_fw_textbox.Text.Substring(19, 4) + "\n" + number1.Substring(6, 4));
                                                    }
                                                }
                                                else
                                                {
                                                    bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                                    bmwazonositas_label.Text = "NOK";
                                                    datumwarning datumwarning1 = new datumwarning();
                                                    datumwarning1.Show();

                                                    //  MessageBox.Show("Nem stimmel a szériaszámban feltüntetett dayofyear:\n" + bmw_fw_textbox.Text.Substring(14, 3) + "\nLogolt dátum:" + number2);
                                                }
                                            }
                                            else if (acoustic_result == "NOK")
                                            {
                                                messagebox("Az akoustik mikroszkóp eredménye NOK!");
                                            }
                                            else
                                            {
                                                messagebox("Nincs akoustik mikroszkóp eredmény!");
                                            }

                                            SqlConnection cnn3 = new SqlConnection(connetionstringgen2);
                                            cnn3.Open();

                                            string addDB3 = "INSERT INTO [dbo].[BMW_Firewall_Check] ([customer_id], [housing_id], [subnumber], [result], [dayofyear], [date1], [date2], [scandate], [type], [type_group], [sw], [subsw]) VALUES ('" + Convert.ToString(bmw_fw_textbox.Text) + "', '" + Convert.ToString(housingid) + "','" + Convert.ToString(bmw_tipus) + "','" + Convert.ToString(bmwazonositas_label.Text) + "','" + Convert.ToString(dayoftheyear) + "','" + Convert.ToString(subnumber2) + "','" + Convert.ToString(number1.Substring(0, subnumber2.Length)) + "','" + DateTime.Now.ToString("s") + "','" + Convert.ToString(type) + "','" + Convert.ToString(type_group) + "','" + Convert.ToString(sw) + "','" + Convert.ToString(subsw) + "')";
                                            SqlCommand createcommand3 = new SqlCommand(addDB3, cnn3);
                                            createcommand3.ExecuteNonQuery();
                                        } // 35up vizsgálat vége
                                        else
                                        {
                                            MessageBox.Show("Ismeretlen típus kérem szóljon egy tesztmérnöknek, hogy frissítse az adatbázist!");
                                        }//hanem 35up és nem f30
                                    }//canframe result
                                    else if (canframe_result == "NOTEST")
                                    {
                                        MessageBox.Show("NINCS CAN Frame eredmény");
                                        bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                        bmwazonositas_label.Text = "NOK";
                                    }
                                    else
                                    {
                                        MessageBox.Show("CANFRAME eredmény NOK");
                                        bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                        bmwazonositas_label.Text = "NOK";
                                    }
                                }//acoustik ok vége
                                else if (acoustic_result == "NOK")
                                {
                                    MessageBox.Show("Akusztik mikroszkóp eredmény NOK");
                                    bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                    bmwazonositas_label.Text = "NOK";
                                }
                                else
                                {
                                    MessageBox.Show("Nincs akousztik mikroszkóp eredmény1!");
                                    bmwazonositas_label.BackColor = System.Drawing.Color.Red;
                                    bmwazonositas_label.Text = "NOK";
                                }
                            }
                            else
                            {
                                MessageBox.Show("Ez a Customer_ID már rögzítve van egy másil Housing_ID-val!");
                            }
                               
                        }
                        else
                        {
                            MessageBox.Show("A beolvasott Housing_ID és az adatbázisban rögzített Housing_ID nem egyezik!");
                        }
                        }//segeddatagridview
                        else 
                        { 
                            MessageBox.Show("Nincs ilyen adat az adatbázisban");
                        }
                    }
                    else               
                    {
                        MessageBox.Show("A beolvasott Customer ID és a beírt Customer ID rész nem egyezik!");
                    }
                    
                    }catch(Exception ex)
                    {
                        MessageBox.Show("Nem várt hiba! A hiba oka:\n"+ex.Message);
                    }                               
                }
            else
            {
                MessageBox.Show("Hibás adatkitöltés!");
            }
            }
            else 
            {
                  MessageBox.Show("Jelentkezzen be!");
                  username_textbox.Focus();
                  username_textbox.Select();
            }

            precustomeridbmw_label.Text = bmw_fw_textbox.Text;
            bmw_fw_textbox.Focus();
            bmw_fw_textbox.SelectAll();

        }

        private void username_textbox_Leave(object sender, EventArgs e)
        {
            if (username_textbox.Text == "polusaadrienn")
            {                
                password_textbox.PasswordChar = '♥';             
            }
            else if (username_textbox.Text == "Administrator" || username_textbox.Text == "laszlobozsar")
            {
                password_textbox.PasswordChar = '♫';
            }
            else
            {
                password_textbox.PasswordChar = '♦';
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string seged = "abc";
                string password = EncodePassword(Convert.ToString(password_textbox.Text), seged);



                string felhasznalo_lekerdezes = "Select * from [dbo].[Registration_Data] WHERE UserName = '" + username_textbox.Text + "' AND Passwd='"+password+"'";
                adatok_lekerdezese(felhasznalo_lekerdezes, seged_datagridview);
                
                if (seged_datagridview.RowCount > 1)
                {
                    globalusername = Convert.ToString(seged_datagridview.Rows[0].Cells["UserName"].Value);
                    globalpassword = Convert.ToString(seged_datagridview.Rows[0].Cells["Passwd"].Value);

                }
                else                 
                {                     
                    MessageBox.Show("Érvénytelen felhasználónév vagy jelszó!");
                    globalpassword = "";
                    globalusername = "";
                    username_textbox.Clear();
                    password_textbox.Clear();                
                }


                felhasznalo_label.Text = globalusername;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            
            }
        }

        private string EncodePassword(string password, string salt)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inarray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inarray);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string fw_eredmeny = "";
            if (globalusername != "")
            {

                if (bmw_fw_textbox.Text != "" && bmw_fw_textbox.Text.Length == 31)
                {
                    if (bmwfwnote_textbox.Text != "")
                    {
                        if (fwok_radiobutton.Checked == true || fwnok_radiobutton.Checked == true)
                        {
                            if (fwok_radiobutton.Checked == true)
                            { fw_eredmeny = "OK"; }

                            if (fwnok_radiobutton.Checked == true)
                            { fw_eredmeny = "NOK"; }

                            try
                            {

                                SqlCommand comm = new SqlCommand();
                                // string connetionstring = "Data Source=10.207.40.200;Initial Catalog=Gen2;Persist Security Info=True;User ID=GEN2;Password=1234";
                                SqlConnection cnn2 = new SqlConnection(connetionstringgen2);
                                comm.Connection = cnn2;
                                cnn2.Open();
                                string StrQuery = "UPDATE [dbo].[BMW_Firewall_Check] SET note='" + Convert.ToString(bmwfwnote_textbox.Text) + "',Result='"+fw_eredmeny+"' WHERE customer_id='" + Convert.ToString(bmw_fw_textbox.Text) + "'";
                                comm.CommandText = StrQuery;
                                comm.ExecuteNonQuery();

                            }
                            catch (Exception ex)
                            { MessageBox.Show(ex.Message); }

                        }
                        else
                        {
                            MessageBox.Show("Válasszon, hogy a termék [OK] vagy [NOK]!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Nem töltötte ki a megjegyzés mezőt!");
                        bmwfwnote_textbox.Focus();
                        bmwfwnote_textbox.Select();
                    }

                }
                else
                {
                    MessageBox.Show("Érvénytelen Customer_ID!");
                    bmw_fw_textbox.Focus();
                    bmw_fw_textbox.Select();
                }

            }
            else
            {
                MessageBox.Show("Jelentkezzen be!");
                username_textbox.Focus();
                username_textbox.Select();
            }

        }

        public void readtodatatable(string query, DataTable datatable)
        {
            datatable.Rows.Clear();
            SqlConnection cnn = new SqlConnection(connetionstringgen2);
            cnn.Open();
            SqlCommand objcmdm = new SqlCommand(query, cnn);
            objcmdm.ExecuteNonQuery();
            SqlDataAdapter adpm = new SqlDataAdapter(objcmdm);          
            adpm.Fill(datatable);
            cnn.Close();
        }
        public void messagebox(string message)
        {
            MessageBox.Show(new Form() { TopMost = true }, message);
        }




    }
}
