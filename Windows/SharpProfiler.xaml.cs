using Sharp_Profiler.CPU;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Sharp_Profiler
{
    public partial class SharpProfiler : Window
    {
        private Cpu cpu;
        private DispatcherTimer timer;
        private uint numberLogicalProcessors;

        public SharpProfiler()
        {
            InitializeComponent();
            cpu = new Cpu();
            timer = new DispatcherTimer();

            numberLogicalProcessors = cpu.getNumberOfLogicalProcessors();

            cpuName.Content = cpu.getName();
            cpuAddressWidth.Content = cpu.getAddressWidth();
            cpuArchitecture.Content = cpu.getArchitecture();
            cpuVoltage.Content = cpu.getVoltage();
            cpuDeviceId.Content = cpu.getDeviceId();
            cpuFamily.Content = cpu.getFamily() == "Other" ? cpu.getFamily() + " " + cpu.getOtherFamilyDescription() : cpu.getFamily();
            cpuL2CacheSize.Content = cpu.getL2CacheSize() + " KB";
            cpuL2CacheSpeed.Content = cpu.getL2CacheSpeed() + " MHz";
            cpuL3CacheSize.Content = cpu.getL3CacheSize() + " KB";
            cpuL3CacheSpeed.Content = cpu.getL3CacheSpeed() + " MHz";
            cpuManufacturer.Content = cpu.getManufacturer();
            cpuNumberOfCores.Content = cpu.getNumberOfCores();
            cpuNumberOfLogicalProcessors.Content = numberLogicalProcessors;
            cpuNumberOfPhysicalProcessors.Content = cpu.getNumberOfPhysicalProcessors();
            cpuPlugAndPlayDeviceId.Content = cpu.getPnpDeviceId() != null ? cpu.getPnpDeviceId() : "Unknown";

            if (cpu.getPowerManagementCapabilities() == null)
            {
                cpuPowerManagementCapabilities.Content = "Unknown";
            }
            else
            {
                foreach (string item in cpu.getPowerManagementCapabilities())
                {
                    cpuPowerManagementCapabilities.Content += item + Environment.NewLine;
                }
            }
            
            cpuId.Content = cpu.getProcessorId();
            cpuType.Content = cpu.getProcessorType();
            cpuRevision.Content = cpu.getRevision();

            //Add CPU usage counters for the first time
            updateCpuUsage(true);

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += updateCpuStats;
            timer.Start();
        }

        private void updateCpuUsage(bool init)
        {
            for (int i = 0; i < numberLogicalProcessors; i++)
            {
                string data = "Core #" + i + ":   " + cpu.UsageCounters[i].NextValue().ToString("00.00") + "%";
                if (init)
                {
                    cpuUsageList.Items.Add(data);
                }
                else
                {
                    cpuUsageList.Items[i] = data;
                }
            }
        }

        private void updateCpuStats(object sender, EventArgs e)
        {
            //CPU usage list
            updateCpuUsage(false);

            //Current clock speed
            cpuCurrentClockSpeed.Content = cpu.getCurrentClockSpeed() + " MHz";

            //Min clock speed
            cpuMinClockSpeed.Content = cpu.getMinClockSpeed() + " MHz";

            //Max clock speed
            cpuMaxClockSpeed.Content = cpu.getMaxClockSpeed() + " MHz";
        }
    }
}
