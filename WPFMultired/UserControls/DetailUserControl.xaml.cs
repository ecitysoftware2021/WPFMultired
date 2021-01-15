﻿using Grabador.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Interaction logic for DetailUserControl.xaml
    /// </summary>
    public partial class DetailUserControl : UserControl
    {
        private Transaction transaction;

        public DetailUserControl(Transaction transaction)
        {
            InitializeComponent();

            grvPublicity.Content = Utilities.UCPublicityBanner;

            this.transaction = transaction;

            this.DataContext = transaction.Product;

            GoTime();
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

        private void Btn_back_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
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

        private void btn_Accept_TouchDown(object sender, TouchEventArgs e)
        {
            SendData();
        }

        private async void SendData()
        {
            try
            {
                if (transaction.Amount > 0)
                {
                    Task.Run(async () =>
                    {
                        await AdminPayPlus.SaveTransactions(this.transaction, false);

                        Utilities.CloseModal();

                        if (this.transaction.IdTransactionAPi == 0)
                        {
                            Utilities.ShowModal(MessageResource.NoProccessInformation, EModalType.Error,this);
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                        else
                        {
                            Task.Run(() =>
                            {
                                CLSGrabador.IniciarGrabacion(new DataVidio
                                {
                                    paypadID = AdminPayPlus.DataConfiguration.ID_PAYPAD.Value,
                                    mailAlert = $"'{Utilities.GetConfiguration("Email")}'",
                                    transactionID = transaction.IdTransactionAPi,
                                    RecorderRoute = Utilities.GetConfiguration("RecorderRoute"),
                                    selectedCamera = 0,
                                    videoPath = $"'{Utilities.GetConfiguration("VideoRoute")}'"
                                });
                                Thread.Sleep(500);
                                CLSGrabador.IniciarGrabacion(new DataVidio
                                {
                                    paypadID = AdminPayPlus.DataConfiguration.ID_PAYPAD.Value,
                                    mailAlert = $"'{Utilities.GetConfiguration("Email")}'",
                                    transactionID = transaction.IdTransactionAPi,
                                    RecorderRoute = Utilities.GetConfiguration("RecorderRoute"),
                                    selectedCamera = 1,
                                    videoPath = $"'{Utilities.GetConfiguration("VideoRoute")}'"
                                });
                            });

                            Utilities.navigator.Navigate(UserControlView.Pay, false, transaction);
                        }
                    });
                    Utilities.ShowModal("Estamos procesando la información, por favor espera un momento.", EModalType.Preload,this);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void BtnCancell_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.navigator.Navigate(UserControlView.Main);
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
