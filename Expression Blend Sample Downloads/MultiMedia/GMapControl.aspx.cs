using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Artem.Web.UI.Controls;

public partial class Gmaps : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btn_Click(object sender, EventArgs e)
    {
        string address = TextBox1.Text;
        GoogleMap1.Address = address;
        GoogleMap1.Markers.Clear();

        GoogleMap1.Markers.Add(new GoogleMarker(address));//, "Address: " + address));


        GoogleMap1.Zoom = 13;

    }
}
