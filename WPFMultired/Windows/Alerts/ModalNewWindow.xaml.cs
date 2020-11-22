using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
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
                Data.usercontrol.Opacity = 0.1;
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
            try
            {
                Data.usercontrol.Opacity = 1;
                DialogResult = true;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Data.usercontrol.Opacity = 1;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
