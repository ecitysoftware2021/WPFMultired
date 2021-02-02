using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
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
    /// Lógica de interacción para DataListUserControl.xaml
    /// </summary>
    public partial class DataListUserControl : UserControl
    {
        private Transaction transaction;

        private ItemList ProductSelect;

        private ValueModel valueModel;

        private DataListViewModel viewModel;
        private string questionMsg = string.Empty;

        public DataListUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            grvPublicity.Content = Utilities.UCPublicityBanner;

            ProductSelect = new ItemList();

            GoTime();

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
                        //Colum2 = string.Concat("(*******", transaction.payer.IDENTIFICATION.Substring(transaction.payer.IDENTIFICATION.Length - 4), ")"),
                        Colum2 = transaction.payer.IDENTIFICATION,
                        Tittle = transaction.Observation,
                        DataList = new List<ItemList>(),
                        ViewList = new CollectionViewSource(),
                    };
                    transaction.Product = null;
                    switch (transaction.eTypeService)
                    {
                        case ETypeServiceSelect.Deposito:
                            lv_depositos.Visibility = Visibility.Visible;
                            viewModel.Colum3 = "            Digita los últimos cuatro (4) números de la cuenta";
                            lblTipTitle.Text = "Valor a consignar";
                            break;
                        case ETypeServiceSelect.TarjetaCredito:
                            lv_tarjetaC.Visibility = Visibility.Visible;
                            viewModel.Colum3 = "            Digita los últimos cuatro (4) números de la tarjeta";
                            lblTipTitle.Text = "Valor a pagar (incluido comisión)";
                            break;
                        case ETypeServiceSelect.EstadoCuenta:
                            lv_estadoC.Visibility = Visibility.Visible;
                            viewModel.Colum3 = "            Digita los últimos cuatro (4) números del crédito";
                            lblTipTitle.Text = "Valor a pagar (incluido comisión)";
                            break;
                    }

                    if (transaction.TypeDocument != "0")
                    {
                        lv_depositos.Visibility = Visibility.Hidden;
                        lv_tarjetaC.Visibility = Visibility.Hidden;
                        lv_estadoC.Visibility = Visibility.Hidden;
                    }

                    viewModel.ConfigurateDataList(transaction);
                }

                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void RefreshView(ItemList item = null)
        {
            try
            {
                viewModel.ViewList.Source = item == null ? viewModel.DataList : viewModel.DataList.Where(x => x.Item2 == item.Item2);
                ProductSelect = item;
                ProductSelect.Item3 = GetImage(true);
                txtValorComision.Text = String.Format("{0:C0}", ProductSelect.Item4);
                transaction.Product = (Product)item.Data;
                txtValor.IsEnabled = true;
                btnQuestion.Visibility = Visibility.Visible;
                valueModel.Val = transaction.Product.Amount;

                switch (transaction.eTypeService)
                {
                    case ETypeServiceSelect.Deposito:
                        lv_depositos.Visibility = Visibility.Visible;
                        lv_depositos.DataContext = viewModel.ViewList;
                        lv_depositos.Items.Refresh();
                        break;
                    case ETypeServiceSelect.TarjetaCredito:
                        lv_tarjetaC.Visibility = Visibility.Visible;
                        lv_tarjetaC.DataContext = viewModel.ViewList;
                        lv_tarjetaC.Items.Refresh();
                        break;
                    case ETypeServiceSelect.EstadoCuenta:
                        lv_estadoC.Visibility = Visibility.Visible;
                        lv_estadoC.DataContext = viewModel.ViewList;
                        lv_estadoC.Items.Refresh();
                        break;
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
                    return "/Images/Others/Newcircle.png";
                }

                return "/Images/Others/Newok.png";
            }
            catch (Exception ex)
            {
            }
            return string.Empty;
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
                if (string.IsNullOrEmpty(txtValor.Text) || valueModel.Val <= 0 || valueModel.Val < min || valueModel.Val > max)
                {
                    ShowErrorMs();
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

                    btnQuestion.Visibility = Visibility.Visible;

                    valueModel.Val = transaction.Product.Amount;
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
                if (txtValor.Text.Length > 14)
                {
                    txtValor.Text = txtValor.Text.Remove(14, 1);
                    return;
                }

                if (txtValor.Text == string.Empty || txtValor.Text == "$")
                {
                    txtValor.Text = "0";
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void txtCountNumber_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.OpenKeyboard(true, sender as TextBox, this, 450);
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
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void btn_Search_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                txtValor.IsEnabled = false;
                txtValor.Text = "0";

                var product = viewModel.DataList.FirstOrDefault(x => x.Item2.Substring(x.Item2.Length - 4, 4) == txtCountNumber.Text);

                if (product == null)
                {
                    lv_depositos.Visibility = Visibility.Hidden;
                    lv_tarjetaC.Visibility = Visibility.Hidden;
                    lv_estadoC.Visibility = Visibility.Hidden;
                    btnQuestion.Visibility = Visibility.Hidden;
                    transaction.Product = null;
                    ProductSelect = new ItemList();
                    string msgAlert = string.Empty;
                    switch (transaction.eTypeService)
                    {
                        case ETypeServiceSelect.Deposito:
                            msgAlert = "Los datos ingresados del número de cuenta no coinciden y/o el estado del producto no permite realizar operación. Valida la información e inténtalo más tarde.";
                            break;
                        case ETypeServiceSelect.TarjetaCredito:
                            msgAlert = "Los datos ingresados del número de tarjeta de crédito no coinciden. Valida la información e inténtalo más tarde.";
                            break;
                        case ETypeServiceSelect.EstadoCuenta:
                            msgAlert = "Los datos ingresados del número de estado de cuenta no coinciden y/o el estado del producto no permite realizar operación. Valida la información e inténtalo más tarde.";
                            break;
                        default:
                            msgAlert = "Los datos ingresados del producto no coinciden. Valida la información e inténtalo más tarde.";
                            break;
                    }

                    Utilities.ShowModal(msgAlert, EModalType.Error, this);

                }
                else
                {
                    RefreshView(product);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void btn_Accept_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (transaction.Product == null && txtCountNumber.Text.Length < 4)
                {
                    Utilities.ShowModal("Digite los últimos (4) dígitos de la cuenta y seleccione un producto para continuar.", EModalType.Error, this);
                }
                else if (!string.IsNullOrEmpty(txtCountNumber.Text) && transaction.Product == null)
                {
                    Utilities.ShowModal("Busca tu producto con el icono de la lupa.", EModalType.Error, this);
                }
                else if (Validar(transaction.Product.AmountMin, transaction.Product.AmountMax))
                {
                    transaction.Amount = valueModel.Val;
                    transaction.Product.AmountUser = valueModel.Val;

                    if (transaction.eTypeService == ETypeServiceSelect.EstadoCuenta || transaction.eTypeService == ETypeServiceSelect.TarjetaCredito)
                    {
                        if (transaction.eTypeService == ETypeServiceSelect.TarjetaCredito && transaction.Amount < transaction.Product.Amount)
                        {
                            Utilities.ShowModal("Te recordamos que, al no cancelar el valor completo sugerido, tu tarjeta de crédito  puede quedar en mora.", EModalType.Error, this);
                        }

                        if (transaction.eTypeService == ETypeServiceSelect.EstadoCuenta && transaction.Amount < transaction.Product.Amount)
                        {
                            Utilities.ShowModal("Te recordamos que, al no cancelar el valor completo sugerido, tu crédito puede quedar en mora.", EModalType.Error, this);
                        }

                        if (transaction.Product.ExtraTarjetaCredito != null && transaction.Product.ExtraTarjetaCredito.FLGHON)
                        {
                            Utilities.ShowModal("La tarjeta de crédito que vas a cancelar genera honorarios, estos serán calculados sobre el valor a pagar; si modificas el valor, se realizará un nuevo recálculo, el cual se verá reflejado en el recibo de la transacción.", EModalType.Error, this);
                        }

                        if (transaction.Product.AccountStateProduct != null && transaction.Product.AccountStateProduct.FLGHON)
                        {
                            Utilities.ShowModal("El crédito que vas a cancelar genera honorarios, estos serán calculados sobre el valor a pagar; si modificas el valor, se realizará un nuevo recálculo, el cual se verá reflejado en el recibo de la transacción..", EModalType.Error, this);
                        }
                    }

                    Utilities.navigator.Navigate(UserControlView.Detail, true, transaction);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ShowErrorMs()
        {
            try
            {
                if (transaction.eTypeService == ETypeServiceSelect.EstadoCuenta)
                {
                    questionMsg = string.Format(Utilities.GetConfiguration("MsgAccountState"), string.Format("{0:C0}", transaction.Product.AmountMin), string.Format("{0:C0}", transaction.Product.AmountMax));

                }
                else
                {
                    questionMsg = string.Format(Utilities.GetConfiguration("MsgGeneric"), string.Format("{0:C0}", transaction.Product.AmountMin), string.Format("{0:C0}", transaction.Product.AmountMax));

                }

                Utilities.ShowModal(questionMsg, EModalType.Error, this);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void btnQuestion_TouchDown(object sender, TouchEventArgs e)
        {
            ShowErrorMs();
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
    }
}