using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
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

        // ReSharper disable once UnassignedGetOnlyAutoProperty

        #endregion

        #region "Constructor"

        public LoginAdministratorUserControl(ETypeAdministrator typeOperation)
        {
            InitializeComponent();

            
            InitView(typeOperation);

           // Search();
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
            txt_qr.Focus();
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

                   //var data = await AdminPayPlus.DataListPaypad(_typeOperation);

                    ////Utilities.navigator.Navigate(UserControlView.Admin, false, data, _typeOperation);

                    //var dataContol = await AdminPayPlus.UpdateAdminProcess(data);

                    //Utilities.PrintVoucher(dataContol);
                }
                else
                {
                    load_gif.Visibility = Visibility.Visible;
                    IsEnabled = false;
                    viewModel.Qr = "Serrato";
                    var response = await AdminPayPlus.ValidateUser(viewModel.User, viewModel.Pass, viewModel.Qr);
                    
                    load_gif.Visibility = Visibility.Hidden;
                    IsEnabled = true;

                    if (response <= 0)
                    {
                        Utilities.ShowModal(MessageResource.ErrorDates, EModalType.Error, false);
                    }
                    else
                    {
                        var data = await AdminPayPlus.DataListPaypad(viewModel.TypeOperation);
                        if (data != null)
                        {
                            data.USER_ADMIN_ID = response;
                            Utilities.navigator.Navigate(UserControlView.Admin, false, data, viewModel.TypeOperation);
                        }
                        else
                        {
                            Utilities.ShowModal(MessageResource.NoInformation, EModalType.Error, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
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

            viewModel.User = string.Empty;
            viewModel.Pass = string.Empty;
            viewModel.Qr = string.Empty;
        }

        private void Txt_qr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !viewModel.IsReadQr)
            { 
                viewModel.IsReadQr = true;
                viewModel.TypeLogin = 1;
                viewModel.User = string.Empty;
                viewModel.Pass = string.Empty;
                Search();
            }
        }
    }
}