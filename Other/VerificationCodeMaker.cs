using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Drawing.Drawing2D;
using Matrix = System.Drawing.Drawing2D.Matrix;
using Point = System.Drawing.Point;

namespace Light_IDE.Other
{

    public struct VerificationCode
    {
        public ImageSource img;
        public string code;
    }


    class VerificationCodeMaker
    {
        /// <summary>
        /// 生成一个验证码图片
        /// </summary>
        /// <returns>验证码图片</returns>
        public static VerificationCode CreatCode()
        {
            int[] code = new int[4];
            Random rand = new Random();
            for (int i = 0; i < code.Length; i++)
            {
                code[i] = rand.Next(0, 10);
            }
            Bitmap map = new Bitmap(200, 75);
            Graphics graphics = Graphics.FromImage(map);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;


            Font[] fonts = new Font[code.Length];
            string[] fontlist = { "OCR A Extended", "Papyrus", "Engravers MT", "Gigi", "Impact",
                "Edwardian Script ITC","Terminal","Juice ITC","华文彩云","Script",
                "Segoe Script","Fixedsys","Eras Demi ITC","Elephent","MV Boli","Ink Free"};
            for (int i = 0; i < fonts.Length; i++)
            {
                fonts[i] = new Font(fontlist[rand.Next(0, fontlist.Length - 1)], 32);
            }

            System.Drawing.Point point = new System.Drawing.Point(-20, 20);
            for (int i = 0; i < code.Length; i++)
            {
                point.X += rand.Next(-20, 20);
                point.Y += rand.Next(-7, 7);
                point.X += 40;
                graphics.DrawString(
                    code[i].ToString(),
                    fonts[i],
                    new SolidBrush(System.Drawing.Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255))),
                    point
            );

            }


            for (int i = 0; i < rand.Next(1000, 2000); i++)
            {
                System.Drawing.Point dot = new System.Drawing.Point(rand.Next(0, 300), rand.Next(0, 75));
                graphics.DrawLine(
                    new System.Drawing.Pen(new SolidBrush(System.Drawing.Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))),
                    dot,
                    new System.Drawing.Point(dot.X += rand.Next(-1, 1), dot.Y += rand.Next(-1, 1))
                    );
            }

            for (int i = 0; i < rand.Next(6, 15); i++)
            {
                graphics.DrawBezier(
                    new System.Drawing.Pen(new SolidBrush(System.Drawing.Color.FromArgb(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)))),
                    0, (float)rand.Next(0, 75), (float)rand.Next(0, 300), (float)rand.Next(0, 75), (float)rand.Next(0, 300), (float)rand.Next(0, 300), 300, (float)rand.Next(0, 75)
                    );
            }

            Matrix matrix = graphics.Transform;
            matrix.RotateAt(rand.Next(-45, 45), new Point(rand.Next(0, 300), rand.Next(0, 75)));
            graphics.Transform = matrix;

            string codeStr = "";
            foreach (var item in code)
            {
                codeStr += item;
            }
            return new VerificationCode() { img = ChangeBitmapToImageSource(map), code = codeStr };
        }
        [DllImport("gdi32.dll", SetLastError = true)]

        private static extern bool DeleteObject(IntPtr hObject);



        /// <summary>
        /// 从bitmap转换成ImageSource
        /// </summary>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static ImageSource ChangeBitmapToImageSource(Bitmap bitmap)

        {

            //Bitmap bitmap = icon.ToBitmap();

            IntPtr hBitmap = bitmap.GetHbitmap();


            ImageSource wpfBitmap = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(

                hBitmap,

                IntPtr.Zero,

                Int32Rect.Empty,

                BitmapSizeOptions.FromEmptyOptions());


            if (!DeleteObject(hBitmap))

            {

                throw new System.ComponentModel.Win32Exception();

            }


            return wpfBitmap;

        }
    }
}
