using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace DisplayControl
{
    public static class RegistryHelper
    {
        const string regEntryName = "FEG Display Control";

        public static void AddStartUpAppEntry()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                var valueNames = key.GetValueNames();
                if (!valueNames.Contains(regEntryName))
                {
                    string exeName = Process.GetCurrentProcess().MainModule.FileName;
                    key.SetValue(regEntryName, exeName);
                }
            }
        }

        //public static void RemoveStartUpAppEntry()
        //{
        //    using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
        //    {
        //        var valueNames = key.GetValueNames();
        //        if (valueNames.Contains(regEntryName))
        //        {
        //            key.DeleteValue(regEntryName);
        //        }
        //    }
        //}
    }
}
