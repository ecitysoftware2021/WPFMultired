﻿#pragma checksum "..\..\..\UserControls\PaymentUserControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DE50573071600B44DF6D35DE54CBDF755C5FF4F904BE42C2D5045BDFE5CFD50D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPFMultired.UserControls;


namespace WPFMultired.UserControls {
    
    
    /// <summary>
    /// PaymentUserControl
    /// </summary>
    public partial class PaymentUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 24 "..\..\..\UserControls\PaymentUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ContentControl grvPublicity;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\UserControls\PaymentUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtHoraActual;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\UserControls\PaymentUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image btnCancell;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\UserControls\PaymentUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView lv_denominations;
        
        #line default
        #line hidden
        
        
        #line 211 "..\..\..\UserControls\PaymentUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtPayValue;
        
        #line default
        #line hidden
        
        
        #line 222 "..\..\..\UserControls\PaymentUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtPayValueData;
        
        #line default
        #line hidden
        
        
        #line 244 "..\..\..\UserControls\PaymentUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image btnConsign;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPFMultired;component/usercontrols/paymentusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\PaymentUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.grvPublicity = ((System.Windows.Controls.ContentControl)(target));
            return;
            case 2:
            this.txtHoraActual = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 3:
            this.btnCancell = ((System.Windows.Controls.Image)(target));
            
            #line 44 "..\..\..\UserControls\PaymentUserControl.xaml"
            this.btnCancell.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.BtnCancell_TouchDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.lv_denominations = ((System.Windows.Controls.ListView)(target));
            return;
            case 5:
            this.txtPayValue = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.txtPayValueData = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 7:
            this.btnConsign = ((System.Windows.Controls.Image)(target));
            
            #line 252 "..\..\..\UserControls\PaymentUserControl.xaml"
            this.btnConsign.StylusDown += new System.Windows.Input.StylusDownEventHandler(this.BtnConsign_StylusDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

