//===============================================================================================
// <A4DN_GeneratedInformation>
// This code was generated using the Accelerator for .Net Code Generator.
// <A4DN_Template Name="WPF.System.Logon.t4" Version="8.0.0.93" GeneratedDate="5/29/2024" />
// </A4DN_GeneratedInformation>
//===============================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using A4DN.Core.WPF.LogonBase;

namespace WPF.OrderManagement
{
    /// <summary>
    /// Interaction logic for Logon.xaml
    /// </summary>
    public partial class Logon : AB_LogonBase
    {
        public Logon()
        {
            InitializeComponent();

            // Multilingual Support
            am_EnrollCulture(new List<System.Globalization.CultureInfo>() {
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-ES"),
                new System.Globalization.CultureInfo("fr-FR"),
                new System.Globalization.CultureInfo("ja-JP"),
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("zh-CN"),
            });

            ap_LanguageComboBoxVisibility = Visibility.Visible;
        }
    }
}