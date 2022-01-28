using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Threading;
using WPFMultired.Classes.Printer;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.Services.Object;
using WPFMultired.Windows;
using WPFMultired.Windows.Alerts;
using Zen.Barcode;

namespace WPFMultired.Classes
{
    public class Utilities
    {
        #region "Referencias"
        public static Navigation navigator { get; set; }

        private static SpeechSynthesizer speechSynthesizer;

        public static System.Windows.Controls.UserControl UCPublicityBanner;

        private static ModalWindow modal { get; set; }

        public static string modalMensaje { get; set; }
        #endregion
        public static string GetConfiguration(string key, bool decodeString = false)
        {
            try
            {
                string value = "";
                AppSettingsReader reader = new AppSettingsReader();
                value = reader.GetValue(key, typeof(String)).ToString();
                if (decodeString)
                {
                    value = Encryptor.Decrypt(value);
                }
                return value;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
                return string.Empty;
            }
        }

        public static bool ShowModal(string message, EModalType type, System.Windows.Controls.UserControl control, bool timer = false, string tittle = "")
        {
            bool response = false;
            try
            {
                ModalModel model = new ModalModel
                {       
                    Tittle = tittle,
                    Messaje = message,
                    TypeModal = type,
                    userControl = control,
                    ImageModal = ImagesUrlResource.AlertBlanck,
                };

                if (type == EModalType.Error)
                {
                    timer = true;
                    model.ImageModal = ImagesUrlResource.AlertError;
                }
                else if (type == EModalType.Information || type == EModalType.MaxAmount)
                {
                    model.ImageModal = ImagesUrlResource.AlertInfo;
                }
                else if (type == EModalType.NoPaper || type == EModalType.ReturnMoney)
                {
                    model.ImageModal = ImagesUrlResource.AlertInfo;
                }

                TimerService.Close();

                if (timer)
                {
                    TimerService.CallBackTimerOut = result =>
                    {
                        CloseModal();
                    };

                    TimerService.Start(int.Parse(Utilities.GetConfiguration("DurationAlert")));
                }

                Application.Current.Dispatcher.Invoke(delegate
                {
                    modal = new ModalWindow(model);
                    modal.ShowDialog();

                    if (modal.DialogResult.HasValue && modal.DialogResult.Value)
                    {
                        response = true;
                    }
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
            }
            GC.Collect();
            return response;
        }

        public static void CloseModal() => Application.Current.Dispatcher.Invoke((Action)delegate
        {
            try
            {
                if (modal != null)
                {
                    modal.Close();
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);

            }
        });

        public static void RestartApp()
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Process pc = new Process();
                    Process pn = new Process();
                    ProcessStartInfo si = new ProcessStartInfo();
                    si.FileName = Path.Combine(Directory.GetCurrentDirectory(), GetConfiguration("NAME_APLICATION"));
                    pn.StartInfo = si;
                    pn.Start();
                    pc = Process.GetCurrentProcess();
                    pc.Kill();
                }));
                GC.Collect();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
            }
        }

        public static void UpdateApp()
        {
            try
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Process pc = new Process();
                    Process pn = new Process();
                    ProcessStartInfo si = new ProcessStartInfo();
                    si.FileName = GetConfiguration("APLICATION_UPDATE");
                    pn.StartInfo = si;
                    pn.Start();
                    pc = Process.GetCurrentProcess();
                    pc.Kill();
                }));
                GC.Collect();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
            }
        }

        public static void PrintVoucher(Transaction transaction)
        {
            try
            {
                if (transaction != null)
                {
                    SolidBrush color = new SolidBrush(Color.Black);
                    Font fontKey = new Font("Arial", 9, System.Drawing.FontStyle.Bold);
                    Font fontValue = new Font("Arial", 9, System.Drawing.FontStyle.Regular);
                    int y = 0;
                    int sum = 20;
                    int x = 200;
                    int xKey = 15;
                    int xMax = 270;

                    float multiplier = (xMax / 30);

                    var data = new List<DataPrinter>()
                        {
                            new DataPrinter{ image = Image.FromFile(GetConfiguration("ImageBoucher")),  x = 35, y= y },
                        };

                    if (transaction.DatosAdicionales == null)
                    {
                        transaction.DatosAdicionales = new DATOSADICIONALES
                        {
                            DESAPL = string.Empty,
                            DESAPO = string.Empty,
                            DESAPR = string.Empty,
                            DESCLI = string.Empty,
                            DESCOM = string.Empty,
                            DESDOC = string.Empty,
                            DESEST = string.Empty,
                            DESEXC = string.Empty,
                            DESFOR = string.Empty,
                            DESHON = string.Empty,
                            DESPAP = string.Empty,
                            DESPRO = string.Empty,
                            DESTOT = string.Empty,
                            ESTTRN = string.Empty,
                            FLGAPO = false,
                            FLGHON = false,
                            FLGPAP = false,
                            FORPAG = string.Empty,
                            NOMCLI = string.Empty,
                            NROAPR = string.Empty,
                            NRONIT = string.Empty,
                            NROPRO = string.Empty,
                            VLRAPL = string.Empty,
                            VLRAPO = string.Empty,
                            VLRCOM = string.Empty,
                            VLREXC = string.Empty,
                            VLRHON = string.Empty,
                            VLRPAP = string.Empty,
                            VLRTOT = string.Empty
                        };
                    }

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.Observation, x = 50, y = y += 100 });


                    //data.Add(new DataPrinter { brush = color, font = fontKey, value = "Multi-Red", x = 100, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Cajero", x = 25, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Tran", x = 85, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Fecha", x = 145, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Hora", x = 218, y = y });

                    data.Add(new DataPrinter { brush = color, font = fontValue, value = AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString() ?? string.Empty, x = 23, y = y += 15 });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.IdTransactionAPi.ToString() ?? string.Empty, x = 83, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = DateTime.Now.Date.ToString("yyyy-MM-dd") ?? string.Empty, x = 140, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = DateTime.Now.ToString("HH:mm:ss") ?? string.Empty, x = 208, y = y });

                    

                    if (transaction.CodeTypeTransaction == GetConfiguration("CodDepositos"))
                    {
                        //Forma de pago
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESFOR, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.FORPAG ?? string.Empty, x = (xMax - transaction.DatosAdicionales.FORPAG.Length * multiplier), y = y });

                        //Nombre cliente
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESCLI, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NOMCLI ?? string.Empty, x = xKey, y = y += sum });

                        //Documento cliente
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESDOC, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Concat(transaction.DatosAdicionales.NRONIT) ?? string.Empty, x = (xMax - transaction.DatosAdicionales.NRONIT.Length * multiplier), y = y });

                        //Estado transaccion
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.ESTTRN, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.DESEST, x = (xMax - transaction.DatosAdicionales.DESEST.Length * multiplier), y = y });

                        //Número de aprobación
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESAPR, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NROAPR, x = (xMax - transaction.DatosAdicionales.NROAPR.Length * multiplier), y = y });

                        // Numero de cuenta
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESPRO, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NROPRO, x = (xMax - transaction.DatosAdicionales.NROPRO.Length * multiplier), y = y });

                        // Valor consignado
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESAPL, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPL), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPL).Length * multiplier), y = y });

                        // Excedente abonado ahorros
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESEXC, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLREXC), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLREXC).Length * multiplier), y = y });

                        // Valor comision
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESCOM, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRCOM), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRCOM).Length * multiplier), y = y });

                        // Total
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESTOT, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT).Length * multiplier), y = y });

                        // Total ingresado
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Total ingresado:", x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Payment.ValorIngresado), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorIngresado).Length * multiplier), y = y });

                        // Total devuelto
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor devuelto:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Payment.ValorDispensado), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });
                    }
                    else if (transaction.CodeTypeTransaction == GetConfiguration("CodEstadoCuenta"))
                    {
                        //Forma de pago
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESFOR, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.FORPAG ?? string.Empty, x = (xMax - transaction.DatosAdicionales.FORPAG.Length * multiplier), y = y });

                        //Nombre cliente
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESCLI, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NOMCLI ?? string.Empty, x = xKey, y = y += sum });

                        //Documento cliente
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESDOC, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Concat(transaction.DatosAdicionales.NRONIT) ?? string.Empty, x = (xMax - transaction.DatosAdicionales.NRONIT.Length * multiplier), y = y });

                        //Estado transaccion
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.ESTTRN, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.DESEST, x = (xMax - transaction.DatosAdicionales.DESEST.Length * multiplier), y = y });

                        //Número de aprobación
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESAPR, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NROAPR, x = (xMax - transaction.DatosAdicionales.NROAPR.Length * multiplier), y = y });

                        // Descripcion producto (Numero de producto)
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESPRO, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NROPRO, x = (xMax - transaction.DatosAdicionales.NROPRO.Length * multiplier), y = y });

                        // Valor credito
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESAPL, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPL), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPL).Length * multiplier), y = y });

                        // Excedente abonado cuenta de ahorros
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESEXC, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLREXC), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLREXC).Length * multiplier), y = y });

                        if (transaction.DatosAdicionales.FLGPAP)
                        {
                            data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESPAP, x = xKey, y = y += sum });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRPAP), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRPAP).Length * multiplier), y = y });
                        }

                        if (transaction.DatosAdicionales.FLGAPO)
                        {
                            data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESAPO, x = xKey, y = y += sum });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPO), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPO).Length * multiplier), y = y });
                        }

                        // Honorarios
                        if (transaction.DatosAdicionales.FLGHON)
                        {
                            data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESHON, x = xKey, y = y += sum });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRHON), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRHON).Length * multiplier), y = y });
                        }

                        // Comision
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESCOM, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRCOM), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRCOM).Length * multiplier), y = y });

                        // Total
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESTOT, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT).Length * multiplier), y = y });

                        // Total ingresado
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Total ingresado:", x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Payment.ValorIngresado), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorIngresado).Length * multiplier), y = y });

                        // Total devuelto
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor devuelto:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Payment.ValorDispensado), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });
                    }
                    else if (transaction.CodeTypeTransaction == GetConfiguration("CodTarjetaCredito"))
                    {
                        //Forma de pago
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESFOR, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.FORPAG ?? string.Empty, x = (xMax - transaction.DatosAdicionales.FORPAG.Length * multiplier), y = y });

                        //Nombre cliente
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESCLI, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NOMCLI ?? string.Empty, x = xKey, y = y += sum });

                        //Documento cliente
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESDOC, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Concat(transaction.DatosAdicionales.NRONIT) ?? string.Empty, x = (xMax - transaction.DatosAdicionales.NRONIT.Length * multiplier), y = y });

                        //Estado transaccion
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.ESTTRN, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.DESEST, x = (xMax - transaction.DatosAdicionales.DESEST.Length * multiplier), y = y });

                        //Número de aprobación
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESAPR, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NROAPR, x = (xMax - transaction.DatosAdicionales.NROAPR.Length * multiplier), y = y });

                        //Descripcion producto
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESPRO, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.DatosAdicionales.NROPRO, x = (xMax - transaction.DatosAdicionales.NROPRO.Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESAPL, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPL), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRAPL).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESEXC, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLREXC), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLREXC).Length * multiplier), y = y });

                        if (transaction.DatosAdicionales.FLGHON)
                        {
                            data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESHON, x = xKey, y = y += sum });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRHON), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRHON).Length * multiplier), y = y });
                        }

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESCOM, x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRCOM), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRCOM).Length * multiplier), y = y });


                        data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESTOT, x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Total ingresado:", x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Payment.ValorIngresado), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorIngresado).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor devuelto:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Payment.ValorDispensado), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });
                    }
                    else if (transaction.CodeTypeTransaction == GetConfiguration("CodRetiros"))
                    {
                        //data.Add(new DataPrinter { brush = color, font = fontKey, value = transaction.DatosAdicionales.DESTOT, x = xKey, y = y += 30 });
                        //data.Add(new DataPrinter { brush = color, font = fontKey, value = string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT), x = (xMax - string.Format("{0:C2}", transaction.DatosAdicionales.VLRTOT).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Forma de pago:", x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = "Efectivo", x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Número de aprobación:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.CodeTransactionAuditory, x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Estado de transacción:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.State.ToString() == "Success" ? "Aprobada" : transaction.State.ToString(), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Número de cuenta:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.Product.Code, x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });

                        //ASI ESTABA
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor comisión:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Product.AmountCommission), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor retirado:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", transaction.Payment.ValorDispensado), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Saldo disponible:", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", Convert.ToDecimal(transaction.Product.VLRTOT)), x = (xMax - string.Format("{0:C2}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });
                    }

                    data.Add(new DataPrinter { brush = color, font = fontValue, value = "-------------------------------------------------------------------", x = 2, y = y += 30 });

                    data.Add(new DataPrinter { brush = color, font = fontValue, value = "¡ Transacción exitosa !", x = 80, y = y += 50 });

                    data.Add(new DataPrinter { image = Image.FromFile(GetConfiguration("logotipo")), x = 120, y = y + 30 });

                    //data.Add(new DataPrinter
                    //{
                    //    brush = color,
                    //    font = fontValue,
                    //    value = "Powered by Multi - Red ©",
                    //    x = 68,
                    //    y = y += 80
                    //});


                    AdminPayPlus.PrintService.Start(data);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "PrintVoucher", ex);
            }
        }

        public static void PrintVoucher(PaypadOperationControl dataControl)
        {
            try
            {
                SolidBrush color = new SolidBrush(Color.Black);
                Font fontKey = new Font("Arial", 8, System.Drawing.FontStyle.Bold);
                Font fontValue = new Font("Arial", 8, System.Drawing.FontStyle.Regular);
                int y = 0;
                int sum = 30;
                int x = 150;
                int xKey = 10;
                int xMax = 270;
                float multiplier = (xMax / 48);
                int xLengthDeno = 120;
                int xLengthQua = 175;

                var data = new List<DataPrinter>()
                {
                    new DataPrinter{ image = Image.FromFile(GetConfiguration("ImageBoucher")),  x = 30, y= y },
                };
                if (dataControl.TYPE == ETypeAdministrator.Balancing)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Arqueo del efectivo", x = 70, y = y += 80 });
                }
                else if (dataControl.TYPE == ETypeAdministrator.Upload)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Provisión de efectivo", x = 75, y = y += 80 });
                }
                else
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Disminución de efectivo", x = 70, y = y += 80 });
                }

                data.Add(new DataPrinter { brush = color, font = fontKey, value = dataControl.NAME_BANCK, x = 105, y = y += sum });
                data.Add(new DataPrinter { brush = color, font = fontKey, value = "Cajero", x = 25, y = y += sum });
                data.Add(new DataPrinter { brush = color, font = fontKey, value = "Tran", x = 75, y = y });
                data.Add(new DataPrinter { brush = color, font = fontKey, value = "Fecha", x = 135, y = y });
                data.Add(new DataPrinter { brush = color, font = fontKey, value = "Hora", x = 205, y = y });

                data.Add(new DataPrinter { brush = color, font = fontValue, value = AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString() ?? string.Empty, x = 23, y = y += 15 });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = dataControl.ID_TRANSACTION ?? string.Empty, x = 80, y = y });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = dataControl.DATE ?? string.Empty, x = 130, y = y });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = dataControl.TIME ?? string.Empty, x = 200, y = y });

                data.Add(new DataPrinter { brush = color, font = fontKey, value = "Agencia:", x = xKey, y = y += sum });
                data.Add(new DataPrinter { brush = color, font = fontKey, value = string.Concat(dataControl.NAME_AGENCY, " (", dataControl.CODE_AGENCY, ")"), x = 70, y = y });

                data.Add(new DataPrinter { brush = color, font = fontKey, value = "Usuario:", x = xKey, y = y += 20 });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Concat(dataControl.NAME_USER, " ( *****", dataControl.ID_USER.Substring(dataControl.ID_USER.Length - 4), ")") ?? string.Empty, x = 70, y = y });

                if (dataControl.TYPE == ETypeAdministrator.ReUploat || dataControl.TYPE == ETypeAdministrator.Upload || dataControl.TYPE == ETypeAdministrator.Balancing)
                {
                    decimal totaldispenser = 0;

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "* Dispensadores *", x = 87, y = y += 30 });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Tipo", x = 5, y = y += 30 });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Casete", x = 40, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Denomi", x = 90, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "C.Ant /", x = 150, y = y - 5 });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "C.Prov", x = 150, y = y + 5 });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor total", x = 220, y = y });

                    foreach (var item in dataControl.DATALIST_FILTER(ETypeAdministrator.Upload))
                    {
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = item.CODE, x = 10, y = y += 25 });
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = item.CASSETTE.ToString(), x = 48, y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", item.VALUE), x = (xLengthDeno - (string.Format("{0:C2}", item.VALUE).Length * multiplier)), y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Concat(item.AMOUNT.ToString(), " / ", item.AMOUNT_NEW.ToString()), x = 145, y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", item.TOTAL_AMOUNT), x = (xMax - (string.Format("{0:C2}", item.TOTAL_AMOUNT).Length * multiplier)), y = y });

                        totaldispenser += item.TOTAL_AMOUNT;
                    }

                    if (dataControl.TYPE == ETypeAdministrator.Balancing)
                    {
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = "----------------------------------------------------------------------", x = 2, y = y += 30 });

                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor arqueado dispensadores: ", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", totaldispenser), x = (xMax - (string.Format("{0:C2}", totaldispenser).Length * multiplier)), y = y });
                    }
                }

                data.Add(new DataPrinter { brush = color, font = fontValue, value = "----------------------------------------------------------------------", x = 2, y = y += 30 });


                if (dataControl.TYPE == ETypeAdministrator.Balancing || dataControl.TYPE == ETypeAdministrator.Diminish)
                {
                    decimal totalAceptance = 0;

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "** Aceptadores **", x = 85, y = y += 30 });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Tipo", x = 10, y = y += 30 });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Denominación", x = 50, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Cant", x = 155, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor total", x = 220, y = y });

                    foreach (var item in dataControl.DATALIST_FILTER(ETypeAdministrator.Balancing))
                    {
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = item.CODE, x = 15, y = y += 25 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", item.VALUE), x = (xLengthDeno - (string.Format("{0:C2}", item.VALUE).Length * multiplier)), y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = item.AMOUNT.ToString(), x = (xLengthQua - (string.Format("{0:C2}", item.AMOUNT).Length * 3)), y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", item.TOTAL_AMOUNT), x = (xMax - (string.Format("{0:C2}", item.TOTAL_AMOUNT).Length * multiplier)), y = y });

                        totalAceptance += item.TOTAL_AMOUNT;
                    }

                    if (dataControl.TYPE == ETypeAdministrator.Balancing)
                    {
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = "----------------------------------------------------------------------", x = 2, y = y += 30 });


                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor arqueado aceptadores: ", x = xKey, y = y += sum });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", totalAceptance), x = (xMax - (string.Format("{0:C2}", totalAceptance).Length * multiplier)), y = y });
                    }
                }

                data.Add(new DataPrinter { brush = color, font = fontValue, value = "----------------------------------------------------------------------", x = 2, y = y += 30 });

                if (dataControl.TYPE == ETypeAdministrator.Upload)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor anterior : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", dataControl.TOTAL_CURRENT), x = (xMax - (string.Format("{0:C2}", dataControl.TOTAL_CURRENT).Length * multiplier)), y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor provisionado : ", x = xKey, y = y += 20 });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", dataControl.TOTAL_NEW), x = (xMax - (string.Format("{0:C2}", dataControl.TOTAL_NEW).Length * multiplier)), y = y });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor actual : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", dataControl.TOTAL), x = (xMax - (string.Format("{0:C2}", dataControl.TOTAL).Length * multiplier)), y = y });
                }
                else if (dataControl.TYPE == ETypeAdministrator.Balancing)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor total arqueo : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", dataControl.TOTAL), x = (xMax - (string.Format("{0:C2}", dataControl.TOTAL).Length * multiplier)), y = y });
                }
                else
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor disminuido : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C2}", dataControl.TOTAL), x = (xMax - (string.Format("{0:C2}", dataControl.TOTAL).Length * multiplier)), y = y });
                }

                data.Add(new DataPrinter { brush = color, font = fontValue, value = "----------------------------------------------------------------------", x = 2, y = y += 30 });

                data.Add(new DataPrinter { brush = color, font = fontValue, value = "____________________", x = 1, y = y += 50 });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = "______________________", x = 140, y = y });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = "Firma", x = xKey, y = y += sum });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = "Firma", x = 140, y = y });

                string[] dataQRArray = dataControl.SAFKEY.Split(new[] { "data=" }, StringSplitOptions.RemoveEmptyEntries);
                if (dataQRArray.Length > 1)
                {
                    string dataQR = dataQRArray[1];

                    data.Add(new DataPrinter { image = GenerateCode(dataQR) ?? Image.FromFile(GetConfiguration("ImageBoucher")), x = 30, y = y += 30 });
                }


                AdminPayPlus.PrintService.Start(data);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
            }
        }

        public static System.Drawing.Image GenerateCode(string code)
        {
            CodeQrBarcodeDraw qrcode = BarcodeDrawFactory.CodeQr;
            return qrcode.Draw(code, 50);
        }

        public static decimal RoundValue(decimal Total)
        {
            try
            {
                string total = Total.ToString();
                Total = Convert.ToDecimal(total.Replace(",", string.Empty));
                decimal roundTotal = 0;
                roundTotal = Math.Floor(Total / 100) * 100;
                return roundTotal;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
                return Total;
            }
        }

        public static bool ValidateModule(decimal module, decimal amount)
        {
            try
            {
                var result = (amount % module);
                return result == 0 ? true : false;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
                return false;
            }
        }

        public static T ConverJson<T>(string path)
        {
            T response = default(T);
            try
            {
                using (StreamReader file = new StreamReader(path, Encoding.UTF8))
                {
                    try
                    {
                        var json = file.ReadToEnd().ToString();
                        if (!string.IsNullOrEmpty(json))
                        {
                            response = JsonConvert.DeserializeObject<T>(json);
                        }
                    }
                    catch (InvalidOperationException ex)
                    {
                        Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
            }
            return response;
        }

        public static bool IsValidEmailAddress(string email)
        {
            try
            {
                Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,8}$");
                return regex.IsMatch(email);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
                return false;
            }
        }

        public static void Speak(string text)
        {
            try
            {
                if (GetConfiguration("ActivateSpeak").ToUpper() == "SI")
                {
                    if (speechSynthesizer == null)
                    {
                        speechSynthesizer = new SpeechSynthesizer();
                    }

                    speechSynthesizer.SpeakAsyncCancelAll();
                    speechSynthesizer.SelectVoice("Microsoft Sabina Desktop");
                    speechSynthesizer.SpeakAsync(text);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
            }
        }

        public static string[] ReadFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return File.ReadAllLines(path);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
            }
            return null;
        }

        public static string GetIpPublish()
        {
            try
            {
                using (var client = new WebClient())
                {
                    return client.DownloadString(GetConfiguration("UrlGetIp"));
                }
            }
            catch (Exception ex)
            {
                // Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
            }
            return GetConfiguration("IpDefoult");
        }

        public static bool ShowModalDetails(Transaction transaction, ETypeDetailModel type)
        {
            bool response = false;
            try
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    var modalDetails = new ModalDetailWindow(transaction, type);
                    modalDetails.ShowDialog();

                    if (modalDetails.DialogResult.HasValue && modalDetails.DialogResult.Value)
                    {
                        response = true;
                    }
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
            }
            GC.Collect();
            return response;
        }

        public static Image DownloadImage(string patchFile)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    var response = webClient.DownloadData(patchFile);
                    var contentType = webClient.ResponseHeaders["Content-Type"];

                    if (response != null && contentType != null &&
                        contentType.StartsWith("image/png", StringComparison.OrdinalIgnoreCase))
                    {
                        using (var ms = new MemoryStream(response))
                        {
                            var qrImage = Image.FromStream(ms);

                            if (qrImage != null)
                            {
                                return qrImage;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex, MessageResource.StandarError);
            }
            return null;
        }

        public static string[] ErrorVector = new string[]
        {
            "STACKER_OPEN",
            "JAM_IN_ACCEPTOR",
            "PAUSE",
            "ER:MD",
            "thickness",
            "Scan",
            "FATAL",
            "Printer"
        };

        public static void OpenKeyboard(bool keyBoard_Numeric, object sender, object thisView, int x = 0, int y = 0)
        {
            try
            {
                WPKeyboard.Keyboard.InitKeyboard(new WPKeyboard.Keyboard.DataKey
                {
                    control = sender,
                    userControl = thisView is System.Windows.Controls.UserControl ? thisView as System.Windows.Controls.UserControl : null,
                    eType = (keyBoard_Numeric == true) ? WPKeyboard.Keyboard.EType.Numeric : WPKeyboard.Keyboard.EType.Standar,
                    window = thisView is Window ? thisView as Window : null,
                    X = x,
                    Y = y,
                });
            }
            catch (Exception ex)
            {
            }
        }
    }
}
