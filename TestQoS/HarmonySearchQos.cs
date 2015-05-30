using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestQoS
{   
    /// <summary>
    /// Реализация QoS, 
    /// использующей алгоритм гармонического поиска
    /// </summary>
    public class HarmonySearchQos : SimpleTBQoS
    {
        private Random rand;

        /// <summary>
        /// Память гармоник
        /// </summary>
        List<List<float>> harmonyMemory;

        /// <summary>
        /// значения цалевой функции для гармотики
        /// </summary>
        List<uint> harmonyMemoryValue;

        /// <summary>
        /// Размер памяти гармоник
        /// </summary>
        private int harmonyMemorySize;

        /// <summary>
        /// Вероятность выбора из гармоник в памяти (от 0 до 1.0)
        /// </summary>
        double harmonyConsiderationRate;

        /// <summary>
        /// Вероятность модификации (от 0 до 1.0)
        /// </summary>
        double pitchAdjustmentRate;

        /// <summary>
        /// Величина модификации
        /// </summary>
        int pitchAdjustmentBandwidth;

        protected override List<float> OptimalTokensPerDts(List<float> firstTokensPerDts)
        {
            //
            // Установка параметров алгоритма
            //
            // Размер памяти гармоник
            this.harmonyMemorySize = 10;
            // Вероятность выбора из гармоник в памяти (от 0 до 1.0)
            this.harmonyConsiderationRate = 0.5;
            // Вероятность модификации (от 0 до 1.0)
            this.pitchAdjustmentRate = 0.5;
            // Величина модификации
            this.pitchAdjustmentBandwidth = 10;

            // 
            // Инициализация начальных параметров
            //
            // Генератор случайных чисел
            rand = new Random((int)DateTime.Now.Ticks);
            // Верхня граница значений
            this.InitMaxTokensPerDts();
            // Память гармоник
            this.InitHarmonyMemory();
            // ограничение итераций (защита от зацикливания)
            int numIterations = 1000;


            // цикл алгоритма
            for (int i = 0; i < numIterations;  i++)
            {
                // создаём новую гармонику
                List<float> newHarmony = new List<float>();

                // собираем новую гармонику поэлементно
                for(int j = 0; j < this.maxTokensPerDts.Count; j++)
                {
                    // выбираем между взятием значения 
                    // из памяти гармоник и выбором наугад
                    if(rand.NextDouble() < this.harmonyConsiderationRate)
                    {
                        // берём соответствующее случайное значение из памяти
                        int memoryCell = rand.Next(harmonyMemorySize);
                        float newItem = this.harmonyMemory[memoryCell][j];
                        newHarmony.Add(newItem);

                        // проверяем, хочет ли судьба изменить это значение
                        if (rand.NextDouble() < this.pitchAdjustmentRate)
                        {
                            // замена значения
                            //
                            // определяем смещение
                            double delta = (2 * rand.NextDouble() - 1) * this.pitchAdjustmentBandwidth;

                            // измненяем значение на (дискретное) значение смещения
                            newHarmony[j] += (int)delta;
                        }
                    }
                    else
                    {
                        // выбираем наугад
                        float newItem = (float)rand.Next((int)this.maxTokensPerDts[j]);
                        newHarmony.Add(newItem);
                    }
                }

                // сравниваем значение с худшим
                int worstHarmonyId = FindWorstHarmony();
                uint newHarmonyValue = this.ObjectiveFunction(newHarmony);
                if (newHarmonyValue > this.harmonyMemoryValue[worstHarmonyId])
                {
                    // заменяем худший на новый
                    this.harmonyMemoryValue[worstHarmonyId] = newHarmonyValue;
                    this.harmonyMemory[worstHarmonyId] = newHarmony;
                }
            }


            return this.harmonyMemory[this.FindBestHarmony()];
        }

        /// <summary>
        /// Инициализация памяти гармоник
        /// </summary>
        private void InitHarmonyMemory()
        {
            this.harmonyMemory = new List<List<float>>();
            this.harmonyMemoryValue = new List<uint>();

            // цикл векторов
            for(int i = 0; i < this.harmonyMemorySize; i++)
            {
                // добавляем очередной вектор
                this.harmonyMemory.Add(new List<float>());   
             
                // цикл элементов вектора
                for(int j = 0; j < this.maxTokensPerDts.Count; j++)
                {
                    // генерируем очередной элемент
                    float tokenPerDt = (float)rand.Next((int)this.maxTokensPerDts[j]);
                    // добавляем
                    this.harmonyMemory[i].Add(tokenPerDt);
                }

                // инициализируем значение гармоники
                harmonyMemoryValue.Add(ObjectiveFunction(harmonyMemory[i]));
            }
        }

        /// <summary>
        /// поиск худшей гармоники, возращает её номер в памяти
        /// </summary>
        /// <returns></returns>
        private int FindWorstHarmony()
        {
            int worstHarmonyId = 0;
            uint worstHarmonyValue = 0;


            for(int i = 0; i < this.harmonyMemorySize; i++)
            {
                if(this.harmonyMemoryValue[i] > worstHarmonyValue)
                {
                    worstHarmonyValue = harmonyMemoryValue[i];
                    worstHarmonyId = i;
                }
            }

            return worstHarmonyId;
        }

        /// <summary>
        /// поиск лучшей гармоники, возращает её номер в памяти
        /// </summary>
        /// <returns></returns>
        private int FindBestHarmony()
        {
            int bestHarmonyId = 0;
            uint bestHarmonyValue = this.harmonyMemoryValue[0];


            for (int i = 0; i < this.harmonyMemorySize; i++)
            {
                if (this.harmonyMemoryValue[i] < bestHarmonyValue)
                {
                    bestHarmonyValue = harmonyMemoryValue[i];
                    bestHarmonyId = i;
                }
            }

            return bestHarmonyId;
        }
    }
}
