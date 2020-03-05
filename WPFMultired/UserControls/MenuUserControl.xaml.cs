using System;
using System.Collections.Generic;
using System.Reflection;
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

            ShowModal();
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
        private void Lv_type_transactions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((ItemList)lv_type_transactions.SelectedItem != null)
                {
                    Utilities.navigator.Navigate(UserControlView.MenuCompaniesUserControl, true, ((ItemList)lv_type_transactions.SelectedItem).Item3);
                }
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
    }
}
