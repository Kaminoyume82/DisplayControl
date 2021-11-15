using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DisplayControl
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            Mutex singleton = new Mutex(false, "58422E21-1096-4A6F-B6C9-9AAFFD2F52DF");
            bool isOwned = singleton.WaitOne(1);
            if (isOwned)
            {
                base.OnStartup(e);
            }
            else
            {
                Shutdown();
            }
        }
    }
}
