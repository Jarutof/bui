using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bui.GUI
{
    public static class ExtentionControl
    {

        public static IEnumerable<IPressable> GetIPressable(this Control source) 
        {
            List<IPressable> result = new List<IPressable>();
            foreach (Control c in source.Controls)
            {
                if (c is IPressable)
                {
                    var pressable = c as IPressable;
                    result.Add(pressable);
                }
                result.AddRange(c.GetIPressable());
            }
            return result;
        }
        public static Bitmap GetBakground(this Control source, Image image, Rectangle rect)
        {
            Bitmap b = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(image, new Rectangle(Point.Empty, rect.Size), rect, GraphicsUnit.Pixel);
            }
            return b;
        }
        
    }
}
