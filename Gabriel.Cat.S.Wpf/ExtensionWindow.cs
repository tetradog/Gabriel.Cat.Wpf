﻿using Microsoft.Win32;
//using System.Windows.Forms;
/*
Creado por tttony 2010
http://tttony.blogspot.com/

*/
namespace Gabriel.Cat.S.Extension
{

    public static class WindowExtension
    {
        //por probar
        //public static bool EstaEnElInicio
        //{
        //    get
        //    {

        //        string path;
        //        string thisApp;
        //        RegistryKey hklm = Registry.LocalMachine;

        //        hklm = hklm.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");//solo sirve en windows...
        //        path = hklm.GetValue(Application.ProductName).ToString();

        //        thisApp = Application.ExecutablePath.ToUpper();
        //        hklm.Close();

        //        return path != thisApp;
        //    }
        //    set
        //    {
        //        RegistryKey hklm = Registry.LocalMachine;
        //        string thisApp;

        //        hklm = hklm.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");

        //        if (value)
        //        {
        //            thisApp = Application.ExecutablePath.ToUpper();
        //            hklm.SetValue(Application.ProductName, thisApp);
        //        }
        //        else
        //        {
        //            //delete registro
        //            hklm.DeleteValue(Application.ProductName);
        //        }

        //        hklm.Close();
        //    }
        //}
        public static bool GetEstaEnPantallaCompleta(this System.Windows.Window windows)
        {    
           return windows.WindowStyle == System.Windows.WindowStyle.None&& windows.WindowState == System.Windows.WindowState.Maximized; 
        }
        public static void SetEstaEnPantallaCompleta(this System.Windows.Window windows,bool setPantallaCompleta=true)
        {
            if (setPantallaCompleta)
            {
                windows.WindowStyle = System.Windows.WindowStyle.None;
                windows.WindowState = System.Windows.WindowState.Maximized;
            }
            else
            {
                windows.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                windows.WindowState = System.Windows.WindowState.Normal;
            }
        }
    }
}
