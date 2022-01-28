﻿using Grabador.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.Services.Object;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para CancelUserControl.xaml
    /// </summary>
    public partial class ReturnMonyUserControl : UserControl
    {
        private Transaction transaction;

        private PaymentViewModel viewModel;

        public ReturnMonyUserControl(Transaction transaction)
        {
            InitializeComponent();
            this.transaction = transaction;
            grvPublicity.Content = Utilities.UCPublicityBanner;
            _ = OrganizeValuesAsync();
        }

        private async Task OrganizeValuesAsync()
        {
            try
            {
                if (transaction.Payment == null && transaction.Type == ETransactionType.Withdrawal)
                {
                    this.viewModel = new PaymentViewModel
                    {
                        PayValue = 0,
                        ValorFaltante = 0,
                        ValorSobrante = Utilities.RoundValue(transaction.Amount),
                        ValorIngresado = 0,
                        ValorDispensado = 0,
                        StatePay = false,
                        Denominations = new List<DenominationMoney>(),
                        Message = MessageResource.MessageReturnMony
                    };

                    transaction.Payment = viewModel;
                }
                else
                {
                    if (transaction.Payment != null)
                    {
                        viewModel = transaction.Payment;
                        viewModel.StatePay = false;
                        viewModel.ValorSobrante = transaction.Payment.ValorIngresado - transaction.Payment.ValorDispensado;
                        viewModel.Message = MessageResource.TransactionCancel;
                    }
                }

                this.DataContext = this.viewModel;

                await ReturnMoneyAsync();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async Task ReturnMoneyAsync()
        {
            try
            {
                await Task.Run(() =>
                {
                    AdminPayPlus.ControlPeripherals.callbackOut = valueOut =>
                    {
                        AdminPayPlus.ControlPeripherals.callbackOut = null;
                        if (!this.viewModel.StatePay)
                        {
                            viewModel.ValorDispensado += valueOut;

                            if (viewModel.ValorDispensado != viewModel.ValorSobrante)
                            {
                                if (transaction.Type != ETransactionType.Withdrawal)
                                {
                                    transaction.State = ETransactionState.CancelError;
                                }
                                else
                                {
                                    transaction.State = ETransactionState.Error;
                                }
                            }

                            FinishCancelPay();
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackTotalOut = totalOut =>
                    {
                        if (!this.viewModel.StatePay)
                        {
                            viewModel.ValorDispensado += totalOut;

                            if (transaction.Type != ETransactionType.Withdrawal)
                            {
                                viewModel.ValorSobrante = viewModel.ValorIngresado;
                                transaction.State = ETransactionState.Cancel;
                            }

                            FinishCancelPay();
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackLog = (log, isBX) =>
                    {
                        viewModel.SplitDenomination(log, isBX);
                        AdminPayPlus.SaveDetailsTransaction(transaction.IdTransactionAPi, 0, 0, 0, string.Empty, log);
                    };

                    AdminPayPlus.ControlPeripherals.callbackError = error =>
                    {
                        var log = new RequestLogDevice
                        {
                            Code = error.Item1,
                            Date = DateTime.Now,
                            Description = error.Item2,
                            Level = ELevelError.Medium,
                            TransactionId = transaction.IdTransactionAPi
                        };

                        if (error.Item1.Equals("Info"))
                        {
                            log.Level = ELevelError.Mild;
                            log.Code = "";
                            AdminPayPlus.SaveLog(log, ELogType.Device);
                        }
                        else
                        {
                            AdminPayPlus.SaveLog(log, ELogType.Device);
                        };
                    };

                    AdminPayPlus.ControlPeripherals.StartDispenser(transaction.Payment.ValorSobrante);
                });
            }
            
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void FinishCancelPay()
        {
            try
            {
                if (!this.viewModel.StatePay)
                {
                    this.viewModel.StatePay = true;
                    transaction.Payment = viewModel;

                    AdminPayPlus.ControlPeripherals.ClearValues();

                    if (transaction.Type == ETransactionType.Withdrawal)
                    {
                        Task.Run(async () =>
                        {
                            transaction.Amount = viewModel.ValorDispensado;

                            var response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Report_Transaction, transaction);

                            Utilities.CloseModal();

                            if (response.Data != null)
                            {
                                if (this.viewModel.ValorDispensado >= transaction.Payment.ValorSobrante)
                                {
                                    transaction = (Transaction)response.Data;
                                }
                                else
                                {
                                    transaction.StateNotification = 1;
                                    Utilities.ShowModal(MessageResource.IncompleteMony, EModalType.Error, this);
                                }

                                transaction.State = ETransactionState.Success;
                            }
                            else
                            {
                                // Error en el servicio "Reportar transaccion"
                                AdminPayPlus.SaveErrorControl(MessageResource.NoInsertTransaction, this.transaction.TransactionId.ToString(), EError.Api, ELevelError.Strong);
                                Utilities.ShowModal(MessageResource.NoInsertTransaction, EModalType.Error, this);
                                transaction.State = ETransactionState.ErrorService;
                            }

                            if (transaction.IdTransactionAPi > 0)
                            {
                                Utilities.navigator.Navigate(UserControlView.PaySuccess, false, this.transaction);
                            }
                            else
                            {
                                AdminPayPlus.SaveErrorControl(MessageResource.NoInsertTransaction, this.transaction.TransactionId.ToString(), EError.Api, ELevelError.Strong);
                                Utilities.ShowModal(MessageResource.NoInsertTransaction, EModalType.Error, this);

                                Utilities.navigator.Navigate(UserControlView.PaySuccess, false, this.transaction);
                            }
                        });

                        Utilities.ShowModal(MessageResource.FinishTransaction, EModalType.Preload, this);
                    }
                    else
                    {
                        transaction.StateNotification = 1;

                        Task<Response> Task_Report_Cash = AdminPayPlus.ApiIntegration.CallService(ETypeService.Report_Cash, transaction);

                        if (!Task_Report_Cash.IsFaulted)
                        {
                            AdminPayPlus.UpdateTransaction(transaction);

                            Utilities.PrintVoucher(this.transaction);

                            Thread.Sleep(8000);

                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                        else
                        {
                            AdminPayPlus.SaveErrorControl(MessageResource.NoInsertTransaction, this.transaction.TransactionId.ToString(), EError.Api, ELevelError.Strong);
                            Utilities.ShowModal(MessageResource.NoInsertTransaction, EModalType.Error, this);
                            transaction.State = ETransactionState.ErrorService;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
