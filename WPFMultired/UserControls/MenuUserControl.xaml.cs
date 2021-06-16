using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        private DataListViewModel viewModel;
        DispatcherTimer timerRestart = new DispatcherTimer();
        int restartCounter = 0;
        public MenuUserControl()
        {
            InitializeComponent();

            grvPublicity.Content = Utilities.UCPublicityBanner;

            GoTime();
        }

        private void ConfigurateView()
        {
            try
            {
                viewModel = new DataListViewModel
                {
                    ViewList = new CollectionViewSource(),
                    DataList = new List<ItemList>()
                };

                if (AdminPayPlus.DataPayPlus.ListTypeTransactions != null && AdminPayPlus.DataPayPlus.ListTypeTransactions.Count > 0)
                {
                    this.DataContext = viewModel;
                    viewModel.DataList = AdminPayPlus.DataPayPlus.ListTypeTransactions;
                    viewModel.ViewList.Source = viewModel.DataList;
                    lv_type_transactions.DataContext = viewModel.ViewList;
                    lv_type_transactions.Items.Refresh();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_back_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                Utilities.navigator.Navigate(UserControlView.Main);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ShowModal()
        {
            try
            {
                Utilities.ShowModalDetails(null, ETypeDetailModel.Qr);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ConfigurateView();
        }

        private void Grid_list_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (((Grid)sender).Tag != null)
                {
                    var company = AdminPayPlus.DataPayPlus.ListCompanies.FirstOrDefault(x => x.Item1.Contains(Utilities.GetConfiguration("SourceEntity"))).Item1;

                    Utilities.navigator.Navigate(UserControlView.Consult, true, company, ((Grid)sender).Tag);
                }
                //decimal ValorSobrante = 2000;
                //string ms = string.Concat("Su transacción tiene una devolución de saldo de ", ValorSobrante.ToString("C", new CultureInfo("en-US")));
                //string tittle = "Solicita vueltos a la máquina o abona el vuelto a la misma cuenta de ahorro a la que estás consignando.";
                //Utilities.ShowModal(ms, EModalType.ReturnMoney, this, false, tittle);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void GoTime()
        {
            try
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1);
                timer.Tick += UpdateTime;
                timer.Start();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            try
            {
                txtHoraActual.Text = DateTime.Now.ToString("HH:mm tt");
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void btn_Restart_TouchEnter(object sender, TouchEventArgs e)
        {
            TimeToRestart();
        }

       
        private void TimeToRestart()
        {
            try
            {

                timerRestart.Interval = TimeSpan.FromSeconds(1);
                timerRestart.Tick += RestartTimeTick;
                timerRestart.Start();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void RestartTimeTick(object sender, EventArgs e)
        {
            restartCounter++;
            if (restartCounter >= int.Parse(Utilities.GetConfiguration("TimeToRestart")))
            {
                Utilities.RestartApp();
            }
        }

        private void btn_Restart_TouchLeave(object sender, TouchEventArgs e)
        {
            restartCounter = 0;
            timerRestart.Tick -= RestartTimeTick;
            timerRestart.Stop();
        }
    }
}
