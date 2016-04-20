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
	/// Interaction logic for AddDialog.xaml
	/// </summary>
	public partial class AddDialog : Window
	{
		public AddDialog()
		{
			this.InitializeComponent();
//			this.SourceInitialized += new EventHandler(AddDialog_SourceInitialized);
			// Insert code required on object creation below this point.
		}
        //void AddDialog_SourceInitialized(object sender, EventArgs e)
        //{
        //    GlassHelper.ExtendGlassFrame(this, new Thickness(-1));
        //}

		private void Save(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
            string MyConString = "SERVER=localhost;" +
                "DATABASE=mydatabase;" +
                "UID=root;" +
                "PASSWORD=apurv;";
            MySqlConnection connection = new MySqlConnection(MyConString);
            connection.Open();

            MySqlCommand command = connection.CreateCommand();
            try
            {
                String myInsertQuery = "INSERT INTO flash1 ( word,meaning,sentence,partofspeech )  VALUES ('" + tbWord.Text + "'," + "'" + tbMeaning.Text + "'," + "'" + tbSentence.Text + "'," + "'" + tbPofS.Text + "')";
                command.CommandText = myInsertQuery;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            connection.Close();			
		}


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbWord.Clear();
            tbMeaning.Clear();
            tbSentence.Clear();
            tbPofS.Clear();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Window.Close();
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
}