#ExternalChecksum("C:\Users\Rechtsanwalt\Desktop\Twisting Navigation OnGoing Development\LawBotTwiggleLab\LawBotTwiggleLab\MainPage.xaml","{406ea660-64cf-4c82-b6f0-42d48172a799}","CF07D30E9CAB8B92002A6DE455EA6164")
'------------------------------------------------------------------------------
' <auto-generated>
'     Dieser Code wurde von einem Tool generiert.
'     Laufzeitversion:2.0.50727.4200
'
'     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
'     der Code erneut generiert wird.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports LawBotTwiggleLab
Imports System
Imports System.Windows
Imports System.Windows.Automation
Imports System.Windows.Automation.Peers
Imports System.Windows.Automation.Provider
Imports System.Windows.Controls
Imports System.Windows.Controls.Primitives
Imports System.Windows.Data
Imports System.Windows.Documents
Imports System.Windows.Ink
Imports System.Windows.Input
Imports System.Windows.Interop
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Media.Imaging
Imports System.Windows.Resources
Imports System.Windows.Shapes
Imports System.Windows.Threading



<Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>  _
Partial Public Class MainPage
    Inherits System.Windows.Controls.UserControl
    
    Friend WithEvents LayoutRoot As System.Windows.Controls.Grid
    
    Friend WithEvents twNavigation As LawBotTwiggleLab.TwiggleButton
    
    Friend WithEvents tbSelectedPageInfo As System.Windows.Controls.TextBlock
    
    Private _contentLoaded As Boolean
    
    '''<summary>
    '''InitializeComponent
    '''</summary>
    <System.Diagnostics.DebuggerNonUserCodeAttribute()>  _
    Public Sub InitializeComponent()
        If _contentLoaded Then
            Return
        End If
        _contentLoaded = true
        System.Windows.Application.LoadComponent(Me, New System.Uri("/LawBotTwiggleLab;component/MainPage.xaml", System.UriKind.Relative))
        Me.LayoutRoot = CType(Me.FindName("LayoutRoot"),System.Windows.Controls.Grid)
        Me.twNavigation = CType(Me.FindName("twNavigation"),LawBotTwiggleLab.TwiggleButton)
        Me.tbSelectedPageInfo = CType(Me.FindName("tbSelectedPageInfo"),System.Windows.Controls.TextBlock)
    End Sub
End Class
