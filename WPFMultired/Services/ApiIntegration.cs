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
using WPFMultired.MR_ValidateAdminQR;
using WPFMultired.MR_ControllCash;
using WPFMultired.MR_ConsultInvoice;
using WPFMultired.MR_NotificInvoice;
using WPFMultired.Resources;
using WPFMultired.Services.Object;
using System.Collections.Generic;
using WPFMultired.DataModel;
using System.Globalization;
using System.IO;

namespace WPFMultired.Services
{
    public class ApiIntegration
    {
        #region "Referencias"

        private long timeStamp;

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
                return string.Concat(text, "|", timeStamp);
            }
            else
            {
                return text.Split('|')[0];
            }
        }

        #endregion

        #region "Métodos"
        public async Task<Response> CallService(ETypeService typeService, object data)
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

                        return GetTypeDocument((Transaction)data);

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

                    case ETypeService.Validate_Admin_QR:

                        return ValidateAdminQr((string)data);

                    case ETypeService.Report_Cash:

                        return ReportCash((Transaction)data);

                    case ETypeService.Consult_Invoice:

                        return ConsultInvoice((Transaction)data);

                    case ETypeService.Report_Invoice:

                        return ReportInvoice((Transaction)data);

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
        /// #1 Método para buscar los idiomas disponibles para la aplicación
        /// </summary>
        /// <returns></returns>
        private Response GetIdioms(string codeEntity)
        {
            try
            {
                ConsultarLenguajeServicesClient client = new ConsultarLenguajeServicesClient();

                using (var factory = new WebChannelFactory<ConsultarLenguajeServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrindlenInput request = new mtrindlenInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetConfiguration("IdiomDefaultId")), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetConfiguration("CodeEntityDefault")), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request GetIdioms: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrindlen(request);

                        AdminPayPlus.SaveErrorControl($"Response GetIdioms: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.O_LISTAREGISTROS.O_RTNCON > 0)
                        {
                            List<ItemList> idioms = new List<ItemList>();
                            foreach (var item in response.O_LISTAREGISTROS.LIST)
                            {
                                idioms.Add(new ItemList
                                {
                                    Item1 = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_NOMLEN, keyDesencript), 2),
                                    Item6 = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_INDLEN, keyDesencript), 2)),
                                    ImageSourse = string.Concat(Utilities.GetConfiguration("ResourcesUrl"), ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_BANDER, keyDesencript), 2), ".png"),
                                });
                            }

                            return new Response { Data = idioms };
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

        /// <summary>
        /// #2 Método para buscar las transacciones disponibles
        /// </summary>
        /// <returns></returns>
        private Response GetTypeTransaction()
        {
            try
            {
                TiposTransaccionesServicesClient client = new TiposTransaccionesServicesClient();

                using (var factory = new WebChannelFactory<TiposTransaccionesServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrtiptrncInput request = new mtrtiptrncInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request GetTypeTransaction: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrtiptrnc(request);

                        AdminPayPlus.SaveErrorControl($"Response GetTypeTransaction: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.LISTAREGISTROS.O_RTNCON > 0)
                        {
                            List<ItemList> transaction = new List<ItemList>();
                            foreach (var item in response.LISTAREGISTROS.LIST)
                            {
                                //if (int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_TIPTRN, keyDesencript), 2)) < 10)
                                //{
                                    transaction.Add(new ItemList
                                    {
                                        Item1 = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_DSCTRN, keyDesencript), 2),
                                        Item2 = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_ENTREC, keyDesencript), 2),
                                        Item3 = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_TIPTRN, keyDesencript), 2),
                                        ImageSourse = string.Concat(Utilities.GetConfiguration("ResourcesUrl"), ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_TIPTRN, keyDesencript), 2), ".png")
                                    });
                                //}
                            }
                            return new Response { Data = transaction };
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

        /// <summary>
        /// #3 Método para buscar las instituciones disponibles
        /// </summary>
        /// <returns></returns>
        private Response GetInstitutions()
        {
            try
            {
                ConsultarInstitucionesServicesClient client = new ConsultarInstitucionesServicesClient();

                using (var factory = new WebChannelFactory<ConsultarInstitucionesServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrintmulInput request = new mtrintmulInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request GetInstitutions: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrintmul(request);

                        AdminPayPlus.SaveErrorControl($"Response GetInstitutions: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.O_LISTAREGISTROS.O_RTNCON > 0)
                        {
                            List<ItemList> entities = new List<ItemList>();
                            foreach (var item in response.O_LISTAREGISTROS.LIST)
                            {
                                var entity = new ItemList
                                {
                                    Item1 = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_CODINS, keyDesencript), 2),
                                    Item2 = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_NOMINS, keyDesencript), 2),
                                };

                                var pathImage = string.Concat(Utilities.GetConfiguration("ResourcesUrl"),
                                                  ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_LOGO, keyDesencript), 2), ".png");
                                if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), string.Concat("../../", pathImage))))
                                {
                                    entity.ImageSourse = pathImage;
                                }
                                else
                                {
                                    entity.ImageSourse = string.Concat(Utilities.GetConfiguration("ResourcesUrl"), Utilities.GetConfiguration("ImageNotFound"));
                                    entity.Item3 = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_NOMINS, keyDesencript), 2);
                                }

                                entities.Add(entity);
                            }
                            return new Response { Data = entities };
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

        /// <summary>
        /// #4 Método para buscar los tipos de documentos disponibles para esa institucion
        /// </summary>
        /// <returns></returns>
        private Response GetTypeDocument(Transaction transaction)
        {
            try
            {
                TiposDocumentosServicesClient client = new TiposDocumentosServicesClient();

                using (var factory = new WebChannelFactory<TiposDocumentosServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrdoctrncInput request = new mtrdoctrncInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_TIPOTRANSACCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeTypeTransaction), keyEncript),
                        };

                        AdminPayPlus.SaveErrorControl($"Request GetTypeDocument: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);
                        
                        var response = client.mtrdoctrnc(request);

                        AdminPayPlus.SaveErrorControl($"Response GetTypeDocument: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);
                        
                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.LISTAREGISTROS.O_RTNCON > 0)
                        {
                            List<TypeDocument> typeDocument = new List<TypeDocument>();
                            foreach (var item in response.LISTAREGISTROS.LIST)
                            {
                                typeDocument.Add(new TypeDocument
                                {
                                    Key = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_TIPDOC, keyDesencript), 2),
                                    Value = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_DSCDOC, keyDesencript), 2),
                                    Type = 2,
                                });
                            }
                            return new Response { Data = typeDocument };
                        }
                    }

                    string a = Encryptor.Decrypt("Dsg4yqMdIsEEPagbylb81ymes//M9mCezr0jpfnTJCckp4w8jACB6Dmj37ezPJ8P", keyDesencript);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }

            return null;
        }

        /// <summary>
        /// #5 Método para buscar los productos del cliente
        /// </summary>
        /// <returns></returns>
        private Response GetProductsClient(Transaction transaction)
        {
            try
            {
                ConsultarProductosClienteServicesClient client = new ConsultarProductosClienteServicesClient();

                using (var factory = new WebChannelFactory<ConsultarProductosClienteServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrprocliInput request = new mtrprocliInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_NUMERODOCUMENTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript),
                            I_TIPODOCUMENTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.TypeDocument.ToString()), keyEncript),
                            I_TIPOTRANSACCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeTypeTransaction), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request GetProductsClient: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrprocli(request);

                        AdminPayPlus.SaveErrorControl($"Response GetProductsClient: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.O_LISTAREGISTROS.O_RTNCON > 0)
                        {
                            transaction.Products = new List<Product>();
                            foreach (var item in response.O_LISTAREGISTROS.LIST)
                            {
                                transaction.Products.Add(new Product
                                {
                                    Code = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_CODPRO, keyDesencript), 2),
                                    CodeSystem = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_CODSIS, keyDesencript), 2),
                                    AcountNumberMasc = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_CTAMAS, keyDesencript), 2),
                                    AmountMax = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_MONMAX, keyDesencript), 2), new CultureInfo("en-US")),
                                    AmountMin = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_MONMIN, keyDesencript), 2), new CultureInfo("en-US")),
                                    Description = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_NOMPRO, keyDesencript), 2),
                                    AcountNumber = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_NROCTA, keyDesencript), 2),
                                    AmountCommission = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_VLRCOM, keyDesencript), 2).Substring(0, (ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_VLRCOM, keyDesencript), 2).Length - 2)), new CultureInfo("en-US")),
                                });
                            }

                            transaction.payer = new PAYER
                            {
                                NAME = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NOMBRECLIENTE, keyDesencript), 2),
                                IDENTIFICATION = transaction.reference
                            };

                            transaction.Type = transaction.CodeTypeTransaction == "00003" ? ETransactionType.Withdrawal : ETransactionType.Deposit;

                            transaction.reference = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_REFERENCIATRANSACCION, keyDesencript), 2);
                            transaction.consecutive = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_TOKENTRANSACCION, keyDesencript), 2);

                            return new Response { Data = transaction };
                        }
                        else
                        {
                            return new Response { Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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

        /// <summary>
        /// #6 Método para generar el codigo OTP
        /// </summary>
        /// <returns></returns>
        private Response GenerateCodeOTP(Transaction transaction)
        {
            try
            {
                GenerarOTPServicesClient client = new GenerarOTPServicesClient();

                using (var factory = new WebChannelFactory<GenerarOTPServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrgenotpInput request = new mtrgenotpInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_TIPOTRN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeTypeTransaction), keyEncript),
                            I_CUENTA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].AcountNumber), keyEncript),
                            I_SISTEMA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].CodeSystem), keyEncript),
                            I_PRODUCTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].Code), keyEncript),
                            I_REFERENCIA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript),
                            I_TOKEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.consecutive), keyEncript),
                            I_VALOR = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Amount.ToString()), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request GenerateCodeOTP: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrgenotp(request);

                        AdminPayPlus.SaveErrorControl($"Response GenerateCodeOTP: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            transaction.CodeOTP = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOTP, keyDesencript), 2);
                            return new Response { Data = transaction };
                        }
                        else
                        {
                            return new Response { Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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

        /// <summary>
        /// #7 Método para validar el codigo OTP
        /// </summary>
        /// <returns></returns>
        private Response ValidateOTP(Transaction transaction)
        {
            try
            {
                ValidarOTPServicesClient client = new ValidarOTPServicesClient();

                using (var factory = new WebChannelFactory<ValidarOTPServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrvalotpInput request = new mtrvalotpInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_TIPOTRN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeTypeTransaction), keyEncript),
                            I_CUENTA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].AcountNumber), keyEncript),
                            I_SISTEMA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].CodeSystem), keyEncript),
                            I_PRODUCTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].Code), keyEncript),
                            I_REFERENCIA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript),
                            I_TOKEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.consecutive), keyEncript),
                            I_VALOR = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Amount.ToString()), keyEncript),
                            I_CODIGOTP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeOTP), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request ValidateOTP: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrvalotp(request);

                        AdminPayPlus.SaveErrorControl($"Response ValidateOTP: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            return new Response { Data = transaction };
                        }
                        else
                        {
                            return new Response { Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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

        /// <summary>
        /// #8 Método para reportar la transacción
        /// </summary>
        /// <returns></returns>
        private Response ReportTransaction(Transaction transaction)
        {
            try
            {
                ProcesarTransaccionServicesClient client = new ProcesarTransaccionServicesClient();

                using (var factory = new WebChannelFactory<ProcesarTransaccionServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();
                        WPFMultired.MR_ReportTransaction.iLISTAREGISTROS denominations = null;

                        if (transaction.Payment.Denominations != null)
                        {
                            denominations = new WPFMultired.MR_ReportTransaction.iLISTAREGISTROS
                            {
                                I_RTNCON = transaction.Payment.Denominations.Count,
                                LIST = new WPFMultired.MR_ReportTransaction.iLISTAREGISTROSLIST[transaction.Payment.Denominations.Count]
                            };
                            var index = 0;

                            foreach (var denomination in transaction.Payment.Denominations)
                            {
                                denominations.LIST[index] = new WPFMultired.MR_ReportTransaction.iLISTAREGISTROSLIST
                                {
                                    I_CANDEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(string.Concat(denomination.Quantity.ToString())), keyEncript),
                                    I_CODDEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(string.Concat(denomination.Denominacion.ToString())), keyEncript),
                                    I_MONEDA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetConfiguration("CuerrenId")), keyEncript),
                                    I_TIPMON = Encryptor.Encrypt(ConcatOrSplitTimeStamp((denomination.Code == "DP" || denomination.Code == "AP") ? "B" : "A"), keyEncript),
                                };
                                index++;
                            }
                        }
                        else
                        {
                            denominations = new WPFMultired.MR_ReportTransaction.iLISTAREGISTROS
                            {
                                I_RTNCON = 0,
                                LIST = { }
                            };
                        }

                        mtrprotrnInput request = new mtrprotrnInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_TIPOTRN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeTypeTransaction), keyEncript),
                            I_CUENTA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].AcountNumber), keyEncript),
                            I_SISTEMA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].CodeSystem), keyEncript),
                            I_PRODUCTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Products[0].Code), keyEncript),
                            I_REFERENCIA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript),
                            I_TOKEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.consecutive), keyEncript),
                            I_VALOR = Encryptor.Encrypt(ConcatOrSplitTimeStamp(string.Concat(transaction.Amount.ToString(), "00")), keyEncript),
                            I_CODIGOTP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeOTP), keyEncript),
                            I_LISTAREGISTROS = denominations
                        };

                        AdminPayPlus.SaveErrorControl($"Request ReportTransaction: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrprotrn(request);

                        AdminPayPlus.SaveErrorControl($"Response ReportTransaction: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            transaction.CodeTransactionAuditory = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOAUTORI, keyDesencript) ?? string.Empty, 2);
                            return new Response { Data = transaction };
                        }
                        else
                        {
                            return new Response { Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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


        /// <summary>
        /// #NN Método para validar el QR
        /// </summary>
        /// <returns></returns>
        private Response ConsultQR(Transaction transaction)
        {
            try
            {
                QRDecodeServicesClient client = new QRDecodeServicesClient();

                using (var factory = new WebChannelFactory<QRDecodeServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrctlaqrcInput request = new mtrctlaqrcInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_QRTEXT = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request ConsultQR: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrctlaqrc(request);

                        AdminPayPlus.SaveErrorControl($"Response ConsultQR: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);
                       
                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            var product = JsonConvert.DeserializeObject<DataQR>(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_OBJECT, keyDesencript), 2));
                            transaction.Product = new Product
                            {
                                Code = product.CODPRO,
                                AcountNumber = product.CTANRO,
                                AcountNumberMasc = product.CTAMAS,
                                AmountCommission = decimal.Parse(product.COMISI),
                                AmountMax = decimal.Parse(product.MONMAX),
                                AmountMin = decimal.Parse(product.MONMIN),
                                CodeSystem = product.CODSIS,
                                Description = product.NOMPRO,
                            };

                            transaction.payer = new PAYER
                            {
                                NAME = product.NOMCLI,
                                IDENTIFICATION = product.NRONIT
                            };

                            transaction.CodeTypeTransaction = product.TIPTRN;
                            transaction.reference = product.REFTRN;
                            transaction.Amount = decimal.Parse(product.VLRTRN);
                            transaction.consecutive = product.TOKTRN;
                            transaction.Type = transaction.CodeTypeTransaction == "00003" ? ETransactionType.Withdrawal : ETransactionType.Deposit;
                            return new Response { Data = transaction };
                        }
                        else
                        {
                            return new Response { Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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

        /// <summary>
        /// #1 Método para buscar los idiomas disponibles para la aplicación
        /// </summary>
        /// <returns></returns>
        private Response GetAdminStatus()
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
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request GetAdminStatus: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrvalarqc(request);

                        AdminPayPlus.SaveErrorControl($"Response GetAdminStatus: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            !string.IsNullOrEmpty(response.O_MOVIMIENTO) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            return new Response { Data = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MOVIMIENTO, keyDesencript), 2)) };
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

        private Response GetAdminOperation(ETypeAdministrator typeAdministrator)
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

                        AdminPayPlus.SaveErrorControl($"Request GetAdminOperation: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrretarqc(request);

                        AdminPayPlus.SaveErrorControl($"Response GetAdminOperation: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) && response.LISTAREGISTROS != null &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.LISTAREGISTROS.O_RTNCON > 0)
                        {
                            PaypadOperationControl result = new PaypadOperationControl()
                            {
                                DATALIST = new List<List>(),
                                TYPE = typeAdministrator,
                                ID = 0
                            };
                            foreach (var denomination in response.LISTAREGISTROS.LIST)
                            {
                                result.DATALIST.Add(new List
                                {
                                    AMOUNT = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANACT, keyDesencript), 2)),
                                    AMOUNT_NEW = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANDEN, keyDesencript), 2)),
                                    VALUE = Decimal.ToInt32(decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CODDEN, keyDesencript), 2), new CultureInfo("en-US"))),
                                    DESCRIPTION = ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_DESDON, keyDesencript), 2),
                                    TOTAL_AMOUNT = (int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANDEN, keyDesencript), 2)) +
                                    int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANACT, keyDesencript), 2))) *
                                    decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CODDEN, keyDesencript), 2), new CultureInfo("en-US")),
                                    CASSETTE = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CASSET, keyDesencript), 2)),
                                    CODE = ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_TIPMON, keyDesencript), 2),
                                    IMAGE = ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_TIPMON, keyDesencript), 2) == "B" ? ImagesUrlResource.ImgBill : ImagesUrlResource.ImgCoin,
                                    DEVICE_TYPE_ID = int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_TIPDEV, keyDesencript), 2)) == 1 ? (int)ETypeDevice.AP
                                    : int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_TIPDEV, keyDesencript), 2)) == 4 ? (int)ETypeDevice.MD
                                    : int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_TIPDEV, keyDesencript), 2)) == 3 ? (int)ETypeDevice.MA
                                    : (int)ETypeDevice.DP,
                                    ID = 0,
                                    CURRENCY_DENOMINATION_ID = 0,
                                    DEVICE_PAYPAD_ID = 0
                                });

                                result.TOTAL_NEW += int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANDEN, keyDesencript), 2)) *
                                    decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CODDEN, keyDesencript), 2), new CultureInfo("en-US"));

                                result.TOTAL_CURRENT += int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CANACT, keyDesencript), 2)) *
                                    decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(denomination.O_CODDEN, keyDesencript), 2), new CultureInfo("en-US"));

                            }
                            result.TOTAL = result.TOTAL_CURRENT + result.TOTAL_NEW;
                            return new Response { Data = result };
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

        private Response SendAdminProcess(PaypadOperationControl dataProcess)
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

                        AdminPayPlus.SaveErrorControl($"Request SendAdminProcess: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrproarqc(request);

                        AdminPayPlus.SaveErrorControl($"Response SendAdminProcess: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

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
                            dataProcess.DATE = DateTime.ParseExact(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_FECHAPROCESO, keyDesencript), 2), "yyyyMMdd", null).ToString("yyyy/MM/dd");
                            dataProcess.TIME = DateTime.ParseExact(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_HORATRANSACCION, keyDesencript), 2), "HHmmssff", null).ToString("HH:mm:ss");
                            dataProcess.SAFKEY = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_SAFKEY, keyDesencript), 2);
                            return new Response { Data = dataProcess };
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

        private Response ValidateAdminQr(string txtQr)
        {
            try
            {
                QRValidateAdminServicesClient client = new QRValidateAdminServicesClient();
                using (var factory = new WebChannelFactory<QRValidateAdminServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrvaladmcInput request = new mtrvaladmcInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp("0"), keyEncript),
                            I_QRTEXT = Encryptor.Encrypt(ConcatOrSplitTimeStamp(txtQr.Replace(" ", "+")), keyEncript),
                        };

                        AdminPayPlus.SaveErrorControl($"Request ValidateAdminQr: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrvaladmc(request);

                        AdminPayPlus.SaveErrorControl($"Response ValidateAdminQr: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            return new Response { Data = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_OBJECT, keyDesencript), 2) };
                        }
                        else
                        {
                            return new Response { Data = null, Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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

        private Response ReportCash(Transaction data)
        {
            try
            {
                ASMControllerServicesClient client = new ASMControllerServicesClient();

                using (var factory = new WebChannelFactory<ASMControllerServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        WPFMultired.MR_ControllCash.iLISTAREGISTROS denominations = null;

                        if (data.Payment.Denominations != null)
                        {
                            denominations = new WPFMultired.MR_ControllCash.iLISTAREGISTROS
                            {
                                I_RTNCON = data.Payment.Denominations.Count,
                                LIST = new WPFMultired.MR_ControllCash.iLISTAREGISTROSLIST[data.Payment.Denominations.Count]
                            };
                            var index = 0;

                            foreach (var denomination in data.Payment.Denominations)
                            {

                                denominations.LIST[index] = new WPFMultired.MR_ControllCash.iLISTAREGISTROSLIST
                                {
                                    I_CANTID = Encryptor.Encrypt(ConcatOrSplitTimeStamp(denomination.Quantity.ToString()), keyEncript),
                                    I_DENOMI = Encryptor.Encrypt(ConcatOrSplitTimeStamp(string.Format("{0:C2}", denomination.Denominacion.ToString()).Replace("$", "")), keyEncript),
                                    I_CODMON = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetConfiguration("CodMon")), keyEncript),
                                    I_TIPOMB = Encryptor.Encrypt(ConcatOrSplitTimeStamp((denomination.Code == "DP" || denomination.Code == "AP") ? "B" : "M"), keyEncript),
                                    I_TIPDEV = Encryptor.Encrypt(ConcatOrSplitTimeStamp(denomination.Rx == 1 ? "5" : denomination.Code == "AP" ? "1" : denomination.Code == "DP" ? "2" : denomination.Code == "MA" ? "3" : "4"), keyEncript),
                                };
                                index++;
                            }
                        }
                        else
                        {
                            denominations = new WPFMultired.MR_ControllCash.iLISTAREGISTROS
                            {
                                I_RTNCON = 0,
                                LIST = { }
                            };
                        }

                        mtrctlbllcInput request = new mtrctlbllcInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp("0"), keyEncript),
                            I_TRANSACCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(data.IdTransactionAPi.ToString()), keyEncript),
                            I_ESTADO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(data.State == ETransactionState.Success ? "1" : "9"), keyEncript),
                            I_TRNTAYLOR = Encryptor.Encrypt(ConcatOrSplitTimeStamp(data.CodeTransactionAuditory), keyEncript),
                            I_LISTAREGISTROS = denominations,
                        };

                        AdminPayPlus.SaveErrorControl($"Request ReportCash: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrctlbllc(request);

                        AdminPayPlus.SaveErrorControl($"Response ReportCash: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);

                        var Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2);
                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            return new Response { Data = data };
                        }
                        else
                        {
                            return new Response { Data = null, Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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

        /// <summary>
        /// #NN Método para validar el QR
        /// </summary>
        /// <returns></returns>
        private Response ConsultInvoice(Transaction transaction)
        {
            try
            {
                ConsultasRedServicesClient client = new ConsultasRedServicesClient();

                using (var factory = new WebChannelFactory<ConsultasRedServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();

                        mtrhalleycInput request = new mtrhalleycInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp("2"), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_LECTURA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.TypeDocument == "0" ? "1" : "0"), keyEncript),
                            I_TIPODOCUMENTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.TypeDocument), keyEncript),
                            I_REFERENCIA = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.reference), keyEncript),
                            I_TIPOTRANSACCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeTypeTransaction), keyEncript)
                        };

                        AdminPayPlus.SaveErrorControl($"Request ConsultInvoice: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrhalleyc(request);

                        AdminPayPlus.SaveErrorControl($"Response ConsultInvoice: {response} LLave: {keyDesencript}","", EError.Api, ELevelError.Mild);

                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0 &&
                            response.LISTAREGISTROS.O_RTNCON > 0)
                        {
                            transaction.Products = new List<Product>();

                            int count = 0;

                            foreach (var item in response.LISTAREGISTROS.LIST)
                            {
                                transaction.Products.Add(new Product
                                {
                                    Code = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_REFCLI, keyDesencript), 2),
                                    CodeSystem = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_REFRED, keyDesencript), 2),
                                    AcountNumberMasc = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_FLGDET, keyDesencript), 2),
                                    AmountMax = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_MONMAX, keyDesencript), 2), new CultureInfo("en-US")),
                                    AmountMin = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_MONMIN, keyDesencript), 2), new CultureInfo("en-US")),
                                    Description = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_DESCRI, keyDesencript), 2),
                                    AcountNumber = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_FLGMOD, keyDesencript), 2),
                                    AmountCommission = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_VLRCOM, keyDesencript), 2), new CultureInfo("en-US")),
                                    Amount = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_VLRREC, keyDesencript), 2), new CultureInfo("en-US")),
                                    AmountTotal = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_VLRREC, keyDesencript), 2), new CultureInfo("en-US")),
                                    img = "/Images/Others/Newcircle.png",
                                    TypeTransaction = ConcatOrSplitTimeStamp(Encryptor.Decrypt(item.O_DATADI, keyDesencript), 2),
                                });

                                switch (transaction.eTypeService)
                                {
                                    case ETypeServiceSelect.Deposito:
                                        break;
                                    case ETypeServiceSelect.TarjetaCredito:
                                        transaction.Products[count].ExtraTarjetaCredito = JsonConvert.DeserializeObject<DataExtraTarjetaCredito>(transaction.Products[count].TypeTransaction);
                                        break;
                                    case ETypeServiceSelect.EstadoCuenta:
                                        transaction.Products[count].AccountStateProduct = JsonConvert.DeserializeObject<AccountStateProduct>(transaction.Products[count].TypeTransaction);
                                        break;
                                }
                                count++;
                            }

                            transaction.payer = new PAYER
                            {
                                NAME = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NOMBRE, keyDesencript), 2),
                                IDENTIFICATION = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NUMERODOCUMENTO, keyDesencript), 2)
                            };

                            transaction.Type = ETransactionType.Deposit;
                            transaction.Amount = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_VALORGLOBAL, keyDesencript), 2), new CultureInfo("en-US"));
                            transaction.Observation = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_DESOPERACION, keyDesencript), 2);

                            return new Response { Data = transaction };
                        }
                        else
                        {
                            return new Response { Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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

        /// <summary>
        /// #8 Método para reportar la transacción
        /// </summary>
        /// <returns></returns>
        private Response ReportInvoice(Transaction transaction)
        {
            try
            {
                RecaudoFacturasServicesClient client = new RecaudoFacturasServicesClient();

                using (var factory = new WebChannelFactory<RecaudoFacturasServicesChannel>())
                {
                    using (new OperationContextScope((IClientChannel)client.InnerChannel))
                    {
                        SetHeaderRequest();
                        WPFMultired.MR_NotificInvoice.iLISTAREGISTROS denominations = null;

                        if (transaction.Payment.Denominations != null)
                        {
                            denominations = new WPFMultired.MR_NotificInvoice.iLISTAREGISTROS
                            {
                                I_RTNCON = transaction.Payment.Denominations.Count,
                                LIST = new WPFMultired.MR_NotificInvoice.iLISTAREGISTROSLIST[transaction.Payment.Denominations.Count]
                            };
                            var index = 0;

                            foreach (var denomination in transaction.Payment.Denominations)
                            {
                                denominations.LIST[index] = new WPFMultired.MR_NotificInvoice.iLISTAREGISTROSLIST
                                {
                                    I_CANTID = Encryptor.Encrypt(ConcatOrSplitTimeStamp(denomination.Quantity.ToString()), keyEncript),
                                    I_DENOMI = Encryptor.Encrypt(ConcatOrSplitTimeStamp(string.Format("{0:C2}", denomination.Denominacion.ToString()).Replace("$", "")), keyEncript),
                                    I_CODMON = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetConfiguration("CodMon")), keyEncript),
                                    I_TIPOMB = Encryptor.Encrypt(ConcatOrSplitTimeStamp((denomination.Code == "DP" || denomination.Code == "AP") ? "B" : "M"), keyEncript),
                                    I_TIPDEV = Encryptor.Encrypt(ConcatOrSplitTimeStamp(denomination.Rx == 1 ? "5" : denomination.Code == "AP" ? "1" : denomination.Code == "DP" ? "2" : denomination.Code == "MA" ? "3" : "4"), keyEncript)
                                };
                                index++;
                            }
                        }
                        else
                        {
                            denominations = new WPFMultired.MR_NotificInvoice.iLISTAREGISTROS
                            {
                                I_RTNCON = 0,
                                LIST = { }
                            };
                        }

                        mtrrecfaccInput request = new mtrrecfaccInput
                        {
                            I_CANAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(codeCanal), keyEncript),
                            I_DIRECCIONIP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(Utilities.GetIpPublish()), keyEncript),
                            I_ENTIDADORIGEN = Encryptor.Encrypt(ConcatOrSplitTimeStamp(sourceEntity), keyEncript),
                            I_TERMINAL = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataConfiguration.ID_PAYPAD.ToString()), keyEncript),
                            I_TIMESTAMP = Encryptor.Encrypt(ConcatOrSplitTimeStamp(((long)(DateTime.UtcNow - timerSeed).TotalMilliseconds).ToString()), keyEncript),
                            I_LENGUAJE = Encryptor.Encrypt(ConcatOrSplitTimeStamp(AdminPayPlus.DataPayPlus.IdiomId.ToString()), keyEncript),
                            I_INSTITUCION = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.CodeCompany), keyEncript),
                            I_KEYASM = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.IdTransactionAPi.ToString()), keyEncript),
                            I_KEYRED = Encryptor.Encrypt(ConcatOrSplitTimeStamp(transaction.Product.CodeSystem), keyEncript),
                            I_VALORDEVUELTO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(string.Format("{0:C2}", transaction.Payment.ValorDispensado.ToString()).Replace("$", "")), keyEncript),
                            I_VALORRECAUDADO = Encryptor.Encrypt(ConcatOrSplitTimeStamp(string.Format("{0:C2}", transaction.Payment.ValorIngresado.ToString()).Replace("$", "")), keyEncript),
                            I_LISTAREGISTROS = denominations
                        };

                        AdminPayPlus.SaveErrorControl($"Request ReportInvoice: {JsonConvert.SerializeObject(request)}  LLave: {keyEncript}", "", EError.Aplication, ELevelError.Mild);

                        var response = client.mtrrecfacc(request);

                        AdminPayPlus.SaveErrorControl($"Response ReportInvoice: {response} LLave: {keyDesencript}", "", EError.Api, ELevelError.Mild);


                        if (response != null && !string.IsNullOrEmpty(response.O_CODIGOERROR) &&
                            int.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_CODIGOERROR, keyDesencript), 2)) == 0)
                        {
                            transaction.consecutive = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_APROBACION, keyDesencript), 2);
                            transaction.Amount = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_VALORRECAUDADO, keyDesencript), 2), new CultureInfo("en-US"));
                            transaction.Payment.ValorSobrante = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_VALOREXCESO, keyDesencript), 2), new CultureInfo("en-US"));
                            transaction.reference = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_REFERENCIA, keyDesencript), 2);
                            transaction.nameentity = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_NOMBREENTIDAD, keyDesencript), 2);
                            transaction.AmountComission = decimal.Parse(ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_VALORCOMISION, keyDesencript), 2), new CultureInfo("en-US"));
                            return new Response { Data = transaction };
                        }
                        else
                        {
                            return new Response { Message = ConcatOrSplitTimeStamp(Encryptor.Decrypt(response.O_MENSAJEERROR, keyDesencript), 2) };
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
                timeStamp = (long)(DateTime.UtcNow - timerSeed).TotalMilliseconds;

                WebOperationContext.Current.OutgoingRequest.Headers.Add("USERNAME", Encryptor.Encrypt(ConcatOrSplitTimeStamp(userName), keyEncript));
                WebOperationContext.Current.OutgoingRequest.Headers.Add("USERTOKEN", Encryptor.Encrypt(ConcatOrSplitTimeStamp(token), keyEncript));
                WebOperationContext.Current.OutgoingRequest.Headers.Add("MESSAGEID", Encryptor.Encrypt(ConcatOrSplitTimeStamp((new Random()).Next(1000, 9999).ToString()), keyEncript));
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        #endregion
    }
}
