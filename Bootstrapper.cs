// EVECSC - Michael
// Bootstrapper.cs
// Last Cleanup: 16/05/2020 19:01
// Created: 16/05/2020 18:29

#region Directives
using System.Windows;
using Caliburn.Micro;
using EVECSC.ViewModels;
#endregion

namespace EVECSC
{
    public class Bootstrapper : BootstrapperBase
    {
        public Bootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }
    }
}