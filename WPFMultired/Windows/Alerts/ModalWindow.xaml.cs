using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;

namespace WPFMultired.Windows
{
    /// <summary>
    /// Lógica de interacción para ModalWindow.xaml
    /// </summary>
    public partial class ModalWindow : Window
    {
        private ModalModel modal;

        public ModalWindow(ModalModel modal)
        {
            InitializeComponent();

            this.modal = modal;

            this.DataContext = this.modal;

            this.modal.userControl.Opacity = 0.2;

            ConfigureModal();
        }

        private void ConfigureModal()
        {
            try
            {
                if (this.modal.TypeModal == EModalType.Preload)
                {
                    this.BtnOk.Visibility = Visibility.Hidden;
                    this.BtnYes.Visibility = Visibility.Hidden;
                    this.BtnFinish.Visibility = Visibility.Hidden;
                    this.BtnEnterMony.Visibility = Visibility.Hidden;
                    this.BtnNo.Visibility = Visibility.Hidden;
                    this.LblMessageTouch.Visibility = Visibility.Hidden;
                    GifLoadder.Visibility = Visibility.Visible;
                }
                else if (this.modal.TypeModal == EModalType.NotExistAccount)
                {
                    this.BtnFinish.Visibility = Visibility.Hidden;
                    this.BtnEnterMony.Visibility = Visibility.Hidden;
                    this.BtnOk.Visibility = Visibility.Visible;
                    this.BtnYes.Visibility = Visibility.Hidden;
                    this.BtnNo.Visibility = Visibility.Hidden;
                    this.LblMessageTouch.Visibility = Visibility.Hidden;
                    GifLoadder.Visibility = Visibility.Hidden;
                    //InitTimer();
                }
                else if (this.modal.TypeModal == EModalType.Error)
                {
                    this.BtnFinish.Visibility = Visibility.Hidden;
                    this.BtnEnterMony.Visibility = Visibility.Hidden;
                    this.BtnOk.Visibility = Visibility.Hidden;
                    this.BtnYes.Visibility = Visibility.Hidden;
                    this.BtnNo.Visibility = Visibility.Hidden;
                    this.LblMessageTouch.Visibility = Visibility.Visible;
                    GifLoadder.Visibility = Visibility.Hidden;
                    //InitTimer();
                }
                else if (this.modal.TypeModal == EModalType.MaxAmount)
                {
                    this.BtnFinish.Visibility = Visibility.Visible;
                    this.BtnEnterMony.Visibility = Visibility.Visible;

                    this.BtnOk.Visibility = Visibility.Hidden;
                    this.BtnYes.Visibility = Visibility.Hidden;
                    this.LblMessageTouch.Visibility = Visibility.Hidden;
                    this.BtnNo.Visibility = Visibility.Hidden;
                    GifLoadder.Visibility = Visibility.Hidden;
                }
                else if (this.modal.TypeModal == EModalType.Information || this.modal.TypeModal == EModalType.NoPaper)
                {

                    this.BtnFinish.Visibility = Visibility.Hidden;
                    this.BtnEnterMony.Visibility = Visibility.Hidden;
                    this.BtnOk.Visibility = Visibility.Hidden;
                    this.BtnYes.Visibility = Visibility.Visible;
                    this.LblMessageTouch.Visibility = Visibility.Hidden;
                    this.BtnNo.Visibility = Visibility.Visible;
                    GifLoadder.Visibility = Visibility.Hidden;
                }
                else if (this.modal.TypeModal == EModalType.ReturnMoney)
                {

                    this.BtnFinish.Visibility = Visibility.Visible;
                    this.BtnEnterMony.Visibility = Visibility.Visible;
                    this.BtnOk.Visibility = Visibility.Hidden;
                    this.BtnYes.Visibility = Visibility.Hidden;
                    this.LblMessageTouch.Visibility = Visibility.Hidden;
                    this.LblMessageReturn.Visibility = Visibility.Visible;
                    this.BtnNo.Visibility = Visibility.Hidden;
                    GifLoadder.Visibility = Visibility.Hidden;
                    LblMessage.FontSize = 30;
                }
                //else if (this.modal.TypeModal == EModalType.FingerFail || this.modal.TypeModal == EModalType.FingerReintent)
                //{
                //    this.BtnFinish.Visibility = Visibility.Hidden;
                //    this.BtnEnterMony.Visibility = Visibility.Hidden;
                //    this.BtnOk.Visibility = Visibility.Hidden;
                //    this.BtnYes.Visibility = Visibility.Hidden;
                //    this.BtnNo.Visibility = Visibility.Hidden;
                //    this.LblMessageTouch.Visibility = Visibility.Visible;
                //    GifLoadder.Visibility = Visibility.Hidden;
                //    //InitTimer();
                //}

                GifLoadder.Visibility = Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Grid_TouchDown(object sender, TouchEventArgs e)
        {
            if (this.modal.TypeModal == EModalType.Error)
            {
                this.DialogResult = true;
            }
        }

        private void BtnOk_TouchDown(object sender, TouchEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnYes_TouchDown(object sender, TouchEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BtnNo_TouchDown(object sender, TouchEventArgs e)
        {
            this.DialogResult = false;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.modal.userControl.Opacity = 1;
        }
    }
}

