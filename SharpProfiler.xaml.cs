using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using Sharp_Profiler.CPU;

namespace Sharp_Profiler
{
    public partial class SharpProfiler : Window
    {
        private Cpu cpu = new Cpu();
        private DispatcherTimer timer = new DispatcherTimer();

        public SharpProfiler()
        {
            InitializeComponent();

            cpuInfo.Text = "CPU Name: " + cpu.getCpuName() + Environment.NewLine;
            cpuInfo.Text += "Number of physical processors: " + cpu.getPhysicalProcessorCount().ToString() + Environment.NewLine;
            cpuInfo.Text += "Number of physical cores: " + cpu.getPhysicalCoreCount() + Environment.NewLine;
            cpuInfo.Text += "Number of logical processors: " + cpu.getLogicalProcessorCount().ToString();

            timer.Interval = TimeSpan.FromSeconds(.7);
            timer.Tick += delegate(object sender, EventArgs e)
            {
                updateCpuUsage();
            };
            timer.Start();
        }

        private void updateCpuUsage()
        {
            cpuList.Items.Clear();
            for (int i = 0; i < cpu.getLogicalProcessorCount(); i++)
            {
                cpuList.Items.Add("Core #" + i + ":   " + cpu.getCpuUsageCounters()[i].NextValue().ToString("00.00") + "%");
            }
        }
    }
}
