using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace JackalHost.Monitors
{
    public partial class AvailableMoveControl : UserControl
    {
        GraphicsPath path = new GraphicsPath();

        public AvailableMoveControl(Color color)
        {
            InitializeComponent();
            BackColor = color;

            path.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            Region = new Region(path);
        }

        private void AvailableMoveControl_Resize(object sender, EventArgs e)
        {
            path.Reset();

            if (Region != null)
            {
                Region.Dispose();
                Region = null;
            }

            path.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            Region = new Region(path);
        }
    }
}
