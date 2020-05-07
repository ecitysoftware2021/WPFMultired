﻿using System;
using System.Windows;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.ViewModel;
using System.Reflection;
using WPFMultired.Services.Object;
using System.Windows.Data;
using System.Collections.Generic;
using WPFMultired.Resources;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para PaymentUserControl.xaml
    /// </summary>
    public partial class PaymentUserControl : UserControl
    {
        private Transaction transaction;

        private PaymentViewModel paymentViewModel;

        public PaymentUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            OrganizeValues();
        }

        private void OrganizeValues()
        {
            try
            {
                //InitTimer();

                this.paymentViewModel = new PaymentViewModel
                {
                    PayValue = transaction.Amount,
                    ValorFaltante = transaction.Amount,
                    ImgContinue = Visibility.Hidden,
                    ImgCancel = Visibility.Visible,
                    ImgCambio = Visibility.Hidden,
                    ValorSobrante = 0,
                    ValorIngresado = 0,
                    viewList = new CollectionViewSource(),
                    Denominations = new List<DenominationMoney>(),
                    ValorDispensado = 0
                };

                this.DataContext = this.paymentViewModel;

                ActivateWallet();

                //SavePay();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void InitTimer()
        {
            try
            {
                TimerService.Close();
                TimerService.CallBackTimerOut = response =>
                {
                    AdminPayPlus.ControlPeripherals.StopAceptance();
                    AdminPayPlus.ControlPeripherals.callbackLog = null;
                    if (!this.paymentViewModel.StatePay)
                    {
                        if (paymentViewModel.ValorIngresado > 0)
                        {
                            transaction.Payment = paymentViewModel;
                            Utilities.navigator.Navigate(UserControlView.ReturnMony, false, this.transaction);
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                };

                TimerService.Start(int.Parse(Utilities.GetConfiguration("DurationAlert")));
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ActivateWallet()
        {
            try
            {
                Task.Run(() =>
                {
                    AdminPayPlus.ControlPeripherals.callbackValueIn = enterValue =>
                    {
                        TimerService.Reset();
                        if (enterValue.Item1 > 0)
                        {
                            if (!this.paymentViewModel.StatePay)
                            {
                                paymentViewModel.ValorIngresado += enterValue.Item1;
                                if (paymentViewModel.ValorIngresado >= transaction.Product.AmountMin)
                                {
                                    this.paymentViewModel.ImgContinue = Visibility.Visible;
                                    this.paymentViewModel.ImgCancel = Visibility.Hidden;
                                }
                                paymentViewModel.RefreshListDenomination(int.Parse(enterValue.Item1.ToString()), 1, enterValue.Item2);

                                AdminPayPlus.SaveDetailsTransaction(transaction.IdTransactionAPi, enterValue.Item1, 2, 1, enterValue.Item2, string.Empty);
                                LoadView();
                            }
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackTotalIn = enterTotal =>
                    {
                        TimerService.Stop();
                        if (!this.paymentViewModel.StatePay)
                        {
                            this.paymentViewModel.ImgContinue = Visibility.Hidden;

                            this.paymentViewModel.ImgCancel = Visibility.Hidden;

                            AdminPayPlus.ControlPeripherals.StopAceptance();

                            if (enterTotal > 0 && paymentViewModel.ValorSobrante > 0)
                            {
                                this.paymentViewModel.ImgCambio = Visibility.Visible;

                                ReturnMoney(paymentViewModel.ValorSobrante, true);
                            }
                            else
                            {
                                SavePay();
                            }
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackError = error =>
                    {
                        AdminPayPlus.SaveLog(new RequestLogDevice
                        {
                            Code = error.Item1,
                            Date = DateTime.Now,
                            Description = error.Item2,
                            Level = ELevelError.Medium,
                            TransactionId = transaction.IdTransactionAPi
                        }, ELogType.Device);

                        if (error.Item2.Contains("FATAL"))
                        {
                            Utilities.ShowModal(MessageResource.ErrorPayment, EModalType.Error);

                            this.paymentViewModel.PayValue = this.paymentViewModel.ValorIngresado;

                            AdminPayPlus.ControlPeripherals.StopAceptance();

                            transaction.Observation += MessageResource.NoContinue;
                            if (!this.paymentViewModel.StatePay)
                            {
                                SavePay(ETransactionState.Error);
                            }
                        }
                    };

                    AdminPayPlus.ControlPeripherals.StartAceptance(paymentViewModel.PayValue);
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ReturnMoney(decimal returnValue, bool state)
        {
            try
            {
                AdminPayPlus.ControlPeripherals.callbackTotalOut = totalOut =>
                {
                    if (state)
                    {
                        paymentViewModel.ValorDispensado = totalOut;
                        SavePay();
                    }
                };

                AdminPayPlus.ControlPeripherals.callbackLog = log =>
                {
                    paymentViewModel.SplitDenomination(log);
                    AdminPayPlus.SaveDetailsTransaction(transaction.IdTransactionAPi, 0, 0, 0, string.Empty, log);
                };

                AdminPayPlus.ControlPeripherals.callbackOut = valueOut =>
                {
                    AdminPayPlus.ControlPeripherals.callbackOut = null;
                    if (state)
                    {
                        paymentViewModel.ValorDispensado = valueOut;

                        if (paymentViewModel.ValorDispensado == paymentViewModel.ValorSobrante)
                        {
                            SavePay();
                        }
                        else
                        {
                            transaction.Observation += MessageResource.IncompleteMony;
                            Utilities.ShowModal(MessageResource.IncompleteMony, EModalType.Error);
                            SavePay(ETransactionState.Error);
                        }
                    }
                };

                AdminPayPlus.ControlPeripherals.StartDispenser(returnValue);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void LoadView()
        {
            try
            {
                Dispatcher.BeginInvoke((Action)delegate
                {
                    paymentViewModel.viewList.Source = paymentViewModel.Denominations;
                    lv_denominations.DataContext = paymentViewModel.viewList;
                    lv_denominations.Items.Refresh();
                });
                GC.Collect();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async void SavePay(ETransactionState statePay = ETransactionState.Initial)
        {
            try
            {
                if (!this.paymentViewModel.StatePay)
                {
                    this.paymentViewModel.StatePay = true;
                    transaction.Payment = paymentViewModel;
                    transaction.State = statePay;

                    AdminPayPlus.ControlPeripherals.ClearValues();
                    
                    if (transaction.IdTransactionAPi > 0)
                    {
                        Task.Run(() =>
                        {
                            this.transaction.Amount = paymentViewModel.ValorIngresado;
                            var response = AdminPayPlus.ApiIntegration.CallService(ETypeService.Report_Transaction, transaction);
                            Utilities.CloseModal();
                            if (response != null)
                            {
                                transaction.State = ETransactionState.Success;
                            }
                            else
                            {
                                transaction.State = ETransactionState.ErrorService;
                            }
                            Utilities.navigator.Navigate(UserControlView.PaySuccess, false, this.transaction);
                        });
                    }
                    else
                    {
                        AdminPayPlus.SaveErrorControl(MessageResource.NoInsertTransaction, this.transaction.TransactionId.ToString(), EError.Api, ELevelError.Strong);
                        Utilities.ShowModal(MessageResource.NoInsertTransaction, EModalType.Error);

                        if (this.paymentViewModel.ValorIngresado > 0)
                        {
                            Utilities.navigator.Navigate(UserControlView.ReturnMony, false, this.transaction);
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void BtnCancell_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                this.paymentViewModel.ImgContinue = Visibility.Hidden;

                this.paymentViewModel.ImgCancel = Visibility.Hidden;

                if (Utilities.ShowModal(MessageResource.CancelTransaction, EModalType.Information))
                {
                    AdminPayPlus.ControlPeripherals.StopAceptance();
                    AdminPayPlus.ControlPeripherals.callbackLog = null;
                    if (!this.paymentViewModel.StatePay)
                    {
                        if (paymentViewModel.ValorIngresado > 0)
                        {
                            transaction.Payment = paymentViewModel;
                            Utilities.navigator.Navigate(UserControlView.ReturnMony, false, this.transaction);
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                }
                else
                {
                    if (paymentViewModel.ValorIngresado > 0)
                    {
                        this.paymentViewModel.ImgContinue = Visibility.Visible;
                    }

                    this.paymentViewModel.ImgCancel = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void BtnConsign_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {
            //StopTimer();

            this.paymentViewModel.ImgContinue = Visibility.Hidden;

            this.paymentViewModel.ImgCancel = Visibility.Hidden;

            if (Utilities.ShowModal("¿Desea depositar el monto ingresado?", EModalType.Information))
            {
                this.paymentViewModel.PayValue = this.paymentViewModel.ValorIngresado;

                AdminPayPlus.ControlPeripherals.StopAceptance();
                AdminPayPlus.ControlPeripherals.callbackLog = null;

                SavePay();
            }
            else
            {
               // InitTimer();

                this.paymentViewModel.ImgContinue = Visibility.Visible;

                this.paymentViewModel.ImgCancel = Visibility.Hidden;
            }
        }
    }
}
