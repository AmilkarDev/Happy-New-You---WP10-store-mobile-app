﻿

#pragma checksum "C:\Users\Malek\Desktop\New You\New You\Quotes.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "B1652A579F90E84D154E8792AFCC6C16"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace New_You
{
    partial class Quotes : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 40 "..\..\..\Quotes.xaml"
                ((global::Windows.UI.Xaml.Controls.ListViewBase)(target)).ItemClick += this.ShowBar;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 61 "..\..\..\Quotes.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Like;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 62 "..\..\..\Quotes.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.twit;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 63 "..\..\..\Quotes.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Fbook;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


