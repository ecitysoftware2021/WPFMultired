using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using WPFMultired.Classes;
using WPFMultired.Classes.Peripherals;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;
using WPFMultired.Windows.Alerts;


namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para ConsultUserControl.xaml
    /// </summary>
    public partial class ConsultUserControl : UserControl
    {
        private DetailViewModel viewModel;

        private Transaction transaction;

        private ReaderBarCode readerBarCode;

        private bool retiros;

        public ConsultUserControl(string company, string typeTransaction)
        {
            InitializeComponent();

            try
            {
                retiros = false;

                grvPublicity.Content = Utilities.UCPublicityBanner;

                if (transaction == null)
                {
                    transaction = new Transaction
                    {
                        CodeCompany = company,
                        CodeTypeTransaction = typeTransaction,
                        State = ETransactionState.Initial
                    };
                }

                if (readerBarCode == null)
                {
                    readerBarCode = new ReaderBarCode();
                }

                GoTime();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Init()
        {
            try
            {
                string codDepositos = Utilities.GetConfiguration("CodDepositos");
                string codEstadoCnt = Utilities.GetConfiguration("CodEstadoCuenta");
                string codTarjetaCrd = Utilities.GetConfiguration("CodTarjetaCredito");
                string codRetiros = Utilities.GetConfiguration("CodRetiros");

                if (transaction.CodeTypeTransaction == codDepositos)
                {
                    transaction.eTypeService = ETypeServiceSelect.Deposito;
                }
                else
                if (transaction.CodeTypeTransaction == codRetiros)
                {
                    transaction.eTypeService = ETypeServiceSelect.Retiros;
                    retiros = true;
                }
                else
                if (transaction.CodeTypeTransaction == codEstadoCnt)
                {
                    transaction.eTypeService = ETypeServiceSelect.EstadoCuenta;
                }
                else
                if (transaction.CodeTypeTransaction == codTarjetaCrd)
                {
                    transaction.eTypeService = ETypeServiceSelect.TarjetaCredito;
                    //txt_title.Width = 800;
                    //txt_title.Text = "Para iniciar, acerca el código de barras de tu extracto al lector del dispositivo o ingresa el número de identificación de la persona a la cual le realizarás la transacción";
                }

                ConfigView();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ConfigView()
        {
            try
            {
                viewModel = new DetailViewModel
                {
                    Row1 = "Tipo de Identificación",
                    Row2 = "Número de Identificación",
                    //OptionsEntries = new CollectionViewSource(),
                    OptionsList = new List<TypeDocument>(),
                    TypePayer = ETypePayer.Person,
                    VisibleId = System.Windows.Visibility.Visible,
                    VisibleInput = System.Windows.Visibility.Hidden
                };

                Task.Run(() =>
                {
                    viewModel.LoadListDocuments(transaction);
                    Thread.Sleep(2000);
                    Utilities.CloseModal();
                });

                Utilities.ShowModal("Estamos procesando la información, por favor espera un momento.", EModalType.Preload, this);

                cmb_type_id.SelectedIndex = 0;

                this.DataContext = viewModel;

                InicielizeBarcodeReader();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void InicielizeBarcodeReader()
        {
            try
            {
                readerBarCode.callbackOut = data =>
                {
                    Application.Current.Dispatcher.Invoke((Action)delegate
                    {
                        WPKeyboard.Keyboard.CloseKeyboard(this);
                    });
                    GC.Collect();

                    transaction.reference = data;
                    transaction.TypeDocument = "0";
                    Consult();
                };

                readerBarCode.callbackError = error =>
                {

                };

                readerBarCode.Start(Utilities.GetConfiguration("BarcodePort"), int.Parse(Utilities.GetConfiguration("BarcodeBaudRate")));
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private bool ValidateFields()
        {
            try
            {
                if (viewModel.Value1.Length < 6)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return false;
            }
        }

        private void Consult()
        {
            Task.Run(async () =>
            {
                try
                {
                    var response = new Services.Object.Response();
                    if (transaction.eTypeService == ETypeServiceSelect.Retiros)
                    {
                        this.transaction.Action = $"{(int)EFingerAction.Validate}";
                        this.transaction.Finger_Byte = "null";
                        response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Validate_Finger, this.transaction);
                    }
                    else
                    {
                        response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Consult_Invoice, this.transaction);
                    }

                    Utilities.CloseModal();

                    if (response != null && response.Data != null)
                    {
                        transaction = (Transaction)response.Data;
                        readerBarCode.Stop();
                        if (!retiros)
                        {
                            Utilities.navigator.Navigate(UserControlView.DataList, true, transaction);
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Fingerprint, true, transaction);
                        }
                    }
                    else
                    {
                        Utilities.ShowModal(response.Message, EModalType.Error, this);
                        viewModel.Value1 = string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    Utilities.CloseModal();
                    Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                }
            });

            PassBoxIdentification.Password = string.Empty;
            Utilities.ShowModal("Estamos procesando la información, por favor espera un momento.", EModalType.Preload, this);

        }

        private void Cmb_type_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var itemSeleted = (TypeDocument)cmb_type_id.SelectedItem;

                if (itemSeleted.Type != (int)viewModel.TypePayer)
                {
                    viewModel.Value1 = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_consult_TouchDown(object sender, TouchEventArgs e)
        {
            try
                {
                viewModel.Value1 = PassBoxIdentification.Password;
                if (ValidateFields())
                {
                    transaction.reference = viewModel.Value1;
                    transaction.TypeDocument = ((TypeDocument)cmb_type_id.SelectedItem).Key;
                    Consult();
                }
                else
                {
                    Utilities.ShowModal(MessageResource.InfoIncorrect, EModalType.Error, this);
                }
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
                readerBarCode.Stop();
                Utilities.navigator.Navigate(UserControlView.Main);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_back_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                readerBarCode.Stop();
                Utilities.navigator.Navigate(UserControlView.Menu, true);
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
                Init();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_show_id_TouchEnter(object sender, TouchEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(PassBoxIdentification.Password))
                {
                    viewModel.Value1 = PassBoxIdentification.Password;
                    viewModel.VisibleId = System.Windows.Visibility.Hidden;
                    viewModel.VisibleInput = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_show_id_TouchLeave(object sender, TouchEventArgs e)
        {
            try
            {
                viewModel.VisibleId = System.Windows.Visibility.Visible;
                viewModel.VisibleInput = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void PassBoxIdentification_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.OpenKeyboard(true, sender, this, 450);
        }

        private void btnQuestion_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.ShowModal(" \"Si el tipo de documento que seleccionaste es un NIT, digítalo con el número de verificación sin guiones ni comas\".", EModalType.Error, this);
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
