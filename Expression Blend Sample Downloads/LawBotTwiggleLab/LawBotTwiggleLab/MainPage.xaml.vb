Partial Public Class MainPage
	Inherits UserControl

	Public Sub New()
		InitializeComponent()
	End Sub

  Private Sub twNavigation_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles twNavigation.Loaded
    SetInfoText()
  End Sub

  Private Sub twNavigation_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles twNavigation.MouseLeftButtonUp
    SetInfoText()
  End Sub

  Private Sub SetInfoText()
    Dim InfoText As String = "Page " + twNavigation.GetSelectedItem + " is selected."
    tbSelectedPageInfo.Text = InfoText
  End Sub
End Class