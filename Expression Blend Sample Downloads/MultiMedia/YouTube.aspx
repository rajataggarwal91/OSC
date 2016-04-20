<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage2.master" AutoEventWireup="true" CodeFile="YouTube.aspx.cs" Inherits="Default2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

     <meta http-equiv="content-type" content="text/html;charset=utf-8" />
    <title>Style 12 (Sienna) - Menu by Apycom.com</title>
    <link type="text/css" href="menu.css" rel="stylesheet" />
    <script type="text/javascript" src="jquery.js"></script>
    <script type="text/javascript" src="menu.js"></script>
    <link href="default.css" rel="stylesheet" type="text/css" media="screen" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" updatemode="Conditional" >
            <ContentTemplate> 
            <div>
                <!-- ALL CONTENT IS SHOWN FOR DEMO PURPOSE ONLY-->
                <asp:DropDownList ID="cmbPlaylist" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="raRaxt_KM9Q">Sound Of Silence (Masters of Chant)</asp:ListItem>
                    <asp:ListItem Value="WALIARHHLII">Miss Teen USA South Carolina 2007</asp:ListItem>
                </asp:DropDownList>
                <br /><br />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            </div>
            </ContentTemplate>
          </asp:UpdatePanel>    
</asp:Content>

