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

namespace WindmillSelector
{
    public class Executives:List<Executive>
    {
        public Executives()
        {
            PopulateTestData();
        }

        #region Populate Test Data
        private void PopulateTestData()
        {
            this.Add(new Executive() { Name = "Mark Philip", Designation = "President", ImagePath = "Images/1.jpg" });
            this.Add(new Executive() { Name = "Amy Gray", Designation = "CEO", ImagePath = "Images/2.jpg" });
            this.Add(new Executive() { Name = "Bea Sanders", Designation = "COO", ImagePath = "Images/3.jpg" });
            this.Add(new Executive() { Name = "Rachel Hans", Designation = "VP, Engineering", ImagePath = "Images/4.jpg" });
            this.Add(new Executive() { Name = "Su Brown", Designation = "VP, Finance", ImagePath = "Images/5.jpg" });
            this.Add(new Executive() { Name = "Becky Jones", Designation = "VP, Sales", ImagePath = "Images/6.jpg" });
            this.Add(new Executive() { Name = "Mary Den", Designation = "CTO", ImagePath = "Images/7.jpg" });
            this.Add(new Executive() { Name = "Su Brown", Designation = "VP, HR", ImagePath = "Images/8.jpg" });
            this.Add(new Executive() { Name = "June Peters", Designation = "VP, Products", ImagePath = "Images/9.jpg" });
            this.Add(new Executive() { Name = "Tom Philip", Designation = "Director, Sales", ImagePath = "Images/10.jpg" });
            this.Add(new Executive() { Name = "Alice Gray", Designation = "Director, Finance", ImagePath = "Images/11.jpg" });
            this.Add(new Executive() { Name = "Fionna Sanders", Designation = "Director, Products", ImagePath = "Images/12.jpg" });
            this.Add(new Executive() { Name = "Phoebe Hans", Designation = "Director, Services", ImagePath = "Images/13.jpg" });
            this.Add(new Executive() { Name = "Mark Hughes", Designation = "Director, HR", ImagePath = "Images/14.jpg" });
            this.Add(new Executive() { Name = "Fern Jones", Designation = "Sr. Consultant", ImagePath = "Images/15.jpg" });
            this.Add(new Executive() { Name = "Pam Den", Designation = "Sr. Consultant", ImagePath = "Images/16.jpg" });
            this.Add(new Executive() { Name = "Noah Wanders", Designation = "Sr. Consultant", ImagePath = "Images/17.jpg" });
            this.Add(new Executive() { Name = "Virginia Peters", Designation = "Sr. Consultant", ImagePath = "Images/18.jpg" });
            this.Add(new Executive() { Name = "David Benson", Designation = "Sr. Consultant", ImagePath = "Images/19.jpg" });
        }
        #endregion
    }
}
