﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFMultired.MR_CodeQR {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://qrdecode.wsbeans.iseries/", ConfigurationName="MR_CodeQR.QRDecodeServices")]
    public interface QRDecodeServices {
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://qrdecode.wsbeans.iseries/QRDecodeServices/mtrctlaqrcRequest", ReplyAction="http://qrdecode.wsbeans.iseries/QRDecodeServices/mtrctlaqrcResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        WPFMultired.MR_CodeQR.mtrctlaqrcResponse mtrctlaqrc(WPFMultired.MR_CodeQR.mtrctlaqrcRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://qrdecode.wsbeans.iseries/QRDecodeServices/mtrctlaqrcRequest", ReplyAction="http://qrdecode.wsbeans.iseries/QRDecodeServices/mtrctlaqrcResponse")]
        System.Threading.Tasks.Task<WPFMultired.MR_CodeQR.mtrctlaqrcResponse> mtrctlaqrcAsync(WPFMultired.MR_CodeQR.mtrctlaqrcRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://qrdecode.wsbeans.iseries/")]
    public partial class mtrctlaqrcInput : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string i_TERMINALField;
        
        private string i_DIRECCIONIPField;
        
        private string i_TIMESTAMPField;
        
        private string i_CANALField;
        
        private string i_LENGUAJEField;
        
        private string i_ENTIDADORIGENField;
        
        private string i_INSTITUCIONField;
        
        private string i_QRTEXTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string I_TERMINAL {
            get {
                return this.i_TERMINALField;
            }
            set {
                this.i_TERMINALField = value;
                this.RaisePropertyChanged("I_TERMINAL");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string I_DIRECCIONIP {
            get {
                return this.i_DIRECCIONIPField;
            }
            set {
                this.i_DIRECCIONIPField = value;
                this.RaisePropertyChanged("I_DIRECCIONIP");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string I_TIMESTAMP {
            get {
                return this.i_TIMESTAMPField;
            }
            set {
                this.i_TIMESTAMPField = value;
                this.RaisePropertyChanged("I_TIMESTAMP");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string I_CANAL {
            get {
                return this.i_CANALField;
            }
            set {
                this.i_CANALField = value;
                this.RaisePropertyChanged("I_CANAL");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string I_LENGUAJE {
            get {
                return this.i_LENGUAJEField;
            }
            set {
                this.i_LENGUAJEField = value;
                this.RaisePropertyChanged("I_LENGUAJE");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string I_ENTIDADORIGEN {
            get {
                return this.i_ENTIDADORIGENField;
            }
            set {
                this.i_ENTIDADORIGENField = value;
                this.RaisePropertyChanged("I_ENTIDADORIGEN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string I_INSTITUCION {
            get {
                return this.i_INSTITUCIONField;
            }
            set {
                this.i_INSTITUCIONField = value;
                this.RaisePropertyChanged("I_INSTITUCION");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string I_QRTEXT {
            get {
                return this.i_QRTEXTField;
            }
            set {
                this.i_QRTEXTField = value;
                this.RaisePropertyChanged("I_QRTEXT");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://qrdecode.wsbeans.iseries/")]
    public partial class mtrctlaqrcResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string o_CODIGOERRORField;
        
        private string o_MENSAJEERRORField;
        
        private string o_OBJECTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string O_CODIGOERROR {
            get {
                return this.o_CODIGOERRORField;
            }
            set {
                this.o_CODIGOERRORField = value;
                this.RaisePropertyChanged("O_CODIGOERROR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string O_MENSAJEERROR {
            get {
                return this.o_MENSAJEERRORField;
            }
            set {
                this.o_MENSAJEERRORField = value;
                this.RaisePropertyChanged("O_MENSAJEERROR");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string O_OBJECT {
            get {
                return this.o_OBJECTField;
            }
            set {
                this.o_OBJECTField = value;
                this.RaisePropertyChanged("O_OBJECT");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="mtrctlaqrc", WrapperNamespace="http://qrdecode.wsbeans.iseries/", IsWrapped=true)]
    public partial class mtrctlaqrcRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qrdecode.wsbeans.iseries/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WPFMultired.MR_CodeQR.mtrctlaqrcInput arg0;
        
        public mtrctlaqrcRequest() {
        }
        
        public mtrctlaqrcRequest(WPFMultired.MR_CodeQR.mtrctlaqrcInput arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="mtrctlaqrcResponse", WrapperNamespace="http://qrdecode.wsbeans.iseries/", IsWrapped=true)]
    public partial class mtrctlaqrcResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://qrdecode.wsbeans.iseries/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WPFMultired.MR_CodeQR.mtrctlaqrcResult @return;
        
        public mtrctlaqrcResponse() {
        }
        
        public mtrctlaqrcResponse(WPFMultired.MR_CodeQR.mtrctlaqrcResult @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface QRDecodeServicesChannel : WPFMultired.MR_CodeQR.QRDecodeServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class QRDecodeServicesClient : System.ServiceModel.ClientBase<WPFMultired.MR_CodeQR.QRDecodeServices>, WPFMultired.MR_CodeQR.QRDecodeServices {
        
        public QRDecodeServicesClient() {
        }
        
        public QRDecodeServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public QRDecodeServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QRDecodeServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public QRDecodeServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WPFMultired.MR_CodeQR.mtrctlaqrcResponse WPFMultired.MR_CodeQR.QRDecodeServices.mtrctlaqrc(WPFMultired.MR_CodeQR.mtrctlaqrcRequest request) {
            return base.Channel.mtrctlaqrc(request);
        }
        
        public WPFMultired.MR_CodeQR.mtrctlaqrcResult mtrctlaqrc(WPFMultired.MR_CodeQR.mtrctlaqrcInput arg0) {
            WPFMultired.MR_CodeQR.mtrctlaqrcRequest inValue = new WPFMultired.MR_CodeQR.mtrctlaqrcRequest();
            inValue.arg0 = arg0;
            WPFMultired.MR_CodeQR.mtrctlaqrcResponse retVal = ((WPFMultired.MR_CodeQR.QRDecodeServices)(this)).mtrctlaqrc(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WPFMultired.MR_CodeQR.mtrctlaqrcResponse> WPFMultired.MR_CodeQR.QRDecodeServices.mtrctlaqrcAsync(WPFMultired.MR_CodeQR.mtrctlaqrcRequest request) {
            return base.Channel.mtrctlaqrcAsync(request);
        }
        
        public System.Threading.Tasks.Task<WPFMultired.MR_CodeQR.mtrctlaqrcResponse> mtrctlaqrcAsync(WPFMultired.MR_CodeQR.mtrctlaqrcInput arg0) {
            WPFMultired.MR_CodeQR.mtrctlaqrcRequest inValue = new WPFMultired.MR_CodeQR.mtrctlaqrcRequest();
            inValue.arg0 = arg0;
            return ((WPFMultired.MR_CodeQR.QRDecodeServices)(this)).mtrctlaqrcAsync(inValue);
        }
    }
}