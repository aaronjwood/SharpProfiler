using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using Sharp_Profiler.CPU;
using System.ComponentModel;

namespace Sharp_Profiler
{
    public partial class SharpProfiler : Window
    {
        private BackgroundWorker bw = new BackgroundWorker();
        private Cpu cpu = new Cpu();
        private DispatcherTimer timer = new DispatcherTimer();

        //Wow, do I need to completely rewrite the UI or what?! Keep this temporary mess for now...
        //Should I even kick off some of the work on a different thread? WMI queries seem pretty performant so far...

        public SharpProfiler()
        {
            InitializeComponent();
            bw.DoWork += new DoWorkEventHandler(bwDoWork);
            bw.RunWorkerAsync();
        }

        public void bwDoWork(object sender, DoWorkEventArgs e)
        {
            this.Dispatcher.BeginInvoke((Action)delegate(){
                cpuInfo.Text = "CPU Name: " + cpu.getName() + Environment.NewLine;
                cpuInfo.Text += "Number of physical processors: " + cpu.getPhysicalProcessorCount().ToString() + Environment.NewLine;
                cpuInfo.Text += "Number of physical cores: " + cpu.getNumberOfCores() + Environment.NewLine;
                cpuInfo.Text += "Number of logical processors: " + cpu.getNumberOfLogicalProcessors().ToString();
            });

            timer.Interval = TimeSpan.FromSeconds(.7);
            timer.Tick += delegate(object timerSender, EventArgs timerEventArgs)
            {
                updateCpuUsage();
            };
            timer.Start();
        }

        private void updateCpuUsage()
        {
            cpuList.Items.Clear();
            for (int i = 0; i < cpu.getNumberOfLogicalProcessors(); i++)
            {
                cpuList.Items.Add("Core #" + i + ":   " + cpu.UsageCounters[i].NextValue().ToString("00.00") + "%");
            }
        }
    }
}
