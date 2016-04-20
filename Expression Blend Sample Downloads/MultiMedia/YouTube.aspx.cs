using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Literal1.Text = GetYouTubeScript(cmbPlaylist.SelectedValue);

    }
    protected string GetYouTubeScript(string id)
    {
        string scr = @"<object width='620' height='540'> ";
        scr = scr + @"<param name='movie' value='http://www.youtube.com/v/" + id + "'></param> ";
        scr = scr + @"<param name='allowFullScreen' value='true'></param> ";
        scr = scr + @"<param name='allowscriptaccess' value='always'></param> ";
        scr = scr + @"<embed src='http://www.youtube.com/v/" + id + "' ";
        scr = scr + @"type='application/x-shockwave-flash' allowscriptaccess='always' ";
        scr = scr + @"allowfullscreen='true' width='620' height='540'> ";
        scr = scr + @"</embed></object>";
        return scr;
    }
}
