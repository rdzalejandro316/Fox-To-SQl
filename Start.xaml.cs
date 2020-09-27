﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FoxToSql
{

    public partial class Start : Window
    {

        DataTable dt_colfox = new DataTable();
        DataTable dt_colsql = new DataTable();
        DataTable dt_compare = new DataTable();

        public Start()
        {
            InitializeComponent();
        }

        private void BtnConnFox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxPathFoxPro.Text))
                {
                    MessageBox.Show("empty path please complete the respective field", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                bool f = TestConnectionFox(TxPathFoxPro.Text);
                TxOkFox.Text = f == true ? "successful connection" : "wrong connection";
                TxOkFox.Foreground = f == true ? Brushes.Green : Brushes.Red;
            }
            catch (Exception)
            {
                MessageBox.Show("error test", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnConnSql_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxPathSqlServer.Text))
                {
                    MessageBox.Show("empty path please complete the respective field", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                bool f = TestConnectionSQl(TxPathSqlServer.Text);
                TxOkSQL.Text = f == true ? "successful connection" : "wrong connection";
                TxOkSQL.Foreground = f == true ? Brushes.Green : Brushes.Red;
            }
            catch (Exception)
            {
                MessageBox.Show("error test", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public bool TestConnectionFox(string root)
        {
            try
            {
                bool flag = false;
                string strCon = @"Provider=VFPOLEDB.1;Data Source=" + root + ";Collating Sequence=MACHINE;Connection Timeout=20;Exclusive=NO;DELETED=True;EXACT=False";
                OleDbConnection con = new OleDbConnection(strCon);
                con.Open();
                if (con.State == ConnectionState.Open) flag = true;
                con.Close();
                return flag;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Invalid Path or File Name", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("connection test error", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool TestConnectionSQl(string root)
        {
            try
            {
                bool flag = false;
                string strCon = root;
                SqlConnection con = new SqlConnection(strCon);
                con.Open();
                if (con.State == ConnectionState.Open) flag = true;
                con.Close();
                return flag;
            }
            catch (OleDbException)
            {
                MessageBox.Show("Invalid Path", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            catch (Exception)
            {
                MessageBox.Show("connection test error", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void BtnLoadFox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TestConnectionFox(TxPathFoxPro.Text))
                {
                    string root = TxPathFoxPro.Text;
                    string strCon = @"Provider=VFPOLEDB.1;Data Source=" + root + ";Collating Sequence=MACHINE;Connection Timeout=20;Exclusive=NO;DELETED=True;EXACT=False";
                    DataTable tableInfo;
                    using (OleDbConnection con = new OleDbConnection(strCon))
                    {
                        con.Open();
                        tableInfo = con.GetSchema("Tables");
                        con.Close();
                    }

                    DataView dv = tableInfo.DefaultView;
                    dv.Sort = "TABLE_NAME desc";
                    //GridFoxPro.ItemsSource = dv;
                    CbTableFox.ItemsSource = dv;
                    CbTableFox.DisplayMemberPath = "TABLE_NAME";
                    CbTableFox.SelectedValuePath = "TABLE_NAME";


                }
            }
            catch (Exception)
            {
                MessageBox.Show("error load", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLoadFoxColumn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CbTableFox.SelectedIndex >= 0)
                {
                    string root = TxPathFoxPro.Text;
                    string strCon = @"Provider=VFPOLEDB.1;Data Source=" + root + ";Collating Sequence=MACHINE;Connection Timeout=20;Exclusive=NO;DELETED=True;EXACT=False";
                    if (!TestConnectionFox(TxPathFoxPro.Text)) return;


                    DataTable dt = new DataTable();
                    string table = CbTableFox.SelectedValue.ToString().Trim();
                    OleDbConnection con = new OleDbConnection(strCon);
                    con.Open();
                    DataTable dtCols = con.GetSchema("Columns");
                    DataRow[] d = dtCols.Select("TABLE_NAME='" + table + "' ");
                    DataTable dt1 = d.CopyToDataTable();

                    dt1.Columns.Add("TYPE_FOX");
                    foreach (DataRow item in dt1.Rows)
                    {
                        OleDbType columnType = (OleDbType)item["DATA_TYPE"];
                        item.BeginEdit();
                        item["TYPE_FOX"] = columnType.ToString();
                        item.EndEdit();
                    }

                    dt_colfox.Clear(); dt_colfox = dt1;
                    GridFoxPro.ItemsSource = dt1.Rows.Count > 0 ? dt1.DefaultView : null;
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Load the tables and select one of them", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error load columns", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLoadSql_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TestConnectionSQl(TxPathSqlServer.Text))
                {
                    string root = TxPathSqlServer.Text;
                    DataTable tableInfo;
                    using (SqlConnection con = new SqlConnection(root))
                    {
                        con.Open();
                        tableInfo = con.GetSchema("Tables");
                        con.Close();
                    }

                    DataView dv = tableInfo.DefaultView;
                    dv.Sort = "TABLE_NAME desc";

                    CbTableSql.ItemsSource = dv;
                    CbTableSql.DisplayMemberPath = "TABLE_NAME";
                    CbTableSql.SelectedValuePath = "TABLE_NAME";

                }
            }
            catch (Exception)
            {
                MessageBox.Show("error load", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnLoadSqlColumn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CbTableSql.SelectedIndex >= 0)
                {

                    if (!TestConnectionSQl(TxPathSqlServer.Text)) return;
                    DataTable dt = new DataTable();
                    string table = CbTableSql.SelectedValue.ToString().Trim();
                    SqlConnection conn = new SqlConnection(TxPathSqlServer.Text);
                    conn.Open();
                    string query = "SELECT COLUMN_NAME,DATA_TYPE AS TYPE_SQL,CHARACTER_MAXIMUM_LENGTH as CHARACTER_MAXIMUM_LENGTH_SQL,NUMERIC_PRECISION as NUMERIC_PRECISION_SQL,NUMERIC_SCALE as NUMERIC_SCALE_SQL FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '" + table + "' ORDER BY ORDINAL_POSITION";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    dt_colsql.Clear(); dt_colsql = dt;
                    GridSqlServer.ItemsSource = dt.Rows.Count > 0 ? dt.DefaultView : null;
                    conn.Close();
                    da.Dispose();
                }
                else
                {
                    MessageBox.Show("Load the tables and select one of them", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error load columns", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnTruncateTable_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (CbTableSql.SelectedIndex >= 0)
                {
                    string table = CbTableSql.SelectedValue.ToString().Trim();
                    if (MessageBox.Show("you want to delete the data from the table " + table + " ?", "Alerta", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {

                        if (!TestConnectionSQl(TxPathSqlServer.Text)) return;
                        DataTable dt = new DataTable();

                        SqlConnection conn = new SqlConnection(TxPathSqlServer.Text);
                        conn.Open();
                        string query = "TRUNCATE TABLE " + table + ";";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        MessageBox.Show("truncate successfully", "alert", MessageBoxButton.OK, MessageBoxImage.Information); ;
                    }
                }
                else
                {
                    MessageBox.Show("Load the tables and select one of them", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (SqlException w)
            {
                MessageBox.Show("error truncate:" + w, "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("error truncate", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public DataTable getLinq(DataTable dt1, DataTable dt2)
        {

            //DataTable dtMerged =
            //     (from a in dt1.AsEnumerable()
            //      join b in dt2.AsEnumerable() on a["COLUMN_NAME"].ToString() equals b["COLUMN_NAME"].ToString()
            //             into g                         
            //      where g.Count() > 0                  
            //      select (a)
            //      ).CopyToDataTable();

            DataTable dt = new DataTable();
            dt.Columns.Add("CHECK", typeof(bool));
            dt.Columns.Add("COLUMN_NAME");
            dt.Columns.Add("TYPE_FOX");
            dt.Columns.Add("CHARACTER_MAXIMUM_LENGTH");
            dt.Columns.Add("NUMERIC_PRECISION");
            dt.Columns.Add("NUMERIC_SCALE");
            dt.Columns.Add("TYPE_SQL");
            dt.Columns.Add("CHARACTER_MAXIMUM_LENGTH_SQL");
            dt.Columns.Add("NUMERIC_PRECISION_SQL");
            dt.Columns.Add("NUMERIC_SCALE_SQL");

            foreach (DataRow item in dt1.Rows)
            {
                string column = item["COLUMN_NAME"].ToString();
                string type_fox = item["TYPE_FOX"].ToString();
                string c_length_fox = item["CHARACTER_MAXIMUM_LENGTH"].ToString();
                string n_pre_fox = item["NUMERIC_PRECISION"].ToString();
                string n_scale_fox = item["NUMERIC_SCALE"].ToString();

                DataRow[] row = dt2.Select("COLUMN_NAME='" + column + "'");
                if (row.Length > 0)
                {
                    string type_sql = row[0]["TYPE_SQL"].ToString();
                    string c_length_sql = row[0]["CHARACTER_MAXIMUM_LENGTH_SQL"].ToString();
                    string n_pre_sql = row[0]["NUMERIC_PRECISION_SQL"].ToString();
                    string n_scale_sql = row[0]["NUMERIC_SCALE_SQL"].ToString();

                    dt.Rows.Add(false, column, type_fox, c_length_fox, n_pre_fox, n_scale_fox, type_sql, c_length_sql, n_pre_sql, n_scale_sql);
                }
            }

            return dt;
        }

        private void BtnCompare_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validation

                if (CbTableFox.SelectedIndex < 0)
                {
                    MessageBox.Show("select a foxpro table", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (CbTableSql.SelectedIndex < 0)
                {
                    MessageBox.Show("select a foxpro table", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (dt_colfox.Rows.Count <= 0)
                {
                    MessageBox.Show("load the table columns FOX PRO", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (dt_colsql.Rows.Count <= 0)
                {
                    MessageBox.Show("load the table columns SQL SERVER", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion

                dt_compare.Clear();
                dt_compare = getLinq(dt_colfox, dt_colsql);
                GridCompare.ItemsSource = dt_compare.DefaultView;

            }
            catch (Exception)
            {
                MessageBox.Show("error when comparing fields", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public OleDbDataReader SelectDBFDR(string sql, string root)
        {

            try
            {
                string strCon = @"Provider=VFPOLEDB.1;Data Source=" + root + ";Collating Sequence=MACHINE;Connection Timeout=20;Exclusive=NO;DELETED=True;EXACT=False";
                OleDbDataReader t;
                OleDbConnection oleDbConnection = new OleDbConnection(strCon);
                oleDbConnection.Open();
                t = new OleDbCommand(sql, oleDbConnection).ExecuteReader();
                return t;
            }
            catch (Exception ex)
            {
                MessageBox.Show("X:" + ex.Message);
                return null;
            }

        }

        private async void BtnPassData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validaciones


                if (dt_compare.Rows.Count <= 0)
                {
                    MessageBox.Show("empty comparison table", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                bool ischeck = false;
                foreach (DataRow item in dt_compare.Rows)
                {
                    if (Convert.ToBoolean(item["CHECK"])) ischeck = true;
                };

                if (!ischeck)
                {
                    MessageBox.Show("confirm at least one column", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }


                #endregion

                if (MessageBox.Show("wants to pass the information ?", "Alerta", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                    string table_fox = CbTableFox.SelectedValue.ToString();
                    string table_sql = CbTableSql.SelectedValue.ToString();
                    string connsql = TxPathSqlServer.Text;

                    List<string> list_col = new List<string>();

                    foreach (DataRow item in dt_compare.Rows)
                    {
                        bool flag = Convert.ToBoolean(item["CHECK"]);
                        if (flag) list_col.Add(item["COLUMN_NAME"].ToString());
                    };


                    string cab_colm_parm = String.Join(",", list_col.ToArray());
                    string query = "select  " + cab_colm_parm + " from " + table_fox + " ";

                    string root = TxPathFoxPro.Text;

                    GridMain.IsEnabled = false;
                    BusyIndicator.IsIndeterminate = true;
                    TxLoad.Visibility = Visibility.Visible;

                    CancellationTokenSource source = new CancellationTokenSource();
                    var slowTask = Task<OleDbDataReader>.Factory.StartNew(() => SelectDBFDR(query, root), source.Token);
                    await slowTask;

                    if (slowTask.IsCompleted)
                    {
                        OleDbDataReader data = ((OleDbDataReader)slowTask.Result);
                        using (System.Data.SqlClient.SqlBulkCopy bc = new System.Data.SqlClient.SqlBulkCopy(connsql))
                        {
                            bc.BulkCopyTimeout = 0;
                            bc.DestinationTableName = table_sql;
                            foreach (var item in list_col) bc.ColumnMappings.Add(item, item);
                            var t = bc.WriteToServerAsync(data);
                            await t;
                            if (t.IsCompleted)
                            {
                                MessageBox.Show("successful data transfer", "alert", MessageBoxButton.OK, MessageBoxImage.Information);
                                GridMain.IsEnabled = true;
                                BusyIndicator.IsIndeterminate = false;
                                TxLoad.Visibility = Visibility.Hidden;
                            }
                        }
                        GridMain.IsEnabled = true;
                        BusyIndicator.IsIndeterminate = false;
                        TxLoad.Visibility = Visibility.Hidden;
                    }
                    GridMain.IsEnabled = true;
                    BusyIndicator.IsIndeterminate = false;
                    TxLoad.Visibility = Visibility.Hidden;
                }


            }
            catch (Exception w)
            {
                MessageBox.Show("error pass data:" + w, "alert", MessageBoxButton.OK, MessageBoxImage.Error);
                GridMain.IsEnabled = true;
                BusyIndicator.IsIndeterminate = false;
                TxLoad.Visibility = Visibility.Hidden;
            }
        }

        private void BtnOPenCommand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Command ww = new Command();
                ww.ShowInTaskbar = false;
                ww.conn_sql = TxPathSqlServer.Text;
                ww.Owner = Application.Current.MainWindow;
                ww.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ww.ShowDialog();
            }
            catch (Exception)
            {
                MessageBox.Show("error open comman", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAlterCol_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (GridCompare.SelectedIndex > 0)
                {
                    DataRowView row = (DataRowView)GridCompare.SelectedItems[0];
                    string column = row["COLUMN_NAME"].ToString().Trim();
                    string type_fox = row["TYPE_FOX"].ToString().Trim();
                    string c_length = row["CHARACTER_MAXIMUM_LENGTH"].ToString().Trim();
                    string n_precision = row["NUMERIC_PRECISION"].ToString().Trim();
                    string n_scale = row["NUMERIC_SCALE"].ToString().Trim();
                    string table = CbTableSql.SelectedValue.ToString();
                    string value = string.IsNullOrEmpty(c_length) ? n_precision + "," + n_scale : c_length;
                    string alter = "ALTER TABLE " + table + " ALTER COLUMN " + column + " " + type_fox + "(" + value + ") ";

                    Command ww = new Command();
                    ww.ShowInTaskbar = false;
                    ww.conn_sql = TxPathSqlServer.Text;
                    ww.query = alter;
                    ww.Owner = Application.Current.MainWindow;
                    ww.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    ww.ShowDialog();
                }
                else
                {
                    MessageBox.Show("select the column to alter", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("error open comman", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }








    }
}
