using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFMultired.Classes;

namespace WPFMultired.Models
{
    public class DataModal
    {
        public UserControl usercontrol { get; set; }
        public string message { get; set; }
        public string url { get; set; }

        public Visibility btnAccept { get; set; }
        public ETypeModal type { get; set; }
    }
}
