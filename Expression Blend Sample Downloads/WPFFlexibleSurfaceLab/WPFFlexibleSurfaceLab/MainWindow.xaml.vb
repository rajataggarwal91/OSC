Imports System.Windows.Media.Animation

Class MainWindow

    Private Sub MainWindow_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        AddHandler gdLayer.MouseMove, AddressOf CatchMouseMove
    End Sub

    Private MousePosition As Point

    Private Sub CatchMouseMove(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs)
        MousePosition = e.GetPosition(gdLayer)
    End Sub

    Private Sub gdLayer_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles gdLayer.MouseLeftButtonDown
        LetItFlow(e.GetPosition(gdLayer))
    End Sub
    Private Sub btTest_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles btTest.Click
        LetItFlow(MousePosition)
        If btTest.Content = "Clicked" Then
            btTest.Content = "Clicked again"
        Else
            btTest.Content = "Clicked"
        End If
    End Sub

    Private Sub ckTest_Click(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles ckTest.Click
        LetItFlow(MousePosition)
        If ckTest.IsChecked Then
            ckTest.Content = "Clicked"
        Else
            ckTest.Content = "Clicked again"
        End If
    End Sub

    Private Sub LetItFlow(ByVal mousePosition As Point)
        Dim CP As Point = mousePosition
        CP.X /= gdLayer.ActualWidth
        CP.Y /= gdLayer.ActualHeight
        myRipple.Center = CP
        Dim wave As Storyboard = FindResource("sbRippleClick")
        wave.Begin()
    End Sub

End Class