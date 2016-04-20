using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace GLASS_WINDOW
{
	/// <summary>
	/// Interaction logic for FrontSide.xaml
	/// </summary>
	public partial class FrontSide : UserControl
	{
		//private MainWindow mw;
		public FrontSide()
		{
			InitializeComponent();
			string MyConString = "SERVER=localhost;" +
			   "DATABASE=mydatabase;" +
			   "UID=root;" +
			   "PASSWORD=apurv;";
			MySqlConnection connection = new MySqlConnection(MyConString);
			MySqlCommand command = connection.CreateCommand();
			MySqlDataReader Reader;
			command.CommandText = "SELECT * FROM flash1 WHERE sno = 1";
			connection.Open();
			Reader = command.ExecuteReader();
			//Choices c = new Choices();

			try
			{
				Reader.Read();
				FlipText.Text = Reader.GetString(1);
				//   RTBFlash.Text = TBword1.Text;
				//    c.Add(TBword1.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			connection.Close();

	
		}
		public void OnNextButtonClick(String s)
		{
			FlipText.Text = s;
			MessageBox.Show(FlipText.Text);

		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			//mw.BackContent(FlipText.Text);
			//MessageBox.Show("Hello WOrld");
		}

		private void btnFlipPrevious_Click(object sender, RoutedEventArgs e)
		{
			string MyConString = "SERVER=localhost;" +
				"DATABASE=mydatabase;" +
				"UID=root;" +
				"PASSWORD=apurv;";
			MySqlConnection connection = new MySqlConnection(MyConString);
			MySqlCommand command = connection.CreateCommand();
			MySqlDataReader Reader;
			command.CommandText = "SELECT * FROM flash1 WHERE Word =  '" + FlipText.Text + "'";
			connection.Open();
			Reader = command.ExecuteReader();
			Reader.Read();
			string a = Reader.GetString(0);
			int p = Convert.ToInt32(a);
			p = p - 1;
			//MessageBox.Show(p.ToString());
			connection.Close();
			connection.Open();
			command.CommandText = "SELECT * FROM flash1 WHERE sno = '" + p.ToString() + "' ";
			Reader = command.ExecuteReader();

			try
			{
				Reader.Read();
				FlipText.Text = Reader.GetString(1); 
				//	fs.OnNextButtonClick(Reader.GetString(1));
				//FlipTB.Text = Reader.GetString(1);
				//  RTBFlash.Text = Reader.GetString(1);
				//TBword1.Text = Reader.GetString(1);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			connection.Close();

		}

		private void btnFlipNext_Click(object sender, RoutedEventArgs e)
		{
			string MyConString = "SERVER=localhost;" +
				"DATABASE=mydatabase;" +
				"UID=root;" +
				"PASSWORD=apurv;";
			MySqlConnection connection = new MySqlConnection(MyConString);
			MySqlCommand command = connection.CreateCommand();
			MySqlDataReader Reader;
			command.CommandText = "SELECT * FROM flash1 WHERE Word =  '" + FlipText.Text + "'";
			connection.Open();
			Reader = command.ExecuteReader();
			Reader.Read();
			string a = Reader.GetString(0);
			int p = Convert.ToInt32(a);
			p = p + 1;
			//MessageBox.Show(p.ToString());
			connection.Close();
			connection.Open();
			command.CommandText = "SELECT * FROM flash1 WHERE sno = '" + p.ToString() + "' ";
			Reader = command.ExecuteReader();

			try
			{
				Reader.Read();
				FlipText.Text = Reader.GetString(1);
				//	fs.OnNextButtonClick(Reader.GetString(1));
				//FlipTB.Text = Reader.GetString(1);
				//  RTBFlash.Text = Reader.GetString(1);
				//TBword1.Text = Reader.GetString(1);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
			connection.Close();
		}

		

	}
}
