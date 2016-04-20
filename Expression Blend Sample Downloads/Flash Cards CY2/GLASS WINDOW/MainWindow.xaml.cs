using System;
using System.Windows;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using MySql.Data.MySqlClient;
using ChangeThemes;


namespace GLASS_WINDOW
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private AddDialog win;
        private SearchWin searcwin;
		private TestWin twin;
		private FrontSide fs = new FrontSide();
		//private ContentControl3D c2 = new ContentControl3D();
	
		public MainWindow()
		{
			this.InitializeComponent();
			this.SourceInitialized += new EventHandler(MainWindow_SourceInitialized);
			// Insert code required on object creation below this point.
		}
		void MainWindow_SourceInitialized(object sender, EventArgs e)
		{
	//		GlassHelper.ExtendGlassFrame(this, new Thickness(-1));

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
            Choices c = new Choices();

            try
            {
                Reader.Read();
                TBword1.Text = Reader.GetString(1);
             //   RTBFlash.Text = TBword1.Text;
            //    c.Add(TBword1.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();

			SpeechRecognizer rec = new SpeechRecognizer();
			rec.SpeechRecognized += rec_SpeechRecognized;
			c.Add(" add");
			c.Add("flip");
			c.Add("previous");
			c.Add("next");
			c.Add("speak");
			c.Add("continuous");
			c.Add("Verbal");
			//Populate the grammar builder with the choices and load it as a new grammar
			GrammarBuilder pizzadetails = new GrammarBuilder(c);
			Grammar g = new Grammar(pizzadetails);
			rec.LoadGrammar(g);
			rec.Enabled = true;
        }

        void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {

            if (e.Result.Text == "next")
            {
                // BtnNext_Click(sender, e);
                   this.BtnNext_Click(sender, e);
            }
            if (e.Result.Text == "previous")
            {
                 this.BtnPrevious_Click(sender, e);
            }
            if (e.Result.Text == "flip")
            {
                this.BtnFlip_Click(sender, e);
            }
            if (e.Result.Text == "add")
            {
                this.AddWord(sender, e);
            }
            if (e.Result.Text == "speak")
            {
                 this.BtnSpeak_Click(sender, e);
            }
            if (e.Result.Text == "Verbal")
            {
                this.BtnSpeak_Click(sender, e);
                MessageBox.Show("hello");
            }
        }

        private void Test(object sender, EventArgs e)
        {
            twin = new TestWin();
            twin.Show();// TODO: Add event handler implementation here.
            btnTest.IsEnabled = false;
        }

        public void EnableBtn()
        {
            btnTest.IsEnabled = true;
        }

        private void BtnSpeak_Click(object sender,EventArgs e)
        {
        	string MyConString = "SERVER=localhost;" +
				"DATABASE=mydatabase;" +
                "UID=root;" +
                "PASSWORD=apurv;";
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader Reader;
            command.CommandText = "SELECT * FROM flash1 WHERE word =  '" + TBword1.Text + "'";
            connection.Open();
            Reader = command.ExecuteReader();
            Reader.Read();
            string a = "Word" + Reader.GetString(1);
            string b = "meaning" + Reader.GetString(2);
            string c = "Sentence" + Reader.GetString(3);
            //a = a +","+","+"."+ b +","+","+"."+ c;			
            SpeechSynthesizer spsynthesizer = new SpeechSynthesizer();
            //	VoiceGender gender = (VoiceGender)Enum.Parse(voiceGenderType, Male);
            spsynthesizer.Rate = -2;
            spsynthesizer.Volume = 100;


            spsynthesizer.Speak(a);
            this.BtnFlip_Click(sender, e);
            //spsynthesizer.Speak("                       ");
            spsynthesizer.Speak(b);
            //spsynthesizer.Speak(a);
            spsynthesizer.Speak(c);

            spsynthesizer.SetOutputToDefaultAudioDevice();
            connection.Close();
		// TODO: Add event handler implementation here.
        }

        private void AddWord(object sender, EventArgs e)
        {
        	win = new AddDialog();
            win.Show();// TODO: Add event handler implementation here.
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
        	string MyConString = "SERVER=localhost;" +
				"DATABASE=mydatabase;" +
                "UID=root;" +
				"PASSWORD=apurv";
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader Reader;
            command.CommandText = "SELECT * FROM flash1 WHERE word =  '" + TBword1.Text + "'";
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
				fs.OnNextButtonClick(Reader.GetString(1));
				// = Reader.GetString(1);
        //        RTBFlash.Text = Reader.GetString(1);
                TBword1.Text = Reader.GetString(1);

            }
            catch (Exception ex)
            {
                MessageBox.Show("No previous element");
            }
            connection.Close();// TODO: Add event handler implementation here.
        }

        private void BtnFlip_Click(object sender,EventArgs e)
        {
        	string MyConString = "SERVER=localhost;" +
                "DATABASE=mydatabase;" +
                "UID=root;" +
                "PASSWORD=apurv;";
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader Reader;
            command.CommandText = "SELECT * FROM flash1 WHERE Word =  '" + TBword1.Text + "'";
            connection.Open();
            Reader = command.ExecuteReader();

            try
            {
                Reader.Read();
                String Str3 = "MEANING::   " + Reader.GetString(2) +"\n\n" + "SENTENCE::   " + Reader.GetString(3) + "\n\n PART OF SPEECH :: " + Reader.GetString(4);

             //   RTBFlash.Text = Str3;
                TBword1.Text = Reader.GetString(1);

                //MessageBox.Show("hello");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();// TODO: Add event handler implementation here.
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
        	string MyConString = "SERVER=localhost;" +
                "DATABASE=mydatabase;" +
                "UID=root;" +
                "PASSWORD=apurv;";
            MySqlConnection connection = new MySqlConnection(MyConString);
            MySqlCommand command = connection.CreateCommand();
            MySqlDataReader Reader;
            command.CommandText = "SELECT * FROM flash1 WHERE Word =  '" + TBword1.Text + "'";
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
				
				fs.OnNextButtonClick(Reader.GetString(1));
				//FlipTB.Text = Reader.GetString(1);
              //  RTBFlash.Text = Reader.GetString(1);
                TBword1.Text = Reader.GetString(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();// TODO: Add event handler implementation here.
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
        	searcwin= new SearchWin();
            searcwin.Show();// TODO: Add event handler implementation here.
        }

        private void S_ShinyBlue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetTheme(e);// TODO: Add event handler implementation here.
        }

        private void S_ShinyRed_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetTheme(e);// TODO: Add event handler implementation here.
        }

        private void S_BureauBlack_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetTheme(e);// TODO: Add event handler implementation here.
        }

        private void S_BureauBlue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetTheme(e);// TODO: Add event handler implementation here.
        }

        private void S_ExpressionDark_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetTheme(e);// TODO: Add event handler implementation here.
        }

        private void S_ExpressionLight_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetTheme(e);// TODO: Add event handler implementation here.
        }

        private void S_WhistlerBlue_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetTheme(e);// TODO: Add event handler implementation here.
        }


        public void SetTheme(System.Windows.RoutedEventArgs er)
        {
            //MessageBox.Show(er.OriginalSource.ToString());  
            String str = er.Source.ToString();
           string str1 = str.Substring(40);
         //MessageBox.Show(str1);
         int i = str1.IndexOf(' ', 0);
         string str2 = str1.Remove(i);
         //MessageBox.Show(str2);
            /////////////////
//            if(er.OriginalSource.ToString().Equals(System.Windows.Controls.MenuItem.

         Uri uri = null;
            if(str2.Equals("ShinyBlue"))
            {
                uri = new Uri(S_ShinyBlue.Tag.ToString(),UriKind.Relative);
            }
            else if (str2.Equals("ShinyRed"))
            {
                uri = new Uri(S_ShinyRed.Tag.ToString(), UriKind.Relative);
            }
            else if (str2.Equals("ExpressionDark"))
            {
                uri = new Uri(S_ExpressionDark.Tag.ToString(), UriKind.Relative);
            }
            else if (str2.Equals("ExpressionLight"))
            {
                uri = new Uri(S_ExpressionLight.Tag.ToString(), UriKind.Relative);
            }
            else if (str2.Equals("BureauBlack"))
            {
                uri = new Uri(S_BureauBlack.Tag.ToString(), UriKind.Relative);
            }
            else if (str2.Equals("BureauBlue"))
            {
                uri = new Uri(S_BureauBlue.Tag.ToString(), UriKind.Relative);
            }
            else if (str2.Equals("WhistlerBlue"))
            {
                uri = new Uri(S_WhistlerBlue.Tag.ToString(), UriKind.Relative);
            }

            LogicSkin.SetCurrentSkin(LayoutRoot, uri);
        }

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(@"http://www.thefreedictionary.com/" + TBword1.Text);
		}

		private void LbReadMore_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			
			System.Diagnostics.Process.Start(@"http://www.thefreedictionary.com/" + TBword1.Text);// TODO: Add event handler implementation here.
		}

		private void BtnBack_Click(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("hello");
			//c1.
		}
		public void BackContent(String s)
		{
		
		}

        

        
/*
        private void win_activated(object sender, System.EventArgs e)
        {
        	// TODO: Add event handler implementation here.
			Choices c = new Choices();
			 c.Add(TBword1.Text);
			
			SpeechRecognizer rec = new SpeechRecognizer();
            rec.SpeechRecognized += rec_SpeechRecognized;
            c.Add(" add");
            c.Add("flip");
            c.Add("previous");
            c.Add("next");
            c.Add("speak");
            c.Add("continuous");
            c.Add("Verbal");
            //Populate the grammar builder with the choices and load it as a new grammar
            GrammarBuilder pizzadetails = new GrammarBuilder(c);
            Grammar g = new Grammar(pizzadetails);
            rec.LoadGrammar(g);
            rec.Enabled = true;
        }
  */     
	}

	}

	public class GlassHelper
	{

		struct MARGINS
		{
			public MARGINS(Thickness t)
			{
				Left = (int)t.Left;
				Right = (int)t.Right;
				Top = (int)t.Top;
				Bottom = (int)t.Bottom;
			}
			public int Left;
			public int Right;
			public int Top;
			public int Bottom;
		}

		[DllImport("dwmapi.dll", PreserveSig = false)]
		static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

		[DllImport("dwmapi.dll", PreserveSig = false)]
		static extern bool DwmIsCompositionEnabled();


		public static bool ExtendGlassFrame(Window window, Thickness margin)
		{
			if (!DwmIsCompositionEnabled())
				return false;

			IntPtr hwnd = new WindowInteropHelper(window).Handle;
			if (hwnd == IntPtr.Zero)
				throw new InvalidOperationException("The Window must be shown before extending glass.");

			// Set the background to transparent from both the WPF and Win32 perspectives
			SolidColorBrush background = new SolidColorBrush(Colors.Red);
			background.Opacity = 0.5;
			window.Background = Brushes.Transparent;
			HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

			MARGINS margins = new MARGINS(margin);
			DwmExtendFrameIntoClientArea(hwnd, ref margins);
			return true;
		}
	


	
}