using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
    public partial class Command : Window
    {
        public string query = "";
        public string conn_sql = "";
        public Command()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(query)) TxQuery.Text = query;
            }
            catch (Exception)
            {
                MessageBox.Show("error load", "alert", MessageBoxButton.OK, MessageBoxImage.Error);
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
            catch (SqlException)
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

        private void BtnExecute_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region validation

                if (string.IsNullOrEmpty(conn_sql))
                {
                    MessageBox.Show("no connection string", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                if (string.IsNullOrEmpty(TxQuery.Text))
                {
                    MessageBox.Show("there is nothing to run", "alert", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                #endregion


                if (MessageBox.Show("you want execute query ?", "Alerta", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                {

                    if (!TestConnectionSQl(conn_sql)) return;

                    DataTable dt = new DataTable();
                    SqlConnection conn = new SqlConnection(conn_sql);
                    conn.Open();
                    string query = TxQuery.Text;
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("successful execution", "alert", MessageBoxButton.OK, MessageBoxImage.Information);
                }


            }
            catch (SqlException w)
            {
                MessageBox.Show("error sql:" + w, "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception w)
            {
                MessageBox.Show("execute error:" + w, "alert", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





    }
}
