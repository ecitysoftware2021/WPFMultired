using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Classes.Peripherals;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;

//     @
//    <))>
//    _/\_
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

        public ConsultUserControl(string company, string typeTransaction)
        {
            InitializeComponent();

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
        }

        private void Init()
        {
            try
            {//TODO:config|
                switch (transaction.CodeTypeTransaction)
                {
                    case "00011":
                        transaction.eTypeService = ETypeServiceSelect.Deposito;
                        break;
                    case "00013":
                        transaction.eTypeService = ETypeServiceSelect.EstadoCuenta;
                        break;
                    case "00014":
                        transaction.eTypeService = ETypeServiceSelect.TarjetaCredito;
                        break;
                    //default:
                    //    transaction.eTypeService = ETypeServiceSelect.TarjetaCredito;
                    //    break;
                }

                ConfigView();
            }
            catch (Exception ex)
            {
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

                Task.Run(() => {
                    viewModel.LoadListDocuments(transaction);
                    Utilities.CloseModal();
                });

                Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload);

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
            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Consult_Invoice, this.transaction);
                        if (response.Data != null)
                        {
                            transaction = (Transaction)response.Data;
                            Utilities.CloseModal();
                            readerBarCode.Stop();
                            Utilities.navigator.Navigate(UserControlView.DataList, false, transaction);
                        }
                        else 
                        {
                            Utilities.CloseModal();
                            Utilities.ShowModal(response.Message ?? MessageResource.ErrorCoincidences, EModalType.Error);
                            viewModel.Value1 = string.Empty;

                            //Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.CloseModal();
                        Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                    }
                });
                PassBoxIdentification.Password = string.Empty;
                Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload);
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
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
                    Utilities.ShowModal(MessageResource.InfoIncorrect, EModalType.Error);
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
    }
}
