﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFMultired.MR_Language {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://consultarlenguaje.wsbeans.iseries/", ConfigurationName="MR_Language.ConsultarLenguajeServices")]
    public interface ConsultarLenguajeServices {
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://consultarlenguaje.wsbeans.iseries/ConsultarLenguajeServices/mtrindlenReque" +
            "st", ReplyAction="http://consultarlenguaje.wsbeans.iseries/ConsultarLenguajeServices/mtrindlenRespo" +
            "nse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        WPFMultired.MR_Language.mtrindlenResponse mtrindlen(WPFMultired.MR_Language.mtrindlenRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://consultarlenguaje.wsbeans.iseries/ConsultarLenguajeServices/mtrindlenReque" +
            "st", ReplyAction="http://consultarlenguaje.wsbeans.iseries/ConsultarLenguajeServices/mtrindlenRespo" +
            "nse")]
        System.Threading.Tasks.Task<WPFMultired.MR_Language.mtrindlenResponse> mtrindlenAsync(WPFMultired.MR_Language.mtrindlenRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://consultarlenguaje.wsbeans.iseries/")]
    public partial class mtrindlenInput : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string i_TERMINALField;
        
        private string i_DIRECCIONIPField;
        
        private string i_TIMESTAMPField;
        
        private string i_CANALField;
        
        private string i_LENGUAJEField;
        
        private string i_ENTIDADORIGENField;
        
        private string i_INSTITUCIONField;
        
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://consultarlenguaje.wsbeans.iseries/")]
    public partial class oLISTAREGISTROSLIST : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string o_INDLENField;
        
        private string o_NOMLENField;
        
        private string o_BANDERField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string O_INDLEN {
            get {
                return this.o_INDLENField;
            }
            set {
                this.o_INDLENField = value;
                this.RaisePropertyChanged("O_INDLEN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string O_NOMLEN {
            get {
                return this.o_NOMLENField;
            }
            set {
                this.o_NOMLENField = value;
                this.RaisePropertyChanged("O_NOMLEN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string O_BANDER {
            get {
                return this.o_BANDERField;
            }
            set {
                this.o_BANDERField = value;
                this.RaisePropertyChanged("O_BANDER");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://consultarlenguaje.wsbeans.iseries/")]
    public partial class oLISTAREGISTROS : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long o_RTNCONField;
        
        private oLISTAREGISTROSLIST[] lISTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public long O_RTNCON {
            get {
                return this.o_RTNCONField;
            }
            set {
                this.o_RTNCONField = value;
                this.RaisePropertyChanged("O_RTNCON");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("LIST", Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public oLISTAREGISTROSLIST[] LIST {
            get {
                return this.lISTField;
            }
            set {
                this.lISTField = value;
                this.RaisePropertyChanged("LIST");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://consultarlenguaje.wsbeans.iseries/")]
    public partial class mtrindlenResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string o_CODIGOERRORField;
        
        private string o_MENSAJEERRORField;
        
        private oLISTAREGISTROS o_LISTAREGISTROSField;
        
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
        public oLISTAREGISTROS O_LISTAREGISTROS {
            get {
                return this.o_LISTAREGISTROSField;
            }
            set {
                this.o_LISTAREGISTROSField = value;
                this.RaisePropertyChanged("O_LISTAREGISTROS");
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="mtrindlen", WrapperNamespace="http://consultarlenguaje.wsbeans.iseries/", IsWrapped=true)]
    public partial class mtrindlenRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://consultarlenguaje.wsbeans.iseries/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WPFMultired.MR_Language.mtrindlenInput arg0;
        
        public mtrindlenRequest() {
        }
        
        public mtrindlenRequest(WPFMultired.MR_Language.mtrindlenInput arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="mtrindlenResponse", WrapperNamespace="http://consultarlenguaje.wsbeans.iseries/", IsWrapped=true)]
    public partial class mtrindlenResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://consultarlenguaje.wsbeans.iseries/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WPFMultired.MR_Language.mtrindlenResult @return;
        
        public mtrindlenResponse() {
        }
        
        public mtrindlenResponse(WPFMultired.MR_Language.mtrindlenResult @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ConsultarLenguajeServicesChannel : WPFMultired.MR_Language.ConsultarLenguajeServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ConsultarLenguajeServicesClient : System.ServiceModel.ClientBase<WPFMultired.MR_Language.ConsultarLenguajeServices>, WPFMultired.MR_Language.ConsultarLenguajeServices {
        
        public ConsultarLenguajeServicesClient() {
        }
        
        public ConsultarLenguajeServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ConsultarLenguajeServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConsultarLenguajeServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ConsultarLenguajeServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WPFMultired.MR_Language.mtrindlenResponse WPFMultired.MR_Language.ConsultarLenguajeServices.mtrindlen(WPFMultired.MR_Language.mtrindlenRequest request) {
            return base.Channel.mtrindlen(request);
        }
        
        public WPFMultired.MR_Language.mtrindlenResult mtrindlen(WPFMultired.MR_Language.mtrindlenInput arg0) {
            WPFMultired.MR_Language.mtrindlenRequest inValue = new WPFMultired.MR_Language.mtrindlenRequest();
            inValue.arg0 = arg0;
            WPFMultired.MR_Language.mtrindlenResponse retVal = ((WPFMultired.MR_Language.ConsultarLenguajeServices)(this)).mtrindlen(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WPFMultired.MR_Language.mtrindlenResponse> WPFMultired.MR_Language.ConsultarLenguajeServices.mtrindlenAsync(WPFMultired.MR_Language.mtrindlenRequest request) {
            return base.Channel.mtrindlenAsync(request);
        }
        
        public System.Threading.Tasks.Task<WPFMultired.MR_Language.mtrindlenResponse> mtrindlenAsync(WPFMultired.MR_Language.mtrindlenInput arg0) {
            WPFMultired.MR_Language.mtrindlenRequest inValue = new WPFMultired.MR_Language.mtrindlenRequest();
            inValue.arg0 = arg0;
            return ((WPFMultired.MR_Language.ConsultarLenguajeServices)(this)).mtrindlenAsync(inValue);
        }
    }
}