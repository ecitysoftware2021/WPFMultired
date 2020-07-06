using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Classes.Peripherals;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls.Administrator
{
    /// <summary>
    /// Lógica de interacción para LoginAdministratorUserControl.xaml
    /// </summary>
    public partial class LoginAdministratorUserControl : UserControl
    {
        #region "Referencias"


        private LoginViewModel viewModel;

        private ReaderBarCode readerBarCode;

        // ReSharper disable once UnassignedGetOnlyAutoProperty

        #endregion

        #region "Constructor"

        public LoginAdministratorUserControl(ETypeAdministrator typeOperation)
        {
            InitializeComponent();

            if (readerBarCode == null)
            {
                readerBarCode = new ReaderBarCode();
            }

            InitView(typeOperation);
        }

        private void InitView(ETypeAdministrator typeOperation)
        {
            viewModel = new LoginViewModel
            {
                TypeOperation = typeOperation,
                TypeLogin = 1,
                Pass = string.Empty,
                User = string.Empty,
                Qr = string.Empty,
                VisibleBtnAcept = Visibility.Hidden,
                VisibleGdLogin = Visibility.Hidden,
                VisibleGdQr = Visibility.Visible,
                IsReadQr = false,
            };

            this.DataContext = viewModel;

            InicielizeBarcodeReader();
        }

        private void InicielizeBarcodeReader()
        {
            try
            {
                readerBarCode.callbackOut = data =>
                {
                    if (!viewModel.IsReadQr)
                    {
                        viewModel.IsReadQr = true;
                        viewModel.TypeLogin = 1;
                        viewModel.User = string.Empty;
                        viewModel.Pass = string.Empty;
                        viewModel.Qr = data;
                        Search();
                    }
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

        #endregion

        #region "Eventos"

        private void BtnConsult_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                viewModel.TypeLogin = 2;
                viewModel.Qr = string.Empty;
                viewModel.Pass = TxtPassword.Password;
                Search();   
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        /// <summary>
        /// Mmetodoc Cancelar Touch
        /// </summary>
        /// <param name="sender">Refrenecia al objecto</param>
        /// <param name="e">Eventos del objeto</param>
        private void BtnCancell_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                readerBarCode.Stop();
                Utilities.navigator.Navigate(UserControlView.Config);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        #endregion

        #region "Métodos"

        private async void Search()
        {
            try
            {
                if (viewModel.TypeLogin == 2 && !Validation())
                {
                   Utilities.ShowModal(viewModel.MessageError, EModalType.Error, false);

                  // var data = await AdminPayPlus.DataListPaypad(viewModel.TypeOperation);

                    ////Utilities.navigator.Navigate(UserControlView.Admin, false, data, _typeOperation);

                   // var dataContol = await AdminPayPlus.UpdateAdminProcess(data);

                    //Utilities.PrintVoucher(dataContol);
                }
                else
                {
                    Task.Run(async () =>
                    {
                        var response = await AdminPayPlus.ValidateUser(viewModel.User, viewModel.Pass, viewModel.Qr);

                        if (response == null ||  response.Item1 == 0)
                        {
                            Utilities.CloseModal();
                            if (response != null && !string.IsNullOrEmpty(response.Item2))
                            {
                                Utilities.ShowModal(response.Item2, EModalType.Error, false);

                            }
                            else
                            {
                                Utilities.ShowModal(MessageResource.ErrorDates, EModalType.Error, false);
                            }

                            viewModel.IsReadQr = false;
                            ClearForm();
                        }
                        else
                        {
                            var data = await AdminPayPlus.DataListPaypad(viewModel.TypeOperation);
                            if (data != null)
                            {
                                Utilities.CloseModal();
                                data.USER_ADMIN_ID = response.Item1;
                                readerBarCode.Stop();
                               Utilities.navigator.Navigate(UserControlView.Admin, false, data, viewModel.TypeOperation);
                            }
                            else
                            {
                                Utilities.CloseModal();
                                Utilities.ShowModal(MessageResource.NoInformation, EModalType.Error, false);
                                viewModel.IsReadQr = false;
                                ClearForm();
                            }
                        }
                    });

                    Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload, false);
                }
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private bool Validation()
        {
            try
            {
                if (string.IsNullOrEmpty(TxtUser.Text))
                {
                    viewModel.MessageError = string.Format(MessageResource.EnterField, "Usuario");
                    return false;
                }
                if (string.IsNullOrEmpty(TxtPassword.Password))
                {
                    viewModel.MessageError = string.Format(MessageResource.EnterField, "Contraseña");
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
        #endregion

        private void Btn_no_qr_TouchDown(object sender, TouchEventArgs e)
        {
            if (viewModel.TypeLogin == 1)
            {
                viewModel.TypeLogin = 2;
                viewModel.VisibleGdLogin = Visibility.Visible;
                viewModel.VisibleGdQr = Visibility.Hidden;
                viewModel.VisibleBtnAcept = Visibility.Visible;
            }
            else
            {
                viewModel.TypeLogin = 1;
                viewModel.VisibleGdLogin = Visibility.Hidden;
                viewModel.VisibleGdQr = Visibility.Visible;
                viewModel.VisibleBtnAcept = Visibility.Hidden;
            }

            ClearForm();
        }

        private void ClearForm()
        {
            viewModel.User = string.Empty;
            viewModel.Pass = string.Empty;
            viewModel.Qr = string.Empty;
        }
    }
}