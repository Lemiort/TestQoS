using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QosGui
{
    public partial class Main : Form
    {
        Settings setForm;
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //TODO: настройки по умолчанию
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // открываем меню настроек
            if(setForm == null)
                setForm = new Settings();

            setForm.ShowDialog();
            
            if(setForm.IsSettingsApplyed)
            {
                // где-то тут применяются настройки
            }

        }
    }
}
