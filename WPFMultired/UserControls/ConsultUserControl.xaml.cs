using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.Services.Object;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para ConsultUserControl.xaml
    /// </summary>
    public partial class ConsultUserControl : UserControl
    {
        private Transaction transaction;

        private DetailViewModel viewModel;

        public ConsultUserControl()
        {
            InitializeComponent();
        }

        private void ConfigView()
        {
            try
            {
                viewModel = new DetailViewModel
                {
                    Row1 = "Identificación *",
                    Row2 = "Nombre *",
                    Row3 = "Celular * ",
                    OptionsEntries = new CollectionViewSource(),
                    OptionsList = new List<TypeDocument>(),
                    TypePayer = ETypePayer.Person
                };

                viewModel.LoadListDocuments();

                cmb_type_id.SelectedIndex = 0;

                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_payment_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {

        }

        private bool ValidateFields()
        {
            try
            {
                if (viewModel.Value1.Length < 6)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(viewModel.Value2))
                {
                    return false;
                }

                if (viewModel.Value3.Length < 6)
                {
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

        private void Consult(string typeDocument)
        {
            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        var response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Products_Client, this.transaction);

                        if (response != null)
                        {
                            await AdminPayPlus.SaveTransactions(this.transaction, false);

                            Utilities.CloseModal();
                        }
                        else
                        {
                            Utilities.CloseModal();
                            Utilities.ShowModal(MessageResource.ErrorTransaction, EModalType.Error);
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                    catch (Exception ex)
                    {
                        Utilities.CloseModal();
                        Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                    }
                });
                Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload);
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
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

        private void Cmb_type_id_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var itemSeleted = (TypeDocument)cmb_type_id.SelectedItem;

                if (itemSeleted.Type != (int)viewModel.TypePayer)
                {
                    if (itemSeleted.Type == (int)ETypePayer.Person)
                    {
                        viewModel.TypePayer = ETypePayer.Person;
                        viewModel.Row2 = "Nombre *";
                    }
                    else
                    {
                        viewModel.TypePayer = ETypePayer.Establishment;
                        viewModel.Row2 = "Razón Social*";
                    }

                    viewModel.Value1 = string.Empty;
                    viewModel.Value2 = string.Empty;
                    viewModel.Value3 = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_consult_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (ValidateFields())
                {

                }
                else
                {
                    Utilities.ShowModal(MessageResource.InfoIncorrect, EModalType.Error);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
