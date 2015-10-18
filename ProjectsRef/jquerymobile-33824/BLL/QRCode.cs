/*  
 *  Copyright © 2012 Matthew David Elgert - mdelgert@yahoo.com
 *
 *  This program is free software; you can redistribute it and/or modify
 *  it under the terms of the GNU Lesser General Public License as published by
 *  the Free Software Foundation; either version 2.1 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU Lesser General Public License for more details.
 *
 *  You should have received a copy of the GNU Lesser General Public License
 *  along with this program; if not, write to the Free Software
 *  Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA 
 * 
 *  Project URL: http://jquerymobile.codeplex.com/                           
 *  URLS: http://code.google.com/p/zxing/issues/detail?id=940, http://code.google.com/p/zxing/
 */

using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;

namespace BLL
{
    public class QRCode : IHttpHandler
    {
        #region Return QRCode

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            string URL = HttpContext.Current.Request.Url.Host.ToString();
            int Width = 200;
            int Height = 200;

            com.google.zxing.qrcode.QRCodeWriter qrCode = new com.google.zxing.qrcode.QRCodeWriter();
            com.google.zxing.common.ByteMatrix byteIMG = qrCode.encode(URL, com.google.zxing.BarcodeFormat.QR_CODE, Width, Height);

            sbyte[][] img = byteIMG.Array;
            Bitmap bmp = new Bitmap(Width, Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            for (int i = 0; i <= img.Length - 1; i++)
            {
                for (int j = 0; j <= img[i].Length - 1; j++)
                {
                    if (img[j][i] == 0)
                    {
                        g.FillRectangle(Brushes.Black, i, j, 1, 1);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, i, j, 1, 1);
                    }
                }
            }

            /// <summary>
            /// Returns the QRCode in the browser
            /// </summary>

            HttpContext.Current.Response.ContentType = "image/jpg";
            bmp.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);

        }

        #endregion

        public static void WriteQRCode(string Path, string URL, int Width, int Height)
        {
            /// <summary>
            /// Example usage
            /// BLL.QRCode.WriteQRCode("D:\\vessea.jpg", "http://www.vessea.com", 300, 300);
            /// </summary>
            
            com.google.zxing.qrcode.QRCodeWriter qrCode = new com.google.zxing.qrcode.QRCodeWriter();
            com.google.zxing.common.ByteMatrix byteIMG = qrCode.encode(URL, com.google.zxing.BarcodeFormat.QR_CODE, Width, Height);

            sbyte[][] img = byteIMG.Array;
            Bitmap bmp = new Bitmap(Width, Height);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);

            for (int i = 0; i <= img.Length - 1; i++)
            {
                for (int j = 0; j <= img[i].Length - 1; j++)
                {
                    if (img[j][i] == 0)
                    {
                        g.FillRectangle(Brushes.Black, i, j, 1, 1);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.White, i, j, 1, 1);
                    }
                }
            }

            bmp.Save(Path, ImageFormat.Jpeg);
        }

    }

}
