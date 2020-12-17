using System;
using System.Collections.Generic;
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
    public partial class AddWhere : Window
    {

        public string table = "";


        public string where = "";
        public AddWhere()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TitleGB.Header =
                string.IsNullOrEmpty(table) ? "add where fox" : "Add where to table (" + table + ")";
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            where = TxWhere.Text;
            this.Close();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            where = "";
            this.Close();
        }


    }
}
