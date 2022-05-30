using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZMultiHack
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        public void UpdateText(string text)
        {
            labelText.Text = text;
            this.Update();
        }

        public void CloseLoading()
        {
            this.Invoke((MethodInvoker)delegate { this.Close(); });
        }
    }
}
