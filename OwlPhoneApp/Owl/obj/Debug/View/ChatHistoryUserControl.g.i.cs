﻿

#pragma checksum "C:\Users\Bo\Documents\GitHub\Owl\OwlPhoneApp\Owl\View\ChatHistoryUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "50673CD89FE2C37AC1ED582898A30CBA"
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
    partial class ChatHistoryUserControl : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Owl.ItemClickedConverter ItemClickedConverter; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView ListView_ChatEntries; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Grid Grid_SendMessage; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressBar ProgressBar_Loading; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock TextBlock_ToUserName; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Image Image_ToUserProfile; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///View/ChatHistoryUserControl.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            ItemClickedConverter = (global::Owl.ItemClickedConverter)this.FindName("ItemClickedConverter");
            ListView_ChatEntries = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("ListView_ChatEntries");
            Grid_SendMessage = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("Grid_SendMessage");
            ProgressBar_Loading = (global::Windows.UI.Xaml.Controls.ProgressBar)this.FindName("ProgressBar_Loading");
            TextBlock_ToUserName = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("TextBlock_ToUserName");
            Image_ToUserProfile = (global::Windows.UI.Xaml.Controls.Image)this.FindName("Image_ToUserProfile");
        }
    }
}



