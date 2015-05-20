using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestQoS;

namespace QosGui
{
    public partial class Main : Form
    {
        /// <summary>
        /// диалог настроек
        /// </summary>
        Settings setForm;

        /// <summary>
        /// сама модель
        /// </summary>
        TestQoS.SimpleTBQoS qos;

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
                qos = new TestQoS.SimpleTBQoS();
                qos.Initializate(setForm.ObservationPeriod(),
                                setForm.NumOfBuckets(),
                                setForm.MinPacketSizes(),
                                setForm.MaxPacketSizes(),
                                setForm.MaxTimePeriods(),
                                setForm.MaxTimePeriods());
                qos.Run();
            }

        }
    }
}
