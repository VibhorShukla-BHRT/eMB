using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PHEDChhattisgarh
{
public partial class AppGenerateCaptcha : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.Clear();
            int height = 43;
            int width = 130;
            Bitmap bmp = new Bitmap(width, height);

            RectangleF rectf = new RectangleF(10, 5, 0, 0);

            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(Session["appcaptcha"].ToString(), new Font("Thaoma",18, FontStyle.Bold), Brushes.Green, rectf);
            g.DrawRectangle(new Pen(Color.Red), 1, 1, width - 2, height - 2);
            g.Flush();
            Response.ContentType = "image/jpeg";
            bmp.Save(Response.OutputStream, ImageFormat.Jpeg);
            g.Dispose();
            bmp.Dispose();
        }
        catch(Exception ee){}
    }
}

}

