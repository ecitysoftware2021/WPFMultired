using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WPFMultired.Classes;
using WPFMultired.Classes.UseFull;
using WPFMultired.MR_CodeOTP;
using WPFMultired.MR_CodeQR;
using WPFMultired.MR_Institutions;
using WPFMultired.MR_Language;
using WPFMultired.MR_ListProducts;
using WPFMultired.MR_ReportTransaction;
using WPFMultired.MR_TypeDocument;
using WPFMultired.MR_TypeTransaction;
using WPFMultired.MR_ValidateOTP;
using WPFMultired.Resources;

namespace WPFMultired.Services
{
    public class ApiIntegration
    {
        #region "Referencias"
        DataService mR_DataService;

        private string Hora;

        private string Canal;

        private string DireccionIp;
        private string EntidadOrigen;
        private string CodigoTerminal;
        private String KEY;
        private Encrytor_MR encrytor_MR;
        #endregion

        #region "Constructor"
        public ApiIntegration()
        {
            try
            {
                KEY = Utilities.GetConfiguration("KEY");

                Canal = Utilities.GetConfiguration("i_CODCAN");

                DireccionIp = Utilities.GetConfiguration("i_DIREIP");

                EntidadOrigen = Utilities.GetConfiguration("i_ENTORI");
                CodigoTerminal = Utilities.GetConfiguration("i_CODTER");
                CodigoTerminal = Utilities.GetConfiguration("i_CODTER");
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
        #endregion

        #region "Métodos"
        public async Task<string> CallServiceMR(DataService mR_DataService)
        {
            try
            {
                this.mR_DataService = mR_DataService;
                encrytor_MR = new Encrytor_MR();
                Hora = DateTime.Now.ToString("yyyyMMddhhmmssfff");

                switch (mR_DataService.etypeService)
                {
                    case EServicio.Type_Transaction:

                        return TypeTransaction();

                    case EServicio.Institutions:

                        return Institutions();

                    case EServicio.Type_Document:

                        return TypeDocument();

                    case EServicio.Products_Client:
                        break;
                    case EServicio.Generate_OTP:
                        break;
                    case EServicio.Report_Transaction:
                        break;
                    case EServicio.Report_Transaction_BD:
                        break;
                    case EServicio.Validate_OTP:
                        break;
                    case EServicio.Language:

                        return TypeLanguage();

                    case EServicio.Consult_QR:
                        break;
                    default:
                        break;
                }
                return null;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #NN Método para validar el QR
        /// </summary>
        /// <returns></returns>
        private string ValidateQR()
        {
            try
            {
                QRDecodeServicesClient qRDecodeServicesClient = new QRDecodeServicesClient();

                using (var factory = new WebChannelFactory<QRDecodeServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)qRDecodeServicesClient.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrctlaqrcInput mtrctlaqrc = new mtrctlaqrcInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Utilities.GetConfiguration("I_CODCAN_QR"), KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(Utilities.GetConfiguration("I_DIREIP_QR"), KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(Utilities.GetConfiguration("I_ENTORI_QR"), KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(Utilities.GetConfiguration("I_CODTER_QR"), KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_INSTITUCION = encrytor_MR.GetFullParameter(Utilities.GetConfiguration("I_INS_QR"), KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY),
                            I_QRTEXT = encrytor_MR.GetFullParameter(mR_DataService.Text_QR, KEY)
                        };

                        var response = qRDecodeServicesClient.mtrctlaqrc(mtrctlaqrc);

                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #1 Método para buscar los idiomas disponibles para la aplicación
        /// </summary>
        /// <returns></returns>
        private string TypeLanguage()
        {
            try
            {
                ConsultarLenguajeServicesClient clientlenguaje = new ConsultarLenguajeServicesClient();

                using (var factory = new WebChannelFactory<ConsultarLenguajeServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)clientlenguaje.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrindlenInput mtrindlen = new mtrindlenInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY),
                            I_INSTITUCION = encrytor_MR.GetFullParameter(EntidadOrigen, KEY)
                        };

                        var response = clientlenguaje.mtrindlen(mtrindlen);

                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #2 Método para buscar las transacciones disponibles
        /// </summary>
        /// <returns></returns>
        private string TypeTransaction()
        {
            try
            {
                ConsultarTiposDeTransaccionServicesClient clientTipoTransaccion = new ConsultarTiposDeTransaccionServicesClient();

                using (var factory = new WebChannelFactory<ConsultarTiposDeTransaccionServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)clientTipoTransaccion.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrtiptrnInput mtrtiptrn = new mtrtiptrnInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY)
                        };

                        var response = clientTipoTransaccion.mtrtiptrn(mtrtiptrn);

                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #3 Método para buscar las instituciones disponibles
        /// </summary>
        /// <returns></returns>
        private string Institutions()
        {
            try
            {
                ConsultarInstitucionesServicesClient clientInstituciones = new ConsultarInstitucionesServicesClient();

                using (var factory = new WebChannelFactory<ConsultarInstitucionesServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)clientInstituciones.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrintmulInput mtrintmul = new mtrintmulInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY)
                        };


                        var response = clientInstituciones.mtrintmul(mtrintmul);

                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #4 Método para buscar los tipos de documentos disponibles para esa institucion
        /// </summary>
        /// <returns></returns>
        private string TypeDocument()
        {
            try
            {
                ConsultarTiposDeDocumentoServicesClient tipoDocumentoClient = new ConsultarTiposDeDocumentoServicesClient();

                using (var factory = new WebChannelFactory<ConsultarTiposDeDocumentoServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)tipoDocumentoClient.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrtipdoccInput mtrtipdocc = new mtrtipdoccInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY),
                            I_INSTITUCION = encrytor_MR.GetFullParameter(mR_DataService.EntidadDestino, KEY)
                        };

                        var response = tipoDocumentoClient.mtrtipdocc(mtrtipdocc);

                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #5 Método para buscar los productos del cliente
        /// </summary>
        /// <returns></returns>
        private string Products()
        {
            try
            {
                ConsultarProductosClienteServicesClient productosCliente = new ConsultarProductosClienteServicesClient();

                using (var factory = new WebChannelFactory<ConsultarProductosClienteServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)productosCliente.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrprocliInput mtrprocli = new mtrprocliInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY),
                            I_INSTITUCION = encrytor_MR.GetFullParameter(mR_DataService.EntidadDestino, KEY),
                            I_NUMERODOCUMENTO = encrytor_MR.GetFullParameter(mR_DataService.I_NUMDOC, KEY),
                            I_TIPODOCUMENTO = encrytor_MR.GetFullParameter(mR_DataService.I_TIPDOC, KEY),
                            I_TIPOTRANSACCION = encrytor_MR.GetFullParameter(mR_DataService.TypeTramite.ToString(), KEY)
                        };

                        var response = productosCliente.mtrprocli(mtrprocli);

                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #6 Método para generar el codigo OTP
        /// </summary>
        /// <returns></returns>
        private string CodeOTP()
        {
            try
            {
                GenerarOTPServicesClient generarOTPClient = new GenerarOTPServicesClient();

                using (var factory = new WebChannelFactory<GenerarOTPServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)generarOTPClient.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrgenotpInput mtrgenotp = new mtrgenotpInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY),
                            I_INSTITUCION = encrytor_MR.GetFullParameter(mR_DataService.EntidadDestino, KEY),
                            I_TIPOTRN = encrytor_MR.GetFullParameter(mR_DataService.TypeTramite.ToString(), KEY),
                            I_CUENTA = encrytor_MR.GetFullParameter(mR_DataService.I_CTANRO, KEY),
                            I_SISTEMA = encrytor_MR.GetFullParameter(mR_DataService.I_CODSIS, KEY),
                            I_PRODUCTO = encrytor_MR.GetFullParameter(mR_DataService.I_CODPRO, KEY),
                            I_REFERENCIA = encrytor_MR.GetFullParameter(mR_DataService.I_REFTRN, KEY),
                            I_TOKEN = encrytor_MR.GetFullParameter(mR_DataService.I_TOKTRN, KEY),
                            I_VALOR = encrytor_MR.GetFullParameter(mR_DataService.I_VLRTRN, KEY)
                        };
                        
                        var responseOTP = generarOTPClient.mtrgenotp(mtrgenotp);
                        
                        return JsonConvert.SerializeObject(responseOTP);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #7 Método para validar el codigo OTP
        /// </summary>
        /// <returns></returns>
        private string ValidateOTP()
        {
            try
            {
                ValidarOTPServicesClient validarOTPClient = new ValidarOTPServicesClient();

                using (var factory = new WebChannelFactory<ValidarOTPServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)validarOTPClient.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        mtrvalotpInput mtrvalotp = new mtrvalotpInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY),
                            I_INSTITUCION = encrytor_MR.GetFullParameter(mR_DataService.EntidadDestino, KEY),
                            I_TIPOTRN = encrytor_MR.GetFullParameter(mR_DataService.TypeTramite.ToString(), KEY),
                            I_CUENTA = encrytor_MR.GetFullParameter(mR_DataService.I_CTANRO, KEY),
                            I_SISTEMA = encrytor_MR.GetFullParameter(mR_DataService.I_CODSIS, KEY),
                            I_PRODUCTO = encrytor_MR.GetFullParameter(mR_DataService.I_CODPRO, KEY),
                            I_REFERENCIA = encrytor_MR.GetFullParameter(mR_DataService.I_REFTRN, KEY),
                            I_TOKEN = encrytor_MR.GetFullParameter(mR_DataService.I_TOKTRN, KEY),
                            I_VALOR = encrytor_MR.GetFullParameter(mR_DataService.I_VLRTRN, KEY),
                            I_CODIGOTP = encrytor_MR.GetFullParameter(mR_DataService.CodOTP, KEY)
                        };

                        var responseValidarOTP = validarOTPClient.mtrvalotp(mtrvalotp);

                        return JsonConvert.SerializeObject(responseValidarOTP);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }

        /// <summary>
        /// #8 Método para reportar la transacción
        /// </summary>
        /// <returns></returns>
        private string ReportTransaction()
        {
            try
            {
                ProcesarTransaccionServicesClient posteeTransaccionClient = new ProcesarTransaccionServicesClient();

                using (var factory = new WebChannelFactory<ProcesarTransaccionServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)posteeTransaccionClient.InnerChannel))
                    {
                        Random r = new Random();
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", encrytor_MR.GetFullParameter("MULTIRED", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", encrytor_MR.GetFullParameter("88A9C419F2A5D10CA8CE5391FE776D6E", KEY));
                        WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", encrytor_MR.GetFullParameter(r.Next(1000, 9999).ToString(), KEY));

                        List<iLISTAREGISTROS> list = new List<iLISTAREGISTROS>();

                        var notificarList = (NotifyListBills)mR_DataService.Objeto;

                        if (notificarList != null)
                        {
                            list.Add(new iLISTAREGISTROS
                            {
                                I_RTNCON = notificarList.cant,
                                LIST = notificarList.I_LIST.ToArray()
                            });
                        }
                        else
                        {
                            list.Add(new iLISTAREGISTROS
                            {
                                I_RTNCON = 0,
                                LIST = null
                            });
                        }

                        mtrprotrnInput mtrprotrn = new mtrprotrnInput
                        {
                            I_CANAL = encrytor_MR.GetFullParameter(Canal, KEY),
                            I_DIRECCIONIP = encrytor_MR.GetFullParameter(DireccionIp, KEY),
                            I_ENTIDADORIGEN = encrytor_MR.GetFullParameter(EntidadOrigen, KEY),
                            I_TERMINAL = encrytor_MR.GetFullParameter(CodigoTerminal, KEY),
                            I_TIMESTAMP = encrytor_MR.GetFullParameter(Encrytor_MR.timeStamp, KEY),
                            I_LENGUAJE = encrytor_MR.GetFullParameter(mR_DataService.Language, KEY),
                            I_INSTITUCION = encrytor_MR.GetFullParameter(mR_DataService.EntidadDestino, KEY),
                            I_TIPOTRN = encrytor_MR.GetFullParameter(mR_DataService.TypeTramite.ToString(), KEY),
                            I_CUENTA = encrytor_MR.GetFullParameter(mR_DataService.I_CTANRO, KEY),
                            I_SISTEMA = encrytor_MR.GetFullParameter(mR_DataService.I_CODSIS, KEY),
                            I_PRODUCTO = encrytor_MR.GetFullParameter(mR_DataService.I_CODPRO, KEY),
                            I_REFERENCIA = encrytor_MR.GetFullParameter(mR_DataService.I_REFTRN, KEY),
                            I_TOKEN = encrytor_MR.GetFullParameter(mR_DataService.I_TOKTRN, KEY),
                            I_VALOR = encrytor_MR.GetFullParameter(mR_DataService.I_VLRTRN, KEY),
                            I_CODIGOTP = encrytor_MR.GetFullParameter(mR_DataService.CodOTP, KEY),
                            I_LISTAREGISTROS = list[0]
                        };

                        var response = posteeTransaccionClient.mtrprotrn(mtrprotrn);

                        return JsonConvert.SerializeObject(response);
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
                return null;
            }
        }
        #endregion
    }
}
