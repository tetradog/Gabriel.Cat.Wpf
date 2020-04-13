﻿using Gabriel.Cat.S.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Gabriel.Cat.S.Utilitats;
namespace Gabriel.Cat.Wpf
{
    public static class Colores
    {
        static TwoKeysList<string, string, System.Windows.Media.Color> colores;
        public static readonly System.Windows.Media.Color[] ListaColores;
        static Colores()
        {
            colores = new TwoKeysList<string, string, System.Windows.Media.Color>();
            System.Reflection.PropertyInfo[] pinfo = typeof(System.Windows.Media.Colors).GetProperties();
            System.Windows.Media.Color color;
            for (int i = 0; i < pinfo.Length; i++)
            {
                try {
                    color = (System.Windows.Media.Color)pinfo[i].GetGetMethod().Invoke(null, null);
                    colores.Add(pinfo[i].Name, color.ToString(), color);
                }
                catch { }
            }
            ListaColores = colores.ValueToArray();
        }
        public static IEnumerable<System.Windows.Media.Color> GetColors()
        {
            return ListaColores;
        }
        public static string GetName(System.Windows.Media.Color color)
        {
            return colores.GetTkey1WhithTkey2(color.ToString());
        }

        public static System.Windows.Media.Color GetColor(string name)
        {
            return colores.GetValueWithKey1(name);
        }

    }
}