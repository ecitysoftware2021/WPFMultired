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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para MenuCompaniesUserControl.xaml
    /// </summary>
    public partial class MenuCompaniesUserControl : UserControl
    {
        private DataListViewModel viewModel;

        private string typeTransaction;

        public MenuCompaniesUserControl(string typeTransaction)
        {
            InitializeComponent();

            this.typeTransaction = typeTransaction;
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

                if (AdminPayPlus.DataPayPlus.ListCompanies.Count > 0)
                {
                    this.DataContext = viewModel;
                    viewModel.DataList = AdminPayPlus.DataPayPlus.ListCompanies;
                    viewModel.ViewList.Source = viewModel.DataList;
                    lv_companies.DataContext = viewModel.ViewList;
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

        private void Lv_companies_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Utilities.navigator.Navigate(UserControlView.Consult, true, ((ItemList)lv_companies.SelectedItem).Item1, typeTransaction);
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

        private void Btn_exit_TouchDown(object sender, TouchEventArgs e)
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
