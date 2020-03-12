﻿#pragma checksum "..\..\..\UserControls\ConsultUserControl.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6B9467476CFFD5275EADAFD12420B2608CD3B6E98D04D7A86156911B3942D3C6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
using MaterialDesignThemes.Wpf.Transitions;
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
using WPFMultired.Keyboard;
using WPFMultired.UserControls;


namespace WPFMultired.UserControls {
    
    
    /// <summary>
    /// ConsultUserControl
    /// </summary>
    public partial class ConsultUserControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image btn_exit;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image btn_back;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txt_title;
        
        #line default
        #line hidden
        
        
        #line 57 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid gd_payer;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmb_type_id;
        
        #line default
        #line hidden
        
        
        #line 102 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.PasswordBox PassBoxIdentification;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TbxIdentification;
        
        #line default
        #line hidden
        
        
        #line 132 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image btn_show_id;
        
        #line default
        #line hidden
        
        
        #line 146 "..\..\..\UserControls\ConsultUserControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image btn_consult;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFMultired;component/usercontrols/consultusercontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControls\ConsultUserControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal System.Delegate _CreateDelegate(System.Type delegateType, string handler) {
            return System.Delegate.CreateDelegate(delegateType, this, handler);
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
            
            #line 9 "..\..\..\UserControls\ConsultUserControl.xaml"
            ((WPFMultired.UserControls.ConsultUserControl)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.btn_exit = ((System.Windows.Controls.Image)(target));
            
            #line 32 "..\..\..\UserControls\ConsultUserControl.xaml"
            this.btn_exit.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.Btn_exit_TouchDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.btn_back = ((System.Windows.Controls.Image)(target));
            
            #line 41 "..\..\..\UserControls\ConsultUserControl.xaml"
            this.btn_back.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.Btn_back_TouchDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.txt_title = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.gd_payer = ((System.Windows.Controls.Grid)(target));
            return;
            case 6:
            this.cmb_type_id = ((System.Windows.Controls.ComboBox)(target));
            
            #line 88 "..\..\..\UserControls\ConsultUserControl.xaml"
            this.cmb_type_id.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Cmb_type_id_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.PassBoxIdentification = ((System.Windows.Controls.PasswordBox)(target));
            return;
            case 8:
            this.TbxIdentification = ((System.Windows.Controls.TextBox)(target));
            return;
            case 9:
            this.btn_show_id = ((System.Windows.Controls.Image)(target));
            
            #line 137 "..\..\..\UserControls\ConsultUserControl.xaml"
            this.btn_show_id.TouchEnter += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.Btn_show_id_TouchEnter);
            
            #line default
            #line hidden
            
            #line 138 "..\..\..\UserControls\ConsultUserControl.xaml"
            this.btn_show_id.TouchLeave += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.Btn_show_id_TouchLeave);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btn_consult = ((System.Windows.Controls.Image)(target));
            
            #line 151 "..\..\..\UserControls\ConsultUserControl.xaml"
            this.btn_consult.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.Btn_consult_TouchDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

