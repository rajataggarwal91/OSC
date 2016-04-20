Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports System.ComponentModel
Imports System.Collections.ObjectModel

<Description("UserControl to implement a twisting navigation panel. Licenced to you under Creative Commons for private use only. For commercial use please contact me under lawbot@t-online.de."), Category("LawBot TwiggleButton")> _
Partial Public Class TwiggleButton
  Inherits UserControl
  Implements INotifyPropertyChanged

#Region " Implementing INotifyPropertyChanged "

  Public Event PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs) Implements INotifyPropertyChanged.PropertyChanged

  Public Sub OnPropertyChanged(ByVal e As PropertyChangedEventArgs)
    If PropertyChangedEvent IsNot Nothing Then
      RaiseEvent PropertyChanged(Me, e)
    End If
  End Sub

#End Region

  Public Sub New()
    ' Für das Initialisieren der Variablen erforderlich
    InitializeComponent()

    Me._isNotSelectedColor = ConvertHexToColor("#FFB8BAE0")
    Me._isSelectedColor = ConvertHexToColor("#FF5D5F94")

  End Sub

  Private ActualTwiggledContent As String

  Private Sub TwiggleButton_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded

    SetTwiggleSelected(Me.SelectedTwiggleItem)

  End Sub

#Region " Text Content Dependency Properties "


  Public Shared ReadOnly ItemTextTopLeftProperty As DependencyProperty = _
                                  DependencyProperty.Register("ItemTextTopLeft", _
                                  GetType(String), _
                                  GetType(TwiggleButton), _
                                  New PropertyMetadata("ContentTopLeft"))

  ''' <summary>
  ''' Gets or sets the text content of the TopLeft item.
  ''' </summary>
  ''' <remarks>String.</remarks>
  <Description("Text content of the TopLeft item. This is a dependency property."), Category("LawBot TwiggleButton")> _
  Public Property ItemTextTopLeft() As String
    Get
      Return CType(GetValue(ItemTextTopLeftProperty), String)
    End Get
    Set(ByVal value As String)
      SetValue(ItemTextTopLeftProperty, value)
      Me.TopLeftContent.Text = value
      OnPropertyChanged(New PropertyChangedEventArgs("ItemTextTopLeft"))
    End Set
  End Property

  ' Text Content DependencyProperty For The TopRight Item
  Public Shared ReadOnly ItemTextTopRightProperty As DependencyProperty = _
                                DependencyProperty.Register("ItemTextTopRight", _
                                GetType(String), _
                                GetType(TwiggleButton), _
                                New PropertyMetadata("ContentTopRight"))

  ''' <summary>
  ''' Gets or sets the text content of the TopRight item.
  ''' </summary>
  ''' <remarks>String.</remarks>
  <Description("Text content of the TopRight item. This is a dependency property."), Category("LawBot TwiggleButton")> _
  Public Property ItemTextTopRight() As String
    Get
      Return CType(GetValue(ItemTextTopRightProperty), String)
    End Get
    Set(ByVal value As String)
      SetValue(ItemTextTopRightProperty, value)
      Me.TopRightContent.Text = value
      OnPropertyChanged(New PropertyChangedEventArgs("ItemTextTopRight"))
    End Set
  End Property

  ' Text Content DependencyProperty For The BottomLeft Item
  Public Shared ReadOnly ItemTextBottomLeftProperty As DependencyProperty = _
                                  DependencyProperty.Register("ItemTextBottomLeft", _
                                  GetType(String), _
                                  GetType(TwiggleButton), _
                                  New PropertyMetadata("ContentBottomLeft"))

  ''' <summary>
  ''' Gets or sets the text content of the BottomLeft item.
  ''' </summary>
  ''' <remarks>String.</remarks>
  <Description("Text content of the BottomLeft item. This is a dependency property."), Category("LawBot TwiggleButton")> _
  Public Property ItemTextBottomLeft() As String
    Get
      Return CType(GetValue(ItemTextBottomLeftProperty), String)
    End Get
    Set(ByVal value As String)
      SetValue(ItemTextBottomLeftProperty, value)
      Me.BottomLeftContent.Text = value
      OnPropertyChanged(New PropertyChangedEventArgs("ItemTextBottomLeft"))
    End Set
  End Property

  ' Text Content DependencyProperty For The BottomRight Item
  Public Shared ReadOnly ItemTextBottomRightProperty As DependencyProperty = _
                              DependencyProperty.Register("ItemTextBottomRight", _
                              GetType(String), _
                              GetType(TwiggleButton), _
                              New PropertyMetadata("ContentBottomRight"))

  ''' <summary>
  ''' Gets or sets the text content of the BottomRight item.
  ''' </summary>
  ''' <remarks>String.</remarks>
  <Description("Text content of the BottomRight item. This is a dependency property."), Category("LawBot TwiggleButton")> _
 Public Property ItemTextBottomRight() As String
    Get
      Return CType(GetValue(ItemTextBottomRightProperty), String)
    End Get
    Set(ByVal value As String)
      SetValue(ItemTextBottomRightProperty, value)
      Me.BottomRightContent.Text = value
      OnPropertyChanged(New PropertyChangedEventArgs("ItemTextBottomRight"))
    End Set
  End Property

#End Region


#Region " Selected Item "

  Private _selectedItem As New Border

  ''' <summary>
  ''' Gets the value of the currently selected item. 
  ''' </summary>
  ''' <value>String</value>
  ''' <returns>String</returns>
  ''' <remarks>ReadOnly</remarks>
  <Description("Gets the value of the currently selected item."), [ReadOnly](True)> _
  Public ReadOnly Property GetSelectedItem() As String
    Get
      Select Case Me._selectedTwiggleItem
        Case Is = TwiggleItemEnum.TopLeft
          Return Me.ItemTextTopLeft
        Case Is = TwiggleItemEnum.TopRight
          Return Me.ItemTextTopRight
        Case Is = TwiggleItemEnum.BottomLeft
          Return Me.ItemTextBottomLeft
        Case Is = TwiggleItemEnum.BottomRight
          Return Me.ItemTextBottomRight
        Case Else
          Return Me.ItemTextTopLeft
      End Select
    End Get
  End Property

  Public Enum TwiggleItemEnum As Integer
    TopLeft = 0
    TopRight = 1
    BottomLeft = 2
    BottomRight = 3
  End Enum

  Private _selectedTwiggleItem As New TwiggleItemEnum

  ''' <summary>
  ''' Gets or sets the selected item, which is selected at the beginning. This is the item that is highlighted visually as selected.
  ''' </summary>
  ''' <remarks>String.</remarks>
  <Description("Sets the item, which is selected at the beginning. This is the item that is highlighted visually as selected."), Category("LawBot TwiggleButton")> _
 Public Property SelectedTwiggleItem() As TwiggleItemEnum
    Get
      Return Me._selectedTwiggleItem
    End Get
    Set(ByVal value As TwiggleItemEnum)
      Me._selectedTwiggleItem = value
      SetSelectedItem(value)
    End Set
  End Property

  Private Sub SetSelectedItem(ByVal ItemID As Integer)
    Select Case ItemID
      Case Is = 0
        Me._selectedItem = TwiggleTopLeft
        VisualStateManager.GoToState(Me, "TopLeftTwiggled", True)
      Case Is = 1
        Me._selectedItem = TwiggleTopRight
        VisualStateManager.GoToState(Me, "TopRightTwiggled", True)
      Case Is = 2
        Me._selectedItem = TwiggleBottomLeft
        VisualStateManager.GoToState(Me, "BottomLeftTwiggled", True)
      Case Is = 3
        Me._selectedItem = TwiggleBottomRight
        VisualStateManager.GoToState(Me, "BottomRightTwiggled", True)
    End Select
  End Sub

#End Region


#Region " TwiggleItems MouseEnter"

  Private Sub TwiggleTopLeft_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles TwiggleTopLeft.MouseEnter
    VisualStateManager.GoToState(Me, "TopLeftTwiggled", True)
  End Sub

  Private Sub TwiggleBottomLeft_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles TwiggleBottomLeft.MouseEnter
    VisualStateManager.GoToState(Me, "BottomLeftTwiggled", True)
  End Sub

  Private Sub TwiggleTopRight_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles TwiggleTopRight.MouseEnter
    VisualStateManager.GoToState(Me, "TopRightTwiggled", True)
  End Sub

  Private Sub TwiggleBottomRight_MouseEnter(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles TwiggleBottomRight.MouseEnter
    VisualStateManager.GoToState(Me, "BottomRightTwiggled", True)
  End Sub

#End Region


#Region " TwiggleButtonStates "

  Private Sub TwiggleButton_MouseLeave(ByVal sender As Object, ByVal e As System.Windows.Input.MouseEventArgs) Handles Me.MouseLeave
    Select Case Me._selectedTwiggleItem
      Case Is = 0
        VisualStateManager.GoToState(Me, "TopLeftTwiggled", True)
      Case Is = 1
        VisualStateManager.GoToState(Me, "TopRightTwiggled", True)
      Case Is = 2
        VisualStateManager.GoToState(Me, "BottomLeftTwiggled", True)
      Case Is = 3
        VisualStateManager.GoToState(Me, "BottomRightTwiggled", True)
    End Select
  End Sub

#End Region


#Region " TwiggleItems MouseLeftButtonDown "

  Private Sub TwiggleTopLeft_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleTopLeft.MouseLeftButtonDown
    VisualStateManager.GoToState(Me, "TopLeftPressed", True)
  End Sub

  Private Sub TwiggleTopRight_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleTopRight.MouseLeftButtonDown
    VisualStateManager.GoToState(Me, "TopRightPressed", True)
  End Sub

  Private Sub TwiggleBottomRight_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleBottomRight.MouseLeftButtonDown
    VisualStateManager.GoToState(Me, "BottomRightPressed", True)
  End Sub

  Private Sub TwiggleBottomLeft_MouseLeftButtonDown(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleBottomLeft.MouseLeftButtonDown
    VisualStateManager.GoToState(Me, "BottomLeftPressed", True)
  End Sub

#End Region


#Region " TwiggleItems MouseLeftButtonUp "

  Private Sub TwiggleTopLeft_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleTopLeft.MouseLeftButtonUp
    SetTwiggleSelected(TwiggleItemEnum.TopLeft)
  End Sub

  Private Sub TwiggleTopRight_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleTopRight.MouseLeftButtonUp
    SetTwiggleSelected(TwiggleItemEnum.TopRight)
  End Sub

  Private Sub TwiggleBottomRight_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleBottomRight.MouseLeftButtonUp
    SetTwiggleSelected(TwiggleItemEnum.BottomRight)
  End Sub

  Private Sub TwiggleBottomLeft_MouseLeftButtonUp(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles TwiggleBottomLeft.MouseLeftButtonUp
    SetTwiggleSelected(TwiggleItemEnum.BottomLeft)
  End Sub

  Private Sub SetTwiggleSelected(ByVal TwiggleItem As TwiggleItemEnum)
    SetBackgroundUnselected()
    Select Case TwiggleItem
      Case Is = TwiggleItemEnum.TopLeft
        Me.SelectedTwiggleItem = TwiggleItemEnum.TopLeft
        VisualStateManager.GoToState(Me, "TopLeftTwiggled", True)
        With Me.TopLeftSelectedBorder
          .Background = New SolidColorBrush(Me._isSelectedColor)
          .Opacity = 0.5
          .Margin = New Thickness(2)
        End With
      Case Is = TwiggleItemEnum.TopRight
        Me.SelectedTwiggleItem = TwiggleItemEnum.TopRight
        VisualStateManager.GoToState(Me, "TopRightTwiggled", True)
        With Me.TopRightSelectedBorder
          .Background = New SolidColorBrush(Me._isSelectedColor)
          .Opacity = 0.5
          .Margin = New Thickness(2)
        End With
      Case Is = TwiggleItemEnum.BottomLeft
        Me.SelectedTwiggleItem = TwiggleItemEnum.BottomLeft
        VisualStateManager.GoToState(Me, "BottomLeftTwiggled", True)
        With Me.BottomLeftSelectedBorder
          .Background = New SolidColorBrush(Me._isSelectedColor)
          .Opacity = 0.5
          .Margin = New Thickness(2)
        End With
      Case Is = TwiggleItemEnum.BottomRight
        Me.SelectedTwiggleItem = TwiggleItemEnum.BottomRight
        VisualStateManager.GoToState(Me, "BottomRightTwiggled", True)
        With Me.BottomRightSelectedBorder
          .Background = New SolidColorBrush(Me._isSelectedColor)
          .Opacity = 0.5
          .Margin = New Thickness(2)
        End With
    End Select
  End Sub

  Private Sub SetBackgroundUnselected()
    Dim bdOldSelected As Border = Me._selectedItem.Child
    If bdOldSelected IsNot Nothing Then
      If bdOldSelected.Background IsNot Nothing Then
        Dim sb As New Storyboard
        Dim fadeout As New DoubleAnimation
        fadeout.From = 1
        fadeout.To = 0
        fadeout.Duration = TimeSpan.FromMilliseconds(400)
        Storyboard.SetTarget(fadeout, bdOldSelected.Background)
        Storyboard.SetTargetProperty(fadeout, New PropertyPath("(Background.Opacity)"))
        sb.Children.Add(fadeout)
        sb.Begin()
      End If
    End If
  End Sub

#End Region


#Region " Twiggle Color Values Selected/Unselected "

  Private _isNotSelectedColor As New Color
  Private _isSelectedColor As New Color

#End Region


#Region " Convert String to Color "

  Private Function ConvertHexToColor(ByVal hexColor As String) As Color

    Dim a, r, g, b As Byte

    Dim result As New Color

    If hexColor.StartsWith("#") Then
      hexColor = hexColor.Substring(1, 8)
    End If

    a = CByte(Int32.Parse(hexColor.Substring(0, 2), Globalization.NumberStyles.AllowHexSpecifier))
    r = CByte(Int32.Parse(hexColor.Substring(2, 2), Globalization.NumberStyles.AllowHexSpecifier))
    g = CByte(Int32.Parse(hexColor.Substring(4, 2), Globalization.NumberStyles.AllowHexSpecifier))
    b = CByte(Int32.Parse(hexColor.Substring(6, 2), Globalization.NumberStyles.AllowHexSpecifier))

    result = Color.FromArgb(a, r, g, b)

    Return result

  End Function

#End Region

End Class
