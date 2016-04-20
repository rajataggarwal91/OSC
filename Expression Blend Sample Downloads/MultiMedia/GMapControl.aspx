<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="GMapControl.aspx.cs" Inherits="Gmaps" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<%@ Register assembly="Artem.GoogleMap" namespace="Artem.Web.UI.Controls" tagprefix="artem" %>


    <title>Siicon Valley Entertainment Guide</title>
    <link type="text/css" href="menu.css" rel="stylesheet" />
    <script type="text/javascript" src="jquery.js"></script>
    <script type="text/javascript" src="menu.js"></script>
    <link href="default.css" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="btn"
        runat="server" Text="Button" onclick="btn_Click" />
    <div>
<artem:GoogleMap ID="GoogleMap1" runat="server" Width="1024px" Height="768px" Key="ABQIAAAAyWjvj4t-s3JAHKCxqTvdRBRjwQa_EwsI4RnwkJzBRa05i6lORBQctbwFxVw_HgGRbpDIgcY9TMlCab" 
Latitude="42.1229" Longitude="24.7879" Zoom="4">
    <Markers>
        <artem:GoogleMarker Latitude="42.1229" Longitude="24.7879"  />
        <artem:GoogleMarker Latitude="42.7" Longitude="23.3" />
    </Markers>
</artem:GoogleMap>


    </div>
</asp:Content>

