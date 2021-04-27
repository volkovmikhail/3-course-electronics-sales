using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ИС_формирование_заказов_компании_по_продаже_электроники
{
    static public class ImageSizeUtil
    {
        static public Bitmap stratchImage(Image image)
        {
            Bitmap bitmap;
            SolidBrush whiteBrush = new SolidBrush(Color.White);
            Rectangle rectangle;
            if (image.Width > image.Height)
            {
                bitmap = new Bitmap(image.Width, image.Width);
                rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.FillRectangle(whiteBrush, rectangle);
                g.DrawImage(image, 0, (bitmap.Height / 2) - (image.Height / 2));
                g.Dispose();
            }
            else
            {
                bitmap = new Bitmap(image.Height, image.Height);
                rectangle = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.FillRectangle(whiteBrush, rectangle);
                g.DrawImage(image, (bitmap.Width / 2) - (image.Width / 2), 0);
                g.Dispose();
            }
            whiteBrush.Dispose();
            return bitmap;
        }
    }
}
