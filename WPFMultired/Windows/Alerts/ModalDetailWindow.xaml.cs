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

            InitView(type);
        }

        private void InitView(ETypeDetailModel type)
        {
            try
            {
                if (viewModel == null)
                {
                    viewModel = new ModalDetailsViewModel();
                }

                viewModel.Type = type;

                switch (viewModel.Type)
                {
                    case ETypeDetailModel.Payment:
                        viewModel.Tittle = "Detalle";
                        viewModel.VisibilityInput = Visibility.Hidden;
                        viewModel.VisibilityComision = Visibility.Visible;
                        viewModel.Commission = transaction.Products[0].AmountCommission;
                        viewModel.VisibilityQr = Visibility.Hidden;
                        viewModel.VisibilityAcept = Visibility.Visible;
                        viewModel.IsReadQr = true;
                        break;
                    case ETypeDetailModel.Withdrawal:
                        viewModel.Tittle = "Detalle";
                        viewModel.VisibilityInput = Visibility.Visible;
                        viewModel.VisibilityComision = Visibility.Visible;
                        viewModel.Commission = transaction.Products[0].AmountCommission;
                        viewModel.VisibilityQr = Visibility.Hidden;
                        viewModel.VisibilityAcept = Visibility.Visible;
                        viewModel.VisibilityAmount = Visibility.Visible;
                        viewModel.VisibilityTxtImput = Visibility.Hidden;
                        viewModel.LblInput = "Ingrese el valor a retirar";
                        viewModel.IsReadQr = true;
                        break;
                    case ETypeDetailModel.CodeOTP:
                        viewModel.Tittle = "Codigo OTP";
                        viewModel.VisibilityInput = Visibility.Visible;
                        viewModel.VisibilityComision = Visibility.Hidden;
                        viewModel.VisibilityQr = Visibility.Hidden;
                        viewModel.VisibilityAmount = Visibility.Hidden;
                        viewModel.VisibilityTxtImput = Visibility.Visible;
                        viewModel.LblInput = "Ingrese el codigo OPT";
                        viewModel.IsReadQr = true;
                        viewModel.VisibilityAcept = Visibility.Visible;
                        break;
                    case ETypeDetailModel.Qr:
                        viewModel.Tittle = "QR";
                        viewModel.VisibilityInput = Visibility.Hidden;
                        viewModel.VisibilityComision = Visibility.Hidden;
                        viewModel.VisibilityQr = Visibility.Visible;
                        viewModel.IsReadQr = false;
                        viewModel.Message = "Si tienes un codigo QR hacercalo al lector para iniciar la transaccion, de lo contrario presiona cancelar";
                        viewModel.VisibilityAcept = Visibility.Hidden;
                        break;
                    default:
                        break;
                }

                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
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
                                InitView(ETypeDetailModel.CodeOTP);
                            }
                        }
                        else
                        {
                            Utilities.ShowModal(MessageResource.ConsignAmount, EModalType.Error);
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
                                InitView(ETypeDetailModel.CodeOTP);
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
                        await AdminPayPlus.SaveTransactions(this.transaction, true);

                        Utilities.CloseModal();

                        if (this.transaction.IdTransactionAPi == 0)
                        {
                            Utilities.ShowModal(MessageResource.NoProccessInformation, EModalType.Error);
                            Utilities.navigator.Navigate(UserControlView.Main);
                            DialogResult = false;
                        }
                        else
                        {
                            if (transaction.Type == ETransactionType.Withdrawal)
                            {
                                Utilities.navigator.Navigate(UserControlView.ReturnMony, false, transaction);
                                DialogResult = true;
                            }
                            else
                            {
                                Utilities.navigator.Navigate(UserControlView.Pay, false, transaction);
                                DialogResult = true;
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
    }
}
