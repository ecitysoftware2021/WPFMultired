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

namespace WPFMultired.Classes
{
    public class Utilities
    {
        #region "Referencias"
        public static Navigation navigator { get; set; }

        private static SpeechSynthesizer speechSynthesizer;

        private static ModalWindow modal { get; set; }
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

        public static bool ShowModal(string message, EModalType type, bool stopTimer = true, bool restarApp = false)
        {
            bool response = false;
            try
            {
                ModalModel model = new ModalModel
                {
                    Tittle = "Estimado Cliente: ",
                    Messaje = message,
                    TypeModal = type,
                    ImageModal = ImagesUrlResource.AlertBlanck,
                };

                if (type == EModalType.Error)
                {
                    model.ImageModal = ImagesUrlResource.AlertError;
                }
                else if (type == EModalType.Information)
                {
                    model.ImageModal = ImagesUrlResource.AlertInfo;
                }
                else if (type == EModalType.NoPaper)
                {
                    model.ImageModal = ImagesUrlResource.AlertInfo;
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
                // Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
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
                    int xMax = 250;
                    float multiplier = (xMax / 48);

                    var data = new List<DataPrinter>()
                        {
                            new DataPrinter{ image = Image.FromFile(GetConfiguration("ImageBoucher")),  x = 5, y= y },
                        };
                    if (transaction.Type == ETransactionType.Deposit)
                    {
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Deposito", x = 100, y = y += 120 });
                    }
                    else
                    {
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Retiro", x = 100, y = y += 120 });
                    }
                    
                    //data.Add(new DataPrinter { brush = color, font = fontKey, value = "Multi-Red", x = 100, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "ASM", x = 25, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Tran", x = 85, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Fecha", x = 145, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Hora", x = 218, y = y });

                    data.Add(new DataPrinter { brush = color, font = fontValue, value = AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString() ?? string.Empty, x = 23, y = y += 15 });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = transaction.IdTransactionAPi.ToString() ?? string.Empty, x = 83, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = DateTime.Now.Date.ToString("yyyy-MM-dd")?? string.Empty, x = 140, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = DateTime.Now.ToString("HH:mm:ss") ?? string.Empty, x = 208, y = y });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Usuario:", x = xKey, y = y += 30 });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Concat(transaction.payer.NAME, " ( *****", transaction.payer.IDENTIFICATION.Substring(transaction.payer.IDENTIFICATION.Length - 4), ")") ?? string.Empty, x = 90, y = y });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Estado transacción:", x = xKey, y = y += 30 });
                    data.Add(new DataPrinter
                    {
                        brush = color,
                        font = fontValue,
                        value = (transaction.State == ETransactionState.Success || transaction.State == ETransactionState.ErrorService || transaction.State == ETransactionState.Error)
                                ? "Aprobada" : "Cancelada",
                        x = x,
                        y = y
                    });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor de la comisión:", x = xKey, y = y += sum });

                    if (transaction.State == ETransactionState.Success)
                    {
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", transaction.Product.AmountCommission), x = (xMax - (string.Format("{0:C0}", transaction.Product.AmountCommission).Length * multiplier)), y = y });
                    }
                    else
                    {
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", 0), x = (xMax - string.Format("{0:C0}", 0).Length * multiplier), y = y });
                    }
                    
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = "-------------------------------------------------------------------", x = 2, y = y += 30 });

                   
                    if (transaction.Type == ETransactionType.Deposit)
                    {
                        
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = "Total ingresado.:", x = xKey, y = y += 30 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", transaction.Payment.ValorIngresado), x = (xMax - string.Format("{0:C0}", transaction.Payment.ValorIngresado).Length * multiplier), y = y });
                       // data.Add(new DataPrinter { brush = color, font = fontKey, value = "Total Devuelto:", x = xKey, y = y += sum });
                      //  data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", transaction.Payment.ValorDispensado), x = x, y = y });
                    }
                    else
                    {
                        if (!transaction.IsCashBack)
                        {
                            data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor solicitado:", x = xKey, y = y += 30 });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", transaction.Payment.ValorSobrante), x = (xMax - string.Format("{0:C0}", transaction.Payment.ValorSobrante).Length * multiplier), y = y });

                            data.Add(new DataPrinter { brush = color, font = fontKey, value = "Total entregado:", x = xKey, y = y += sum });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", transaction.Payment.ValorDispensado), x = (xMax - string.Format("{0:C0}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });
                        }
                        else
                        {
                            data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor solicitado en cashback:", x = xKey, y = y += 30 });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", transaction.Payment.ValorSobrante), x = (xMax - string.Format("{0:C0}", transaction.Payment.ValorSobrante).Length * multiplier), y = y });

                            data.Add(new DataPrinter { brush = color, font = fontKey, value = "Total entregado en cashbak:", x = xKey, y = y += sum });
                            data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", transaction.Payment.ValorDispensado), x = (xMax - string.Format("{0:C0}", transaction.Payment.ValorDispensado).Length * multiplier), y = y });
                        }
                    }

                    data.Add(new DataPrinter { brush = color, font = fontValue, value = "-------------------------------------------------------------------", x = 2, y = y += 30 });

                    data.Add(new DataPrinter { brush = color, font = fontValue, value = "¡ Transacción exitosa !", x = 50, y = y += 50 });

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
                    new DataPrinter{ image = Image.FromFile(GetConfiguration("ImageBoucher")),  x = 5, y= y },
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
                data.Add(new DataPrinter { brush = color, font = fontKey, value = "ASM", x = 25, y = y += sum });
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

                if (dataControl.TYPE == ETypeAdministrator.Balancing || dataControl.TYPE == ETypeAdministrator.Diminish)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "** Aceptadores **", x = 85, y = y += 30 });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Tipo", x = 10, y = y += 30 });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Denominación", x = 50, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Cant", x = 155, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor total", x = 220, y = y });

                    foreach (var item in dataControl.DATALIST_FILTER(ETypeAdministrator.Balancing))
                    {
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = item.Code == "MA" ? "M" : "B", x = 15, y = y += 25 });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", item.Denominacion), x = (xLengthDeno - (string.Format("{0:C0}", item.Denominacion).Length * multiplier)), y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = item.Quantity.ToString(), x = (xLengthQua - (string.Format("{0:C0}", item.Quantity).Length * 3)), y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", item.Total), x = (xMax - (string.Format("{0:C0}", item.Total).Length * multiplier)), y = y });
                    }
                }

                if (dataControl.TYPE == ETypeAdministrator.ReUploat || dataControl.TYPE == ETypeAdministrator.Upload || dataControl.TYPE == ETypeAdministrator.Balancing)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "* Dispensadores *", x = 87, y = y += 30 });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Tipo", x = 5, y = y += 30 });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Casete", x = 40, y = y});
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Denomi", x = 90, y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Cant A /", x = 150, y = y - 5 });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Cant p", x = 150, y = y + 5});
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor total", x = 220, y = y });

                    foreach (var item in dataControl.DATALIST_FILTER(ETypeAdministrator.Upload))
                    {
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = item.Code == "MA" ? "M" : "B", x = 10, y = y += 25 });
                        data.Add(new DataPrinter { brush = color, font = fontKey, value = item.Code, x = 48, y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", item.Denominacion), x = (xLengthDeno - (string.Format("{0:C0}", item.Denominacion).Length * multiplier)), y = y});
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Concat(item.Quantity.ToString(), " / ", item.Quantity.ToString()), x = 145, y = y });
                        data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", item.Total), x = (xMax - (string.Format("{0:C0}", item.Total).Length * multiplier)), y = y });
                    }
                }

                data.Add(new DataPrinter { brush = color, font = fontValue, value = "----------------------------------------------------------------------", x = 2, y = y += 30 });

                if (dataControl.TYPE == ETypeAdministrator.Upload)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor anterior : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", dataControl.TOTAL_CURRENT), x = (xMax - (string.Format("{0:C0}", dataControl.TOTAL_CURRENT).Length * multiplier)), y = y });
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor provisionado : ", x = xKey, y = y += 20 });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", dataControl.TOTAL), x = (xMax - (string.Format("{0:C0}", dataControl.TOTAL).Length * multiplier)), y = y });

                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor actual : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", dataControl.TOTAL + dataControl.TOTAL_CURRENT), x = (xMax - (string.Format("{0:C0}", dataControl.TOTAL + dataControl.TOTAL_CURRENT).Length * multiplier)), y = y });
                }
                else if (dataControl.TYPE == ETypeAdministrator.Balancing)
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor arqueado : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", dataControl.TOTAL + dataControl.TOTAL_CURRENT), x = (xMax - (string.Format("{0:C0}", dataControl.TOTAL + dataControl.TOTAL_CURRENT).Length * multiplier)), y = y });
                }
                else
                {
                    data.Add(new DataPrinter { brush = color, font = fontKey, value = "Valor disminuido : ", x = xKey, y = y += sum });
                    data.Add(new DataPrinter { brush = color, font = fontValue, value = string.Format("{0:C0}", dataControl.TOTAL + dataControl.TOTAL_CURRENT), x = (xMax - (string.Format("{0:C0}", dataControl.TOTAL + dataControl.TOTAL_CURRENT).Length * multiplier)), y = y });
                }

                data.Add(new DataPrinter { brush = color, font = fontValue, value = "----------------------------------------------------------------------", x = 2, y = y += 30 });

                data.Add(new DataPrinter { brush = color, font = fontValue, value = "____________________", x = 1, y = y += 50 });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = "______________________", x = 140, y = y });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = "Firma", x = xKey, y = y += sum });
                data.Add(new DataPrinter { brush = color, font = fontValue, value = "Firma", x = 140, y = y });

                data.Add(new DataPrinter { image = DownloadImage(dataControl.SAFKEY) ?? Image.FromFile(GetConfiguration("ImageBoucher")), x = 30, y = y +=30 });

                AdminPayPlus.PrintService.Start(data);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Utilities", ex);
            }
        }

        public static decimal RoundValue(decimal Total)
        {
            try
            {
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
                using (var client = new  WebClient())
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
    }
}
