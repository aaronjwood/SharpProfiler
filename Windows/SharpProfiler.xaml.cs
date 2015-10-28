using Sharp_Profiler.CPU;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;

namespace Sharp_Profiler
{
    public partial class SharpProfiler : Window
    {
        private Cpu cpu = new Cpu();
        private DispatcherTimer timer = new DispatcherTimer();
        private int numberLogicalProcessors;
        private PerformanceCounter loadPercentage;

        /// <summary>
        /// Window initialization
        /// </summary>
        public SharpProfiler()
        {
            InitializeComponent();

            numberLogicalProcessors = cpu.getNumberOfLogicalProcessors();
            loadPercentage = cpu.getLoadPercentage();

            cpuName.Content = cpu.getName() ?? "Unknown";
            cpuAddressWidth.Content = cpu.getAddressWidth()?.ToString() ?? "Unknown";
            cpuArchitecture.Content = cpu.getArchitecture() ?? "Unknown";
            cpuVoltage.Content = cpu.getVoltage()?.ToString() ?? "Unknown";
            cpuDeviceId.Content = cpu.getDeviceId() ?? "Unknown";
            cpuFamily.Content = cpu.getFamily() == "Other" ? cpu.getFamily() + " " + cpu.getOtherFamilyDescription() : cpu.getFamily();
            cpuL2CacheSize.Content = (cpu.getL2CacheSize()?.ToString() ?? "Unknown") + " KB";
            cpuL2CacheSpeed.Content = (cpu.getL2CacheSpeed()?.ToString() ?? "Unknown") + " MHz";
            cpuL3CacheSize.Content = (cpu.getL3CacheSize()?.ToString() ?? "Unknown") + " KB";
            cpuL3CacheSpeed.Content = (cpu.getL3CacheSpeed()?.ToString() ?? "Unknown") + " MHz";
            cpuManufacturer.Content = cpu.getManufacturer() ?? "Unknown";
            cpuNumberOfLogicalProcessors.Content = numberLogicalProcessors;
            cpuNumberOfCores.Content = cpu.getNumberOfCores()?.ToString() ?? "Unknown";
            cpuNumberOfPhysicalProcessors.Content = cpu.getNumberOfPhysicalProcessors()?.ToString() ?? "Unknown";
            cpuPlugAndPlayDeviceId.Content = cpu.getPnpDeviceId() ?? "Unknown";

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

            cpuId.Content = cpu.getProcessorId() ?? "Unknown";
            cpuType.Content = cpu.getProcessorType() ?? "Unknown";
            cpuRevision.Content = cpu.getRevision()?.ToString() ?? "Unknown";
            cpuCurrentLoad.Content = loadPercentage.NextValue().ToString() + "%";
            cpuVirtualization.Content = ((cpu.getVirtualizationEnabled()?.ToString() ?? "Unknown") == "True" ? "Yes" : "No");

            //Add CPU usage counters for the first time
            updateCpuUsage(true);

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += updateCpuStats;
            timer.Start();
        }

        /// <summary>
        /// Updates the CPU core usages either for the first time or continually
        /// </summary>
        /// <param name="init">Dictates if the usages should be added for the first time or updated</param>
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

        /// <summary>
        /// Continually updates various CPU values that constantly change in a running system
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

            //Current load
            cpuCurrentLoad.Content = loadPercentage.NextValue().ToString("00.00") + "%";
        }
    }
}
