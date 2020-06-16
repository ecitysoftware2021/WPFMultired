using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
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
    /// Lógica de interacción para DetailCountUserControl.xaml
    /// </summary>
    public partial class DetailAcountUserControl : UserControl
    {
        private DataListViewModel viewModel;

        private Transaction transaction;
        public DetailAcountUserControl(Transaction transaction)
        {
            InitializeComponent();

            if (transaction != null)
            {
                this.transaction = transaction;
            }

            ConfigurateView();
        }

        private void ConfigurateView()
        {
            try
            {
                viewModel = new DataListViewModel
                {
                    Colum1 = "Detalle del Trámite",
                    Colum2 = "Detalle del Trámite",
                    ViewList = new CollectionViewSource(),
                    DataList = new List<ItemList> {
                        { new ItemList {Item1 = "Conceptos estatutarios", Item5 = 0}},
                        { new ItemList {Item1 = "Financiero", Item5 = 0 }},
                        { new ItemList {Item1 = "Protección", Item5 = 0 }},
                        { new ItemList {Item1 = "Salud", Item5 = 0 }},
                        { new ItemList {Item1 = "Créditos Cooperativos", Item5 = 0 }},
                    },
                };

                this.DataContext = viewModel;
                viewModel.ViewList.Source = viewModel.DataList;
                lv_data_list.DataContext = viewModel.ViewList;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
        private void Btn_acept_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                SendData();
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

        private void Btn_back_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                Utilities.navigator.Navigate(UserControlView.Consult);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async void SendData()
        {
            try
            {
                if (transaction.Amount > 0 || transaction.Type == ETransactionType.Deposit)
                {
                    Task.Run(async () =>
                    {
                        await AdminPayPlus.SaveTransactions(this.transaction, false);

                        Utilities.CloseModal();

                        if (this.transaction.IdTransactionAPi == 0)
                        {
                            Utilities.ShowModal(MessageResource.NoProccessInformation, EModalType.Error);
                            Utilities.navigator.Navigate(UserControlView.Main);
  
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Pay, false, transaction);;
                        }
                    });
                    Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
