using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FoxToSql
{

    public partial class ColumnDifferences : Window
    {
        public DataTable dtdiference;
        public bool BtnAlterEnable;
        public string Table;


        public ColumnDifferences()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BtnAddColumn.IsEnabled = BtnAlterEnable;
                ColumnCheck.Visibility = BtnAlterEnable == true ? Visibility.Visible : Visibility.Hidden;
                TypeColumn.Visibility = BtnAlterEnable == true ? Visibility.Visible : Visibility.Hidden;

                if (dtdiference.Rows.Count > 0)
                {
                    GridColumnsDiference.ItemsSource = dtdiference.DefaultView;
                    TxTotal.Text = dtdiference.Rows.Count.ToString();
                }
                else
                {
                    GridColumnsDiference.ItemsSource = null;
                    TxTotal.Text = "0";
                }

            }
            catch (Exception)
            {
                MessageBox.Show("error load data", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddColumn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validacion

                bool flag = false;
                foreach (DataRow item in dtdiference.Rows)
                {
                    if (Convert.ToBoolean(item["CHECK"])) flag = true;

                }

                if (!flag)
                {
                    MessageBox.Show("check a table column", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                #endregion


                Command ww = new Command();
                ww.ShowInTaskbar = false;
                ww.conn_sql = ((Start)Application.Current.MainWindow).TxPathSqlServer.Text;

                string query = "";
                foreach (DataRow item in dtdiference.Rows)
                {
                    bool check = Convert.ToBoolean(item["CHECK"]);
                    string column = item["COLUMN_NAME"].ToString();
                    OleDbType Type = (OleDbType)item["TYPE"];
                    string c_length = item["CHARACTER_MAXIMUM_LENGTH"].ToString();
                    string n_precision = item["NUMERIC_PRECISION"].ToString();
                    string n_scale = item["NUMERIC_SCALE"].ToString();
                    var type = return_type(Type, c_length, n_precision, n_scale);

                    if (check) query += "alter table  " + Table + " add  " + column + " " + type.Item1 + type.Item2 + ";" + Environment.NewLine;
                }

                ww.query = query;
                ww.Owner = Application.Current.MainWindow;
                ww.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ww.ShowDialog();

            }
            catch (Exception w)
            {
                MessageBox.Show("error open comman:" + w, "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public Tuple<string, string> return_type(OleDbType Type, string clength, string nprecision, string nscale)
        {
            string columnType = "";
            string valueType = "";
            switch (Type)
            {
                case OleDbType.Date: columnType = "DATETIME"; break;
                case OleDbType.DBDate: columnType = "DATETIME"; break;
                case OleDbType.Decimal:
                    columnType = "DECIMAL";
                    valueType = string.IsNullOrEmpty(clength) ? "(" + nprecision + "," + nscale + ")" : "(" + clength + ")";
                    break;
                case OleDbType.Double:
                    columnType = "NUMERIC";
                    valueType = string.IsNullOrEmpty(clength) ? "(" + nprecision + "," + nscale + ")" : "(" + clength + ")";
                    break;
                case OleDbType.Numeric:
                    columnType = "NUMERIC";
                    valueType = string.IsNullOrEmpty(clength) ? "(" + nprecision + "," + nscale + ")" : "(" + clength + ")";
                    break;
                case OleDbType.VarChar:
                    columnType = "VARCHAR";
                    valueType = string.IsNullOrEmpty(clength) ? "(" + nprecision + "," + nscale + ")" : "(" + clength + ")";
                    break;
                case OleDbType.Char:
                    columnType = "CHAR";
                    valueType = string.IsNullOrEmpty(clength) ? "(" + nprecision + "," + nscale + ")" : "(" + clength + ")";
                    break;
            }

            return new Tuple<string, string>(columnType, valueType);
        }

        private void CheckAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (DataRow item in dtdiference.Rows) item["CHECK"] = false;
        }

        private void CheckAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (DataRow item in dtdiference.Rows) item["CHECK"] = true;
        }


    }
}
