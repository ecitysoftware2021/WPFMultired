using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;

namespace WPFMultired.Windows.Alerts
{
    /// <summary>
    /// Interaction logic for ModalNewWindow.xaml
    /// </summary>
    public partial class ModalNewWindow : Window
    {
        private DataModal Data;

        public ModalNewWindow(DataModal data)
        {
            InitializeComponent();

            try
            {
                Data = data;
                Data.usercontrol.Opacity = 0.3;
                Data.url = Data.type == ETypeModal.Alert ? "/Images/Backgrounds/NewAlert.png" : "/Images/Backgrounds/NewQuestion.png";
                this.DataContext = Data;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            Data.usercontrol.Opacity = 1;
            DialogResult = true;
        }
    }
}
