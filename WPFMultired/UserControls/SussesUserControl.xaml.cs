using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.Services.Object;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para SussesUserControl.xaml
    /// </summary>
    public partial class SussesUserControl : UserControl
    {
        private Transaction transaction;

        public SussesUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            FinishTrnsaction();
        }

        private void FinishTrnsaction()
        {
            try
            {
                if (transaction.State == ETransactionState.Success)
                {
                    AdminPayPlus.SaveLog(new RequestLog
                    {
                        Description = MessageResource.SussesTransaction,
                        Reference = transaction.reference
                    }, ELogType.General);
                }
                else
                {
                    AdminPayPlus.SaveLog(new RequestLog
                    {
                        Description = MessageResource.NoveltyTransation,
                        Reference = transaction.reference
                    }, ELogType.General);
                }

                GC.Collect();

                Task.Run(() =>
                {
                    AdminPayPlus.UpdateTransaction(this.transaction);

                    Thread.Sleep(2000);

                    Utilities.PrintVoucher(this.transaction);

                    Thread.Sleep(6000);

                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        if (transaction.State == ETransactionState.Error)
                        {
                            Utilities.RestartApp();
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }

                    });
                    GC.Collect();
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}