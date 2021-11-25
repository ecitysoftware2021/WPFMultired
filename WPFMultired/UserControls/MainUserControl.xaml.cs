﻿using Grabador.Transaccion;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Resources;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para MainUserControl.xaml
    /// </summary>
    public partial class MainUserControl : UserControl
    {
        #region "Referencias"

        private List<string> _images;

        private ImageSleader _imageSleader;

        private bool _validatePaypad;

        #endregion

        #region "Constructor"
        public MainUserControl(bool validatePaypad = true)
        {
            InitializeComponent();

            CLSGrabador.FinalizarGrabacion();

            _validatePaypad = validatePaypad;

            Init();
        }
        #endregion

        #region "Métodos"
        private void Init()
        {
            try
            {
                ConfiguratePublish();
                AdminPayPlus.NotificateInformation();
                //AdminPayPlus.VerifyTransaction();
                InitValidation();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void InitValidation()
        {
            try
            {
                Task.Run(async() =>
                {
                    while (_validatePaypad)
                    {

                        //TODO: AdminPayPlus.ValidatePaypad ya esta validado no entiendo por que validan 2 veces
                        //var rsult = await AdminPayPlus.ValidatePaypad();

                        //if (rsult == true) 
                        //{
                        //    _validatePaypad = false;
                        //}

                        ValidateVersion();
                        Thread.Sleep(int.Parse(Utilities.GetConfiguration("DurationAlert")));
                    }
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ValidateVersion()
        {
            try
            {
                if (AdminPayPlus.DataPayPlus.StateUpdate)
                {
                    _validatePaypad = false;
                    Utilities.ShowModal(MessageResource.UpdateAplication, EModalType.Error, this, true);
                    Utilities.UpdateApp();
                }
                else _validatePaypad = false;

            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ConfiguratePublish()
        {
            try
            {
                if (_imageSleader == null)
                {
                    _imageSleader = new ImageSleader((List<String>)AdminPayPlus.DataPayPlus.ListImages, Utilities.GetConfiguration("PathPublish"));

                    this.DataContext = _imageSleader.imageModel;

                    _imageSleader.time = int.Parse(Utilities.GetConfiguration("TimerPublicity"));

                    _imageSleader.isRotate = true;

                    _imageSleader.Start();
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
        #endregion

        #region "Eventos"
        private void ValidateStatus()
        {
            try
            {
                if (AdminPayPlus.DataPayPlus.StateBalanece)
                {
                    Utilities.navigator.Navigate(UserControlView.Login, false, ETypeAdministrator.Balancing);
                }
                else if (AdminPayPlus.DataPayPlus.StateUpload)
                {
                    Utilities.navigator.Navigate(UserControlView.Login, false, ETypeAdministrator.Upload);
                }
                else if (AdminPayPlus.DataPayPlus.StateDiminish)
                {
                    Utilities.navigator.Navigate(UserControlView.Login, false, ETypeAdministrator.Diminish);
                }
                else if (AdminPayPlus.DataPayPlus.StateAceptance && AdminPayPlus.DataPayPlus.StateDispenser)
                {
                    int response = AdminPayPlus.PrintService.StatusPrint();

                    if (response != 0)
                    {
                        if (response == 7 || response == 8)
                        {
                            AdminPayPlus.SaveErrorControl(AdminPayPlus.PrintService.MessageStatus(response), MessageResource.InformationError, EError.Nopapper, ELevelError.Medium);
                        }
                        else
                        {
                            AdminPayPlus.SaveErrorControl(AdminPayPlus.PrintService.MessageStatus(response), MessageResource.InformationError, EError.Printer, ELevelError.Medium);
                        }

                        if (response != 8)
                        {
                            if (Utilities.ShowModal(MessageResource.ErrorNoPaper, EModalType.Information, this))
                            {
                                Redirect(true);
                            }
                        }
                        else
                        {
                            Redirect(true);
                        }
                    }
                    else
                    {
                        Redirect(true);
                    }
                }
                else
                {
                    _imageSleader.Stop();
                    Utilities.ShowModal(MessageResource.OutService, EModalType.Error, this);
                    Utilities.RestartApp();
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
        #endregion

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                ValidateStatus();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }


        private void Redirect(bool isSusses)
        {
            try
            {
                if (isSusses)
                {
                    _validatePaypad = false;
                    _imageSleader.Stop();
                    Utilities.navigator.Navigate(UserControlView.Menu, true);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}