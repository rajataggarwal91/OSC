/////////////////////////////////////////////////////////////////
//
// Silverlight + FarSeer Physics Helper
//
// by Andy Beaulieu - http://www.andybeaulieu.com
//
// LICENSE: This code is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. ANY 
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF 
// MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS
// OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR 
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF 
// LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//
/////////////////////////////////////////////////////////////////
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Spritehand.FarseerHelper
{
    public class PointItemCollection : ObservableCollection<PointObject>
    {

    }

    public class PointObject
    {
        public PointObject(string elementName, string pointList)
        {
            ElementName = elementName;
            FromStringPoints(pointList);
        }

        public PointObject(string elementName, List<Point> pointList)
        {
            ElementName = elementName;
            ListPoints = pointList;
        }


        public string ElementName { get; set; }
        public List<Point> ListPoints { get; set; }

        public void FromStringPoints(string points)
        {
            ListPoints = new List<Point>();
            string[] splitPoints = points.Split(' ');
            try
            {
                foreach (string pointSet in splitPoints)
                {
                    if (pointSet.Trim().Length == 0)
                        continue;
                    string[] twoPoints = pointSet.Split(',');
                    double x = Convert.ToDouble(twoPoints[0]);
                    double y = Convert.ToDouble(twoPoints[1]);
                    Point pt = new Point(x, y);
                    ListPoints.Add(pt);
                }
            }
            catch (Exception)
            {
                throw new Exception("There was a format problem in the PointListCollection property of the Physics Controller.");
            }

        }
    }
}
