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

namespace _3DFlipCard
{
    public class Executives: List<Executive>
    {
        public Executives()
        {
            this.Add(new Executive() { Name = "Mark Philip", Designation = "President", ImagePath = "Images/1.jpg" });
            this.Add(new Executive() { Name = "Amy Gray", Designation = "CEO", ImagePath = "Images/2.jpg" });
            this.Add(new Executive() { Name = "Bea Sanders", Designation = "COO", ImagePath = "Images/3.jpg" });
            this.Add(new Executive() { Name = "Rachel Hans", Designation = "VP, Engineering", ImagePath = "Images/4.jpg" });
            this.Add(new Executive() { Name = "Su Brown", Designation = "VP, Finance", ImagePath = "Images/5.jpg" });
            this.Add(new Executive() { Name = "Becky Jones", Designation = "VP, Sales", ImagePath = "Images/6.jpg" });
            this.Add(new Executive() { Name = "Mary Den", Designation = "CTO", ImagePath = "Images/7.jpg" });
            this.Add(new Executive() { Name = "Mark Hughes", Designation = "VP, HR", ImagePath = "Images/8.jpg" });
            this.Add(new Executive() { Name = "June Peters", Designation = "VP, Products", ImagePath = "Images/9.jpg" });
        }
    }
}
