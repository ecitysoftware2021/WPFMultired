using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;
using System.Windows;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para DataListUserControl.xaml
    /// </summary>
    public partial class DataListUserControl : UserControl
    {
        private Transaction transaction;

        private ItemList ProductSelect;

        private ValueModel valueModel;

        private DataListViewModel viewModel;

        public DataListUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            ProductSelect = new ItemList();

            ConfigurateView();
        }

        private void ConfigurateView()
        {
            try
            {
                if (viewModel == null)
                {
                    valueModel = new ValueModel
                    {
                        Val = 0
                    };

                    this.grvDetalle.DataContext = valueModel;

                    txtValor.IsEnabled = false;

                    viewModel = new DataListViewModel
                    {
                        Colum1 = transaction.payer.NAME,
                        Amount = transaction.Amount,
                        Colum2 = string.Concat("(*******", transaction.payer.IDENTIFICATION.Substring(transaction.payer.IDENTIFICATION.Length - 4), ")"),
                        Tittle = transaction.Observation,
                        DataList = new List<ItemList>(),
                        ViewList = new CollectionViewSource(),
                    };

                    viewModel.ConfigurateDataList(transaction.Products);
                    RefreshView();
                    //lv_data_list.Visibility = Visibility.Hidden;
                }

                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void RefreshView(bool search = false, ItemList item = null)
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

                if (search && item != null)
                {
                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        viewModel.ViewList.Source = viewModel.DataList.Where(x => x.Item2 == item.Item2);
                        lv_data_list.Visibility = Visibility.Visible;
                        lv_data_list.DataContext = viewModel.ViewList;
                        lv_data_list.Items.Refresh();
                    });
                    GC.Collect();
                }
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

        private string GetImage(bool flag)
        {
            try
            {
                if (!flag)
                {
                    return "/Images/Others/circle.png";
                }

                return "/Images/Others/ok.png";
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
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
                            Utilities.navigator.Navigate(UserControlView.Detail, true, transaction);
                        }
                    });
                    Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload);
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

        private bool Validar(decimal min, decimal max)
        {
            try
            {
                if (string.IsNullOrEmpty(txtValor.Text) || valueModel.Val <= 0)
                {
                    txtErrorValor.Text = "Debe ingresar el valor a pagar";
                    return false;
                }

                if (valueModel.Val % 100 != 0)
                {
                    txtErrorValor.Text = string.Concat("Esta máquina sólo recibe multiplos de 100",
                    Environment.NewLine, "Ejemplo: $100, $1.000, $10.000... etc.");
                    return false;
                }

                if (valueModel.Val < min || valueModel.Val > max)
                {
                    txtErrorValor.Text = string.Concat("Debe ingresar un valor entre",
                    Environment.NewLine, string.Format("{0} y {1}", min.ToString("C"), max.ToString("C")));
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                var product = (((Grid)sender).DataContext as ItemList);

                if (((Grid)sender).Tag != null)
                {
                    ProductSelect.Item3 = GetImage(false);

                    product.Item3 = GetImage(true);

                    ProductSelect = product;

                    txtValorComision.Text = String.Format("{0:C0}", product.Item4);

                    transaction.Product = (Product)((Grid)sender).Tag;

                    txtValor.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void txtValor_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.OpenKeyboard(true, sender as TextBox, this);
        }

        private void txtValor_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                txtErrorValor.Text = string.Empty;

                if (txtValor.Text.Length > 8)
                {
                    txtValor.Text = txtValor.Text.Remove(8, 1);
                    return;
                }

                if (txtValor.Text == string.Empty || txtValor.Text == "$")
                {
                    txtValor.Text = "0";
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void txtCountNumber_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.OpenKeyboard(true, sender as TextBox, this,450);
        }

        private void txtCountNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            { 
                if (txtCountNumber.Text.Length > 4)
                {
                    txtCountNumber.Text = txtCountNumber.Text.Remove(4, 1);
                    return;
                }

                txtErrorSearch.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
            }
        }

        private void btn_Search_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                var product = viewModel.DataList.FirstOrDefault(x => x.Item2.Substring(x.Item2.Length-4,4) == txtCountNumber.Text);

                if (product == null)
                {
                    txtErrorSearch.Visibility = Visibility.Visible;
                }
                else
                {
                    txtValor.IsEnabled = false;
                    txtValor.Text = "0";

                    RefreshView(true, product);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btn_Accept_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (ProductSelect.Data != null && Validar(transaction.Product.AmountMin, transaction.Product.AmountMax))
                {
                    transaction.Amount = valueModel.Val;
                    transaction.Product.AmountUser = valueModel.Val;

                    SendData();
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}