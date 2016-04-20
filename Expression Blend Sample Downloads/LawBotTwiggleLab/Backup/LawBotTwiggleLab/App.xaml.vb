Partial Public Class App
	Inherits Application

	Public Sub New()
		InitializeComponent()
	End Sub

	Private Sub Application_Startup(ByVal o As Object, ByVal e As StartupEventArgs) Handles Me.Startup
		Me.RootVisual = New MainPage()
	End Sub

	Private Sub Application_Exit(ByVal o As Object, ByVal e As EventArgs) Handles Me.Exit
	End Sub

	Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs) Handles Me.UnhandledException
		' Wenn die Anwendung nicht im Debugger ausgeführt wird, verwenden Sie den Ausnahmemechanismus des Browsers,
		' um die Ausnahme zu melden. Im IE wird in diesem Fall ein gelbes 
		' Warnsymbol in der Statuszeile angezeigt; Firefox zeigt einen Skriptfehler an.
		If Not System.Diagnostics.Debugger.IsAttached Then

			' HINWEIS: Dies ermöglicht, dass die  Anwendung weiter ausgeführt werden kann, wenn eine Ausnahme ausgelöst,
			' aber noch nicht behandelt worden ist. 
			' Bei Produktionsanwendungen sollte diese Fehlerbehandlung durch eine Anwendung ersetzt werden, die 
			' den Fehler an die Website meldet und die Anwendung beendet.
			e.Handled = True
			Deployment.Current.Dispatcher.BeginInvoke(New Action(Of ApplicationUnhandledExceptionEventArgs)(AddressOf ReportErrorToDOM), e)
		End If
	End Sub

	Private Sub ReportErrorToDOM(ByVal e As ApplicationUnhandledExceptionEventArgs)
		Try
			Dim errorMsg As String = e.ExceptionObject.Message + e.ExceptionObject.StackTrace
			errorMsg = errorMsg.Replace(""""c, "'"c).Replace(ChrW(13) & ChrW(10), "\n")

			System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(""Unhandled Error in Silverlight Application " + errorMsg + """);")
		Catch

		End Try
	End Sub
End Class