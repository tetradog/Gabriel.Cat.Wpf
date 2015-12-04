﻿/*
 * Creado por SharpDevelop.
 * Usuario: Gabriel
 * Fecha: 13/07/2015
 * Hora: 21:20
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Gabriel.Cat;
using System.Linq;
namespace Gabriel.Cat.Utilitats.Wpf
{
	public static class ExtensionWpf
	{
        #region DateTimePicker
        //public static DateTime ToDateTime(this DateTimePicker dtp)
        //{
        //    return dtp.Value.Date;
        //}
        #endregion
        public static int ToArgb(this Color color)
		{
			byte[] argb =  {
				color.A,
				color.R,
				color.G,
				color.B
			};
			return Serializar.ToInt(argb);
		}

        public static Color Invertir(this Color color)
        {
            return Color.FromArgb((byte)Math.Abs((int)color.A-255),(byte) Math.Abs((int)color.R - 255),(byte) Math.Abs((int)color.G - 255),(byte) Math.Abs((int)color.B - 255));
        }
        public static bool EsClaro(this Color color)
        {
            return (color.R+color.G+color.B) / 3 > 255 / 2;
        }

        public static double HeightItem(this StackPanel stkPanel, UIElement item)
        {
            double altura = 0;
            for (int i = 0, iFinal = stkPanel.Children.IndexOf(item); i < iFinal; i++)
            {
                altura += stkPanel.Children[i].RenderSize.Height;
            }
            return altura;
        }
        public static void SetImage(this Image img, Uri path)
        {
            BitmapImage imgCargada = new BitmapImage();
            imgCargada.BeginInit();
            imgCargada.UriSource = path;
            imgCargada.EndInit();
            img.Source = imgCargada;
        }
        public static void SetImage(this Image img, System.Drawing.Bitmap bmp)
        {
            BitmapImage imgCargada = new BitmapImage();
            imgCargada.SetImage(bmp);
            img.Source = imgCargada;
        }
        public static void SetImage(this BitmapImage bmpImg, System.Drawing.Bitmap bmp)
        {
            bmpImg.BeginInit();
            bmpImg.StreamSource = bmp.ToStream();
            bmpImg.EndInit();
        }
        public static System.Drawing.Bitmap GetImage(this BitmapImage bmpImg)
        {
            return new System.Drawing.Bitmap(bmpImg.StreamSource);
        }

        public static void AddRange(this UIElementCollection coleccion, IEnumerable<UIElement> elementos)
        {
            foreach (UIElement element in elementos)
                coleccion.Add(element);
        }
        public static void Sort(this UIElementCollection coleccion)
        {
                List<UIElement> items =new List<UIElement>(coleccion.OfType<UIElement>());
                items.Sort();
                for (int i = 0; i < items.Count; i++)
                    coleccion.ChangeItemPosition(items[i],i);
        }
        public static void ChangeItemPosition(this UIElementCollection coleccion, UIElement elementColection, int newPosition)
        {
            int posicionAnt = coleccion.IndexOf(elementColection);
            if (posicionAnt != newPosition)
            {
                coleccion.RemoveAt(posicionAnt);
                coleccion.Insert(newPosition, elementColection);
            }
        }
    }
}

