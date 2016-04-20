<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="ABCNews.aspx.cs" Inherits="ABC_1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <title>Style 12 (Sienna) - Menu by Apycom.com</title>
    <link type="text/css" href="menu.css" rel="stylesheet" />
    <script type="text/javascript" src="jquery.js"></script>
    <script type="text/javascript" src="menu.js"></script>
    <link href="default.css" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  <div>
    <IFRAME id="frame1" width="1000px" height="768px" src="http://abc.go.com/schedule" scrolling="auto">
</IFRAME>
    </div>
</asp:Content>

