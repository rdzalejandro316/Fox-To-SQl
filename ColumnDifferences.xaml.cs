using System;
using System.Collections.Generic;
using System.Data;
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
        public ColumnDifferences()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
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


    }
}
