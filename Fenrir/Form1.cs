using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sleipnir;

namespace Fenrir
{
    public partial class Fenrir : Form
    {
        public Fenrir()
        {
            InitializeComponent();
        }

        private void ProxyStartButton_Click(object sender, EventArgs e)
        {
            new Server(Convert.ToInt32(portNumber.Value));
            
        }
    }
}
