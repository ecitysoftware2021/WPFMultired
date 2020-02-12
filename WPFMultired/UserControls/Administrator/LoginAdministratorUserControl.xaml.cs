using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Resources;

namespace WPFMultired.UserControls.Administrator
{
    /// <summary>
    /// Lógica de interacción para LoginAdministratorUserControl.xaml
    /// </summary>
    public partial class LoginAdministratorUserControl : UserControl
    {
        #region "Referencias"

        private string _messageError;

        private string _name;

        private string _pass;

        private ETypeAdministrator _typeOperation;

        // ReSharper disable once UnassignedGetOnlyAutoProperty

        #endregion

        #region "Constructor"

        public LoginAdministratorUserControl(ETypeAdministrator typeOperation)
        {
            InitializeComponent();
            _typeOperation = typeOperation;
            //Search();
        }

        #endregion

        #region "Eventos"

        private void BtnConsult_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
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
                if (!Validation())
                {
                   Utilities.ShowModal(_messageError, EModalType.Error, false);

                    //var data = await AdminPayPlus.DataListPaypad(_typeOperation);

                    ////Utilities.navigator.Navigate(UserControlView.Admin, false, data, _typeOperation);

                    //var dataContol = await AdminPayPlus.UpdateAdminProcess(data);

                    //Utilities.PrintVoucher(dataContol);
                }
                else
                {
                    _name = TxtUser.Text;

                    _pass = TxtPassword.Password;

                    load_gif.Visibility = Visibility.Visible;
                    IsEnabled = false;

                    var response = await AdminPayPlus.ValidateUser(_name, _pass);

                    load_gif.Visibility = Visibility.Hidden;
                    IsEnabled = true;
                    if (response <= 0)
                    {
                        Utilities.ShowModal(MessageResource.ErrorDates, EModalType.Error, false);
                    }
                    else
                    {
                        var data = await AdminPayPlus.DataListPaypad(_typeOperation);
                        if (data != null)
                        {
                            data.USER_ADMIN_ID = response;
                            Utilities.navigator.Navigate(UserControlView.Admin, false, data, _typeOperation);
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
                    _messageError = string.Format(MessageResource.EnterField, "Usuario");
                    return false;
                }
                if (string.IsNullOrEmpty(TxtPassword.Password))
                {
                    _messageError = string.Format(MessageResource.EnterField, "Contraseña");
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
    }
}