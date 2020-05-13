using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bui.GUI.Windows
{
    public partial class WindowMenu : Form
    {
        public WindowMenu()
        {
            InitializeComponent();
        }

        private void WindowMenu_Load(object sender, EventArgs e)
        {
            BackColor = Color.White;
            TransparencyKey = Color.White;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
