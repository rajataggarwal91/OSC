using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using MySql.Data.MySqlClient;

namespace GLASS_WINDOW
{
	/// <summary>
	/// Interaction logic for SearchWin.xaml
	/// </summary>
	public partial class SearchWin : Window
	{
        String str_searchby;
		public SearchWin()
		{
			this.InitializeComponent();
	//		this.SourceInitialized += new EventHandler(SearchWin_SourceInitialized);
            //string str1 = searchword.Text;
            
			
			// Insert code required on object creation below this point.
		}
        //void SearchWin_SourceInitialized(object sender, EventArgs e)
        //{
        //	GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        //}

        private void btnSearch_Click(object sender, System.Windows.RoutedEventArgs e)
        {
             String MyConString = "SERVER = localhost;" +
                "DATABASE = mydatabase;" +
                "UID = root;" +
                "PASSWORD = apurv;";
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader reader;
            command.CommandText = "SELECT * FROM flash1 WHERE Word = '" + tbBox.Text + "'";
            connection.Open();
            
			
            try
            {
                /*resultblock.Text*/
                reader = command.ExecuteReader();
                reader.Read();
                string str = "Meaning : " + reader.GetString(2) + "\n\nSentence : " + reader.GetString(3);
		tbBlock.Text = str;
			//	tbBlock.Text ="  hello!!!!!!!!!!";
	//			MessageBox.Show(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
             
            }
            connection.Close();
			
        }

        
	}
}