using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using Sharp_Profiler.CPU;

namespace Sharp_Profiler
{
    public partial class SharpProfiler : Window
    {
        private DispatcherTimer timer;

        public SharpProfiler()
        {
            InitializeComponent();
            Cpu cpu = new Cpu();
            timer = new DispatcherTimer();

            cpuInfo.Text = "Number of logical processors: " + cpu.countCores().ToString()+Environment.NewLine;
            cpuInfo.Text += "Number of physical cores: " + cpu.countCpu();
            //Build the performance counters
            for (int i = 0; i < cpu.countCores(); i++)
            {
                cpu.getCounters()[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
                cpuList.Items.Add("Core #" + i + ": " + cpu.getCounters()[i].NextValue().ToString("0.##") + "%");

            }

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += delegate(object sender, EventArgs e)
            {
                for (int i = 0; i < cpuList.Items.Count; i++)
                {
                    cpuList.Items[i] = "Core #" + i + ": " + cpu.getCounters()[i].NextValue().ToString("0.##") + "%";
                }
            };
            timer.Start();
        }
    }
}
