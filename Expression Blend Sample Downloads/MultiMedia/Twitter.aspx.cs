using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Yedda;
using System.Xml;
using System.IO;
using System.Text;

public partial class _Default : System.Web.UI.Page 
{
    protected void Page_Load(object sender, EventArgs e)
    {


        string TwitterUserName = "advait.bhake@gmail.com";
        string TwitterPassword = "advait1234";
        

        if (TwitterUserName != "" && TwitterPassword != "")
        {
            try
            {
                Twitter twitter = new Twitter();
                XmlDocument doc = twitter.GetFollowersAsXML(TwitterUserName, TwitterPassword);
               XmlNodeList nodes = doc.SelectNodes("/users/user");
                StringBuilder sb = new StringBuilder();


    

 

                  foreach (XmlNode node in nodes)
                      {
                         sb.Append("<table><tr><td>");
                         sb.Append("<a href=" + '"' + node.SelectSingleNode("url").InnerText + '"' +">");
                        sb.Append("<img  alt='adad' src='"+node.SelectSingleNode("profile_image_url").InnerText+"'/></a>"+"<br\\>");
                       
                        string str = "</tr></td>";
                        sb.Append(str);
                        string str1 = "<tr><td>";
                        sb.Append(str1);
                        sb.Append(node.SelectSingleNode("screen_name").InnerText + "<br\\>");
                        sb.Append(",  " + node.SelectSingleNode("location").InnerText + "<br\\>");
                        sb.Append(",  " + node.SelectSingleNode("description").InnerText + "<br\\>");
                      sb.Append("</table></tr></td>");
                      }
                   tweet.InnerHtml = sb.ToString();
            }
            catch (Exception ex)
            {
              string str=  ex.Message;
            }
            finally
            { }
        }

       }
}
