﻿

#pragma checksum "C:\Users\Bo\Documents\GitHub\Owl\OwlPhoneApp\Owl\View\PostsUserControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D7F3C6AC33D81CFEE5B3B9FB6C7A5D9D"
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
    partial class PostsUserControl : global::Windows.UI.Xaml.Controls.UserControl
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Owl.ItemClickedConverter ItemClickedConverter; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ListView ListView_Posts; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private global::Windows.UI.Xaml.Controls.ProgressBar ProgressBar_Loading; 
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        private bool _contentLoaded;

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            global::Windows.UI.Xaml.Application.LoadComponent(this, new global::System.Uri("ms-appx:///View/PostsUserControl.xaml"), global::Windows.UI.Xaml.Controls.Primitives.ComponentResourceLocation.Application);
 
            ItemClickedConverter = (global::Owl.ItemClickedConverter)this.FindName("ItemClickedConverter");
            ListView_Posts = (global::Windows.UI.Xaml.Controls.ListView)this.FindName("ListView_Posts");
            ProgressBar_Loading = (global::Windows.UI.Xaml.Controls.ProgressBar)this.FindName("ProgressBar_Loading");
        }
    }
}



