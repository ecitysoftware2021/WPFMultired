﻿#pragma checksum "..\..\..\..\Windows\Alerts\ModalWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "8570712C2C638E1964E3862EAFA7BAFFBC63F7197E9268A4AD82CF8F9991BB05"
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
using WPFMultired.Windows;
using WpfAnimatedGif;


namespace WPFMultired.Windows {
    
    
    /// <summary>
    /// ModalWindow
    /// </summary>
    public partial class ModalWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image GifLoadder;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LblTittle;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LblMessage;
        
        #line default
        #line hidden
        
        
        #line 68 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock LblMessageTouch;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BtnOk;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BtnNo;
        
        #line default
        #line hidden
        
        
        #line 100 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BtnYes;
        
        #line default
        #line hidden
        
        
        #line 109 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BtnFinish;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image BtnEnterMony;
        
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
            System.Uri resourceLocater = new System.Uri("/WPFMultired;component/windows/alerts/modalwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
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
            
            #line 18 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
            ((System.Windows.Controls.Grid)(target)).TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.Grid_TouchDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.GifLoadder = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.LblTittle = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 4:
            this.LblMessage = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 5:
            this.LblMessageTouch = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 6:
            this.BtnOk = ((System.Windows.Controls.Image)(target));
            
            #line 88 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
            this.BtnOk.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.BtnOk_TouchDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.BtnNo = ((System.Windows.Controls.Image)(target));
            
            #line 97 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
            this.BtnNo.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.BtnNo_TouchDown);
            
            #line default
            #line hidden
            return;
            case 8:
            this.BtnYes = ((System.Windows.Controls.Image)(target));
            
            #line 106 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
            this.BtnYes.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.BtnYes_TouchDown);
            
            #line default
            #line hidden
            return;
            case 9:
            this.BtnFinish = ((System.Windows.Controls.Image)(target));
            
            #line 115 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
            this.BtnFinish.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.BtnYes_TouchDown);
            
            #line default
            #line hidden
            return;
            case 10:
            this.BtnEnterMony = ((System.Windows.Controls.Image)(target));
            
            #line 124 "..\..\..\..\Windows\Alerts\ModalWindow.xaml"
            this.BtnEnterMony.TouchDown += new System.EventHandler<System.Windows.Input.TouchEventArgs>(this.BtnNo_TouchDown);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

