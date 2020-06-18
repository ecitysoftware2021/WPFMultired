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
    /// Lógica de interacción para DataListUserControl.xaml
    /// </summary>
    public partial class DataListUserControl : UserControl
    {
        private Transaction transaction;

        private DataListViewModel viewModel;

        public DataListUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            ConfigurateView();
        }

        private void ConfigurateView()
        {
            try
            {
                if (viewModel == null)
                {
                    viewModel = new DataListViewModel
                    {
                        Colum1 = transaction.payer.NAME,
                        Amount = transaction.Amount,
                        Colum2 = string.Concat("(*******", transaction.payer.IDENTIFICATION.Substring(transaction.payer.IDENTIFICATION.Length - 4), ")"),
                        Tittle = transaction.Observation,
                        DataList = new List<ItemList>(),
                        ViewList = new CollectionViewSource()
                    };

                    viewModel.ConfigurateDataList(transaction.Products);
                    RefreshView();
                }

                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void RefreshView()
        {
            try
            {
                Dispatcher.BeginInvoke((Action)delegate
                {
                    viewModel.ViewList.Source = viewModel.DataList;
                    lv_data_list.DataContext = viewModel.ViewList;
                    lv_data_list.Items.Refresh();
                });
                GC.Collect();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ProcesItemSelect(int index)
        {
            try
            {
                bool alertResult = false;
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
                Utilities.navigator.Navigate(UserControlView.Consult, true, transaction.CodeCompany, transaction.CodeTypeTransaction);
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

        private void Select_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                var itemSelected = (Image)sender;
                if (itemSelected != null)
                {
                    int index = int.Parse(((Image)sender).Tag.ToString());
                    TimerService.Stop();
                    ProcesItemSelect(index);
                }
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
                if (transaction.Amount > 0)
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
                            Utilities.navigator.Navigate(UserControlView.Pay, false, transaction);
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

        /* private void ShowModal()
         {
             try
             {
                 transaction.IsCashBack = false;
                 if (!Utilities.ShowModalDetails(transaction, 
                     (transaction.Type == ETransactionType.Deposit ? ETypeDetailModel.Payment : ETypeDetailModel.Withdrawal)))
                 {
                     //Utilities.ShowModal(MessageResource.NoContinueTransaction,EModalType.Error);
                 }
             }
             catch (Exception ex)
             {
                 Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
             }
         }*/

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (((Grid)sender).Tag != null)
                {
                    transaction.Product = (Product)((Grid)sender).Tag;
                    lv_data_list.SelectedItem = null;
                    SendData();
                    //ShowModal();
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}