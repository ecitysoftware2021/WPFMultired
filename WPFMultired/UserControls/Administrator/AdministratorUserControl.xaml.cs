using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Resources;
using WPFMultired.Services.Object;

namespace WPFMultired.UserControls.Administrator
{
    /// <summary>
    /// Lógica de interacción para AdministratorUserControl.xaml
    /// </summary>
    public partial class AdministratorUserControl : UserControl
    {
        #region "Referencias"
        private PaypadOperationControl dataContol;

        private int type = 1;

        private ETypeAdministrator typeOperation;
        #endregion

        #region "Constructor"
        public AdministratorUserControl(PaypadOperationControl dataContol, ETypeAdministrator typeOperation)
        {
            InitializeComponent();

            this.dataContol = dataContol;

            this.typeOperation = typeOperation;

            InitView();
        }
        #endregion

        #region "ListView"
        private void InitView()
        {
            try
            {
                this.DataContext = dataContol;

                if (typeOperation == ETypeAdministrator.Balancing || typeOperation == ETypeAdministrator.Diminish)
                {
                    txtDescription.Text = MessageResource.RemoveMonyAceptance;
                }
                else
                {
                    txtDescription.Text = MessageResource.EnterMonyDispenser;
                }

                RefreshList();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void RefreshList()
        {
            try
            {
                dataContol.viewList = new CollectionViewSource();

                if (this.type == 1 && this.typeOperation == ETypeAdministrator.Balancing)
                {
                    var dataList = dataContol.DATALIST.Where(x => (x.DEVICE_TYPE_ID == (int)ETypeDevice.AP || x.DEVICE_TYPE_ID == (int)ETypeDevice.MA) && (x.AMOUNT_NEW + x.AMOUNT) > 0).ToList();

                    if (dataList.Count > 0)
                    {
                        dataContol.viewList.Source = dataList;
                    }
                    else
                    {
                        ChangeView();
                        return;
                    }
                }
                else if (this.type == 2 && this.typeOperation == ETypeAdministrator.Balancing)
                {
                    dataContol.viewList.Source = dataContol.DATALIST.Where(x => (x.DEVICE_TYPE_ID == (int)ETypeDevice.DP || x.DEVICE_TYPE_ID == (int)ETypeDevice.MD) && (x.AMOUNT_NEW + x.AMOUNT) > 0).ToList();
                }
                else
                {
                    dataContol.viewList.Source = dataContol.DATALIST;
                }

                Dispatcher.BeginInvoke((Action)delegate
                {
                    lv_denominations.DataContext = dataContol.viewList;
                    lv_denominations.Items.Refresh();
                });
                GC.Collect();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
        #endregion

        private void BtnNext_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (type == 1 && typeOperation == ETypeAdministrator.Balancing)
                {
                    ChangeView();
                }
                else
                {
                    UpdateDataControl();
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ChangeView()
        {
            type = 2;
            txtDescription.Text = MessageResource.RemoveMonyDispenser;
            RefreshList();
        }

        //private void BtnCancell_TouchDown(object sender, TouchEventArgs e)
        //{
        //    try
        //    {
        //        Utilities.navigator.Navigate(UserControlView.Config);
        //    }
        //    catch (Exception ex)
        //    {
        //        Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
        //    }
        //}


        #region "Eventos"
        private async void UpdateDataControl()
        {
            try
            {
                Task.Run(async () =>
                {

                    this.dataContol.viewList = null;

                    this.dataContol = await AdminPayPlus.UpdateAdminProcess(dataContol);

                    Thread.Sleep(2000);

                    if (this.dataContol != null)
                    {

                        Utilities.PrintVoucher(this.dataContol);

                        Thread.Sleep(6000);

                        Utilities.CloseModal();

                        Utilities.ShowModal(MessageResource.TransactionFinish, EModalType.Error, false);

                        Utilities.RestartApp();
                    }
                    else
                    {
                        Utilities.CloseModal();
                        Utilities.ShowModal(MessageResource.ErrorTransaction, EModalType.Error, false);
                        Utilities.RestartApp();
                    }
                });

                Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload, false);
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                Utilities.RestartApp();
            }
        }
        #endregion
    }
}