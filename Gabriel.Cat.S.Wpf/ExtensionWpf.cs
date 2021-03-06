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
using System.Drawing;
using System.Runtime.InteropServices;
using Gabriel.Cat.Wpf;
using System.Collections;
using System.Xml;
using System.Windows.Markup;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Gabriel.Cat.S.Extension;
using Gabriel.Cat.S.Utilitats;
using System.Drawing.Imaging;

namespace Gabriel.Cat.S.Extension
{
    public delegate void MetodoTratarByteArray(byte[] byteArray);
    public unsafe delegate void MetodoTratarBytePointer(byte* prtByteArray);

    public static class ExtensionWpf
    {
        public static string GetFilePath(this IDataObject dataObj, int file = 0)
        {
            string path;
            if (dataObj.GetDataPresent(DataFormats.FileDrop))
            {
                path = ((string[])dataObj.GetData(DataFormats.FileDrop))[file];
            }
            else path = default;
            return path;
        }
        public static byte[] GetFileData(this IDataObject dataObj,int file = 0)
        {
            byte[] data;
            string path = dataObj.GetFilePath(file);
            if (path != default)
            {
                data = System.IO.File.ReadAllBytes(path);
            }
            else data = default;
            return data;
        }
        public static System.Drawing.Point GetPoint(this Visual visual, MouseEventArgs e)
        {
            return GetPoint(visual, e.GetPosition(e.MouseDevice.DirectlyOver));
        }
  
        public static System.Drawing.Point GetPoint(this Visual visual, System.Windows.Point point)
        {
            DpiScale dpi = VisualTreeHelper.GetDpi(visual);
            return new System.Drawing.Point((int)Math.Truncate(dpi.PixelsPerInchX / 10 * point.X), (int)Math.Truncate(dpi.PixelsPerInchY / 10 * point.Y));
        }
        public static bool InvokeRequired(this Dispatcher dispatcher)
        {
            return !dispatcher.CheckAccess();
        }

        #region UIElementCollection
        public static void RemoveRange(this UIElementCollection coleccion,IEnumerable<UIElement> elementosParaQuitar)
        {
            foreach (UIElement element in elementosParaQuitar)
                coleccion.Remove(element);
        }
        public static void WhileEach(this UIElementCollection coleccion,MetodoWhileEach<UIElement> metodo)
        {
            for (int i = 0; i < coleccion.Count && metodo(coleccion[i]); i++) ;
        }
        #endregion
        #region ItemCollection

        public static void WhileEach(this ItemCollection coleccion, MetodoWhileEach<object> metodo)
        {
            for (int i = 0; i < coleccion.Count && metodo(coleccion.GetItemAt(i)); i++) ;
        }
        public static void AddRange(this ItemCollection items, IEnumerable list)
        {
            foreach (object obj in list)
                items.Add(obj);
        }
        #endregion
        #region ObjViewer
        public static ObjViewer[] ToObjViewerArray(this IEnumerable list, ObjViewerEventHandler metodoClic)
        {
            List<ObjViewer> objViewerList = new List<ObjViewer>();
            foreach (Object obj in list)
            {
                objViewerList.Add(obj.ToObjViewer(metodoClic));
            }
            return objViewerList.ToArray();
        }
        public static ObjViewer ToObjViewer(this Object obj, ObjViewerEventHandler metodoClic)
        {

            ObjViewer objViewer = new ObjViewer(obj);
            objViewer.ObjSelected += metodoClic;
            return objViewer;
        }
        #endregion
        #region DateTimePicker
        //public static DateTime ToDateTime(this DateTimePicker dtp)
        //{
        //    return dtp.Value.Date;
        //}
        #endregion
         
        public static Bitmap ToBitmap(this System.Windows.Media.Imaging.GifBitmapEncoder gifEncoder)
        {
            Stream str = new MemoryStream();
            gifEncoder.Save(str);
            return new Bitmap(str);

        }
        public static Bitmap ToBitmap(this System.Windows.Controls.Image img)
        {
            return img.Source.ToBitmap();
        }
        public static Bitmap ToBitmap(this ImageSource imgSource)
        {
            BitmapSource bitmapSource = (BitmapSource)imgSource;
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            Bitmap bitmap;
            IntPtr memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            bitmap = new Bitmap(width, height, stride, System.Drawing.Imaging.PixelFormat.Format32bppPArgb, memoryBlockPointer);
            return bitmap;
        }
        public static int ToArgb(this System.Windows.Media.Color color)
        {
            byte[] argb =  {
                color.A,
                color.R,
                color.G,
                color.B
            };
            return Serializar.ToInt(argb);
        }
        public static System.Windows.Controls.Image ToImage(this Bitmap bmp)
        {
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.SetImage(bmp);
            return img;
        }
        public static System.Windows.Controls.Image ToImage(this System.Windows.Media.Color color,int width,int height)
        {
            return color.ToBitmap(width, height).ToImage();
        }
        public static Bitmap ToBitmap(this System.Windows.Media.Color color,int width,int height)
        {
            int pos=0;
            Bitmap bmp = new Bitmap(width,height);
            
            bmp.TrataBytes((matrizBytes) =>
            {
                for (int y = 0, xFinal = width * Pixel.ARGB; y < height; y++)
                    for (int x = 0; x < xFinal; x += Pixel.ARGB) {
                        
                        matrizBytes[pos+Pixel.R] = color.R;
                        matrizBytes[pos + Pixel.G] = color.G;
                        matrizBytes[pos+Pixel.B] = color.B;
                        matrizBytes[pos+Pixel.A] = byte.MaxValue;
                        pos += Pixel.ARGB;
                    }
            });
            return bmp;
        }
        public static string GetName(this System.Windows.Media.Color color)
        {
            return Colores.GetName(color);      
        }
        public static System.Windows.Media.Color Invertir(this System.Windows.Media.Color color)
        {
            return System.Windows.Media.Color.FromArgb((byte)Math.Abs((int)color.A - byte.MaxValue), (byte)System.Math.Abs((int)color.R - byte.MaxValue), (byte)System.Math.Abs((int)color.G - byte.MaxValue), (byte)System.Math.Abs((int)color.B - byte.MaxValue));
        }
        public static bool EsClaro(this System.Windows.Media.Color color)
        {
            return (color.R + color.G + color.B) / 3 > byte.MaxValue / 2;
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
        public static void SetImage(this System.Windows.Controls.Image img, Uri path)
        {
            BitmapImage imgCargada = new BitmapImage();
            imgCargada.BeginInit();
            imgCargada.UriSource = path;
            imgCargada.EndInit();
            img.Source = imgCargada;
        }
        public static void SetImage(this System.Windows.Controls.Image img, System.Drawing.Bitmap bmp)
        {
            BitmapImage imgCargada = new BitmapImage();
            imgCargada.SetImage(bmp);
            img.Source = imgCargada;
        }

        public static void SetImage(this BitmapImage bmpImg, System.Drawing.Bitmap bmp)
        {
            bmpImg.BeginInit();
            bmpImg.StreamSource = bmp.ToStream();//optimizar
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
            List<UIElement> items = new List<UIElement>(coleccion.OfType<UIElement>());
            items.Sort();
            for (int i = 0; i < items.Count; i++)
                coleccion.ChangeItemPosition(items[i], i);
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
        #region Color
        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        public static System.Drawing.Color ToDrawingColor(this System.Windows.Media.Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }
        #endregion
        #region Point
        public static System.Windows.Point ToWindowsPoint(this System.Drawing.Point point)
        {
            return new System.Windows.Point(point.X,point.Y);
        }
        public static System.Drawing.Point ToDrawingPoint(this System.Windows.Point point)
        {
            return new  System.Drawing.Point(Convert.ToInt32(point.X),Convert.ToInt32(point.Y));
        }
        #endregion
        #region RichTextBox

        public static void UndeLineSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
        }
        public static void StrikethroughSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Strikethrough);
        }
        public static void OverLineSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.OverLine);
        }
        public static void BaselineSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Baseline);
        }
        public static void BoldSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
        }
        public static void BlackSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Black);
        }
        public static void DemiBoldSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.DemiBold);
        }
        public static void ExtraBlackSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.ExtraBlack);
        }
        public static void ExtraBoldSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.ExtraBold);
        }
        public static void ExtraLightSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.ExtraLight);
        }
        public static void HeavySelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Heavy);
        }
        public static void LightSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Light);
        }
        public static void MediumSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Medium);
        }
        public static void NormalSelection(this RichTextBox rtBox)
        {
            //le quita todo!!
            TextRange txtRange = rtBox.SelectedText();
            txtRange.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
            txtRange.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
            txtRange.ApplyPropertyValue(Inline.TextDecorationsProperty, default);
            rtBox.SelectionMarcador(Colors.Transparent);

        }
        public static void RegularSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Regular);
        }
        public static void SemiBoldSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.SemiBold);
        }
        public static void ThinSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Thin);
        }
        public static void UltraBlackSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.UltraBlack);
        }
        public static void UltraBoldSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.UltraBold);
        }
        public static void UltraLightSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.UltraLight);
        }
        public static void ItalicSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
        }
        public static void ObliqueSelection(this RichTextBox rtBox)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Oblique);
        }
        public static void SelectionAligment(this RichTextBox rtBox, TextAlignment aligment)
        {
            rtBox.SelectedText().ApplyPropertyValue(Paragraph.TextAlignmentProperty,aligment);
        }
        public static void SelectionSize(this RichTextBox rtBox, double leterSize)
        {
            rtBox.SelectedText().ApplyPropertyValue(Paragraph.FontSizeProperty, leterSize);
        }
        public static void FontFamilySelection(this RichTextBox rtBox, System.Windows.Media.FontFamily fontFamily)
        {
            rtBox.SelectedText().ApplyPropertyValue(Paragraph.FontFamilyProperty,fontFamily);
        }
        public static void SelectionMarcador(this RichTextBox rtBox, System.Windows.Media.Color color)
        {
            rtBox.SelectedText().ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(color));
        }
    
        public static TextRange SelectedText(this RichTextBox rtBox)
        {
            return new TextRange(rtBox.Selection.Start, rtBox.Selection.End); 
        }
        public static void SetText(this RichTextBox rtBox, string text)
        {
            new TextRange(rtBox.Document.ContentStart, rtBox.Document.ContentEnd).Text = text;
        }
        public static string GetText(this RichTextBox rtBox)
        {
            return new TextRange(rtBox.Document.ContentStart, rtBox.Document.ContentEnd).Text;
        }
        public static void SetelectedText(this RichTextBox rtBox, string text)
        {
            rtBox.SelectedText().Text = text;
        }
        public static string GetSelectedText(this RichTextBox rtBox)
        {
            return rtBox.SelectedText().Text;
        }
        public static void SelectedColor(this RichTextBox rtbox,System.Windows.Media.Color color)
        {
          
            rtbox.SelectedText().ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(color));
        }
        public static Object LoadObjectXml(this XmlNode obj)
        {
            StringReader stringReader = new StringReader(obj.OuterXml);
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader);

        }
        public static XmlNode SaveObjectXml(this Object obj)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(XamlWriter.Save(obj));
            return xml.FirstChild;
        }

        public static XmlNodeList GetElementsByTagName(this XmlNodeList list, string tagName)
        {
            XmlDocument xmlList = new XmlDocument();
            foreach (XmlNode nodo in list)
                if (nodo.Name == tagName)
                    xmlList.AppendChild(nodo);
            return xmlList.ChildNodes;
        }
        public static string ToStringRtf(this RichTextBox rt)
        {
            string textRtf;
            TextRange range = new TextRange(rt.Document.ContentStart, rt.Document.ContentEnd);
            MemoryStream stream = new MemoryStream();
            StreamReader reader;
            range.Save(stream, DataFormats.Rtf);
            stream.Position = 0;
            reader = new StreamReader(stream, Encoding.UTF8);
            textRtf = reader.ReadToEnd();
            reader.Close();
            stream.Close();
            return textRtf;
        }
        public static void LoadStringRtf(this RichTextBox rt,string stringInRtfFormat)
        {
            MemoryStream stream = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(stringInRtfFormat));
            rt.SelectAll();//esta para solucionar los enters fantasma
            rt.SelectedText().Load(stream, DataFormats.Rtf);
            stream.Close();
        }
        #endregion


    }
}

