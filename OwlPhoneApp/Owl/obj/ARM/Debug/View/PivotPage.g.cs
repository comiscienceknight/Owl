﻿

#pragma checksum "C:\Users\bhu\Documents\GitHub\Owl\OwlPhoneApp\Owl\View\PivotPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3EE10832958810354B91696A2F75C47D"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Owl
{
    partial class PivotPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 17 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Filter_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 19 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_RefreshPost_Click;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 22 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_MySelf_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 25 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Logout_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 53 "..\..\..\View\PivotPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Pivot)(target)).SelectionChanged += this.Pivot_Main_SelectionChanged;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


