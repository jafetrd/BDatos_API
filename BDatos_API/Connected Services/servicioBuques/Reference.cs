﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BDatos_API.servicioBuques {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="Buques", Namespace="http://schemas.datacontract.org/2004/07/ServicioBroker.Models")]
    [System.SerializableAttribute()]
    public partial class Buques : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string BUQUEField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string IDField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string VIAJEField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string BUQUE {
            get {
                return this.BUQUEField;
            }
            set {
                if ((object.ReferenceEquals(this.BUQUEField, value) != true)) {
                    this.BUQUEField = value;
                    this.RaisePropertyChanged("BUQUE");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string ID {
            get {
                return this.IDField;
            }
            set {
                if ((object.ReferenceEquals(this.IDField, value) != true)) {
                    this.IDField = value;
                    this.RaisePropertyChanged("ID");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string VIAJE {
            get {
                return this.VIAJEField;
            }
            set {
                if ((object.ReferenceEquals(this.VIAJEField, value) != true)) {
                    this.VIAJEField = value;
                    this.RaisePropertyChanged("VIAJE");
                }
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
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="servicioBuques.IBuques", CallbackContract=typeof(BDatos_API.servicioBuques.IBuquesCallback))]
    public interface IBuques {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/Subscribe", ReplyAction="http://tempuri.org/IBuques/SubscribeResponse")]
        void Subscribe();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/Subscribe", ReplyAction="http://tempuri.org/IBuques/SubscribeResponse")]
        System.Threading.Tasks.Task SubscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/Unsubscribe", ReplyAction="http://tempuri.org/IBuques/UnsubscribeResponse")]
        void Unsubscribe();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/Unsubscribe", ReplyAction="http://tempuri.org/IBuques/UnsubscribeResponse")]
        System.Threading.Tasks.Task UnsubscribeAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/obtenerTodosBuque", ReplyAction="http://tempuri.org/IBuques/obtenerTodosBuqueResponse")]
        BDatos_API.servicioBuques.Buques[] obtenerTodosBuque();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/obtenerTodosBuque", ReplyAction="http://tempuri.org/IBuques/obtenerTodosBuqueResponse")]
        System.Threading.Tasks.Task<BDatos_API.servicioBuques.Buques[]> obtenerTodosBuqueAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/IBuques", ReplyAction="http://tempuri.org/IBuques/IBuquesResponse")]
        void IBuques(string BUQUE, string VIAJE, string tipo_Cambio);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/IBuques", ReplyAction="http://tempuri.org/IBuques/IBuquesResponse")]
        System.Threading.Tasks.Task IBuquesAsync(string BUQUE, string VIAJE, string tipo_Cambio);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBuquesCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IBuques/cambiosBuques", ReplyAction="http://tempuri.org/IBuques/cambiosBuquesResponse")]
        void cambiosBuques(string BUQUE, string VIAJE, string tipo_Cambio);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IBuquesChannel : BDatos_API.servicioBuques.IBuques, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class BuquesClient : System.ServiceModel.DuplexClientBase<BDatos_API.servicioBuques.IBuques>, BDatos_API.servicioBuques.IBuques {
        
        public BuquesClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public BuquesClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public BuquesClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public BuquesClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public BuquesClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void Subscribe() {
            base.Channel.Subscribe();
        }
        
        public System.Threading.Tasks.Task SubscribeAsync() {
            return base.Channel.SubscribeAsync();
        }
        
        public void Unsubscribe() {
            base.Channel.Unsubscribe();
        }
        
        public System.Threading.Tasks.Task UnsubscribeAsync() {
            return base.Channel.UnsubscribeAsync();
        }
        
        public BDatos_API.servicioBuques.Buques[] obtenerTodosBuque() {
            return base.Channel.obtenerTodosBuque();
        }
        
        public System.Threading.Tasks.Task<BDatos_API.servicioBuques.Buques[]> obtenerTodosBuqueAsync() {
            return base.Channel.obtenerTodosBuqueAsync();
        }
        
        public void IBuques(string BUQUE, string VIAJE, string tipo_Cambio) {
            base.Channel.IBuques(BUQUE, VIAJE, tipo_Cambio);
        }
        
        public System.Threading.Tasks.Task IBuquesAsync(string BUQUE, string VIAJE, string tipo_Cambio) {
            return base.Channel.IBuquesAsync(BUQUE, VIAJE, tipo_Cambio);
        }
    }
}
