﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WPFMultired.MR_OperationAdmin {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://retrieveoperationadmin.wsbeans.iseries/", ConfigurationName="MR_OperationAdmin.RetrieveOperationAdminServices")]
    public interface RetrieveOperationAdminServices {
        
        // CODEGEN: El parámetro 'return' requiere información adicional de esquema que no se puede capturar con el modo de parámetros. El atributo específico es 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="http://retrieveoperationadmin.wsbeans.iseries/RetrieveOperationAdminServices/mtrr" +
            "etarqcRequest", ReplyAction="http://retrieveoperationadmin.wsbeans.iseries/RetrieveOperationAdminServices/mtrr" +
            "etarqcResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        WPFMultired.MR_OperationAdmin.mtrretarqcResponse mtrretarqc(WPFMultired.MR_OperationAdmin.mtrretarqcRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://retrieveoperationadmin.wsbeans.iseries/RetrieveOperationAdminServices/mtrr" +
            "etarqcRequest", ReplyAction="http://retrieveoperationadmin.wsbeans.iseries/RetrieveOperationAdminServices/mtrr" +
            "etarqcResponse")]
        System.Threading.Tasks.Task<WPFMultired.MR_OperationAdmin.mtrretarqcResponse> mtrretarqcAsync(WPFMultired.MR_OperationAdmin.mtrretarqcRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.8.3752.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://retrieveoperationadmin.wsbeans.iseries/")]
    public partial class mtrretarqcInput : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string i_TERMINALField;
        
        private string i_DIRECCIONIPField;
        
        private string i_TIMESTAMPField;
        
        private string i_CANALField;
        
        private string i_LENGUAJEField;
        
        private string i_ENTIDADORIGENField;
        
        private string i_INSTITUCIONField;
        
        private string i_MOVIMIENTOField;
        
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
        public string I_MOVIMIENTO {
            get {
                return this.i_MOVIMIENTOField;
            }
            set {
                this.i_MOVIMIENTOField = value;
                this.RaisePropertyChanged("I_MOVIMIENTO");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://retrieveoperationadmin.wsbeans.iseries/")]
    public partial class listaregistroslist : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string o_CASSETField;
        
        private string o_MONEDAField;
        
        private string o_DESMONField;
        
        private string o_TIPMONField;
        
        private string o_DESDONField;
        
        private string o_CODDENField;
        
        private string o_CANDENField;
        
        private string o_CANACTField;
        
        private string o_TIPDEVField;
        
        private string o_DESDEVField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public string O_CASSET {
            get {
                return this.o_CASSETField;
            }
            set {
                this.o_CASSETField = value;
                this.RaisePropertyChanged("O_CASSET");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=1)]
        public string O_MONEDA {
            get {
                return this.o_MONEDAField;
            }
            set {
                this.o_MONEDAField = value;
                this.RaisePropertyChanged("O_MONEDA");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=2)]
        public string O_DESMON {
            get {
                return this.o_DESMONField;
            }
            set {
                this.o_DESMONField = value;
                this.RaisePropertyChanged("O_DESMON");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=3)]
        public string O_TIPMON {
            get {
                return this.o_TIPMONField;
            }
            set {
                this.o_TIPMONField = value;
                this.RaisePropertyChanged("O_TIPMON");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=4)]
        public string O_DESDON {
            get {
                return this.o_DESDONField;
            }
            set {
                this.o_DESDONField = value;
                this.RaisePropertyChanged("O_DESDON");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=5)]
        public string O_CODDEN {
            get {
                return this.o_CODDENField;
            }
            set {
                this.o_CODDENField = value;
                this.RaisePropertyChanged("O_CODDEN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=6)]
        public string O_CANDEN {
            get {
                return this.o_CANDENField;
            }
            set {
                this.o_CANDENField = value;
                this.RaisePropertyChanged("O_CANDEN");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=7)]
        public string O_CANACT {
            get {
                return this.o_CANACTField;
            }
            set {
                this.o_CANACTField = value;
                this.RaisePropertyChanged("O_CANACT");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=8)]
        public string O_TIPDEV {
            get {
                return this.o_TIPDEVField;
            }
            set {
                this.o_TIPDEVField = value;
                this.RaisePropertyChanged("O_TIPDEV");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=9)]
        public string O_DESDEV {
            get {
                return this.o_DESDEVField;
            }
            set {
                this.o_DESDEVField = value;
                this.RaisePropertyChanged("O_DESDEV");
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://retrieveoperationadmin.wsbeans.iseries/")]
    public partial class listaregistros : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int o_RTNCONField;
        
        private listaregistroslist[] lISTField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified, Order=0)]
        public int O_RTNCON {
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
        public listaregistroslist[] LIST {
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
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://retrieveoperationadmin.wsbeans.iseries/")]
    public partial class mtrretarqcResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string o_CODIGOERRORField;
        
        private string o_MENSAJEERRORField;
        
        private listaregistros lISTAREGISTROSField;
        
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
        public listaregistros LISTAREGISTROS {
            get {
                return this.lISTAREGISTROSField;
            }
            set {
                this.lISTAREGISTROSField = value;
                this.RaisePropertyChanged("LISTAREGISTROS");
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
    [System.ServiceModel.MessageContractAttribute(WrapperName="mtrretarqc", WrapperNamespace="http://retrieveoperationadmin.wsbeans.iseries/", IsWrapped=true)]
    public partial class mtrretarqcRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://retrieveoperationadmin.wsbeans.iseries/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WPFMultired.MR_OperationAdmin.mtrretarqcInput arg0;
        
        public mtrretarqcRequest() {
        }
        
        public mtrretarqcRequest(WPFMultired.MR_OperationAdmin.mtrretarqcInput arg0) {
            this.arg0 = arg0;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="mtrretarqcResponse", WrapperNamespace="http://retrieveoperationadmin.wsbeans.iseries/", IsWrapped=true)]
    public partial class mtrretarqcResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://retrieveoperationadmin.wsbeans.iseries/", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public WPFMultired.MR_OperationAdmin.mtrretarqcResult @return;
        
        public mtrretarqcResponse() {
        }
        
        public mtrretarqcResponse(WPFMultired.MR_OperationAdmin.mtrretarqcResult @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface RetrieveOperationAdminServicesChannel : WPFMultired.MR_OperationAdmin.RetrieveOperationAdminServices, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class RetrieveOperationAdminServicesClient : System.ServiceModel.ClientBase<WPFMultired.MR_OperationAdmin.RetrieveOperationAdminServices>, WPFMultired.MR_OperationAdmin.RetrieveOperationAdminServices {
        
        public RetrieveOperationAdminServicesClient() {
        }
        
        public RetrieveOperationAdminServicesClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public RetrieveOperationAdminServicesClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RetrieveOperationAdminServicesClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public RetrieveOperationAdminServicesClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        WPFMultired.MR_OperationAdmin.mtrretarqcResponse WPFMultired.MR_OperationAdmin.RetrieveOperationAdminServices.mtrretarqc(WPFMultired.MR_OperationAdmin.mtrretarqcRequest request) {
            return base.Channel.mtrretarqc(request);
        }
        
        public WPFMultired.MR_OperationAdmin.mtrretarqcResult mtrretarqc(WPFMultired.MR_OperationAdmin.mtrretarqcInput arg0) {
            WPFMultired.MR_OperationAdmin.mtrretarqcRequest inValue = new WPFMultired.MR_OperationAdmin.mtrretarqcRequest();
            inValue.arg0 = arg0;
            WPFMultired.MR_OperationAdmin.mtrretarqcResponse retVal = ((WPFMultired.MR_OperationAdmin.RetrieveOperationAdminServices)(this)).mtrretarqc(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<WPFMultired.MR_OperationAdmin.mtrretarqcResponse> WPFMultired.MR_OperationAdmin.RetrieveOperationAdminServices.mtrretarqcAsync(WPFMultired.MR_OperationAdmin.mtrretarqcRequest request) {
            return base.Channel.mtrretarqcAsync(request);
        }
        
        public System.Threading.Tasks.Task<WPFMultired.MR_OperationAdmin.mtrretarqcResponse> mtrretarqcAsync(WPFMultired.MR_OperationAdmin.mtrretarqcInput arg0) {
            WPFMultired.MR_OperationAdmin.mtrretarqcRequest inValue = new WPFMultired.MR_OperationAdmin.mtrretarqcRequest();
            inValue.arg0 = arg0;
            return ((WPFMultired.MR_OperationAdmin.RetrieveOperationAdminServices)(this)).mtrretarqcAsync(inValue);
        }
    }
}
