using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
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

        public MenuUserControl()
        {
            InitializeComponent();
            //ShowModal();
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
            try
            {
                ConfigurateView();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Grid_list_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (((Grid)sender).Tag != null)
                {
                    if (AdminPayPlus.DataPayPlus.ListCompanies.Count == 1)
                    {
                        Utilities.navigator.Navigate(UserControlView.Consult, true, AdminPayPlus.DataPayPlus.ListCompanies.FirstOrDefault().Item1, ((Grid)sender).Tag);
                    } else if(AdminPayPlus.DataPayPlus.ListCompanies.Count > 1) {
                        Utilities.navigator.Navigate(UserControlView.MenuCompaniesUserControl, true, ((Grid)sender).Tag);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
