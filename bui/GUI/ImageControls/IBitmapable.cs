using System;
using System.Drawing;
using System.Threading.Tasks;

namespace bui.GUI
{
    public interface IBitmapable
    {
        event Action<Bitmap, Point> OnDirty;
        int Display { get; }
        Image ActiveBitmap { get; }
        Point Location { get; }
        Point LocationAbsolute { get; }
        bool Visible { get; set; }
        bool IsDirty { get; set; }
        //Task<(Bitmap, Point)> GetBitmap(Bitmap bitmap);
        void ApplyBackground(Bitmap bitmap, Point point);

        void TransferBackground(Bitmap bitmap, Point point);
        void TransferBackground();
    }
}
