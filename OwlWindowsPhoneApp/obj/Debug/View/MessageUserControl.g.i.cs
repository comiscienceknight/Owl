﻿

#pragma checksum "C:\Users\Bo\Documents\GitHub\Owl\OwlWindowsPhoneApp\View\MessageUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B109C08EA80DB9595D4A1C54B7D6F20B"
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
    partial class MessageUserControl : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Grid ContentRoot; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressBar ProgressBar_Loading; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView ListView_Messages; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Coding4Fun.Toolkit.Controls.ChatBubbleTextBox ChatBubbleTextBox_InputMessage; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.Button Button_SendMessage; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.TextBlock TextBlock_PageTitle; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///View/MessageUserControl.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            ContentRoot = (global::Windows.UI.Xaml.Controls.Grid)this.FindName("ContentRoot");
            ProgressBar_Loading = (global::Windows.UI.Xaml.Controls.ProgressBar)this.FindName("ProgressBar_Loading");
            ListView_Messages = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("ListView_Messages");
            ChatBubbleTextBox_InputMessage = (global::Coding4Fun.Toolkit.Controls.ChatBubbleTextBox)this.FindName("ChatBubbleTextBox_InputMessage");
            Button_SendMessage = (global::Windows.UI.Xaml.Controls.Button)this.FindName("Button_SendMessage");
            TextBlock_PageTitle = (global::Windows.UI.Xaml.Controls.TextBlock)this.FindName("TextBlock_PageTitle");
        }
    }
}


