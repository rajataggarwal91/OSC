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
using MySql.Data.MySqlClient;

namespace GLASS_WINDOW
{
	/// <summary>
	/// Interaction logic for TestWin.xaml
	/// </summary>
    public partial class TestWin : Window
    {
        	//public int cnt;
		int cnt;
		int[] question = new int[20];
		int m=1,n=0;
		int[] answers = new int[20];
		int[] solution = new int[20];
		//int gen_sno;
		public TestWin()
		{
			this.InitializeComponent();

	//		this.SourceInitialized += new EventHandler(SearchWin_SourceInitialized);
			//string str1 = searchword.Text;


			// Insert code required on object creation below this point.
		}
        //void SearchWin_SourceInitialized(object sender, EventArgs e)
        //{
        //    //GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        //}
		

		

        public int GenerateRandom(int count)
        {
			//
			Random random = new Random();

			int crrOption = random.Next(1, count);
			//arr[0] = crrOption;

			return crrOption;
			
        }
		
		
		private void Test_Loaded(object sender, System.Windows.RoutedEventArgs e)
		{
			int[] arr = new int[4];
			int[] a = new int[4];
			string MyConString = "SERVER=localhost;" +
			   "DATABASE=mydatabase;" +
			   "UID=root;" +
			   "PASSWORD=apurv;";
			MySqlConnection connection = new MySqlConnection(MyConString);
			MySqlCommand command = connection.CreateCommand();
			MySqlDataReader Reader;
			command.CommandText = "SELECT count(*) FROM flash1";
			connection.Open();


			try
			{
				Reader = command.ExecuteReader();
				Reader.Read();
				cnt = Convert.ToInt32(Reader.GetString(0));
				//int gen_sno = GenerateRandom(cnt);
				//                MessageBox.Show(cnt.ToString());
				Reader.Close();
				
				int crrOption = GenerateRandom(4);
				a[0] = crrOption;
				solution[0] = crrOption;//record solution for given first question
				Reader.Close();
				//arr[0] = gen_sno;//array of word extraction 7
				int i, k = 0;
				int p;
				for (i = 0; i < 4; i++)//for generating 3 different random numbers
				{
					p = GenerateRandom(cnt);
					for (int j = 0; j < 4; j++)
					{

						if (p == arr[j])
						{
							p = GenerateRandom(cnt);
							j = 0;
						}
					}

					arr[k] = p;
					k++;
					while (true)
					{
						if (arr[0] != arr[1])
						{
							break;
						}
						else
						{
							p = GenerateRandom(cnt);
							for (int j = 0; j < 4; j++)
							{

								if (p == arr[j])
								{
									p = GenerateRandom(cnt);
									j = 0;
								}
							}
							arr[1] = p;
						}

					}

				}
				
				
				for (i = 0; i < 3; i++)
				{
					
					crrOption++;
					if (crrOption > 4)
					{

						crrOption = (crrOption % 4);
					}
					a[(i + 1)] = crrOption;
				}
				command.CommandText = "SELECT Word FROM flash1 where sno = '" + arr[0].ToString() + "'";
				Reader = command.ExecuteReader();
				Reader.Read();
				l1.Content = Reader.GetString(0);
				Reader.Close();
				for (i = 0; i < 4; i++)
				{
					command.CommandText = "SELECT Meaning FROM flash1 where sno = '" + arr[i].ToString() + "'";
					Reader = command.ExecuteReader();
					Reader.Read();
					switch (a[i])
					{
						case 1:
							RB1.Content = Reader.GetString(0);
							break;
						case 2:
							RB2.Content = Reader.GetString(0);
							break;
						case 3:
							RB3.Content = Reader.GetString(0);
							break;
						case 4:
							RB4.Content = Reader.GetString(0);
							break;
					}
					Reader.Close();
				}
				Reader.Close();
				question[0] = arr[0];
				connection.Close();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		
		private void btnNext_clicked(object sender, EventArgs e)
		{
			uncheckall();
			if (m < 20)
			{
				int[] arr = new int[4];
				int[] a = new int[4];
				string MyConString = "SERVER=localhost;" +
				   "DATABASE=mydatabase;" +
				   "UID=root;" +
				   "PASSWORD=apurv;";
				MySqlConnection connection = new MySqlConnection(MyConString);
				MySqlCommand command = connection.CreateCommand();
				MySqlDataReader Reader;
				connection.Open();


				try
				{

					int crrOption = GenerateRandom(4);//crrOption is answer for current question
					a[0] = crrOption;
					solution[m] = crrOption;
					//arr[0] = gen_sno;//array of word extraction 7
					int i, k = 0;
					int p;
					for (i = 0; i < 4; i++)//for generating 4 different random numbers
					{
						p = GenerateRandom(cnt);
						for (int j = 0; j < 4; j++)//checks if random number generated is in arr[] and generates new if it is present
						{

							if (p == arr[j])
							{
								p = GenerateRandom(cnt);
								j = 0;
							}
						}

						arr[k] = p;
						k++;
						while (true)//for arr[0]==arr[1] ghost error
						{
							if (arr[0] != arr[1])
							{
								break;
							}
							else
							{
								p = GenerateRandom(cnt);
								for (int j = 0; j < 4; j++)
								{

									if (p == arr[j])
									{
										p = GenerateRandom(cnt);
										j = 0;
									}
								}
								arr[1] = p;
							}

						}

						for (int z = 0; z < m; z++)//checks for arr[0]!=question[anyumber]
						{
							if (arr[0] == question[z])
							{
								arr[0] = GenerateRandom(cnt);
								z = 0;
								for (int j = 1; j < 4; j++)//checks for arr[0]!=arr[anynumber]
								{
									if (arr[0] == arr[j])
									{
										arr[0] = GenerateRandom(cnt);
										j = 1;
									}
								}
							}

						}

					}

					for (i = 0; i < 3; i++)//generates slots for display of options
					{
						crrOption++;
						if (crrOption > 4)
						{
							crrOption = (crrOption % 4);
						}
						a[(i + 1)] = crrOption;
					}
					command.CommandText = "SELECT Word FROM flash1 where sno = '" + arr[0].ToString() + "'";//select he question
					Reader = command.ExecuteReader();
					Reader.Read();
					l1.Content = Reader.GetString(0);//displays the question
					Reader.Close();
					for (i = 0; i < 4; i++)//Displays the options 
					{
						command.CommandText = "SELECT Meaning FROM flash1 where sno = '" + arr[i].ToString() + "'";//Selects 4 diff meanings
						Reader = command.ExecuteReader();
						Reader.Read();
						switch (a[i])
						{
							case 1:
								RB1.Content = Reader.GetString(0);
								break;
							case 2:
								RB2.Content = Reader.GetString(0);
								break;
							case 3:
								RB3.Content = Reader.GetString(0);
								break;
							case 4:
								RB4.Content = Reader.GetString(0);
								break;
						}
						Reader.Close();
					}
					Reader.Close();
					question[m] = arr[0];//updates question array so that the next word is not same
					m++;//array index
					connection.Close();
                    n++;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			else
			{
				int yourscore = score();
				MessageBox.Show("score" + yourscore.ToString());
			}


		}
		public void uncheckall()
		{
			RB1.IsChecked = false;
			RB2.IsChecked = false;
			RB3.IsChecked = false;
			RB4.IsChecked = false;

		}
		
		public int score()
		{
			int point = 0;
			for (int i = 0; i < 20 && solution[i] != 0 && answers[i] != 0; i++)
			{
				if(answers[i]==solution[i])
				point++;
			}
			return point;
		}

		private void Window_Closed(object sender, System.EventArgs e)
		{
			int yourscore = score();
			MessageBox.Show("SCORE " + yourscore.ToString());
            // TODO: Add event handler implementation here.
		}

		private void RB1Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			answers[n] = 1;
		}

		private void RB2Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			answers[n] = 2;
		}

		private void RB3Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			answers[n] = 3;
		}

		private void RB4Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
			answers[n] = 4;
		}
	}
    
}