using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace compiler
{
    public partial class SettingsFormcs : Form
    {
        public SettingsFormcs()
        {
            InitializeComponent();
            comboBox1.SelectedItem = Properties.Settings.Default.FontSize;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                Properties.Settings.Default.FontSize = Convert.ToInt32(comboBox1.SelectedItem);
            }
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
