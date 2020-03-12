using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;

namespace WPFMultired.Windows.Alerts
{
    /// <summary>
    /// Lógica de interacción para ModalDetailWindow.xaml
    /// </summary>
    public partial class ModalDetailWindow : Window
    {
        private ModalDetailsViewModel viewModel;

        private Transaction transaction;


        public ModalDetailWindow(Transaction transaction, ETypeDetailModel type)
        {
            InitializeComponent();

            if (transaction != null)
            {
                this.transaction = transaction;
            }
            else
            {
                transaction = new Transaction();
            }

            if (viewModel == null)
            {
                viewModel = new ModalDetailsViewModel()
                {
                    Type = type,
                    Commission = (transaction != null && transaction.Product != null) ? transaction.Product.AmountCommission : 0
                };
            }

            this.DataContext = viewModel;

        }

        private void SetFocus()
        {
            if (viewModel.Type == ETypeDetailModel.Withdrawal)
            {
                txt_amount.Focus();
            }
            else if (viewModel.Type == ETypeDetailModel.Qr)
            {
                txt_qr.Focus();
            }
            else if (viewModel.Type == ETypeDetailModel.CodeOTP)
            {
                txt_input.Focus();
            }
        }

        private void Btn_cancel_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                DialogResult = false;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_finish_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                ProcessData();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ProcessData()
        {
            try
            {
                switch (viewModel.Type)
                {
                    case ETypeDetailModel.Payment:
                        transaction.Amount = transaction.Product.AmountMax;
                        SendData();
                        break;
                    case ETypeDetailModel.Withdrawal:
                        if (viewModel.Amount >= transaction.Product.AmountMin && viewModel.Amount <= transaction.Product.AmountMax)
                        {
                            transaction.Amount = viewModel.Amount;
                            if (viewModel.CallService(transaction) && !string.IsNullOrEmpty(transaction.CodeOTP))
                            {
                                viewModel.Type = ETypeDetailModel.CodeOTP;
                                SetFocus();
                            }
                        }
                        else
                        {
                            Utilities.ShowModal("Ingrese un valor a retirar valido", EModalType.Error);
                        }
                        break;
                    case ETypeDetailModel.CodeOTP:
                        if (!string.IsNullOrEmpty(viewModel.TxtInput))
                        {
                            transaction.CodeOTP = viewModel.TxtInput;
                            if (viewModel.CallService(transaction))
                            {
                                SendData();
                            }
                        }
                        break;
                    case ETypeDetailModel.Qr:
                        if (viewModel.CallService(transaction))
                        {
                            if (transaction.Type == ETransactionType.Withdrawal)
                            {
                                viewModel.Type = ETypeDetailModel.CodeOTP;
                            }
                            else
                            {
                                transaction.Amount = transaction.Product.AmountMax;
                                SendData();
                            }
                        }
                        break;
                    default:
                        break;
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
                if (transaction.Amount > 0 || transaction.Type == ETransactionType.Pay)
                {
                    Task.Run(async () =>
                    {
                        await AdminPayPlus.SaveTransactions(this.transaction, false);

                        Utilities.CloseModal();

                        if (this.transaction.IdTransactionAPi == 0)
                        {
                                Utilities.ShowModal(MessageResource.NoProccessInformation, EModalType.Error);
                                Utilities.navigator.Navigate(UserControlView.Main);
                                Application.Current.Dispatcher.Invoke(delegate
                                {
                                    DialogResult = false;
                                });
                        }
                        else
                        {
                            if (transaction.Type == ETransactionType.Withdrawal)
                            {
                                if (this.transaction.IsReturn)
                                {
                                    Utilities.navigator.Navigate(UserControlView.ReturnMony, false, transaction);
                                    Application.Current.Dispatcher.Invoke(delegate
                                    {
                                        DialogResult = true;
                                    });
                                }
                                else
                                {
                                    Utilities.ShowModal("El dispositivo no cuenta con el dinero solicitado", EModalType.Error);
                                }
                            }
                            else
                            {
                                Utilities.navigator.Navigate(UserControlView.Pay, false, transaction);
                                Application.Current.Dispatcher.Invoke(delegate
                                {
                                    DialogResult = true;
                                });
                            }
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

        private void Txt_qr_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && !viewModel.IsReadQr)
                {
                    viewModel.IsReadQr = true;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                SetFocus();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Txt_amount_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (txt_amount.Text.Equals("$"))
            {
                txt_amount.Text = "0";
            }
        }
    }
}
