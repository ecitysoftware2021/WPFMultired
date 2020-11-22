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
using WPFMultired.Windows.Alerts;

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

            grvPublicity.Content = Utilities.UCPublicityBanner;

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

                    viewModel.ConfigurateDataList(transaction);
                    RefreshView();

                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        switch (transaction.eTypeService)
                        {
                            case ETypeServiceSelect.Deposito:
                                lv_depositos.Visibility = Visibility.Visible;
                                break;
                            case ETypeServiceSelect.TarjetaCredito:
                                txtMsInfo.Text = "Estimado asociado, de no cancelar el valor completo sugerido, su tarjeta quedará en mora.";
                                lv_tarjetaC.Visibility = Visibility.Visible;
                                break;
                            case ETypeServiceSelect.EstadoCuenta:
                                txtMsInfo.Text = "Estimado asociado, de no cancelar el valor completo sugerido, su crédito quedará en mora.";
                                lv_estadoC.Visibility = Visibility.Visible;
                                break;
                        }

                        if (transaction.TypeDocument != "0")
                        {
                            lv_depositos.Visibility = Visibility.Hidden;
                            lv_tarjetaC.Visibility = Visibility.Hidden;
                            lv_estadoC.Visibility = Visibility.Hidden;
                        }
                    });
                    GC.Collect();
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
                Dispatcher.BeginInvoke((Action)delegate
                {
                    viewModel.ViewList.Source = item == null ? viewModel.DataList : viewModel.DataList.Where(x => x.Item2 == item.Item2);

                    if (ProductSelect != null)
                    {
                        ProductSelect.Item3 = GetImage(false);
                    }

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
                txtErrorValor.Text = string.Empty;

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

                    ModalNewWindow modal = new ModalNewWindow(new DataModal
                    {
                        type = ETypeModal.Question,
                        usercontrol = this,
                        btnAccept = Visibility.Visible,
                        message = "Los datos ingresados del número de cuenta no coinciden. Valida la información e inténte más tarde."
                    });

                    modal.ShowDialog();
                }
                else
                {
                    RefreshView(product);
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
                if (transaction.Product == null)
                {
                    ModalNewWindow modal = new ModalNewWindow(new DataModal
                    {
                        type = ETypeModal.Question,
                        usercontrol = this,
                        btnAccept = Visibility.Visible,
                        message = "Digite los últimos (4) dígitos de la cuenta y seleccione un producto para continuar."
                    });

                    modal.ShowDialog();
                }
                else
                if (Validar(transaction.Product.AmountMin, transaction.Product.AmountMax))
                {
                    transaction.Amount = valueModel.Val;
                    transaction.Product.AmountUser = valueModel.Val;

                    Utilities.navigator.Navigate(UserControlView.Detail, true, transaction);
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowErrorMs()
        {
            try
            {
                ModalNewWindow modal = new ModalNewWindow(new DataModal
                {
                    type = ETypeModal.Question,
                    usercontrol = this,
                    btnAccept = Visibility.Visible,
                    message = $"Por favor, digita el valor a consignar (mínimo {transaction.Product.AmountMin.ToString("C")} - máximo {transaction.Product.AmountMax.ToString("C")}). El dispositivo recibe montos de dinero redondeados al múltiplo de $100."
                });

                modal.ShowDialog();
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
    }
}