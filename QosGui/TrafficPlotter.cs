using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QosGui
{
    /// <summary>
    /// Построитель трафика.
    /// Рекомендуется вкладывать его с панель,
    /// потому что так легче управлять его параметрами
    /// </summary>
    public partial class TrafficPlotter : UserControl
    {
        /// <summary>
        /// TODO: инициализацию всего и вся
        /// </summary>
        public TrafficPlotter()
        {
            InitializeComponent();

            // выбираем тип расположения "заполнить",
            // чтобы размеры control'а зависили от родителя
            this.Dock = DockStyle.Fill;

            // Подпись графика
            plotter.Titles.FindByName("GraphTitle").Text = "Какой-то бред";

            // подпись осей
            plotter.ChartAreas.FindByName("ChartArea1").AxisX.Title = "Время (сек)";
            plotter.ChartAreas.FindByName("ChartArea1").AxisY.Title = "Размер трафика (байт)";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tittle">заголовок графика</param>
        public TrafficPlotter(String tittle) : this()
        {
            // Подпись графика
            plotter.Titles.FindByName("GraphTitle").Text = tittle;
        }

        /// <summary>
        /// Заполнение графика инфой (простой временный (канеееечна) вариант)
        /// следует заполнять инфой попорядку
        /// </summary>
        /// <param name="traffic">трафик (сконее всего сришедшай за сек/мсек)</param>
        /// <param name="time">время в секундах (или миллисекундах хз как удобнее)</param>
        public void AddPoint(uint traffic, double time)
        {
            plotter.Series.FindByName("Traffic").Points.AddXY(time, traffic);            
        }

        /// <summary>
        /// Заполнение графика инфой (простой временный (канеееечна) вариант)
        /// следует заполнять инфой попорядку
        /// </summary>
        /// <param name="traffic">трафик (сконее всего сришедшай за сек/мсек)</param>
        /// <param name="time">время в секундах (или миллисекундах хз как удобнее)</param>
        public void AddPoint(float traffic, double time)
        {
            plotter.Series.FindByName("Traffic").Points.AddXY(time, traffic);
        } 

   
        /// <summary>
        /// стереть график
        /// </summary>
        public void Clear()
        {
            plotter.Series.FindByName("Traffic").Points.Clear();
        }

        /// <summary>
        /// удаление первой точки графика
        /// </summary>
        public void RemoveFirst()
        {
            plotter.Series.FindByName("Traffic").Points.RemoveAt(0);
        }
    }
}
