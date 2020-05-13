using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bui.GUI
{
    public interface IPressable
    {
        bool CanPress { get; }

        void Press();
        void Release();
        bool IsHit(Point location);
    }
}
