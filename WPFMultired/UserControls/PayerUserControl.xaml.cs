using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using WPFMultired.Classes;
using WPFMultired.Classes.UseFull;
using WPFMultired.DataModel;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para PayerUserControl.xaml
    /// </summary>
    public partial class PayerUserControl : UserControl
    {
        private DetailViewModel viewModel;

        private Transaction transaction;

        private ReaderBarCode readerBarCode;

        public PayerUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            if (readerBarCode == null)
            {
                readerBarCode = new ReaderBarCode();
            }

            ConfigView();
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

                viewModel.LoadList();
                cmb_type_id.SelectedIndex = 0;

                this.DataContext = viewModel;

                InicielizeBarcodeReader();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void InicielizeBarcodeReader()
        {
            try
            {
                readerBarCode.callbackOut = data =>
                {
                    if (data != null && viewModel.TypePayer == ETypePayer.Person)
                    {
                        viewModel.Value1 = data.Document;
                        viewModel.Value2 = string.Concat(data.FirstName, " ", data.LastName, " ", data.SecondLastName);
                        viewModel.Value3 = string.Empty;
                    }
                };

                readerBarCode.callbackError = error =>
                {

                };

                readerBarCode.Start(Utilities.GetConfiguration("BarcodePort"), int.Parse(Utilities.GetConfiguration("BarcodeBaudRate")));
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_payment_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                if (ValidateFields())
                {
                    SaveTransaction(((TypeDocument)cmb_type_id.SelectedItem).Key);
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

        private void SaveTransaction(string typeDocument)
        {
            try
            {
                Task.Run(async () =>
                {
                    try
                    {
                        transaction.payer = new PAYER
                        {
                            IDENTIFICATION = viewModel.Value1,
                            NAME = viewModel.Value2,
                            PHONE = decimal.Parse(viewModel.Value3),
                            TYPE_PAYER = viewModel.TypePayer == ETypePayer.Person ? "Persona" : "Empresa",
                            TYPE_IDENTIFICATION = typeDocument
                        };


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
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_back_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {
                readerBarCode.Stop();
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
    }
}
