﻿

#pragma checksum "C:\Users\Bo\Documents\GitHub\Owl\OwlWindowsPhoneApp\View\CameraPhotoUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7AB3CFBBDA564646FC42606351F57ED0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OwlWindowsPhoneApp
{
    partial class CameraPhotoUserControl : global::Windows.UI.Xaml.Controls.UserControl, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 14 "..\..\View\CameraPhotoUserControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.CaptureElement_Photo_PointerPressed;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 20 "..\..\View\CameraPhotoUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_RotateCamera_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 23 "..\..\View\CameraPhotoUserControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerPressed += this.AppBarButton_Focus_PointerPressed;
                 #line default
                 #line hidden
                #line 24 "..\..\View\CameraPhotoUserControl.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).PointerReleased += this.AppBarButton_Focus_PointerReleased;
                 #line default
                 #line hidden
                #line 24 "..\..\View\CameraPhotoUserControl.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Focus_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


