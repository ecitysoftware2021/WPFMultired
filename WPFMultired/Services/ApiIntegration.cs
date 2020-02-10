using Newtonsoft.Json;
using System;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.MR_CodeOTP;
using WPFMultired.MR_CodeQR;
using WPFMultired.MR_Institutions;
using WPFMultired.MR_Language;
using WPFMultired.MR_ListProducts;
using WPFMultired.MR_ReportTransaction;
using WPFMultired.MR_TypeDocument;
using WPFMultired.MR_TypeTransaction;
using WPFMultired.MR_ValidateOTP;
using WPFMultired.MR_ValidateStatusAdmin;
using WPFMultired.MR_OperationAdmin;
using WPFMultired.MR_ProcessAdmin;
using WPFMultired.Resources;
using WPFMultired.Services.Object;
using System.Collections.Generic;

namespace WPFMultired.Services
{
    public class ApiIntegration
    {
        #region "Referencias"

        private string Hora;

        private string codeCanal;

        private string sourceEntity;

        private static string keyEncript;

        private static string keyDesencript;

        private static string userName;

        private static string token;

        private static readonly DateTime timerSeed = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        #endregion

        #region "Constructor"
        public ApiIntegration()
        {
            try
            {
                codeCanal = Utilities.GetConfiguration("CodCanal");

                sourceEntity = Utilities.GetConfiguration("SourceEntity");

                ConfigurateValues();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ConfigurateValues()
        {
            try
            {
                string[] keys = Utilities.ReadFile(@"" + ConstantsResource.PathInfo);

                if (keys.Length > 0)
                {
                    keyEncript = keys[0];
                    keyDesencript = keys[1];
                    userName = keys[2];
                    token = keys[3];
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private string ConcatOrSplitTimeStamp(string text, int operation = 1)
        {
            if (operation == 1)
            {
                return string.Concat(text, "|", (long)(DateTime.UtcNow - timerSeed).TotalMilliseconds);
            }
            else
            {
                return text.Split('|')[0];
            }
        }

        #endregion

        #region "Métodos"
        public async Task<object> CallService(ETypeService typeService, object data)
        {
            try
            {
                switch (typeService)
                {
                    case ETypeService.Validate_Status_Admin:

                        return GetAdminStatus();

                    case ETypeService.Type_Transaction:

                        return GetTypeTransaction();

                    case ETypeService.Institutions:

                        return GetInstitutions();

                    case ETypeService.Type_Document:

                        return GetTypeDocument((string)data);

                    case ETypeService.Products_Client:

                        return GetProductsClient((Transaction)data);

                    case ETypeService.Generate_OTP:

                        return GenerateCodeOTP((Transaction)data);

                    case ETypeService.Validate_OTP:

                        return ValidateOTP((Transaction)data);

                    case ETypeService.Report_Transaction:

                        return ReportTransaction((Transaction)data);

                    case ETypeService.Idioms:

                        return GetIdioms((string)data);

                    case ETypeService.Consult_QR:

                        return ConsultQR((Transaction)data);

                    case ETypeService.Operation_Admin:

                        return GetAdminOperation((ETypeAdministrator)data);

                    case ETypeService.Procces_Admin:

                        return SendAdminProcess((PaypadOperationControl)data);

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
        /// #2 Método para buscar las transacciones disponibles
        /// </summary>
        /// <returns></returns>
        private string GetTypeTransaction()
        {
            try
            {
                ConsultarTiposDeTransaccionServicesClient cliente = new ConsultarTiposDeTransaccionServicesClient();

                using (var factory = new WebChannelFactory<ConsultarTiposDeTransaccionServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)cliente.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrtiptrnInput mtrtiptrn = new mtrtiptrnInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript)
                        };

                        return JsonConvert.SerializeObject(cliente.mtrtiptrn(mtrtiptrn));
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
        private string GetInstitutions()
        {
            try
            {
                ConsultarInstitucionesServicesClient client = new ConsultarInstitucionesServicesClient();

                using (var factory = new WebChannelFactory<ConsultarInstitucionesServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrintmulInput mtrintmul = new mtrintmulInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript)
                        };

                        return JsonConvert.SerializeObject(client.mtrintmul(mtrintmul));
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
        private string GetTypeDocument(string codeEntity)
        {
            try
            {
                ConsultarTiposDeDocumentoServicesClient client = new ConsultarTiposDeDocumentoServicesClient();

                using (var factory = new WebChannelFactory<ConsultarTiposDeDocumentoServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrtipdoccInput mtrtipdocc = new mtrtipdoccInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeEntity), keyEncript)
                        };

                        return JsonConvert.SerializeObject(client.mtrtipdocc(mtrtipdocc));
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
        private string GetProductsClient(Transaction transaction)
        {
            try
            {
                ConsultarProductosClienteServicesClient cliente = new ConsultarProductosClienteServicesClient();

                using (var factory = new WebChannelFactory<ConsultarProductosClienteServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)cliente.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrprocliInput mtrprocli = new mtrprocliInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeClient), keyEncript),
                            I_NUMERODOCUMENTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript),
                            I_TIPODOCUMENTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.TypeDocument.ToString()), keyEncript),
                            I_TIPOTRANSACCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((int)transaction.Type).ToString()), keyEncript)
                        };

                        return JsonConvert.SerializeObject(cliente.mtrprocli(mtrprocli));
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
        private string GenerateCodeOTP(Transaction transaction)
        {
            try
            {
                GenerarOTPServicesClient client = new GenerarOTPServicesClient();

                using (var factory = new WebChannelFactory<GenerarOTPServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrgenotpInput mtrgenotp = new mtrgenotpInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeClient), keyEncript),
                            //I_TIPOTRN = encrytor_MR.GetFullParameter(mR_DataService.TypeTramite.ToString(), KEY),
                            //I_CUENTA = encrytor_MR.GetFullParameter(mR_DataService.I_CTANRO, KEY),
                            //I_SISTEMA = encrytor_MR.GetFullParameter(mR_DataService.I_CODSIS, KEY),
                            //I_PRODUCTO = encrytor_MR.GetFullParameter(mR_DataService.I_CODPRO, KEY),
                            //I_REFERENCIA = encrytor_MR.GetFullParameter(mR_DataService.I_REFTRN, KEY),
                            //I_TOKEN = encrytor_MR.GetFullParameter(mR_DataService.I_TOKTRN, KEY),
                            //I_VALOR = encrytor_MR.GetFullParameter(mR_DataService.I_VLRTRN, KEY)
                        };

                        return JsonConvert.SerializeObject(client.mtrgenotp(mtrgenotp));
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
        private string ValidateOTP(Transaction transaction)
        {
            try
            {
                ValidarOTPServicesClient client = new ValidarOTPServicesClient();

                using (var factory = new WebChannelFactory<ValidarOTPServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrvalotpInput mtrvalotp = new mtrvalotpInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeClient), keyEncript),
                            //I_TIPOTRN = encrytor_MR.GetFullParameter(mR_DataService.TypeTramite.ToString(), KEY),
                            //I_CUENTA = encrytor_MR.GetFullParameter(mR_DataService.I_CTANRO, KEY),
                            //I_SISTEMA = encrytor_MR.GetFullParameter(mR_DataService.I_CODSIS, KEY),
                            //I_PRODUCTO = encrytor_MR.GetFullParameter(mR_DataService.I_CODPRO, KEY),
                            //I_REFERENCIA = encrytor_MR.GetFullParameter(mR_DataService.I_REFTRN, KEY),
                            //I_TOKEN = encrytor_MR.GetFullParameter(mR_DataService.I_TOKTRN, KEY),
                            //I_VALOR = encrytor_MR.GetFullParameter(mR_DataService.I_VLRTRN, KEY),
                            //I_CODIGOTP = encrytor_MR.GetFullParameter(mR_DataService.CodOTP, KEY)
                        };

                        return JsonConvert.SerializeObject(client.mtrvalotp(mtrvalotp));
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
        private string ReportTransaction(Transaction transaction)
        {
            try
            {
                ProcesarTransaccionServicesClient client = new ProcesarTransaccionServicesClient();

                using (var factory = new WebChannelFactory<ProcesarTransaccionServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        //List<iLISTAREGISTROS> list = new List<iLISTAREGISTROS>();

                        //var notificarList = (NotifyListBills)mR_DataService.Objeto;

                        //if (notificarList != null)
                        //{
                        //    list.Add(new iLISTAREGISTROS
                        //    {
                        //        I_RTNCON = notificarList.cant,
                        //        LIST = notificarList.I_LIST.ToArray()

                        //    });
                        //}
                        //else
                        //{
                        //    list.Add(new iLISTAREGISTROS
                        //    {
                        //        I_RTNCON = 0,
                        //        LIST = null
                        //    });
                        //}

                        mtrprotrnInput mtrprotrn = new mtrprotrnInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeClient), keyEncript),

                            //I_TIPOTRN = encrytor_MR.GetFullParameter(mR_DataService.TypeTramite.ToString(), KEY),
                            //I_CUENTA = encrytor_MR.GetFullParameter(mR_DataService.I_CTANRO, KEY),
                            //I_SISTEMA = encrytor_MR.GetFullParameter(mR_DataService.I_CODSIS, KEY),
                            //I_PRODUCTO = encrytor_MR.GetFullParameter(mR_DataService.I_CODPRO, KEY),
                            //I_REFERENCIA = encrytor_MR.GetFullParameter(mR_DataService.I_REFTRN, KEY),
                            //I_TOKEN = encrytor_MR.GetFullParameter(mR_DataService.I_TOKTRN, KEY),
                            //I_VALOR = encrytor_MR.GetFullParameter(mR_DataService.I_VLRTRN, KEY),
                            //I_CODIGOTP = encrytor_MR.GetFullParameter(mR_DataService.CodOTP, KEY),
                            //I_LISTAREGISTROS = list[0]
                        };

                        return JsonConvert.SerializeObject(client.mtrprotrn(mtrprotrn));
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
        /// #NN Método para validar el QR
        /// </summary>
        /// <returns></returns>
        private string ConsultQR(Transaction transaction)
        {
            try
            {
                QRDecodeServicesClient client = new QRDecodeServicesClient();

                using (var factory = new WebChannelFactory<QRDecodeServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrctlaqrcInput mtrctlaqrc = new mtrctlaqrcInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeClient), keyEncript),
                            //I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeEntity), keyEncript),
                            I_QRTEXT = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript)
                        };

                        return JsonConvert.SerializeObject(client.mtrctlaqrc(mtrctlaqrc));
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
        private string GetIdioms(string codeEntity)
        {
            try
            {
                ConsultarLenguajeServicesClient client = new ConsultarLenguajeServicesClient();

                using (var factory = new WebChannelFactory<ConsultarLenguajeServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrindlenInput mtrindlen = new mtrindlenInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeEntity), keyEncript)
                        };

                        return JsonConvert.SerializeObject(client.mtrindlen(mtrindlen));
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
        private int GetAdminStatus()
        {
            try
            {
                ValidateStatusAdminServicesClient client = new ValidateStatusAdminServicesClient();
                using (var factory = new WebChannelFactory<ValidateStatusAdminServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrvalarqcInput request = new mtrvalarqcInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp("0"), keyEncript)
                        };

                        var response = client.mtrvalarqc(request);
                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            !string.IsNullOrEmpty(response.O_MOVIMIENTO) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            return int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MOVIMIENTO, keyDesencript), 2));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
            return 0;
        }

        private PaypadOperationControl GetAdminOperation(ETypeAdministrator typeAdministrator)
        {
            try
            {
                RetrieveOperationAdminServicesClient client = new RetrieveOperationAdminServicesClient();
                using (var factory = new WebChannelFactory<RetrieveOperationAdminServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrretarqcInput request = new mtrretarqcInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp("0"), keyEncript),
                            I_MOVIMIENTO = Encryptor.Encrypt(((int)typeAdministrator).ToString(), keyEncript)
                        };

                        var response = client.mtrretarqc(request);
                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) && response.LISTAREGISTROS != null &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.LISTAREGISTROS.O_RTNCON > 0)
                        {
                            PaypadOperationControl result = new PaypadOperationControl()
                            {
                                DATALIST = new List<List>(),
                                TYPE = typeAdministrator
                            };
                            foreach (var denomination in response.LISTAREGISTROS.LIST)
                            {
                                result.DATALIST.Add(new List
                                { 
                                    AMOUNT = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANDEN, keyDesencript), 2)),
                                    AMOUNT_NEW = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANACT, keyDesencript), 2)),
                                    VALUE = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CODDEN, keyDesencript), 2).Replace(",", "")),
                                    DESCRIPTION = ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_DESDON, keyDesencript), 2),
                                    TOTAL_AMOUNT = (int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANDEN, keyDesencript), 2)) + 
                                    int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANACT, keyDesencript), 2))) * 
                                    int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CODDEN, keyDesencript), 2).Replace(",", "")),
                                    CASSETTE = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CASSET, keyDesencript), 2)),
                                    CODE = ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_TIPMON, keyDesencript), 2),
                                    IMAGE = ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_TIPMON, keyDesencript), 2) == "B" ? ImagesUrlResource.ImgBill : ImagesUrlResource.ImgCoin
                                });

                                result.TOTAL += int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANDEN, keyDesencript), 2)) *
                                    int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CODDEN, keyDesencript), 2).Replace(",", ""));   
                            }

                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }

            return null;
        }

        private PaypadOperationControl SendAdminProcess(PaypadOperationControl dataProcess)
        {
            try
            {
                ProcessAdminServicesClient client = new ProcessAdminServicesClient();
                using (var factory = new WebChannelFactory<ProcessAdminServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrproarqcInput request = new mtrproarqcInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp("0"), keyEncript),
                            I_MOVIMIENTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((int)dataProcess.TYPE).ToString()), keyEncript)
                        };

                        var response = client.mtrproarqc(request);
                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            dataProcess.ID_TRANSACTION = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NUMEROTRANSACCION, keyDesencript), 2);
                            dataProcess.CODE_AGENCY = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_AGENCIATRANSACCION, keyDesencript), 2);
                            dataProcess.ID_USER = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NITCAJERO, keyDesencript), 2);
                            dataProcess.NAME_BANCK = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NOMBREBANCO, keyDesencript), 2);
                            dataProcess.NAME_AGENCY = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NOMBREAGENCIA, keyDesencript), 2);
                            dataProcess.NAME_COMPANY = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NOMBREBANCO, keyDesencript), 2);
                            dataProcess.NAME_USER = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NOMBRECAJERO, keyDesencript), 2);
                            dataProcess.TIME = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_HORATRANSACCION, keyDesencript), 2);
                            dataProcess.DATE = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_FECHAPROCESO, keyDesencript), 2);

                            return dataProcess;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }

            return null;
        }

        private void SetHeaderRequest()
        {
            try
            {
                WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", Encryptor.Encrypt(ConcatOrSplitTimeStamp(userName), keyEncript));
                WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", Encryptor.Encrypt(ConcatOrSplitTimeStamp(token), keyEncript));
                WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", Encryptor.Encrypt(ConcatOrSplitTimeStamp((new Random()).Next(1000, 9999).ToString()), keyEncript));
            }
            catch (Exception ex )
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        #endregion
    }
}
