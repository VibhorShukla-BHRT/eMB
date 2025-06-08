using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PHEDChhattisgarh
{
public class Captcha
{
    public Captcha()
    {

    }
    public void FillCapctha(string SessionName)
    { 
        try
        {
            Random random = new Random();
            string combination = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyzl";//I
            //string combination = "0123456789ABCDEFGHJKLMNOPQRSTUVWXYZ";
            StringBuilder captcha = new StringBuilder();
            for (int i = 0; i < 6; i++)
                captcha.Append(combination[random.Next(combination.Length)]);
            HttpContext.Current.Session[SessionName] = captcha.ToString();
            //str= "~/Credential/GenerateCaptcha.aspx?" + DateTime.Now.Ticks.ToString();

            HttpContext.Current.Response.Clear();
            int height = 30;
            int width = 100;
            Bitmap bmp = new Bitmap(width, height);

            RectangleF rectf = new RectangleF(10, 5, 0, 0);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(HttpContext.Current.Session[SessionName].ToString(), new Font("Thaoma", 12, FontStyle.Italic), Brushes.Green, rectf);
            g.DrawRectangle(new Pen(Color.Red), 1, 1, width - 2, height - 2);
            g.Flush();
            HttpContext.Current.Response.ContentType = "image/jpeg";
            bmp.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Jpeg);
            g.Dispose();
            bmp.Dispose();
        }
        catch
        {
            throw;
        }
    }
}

}