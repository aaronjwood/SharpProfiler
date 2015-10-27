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

            cpuName.Content = cpu.getName();
            cpuAddressWidth.Content = cpu.getAddressWidth();
            cpuArchitecture.Content = cpu.getArchitecture();
            cpuVoltage.Content = cpu.getVoltage();
            cpuDeviceId.Content = cpu.getDeviceId();
            cpuFamily.Content = cpu.getFamily() == "Other" ? cpu.getFamily() + " " + cpu.getOtherFamilyDescription() : cpu.getFamily();
            cpuL2CacheSize.Content = cpu.getL2CacheSize() + " KB";
            cpuL2CacheSpeed.Content = cpu.getL2CacheSpeed() == 0 ? "Unknown" : cpu.getL2CacheSpeed() + " MHz";
            cpuL3CacheSize.Content = cpu.getL3CacheSize() + " KB";
            cpuL3CacheSpeed.Content = cpu.getL3CacheSpeed() == 0 ? "Unknown" : cpu.getL3CacheSpeed() + " MHz";
            cpuManufacturer.Content = cpu.getManufacturer();
            cpuNumberOfLogicalProcessors.Content = numberLogicalProcessors;

            UInt32? cores = cpu.getNumberOfCores();
            if (cores == null)
            {
                cpuNumberOfCores.Content = "Unknown";
            }
            else
            {
                cpuNumberOfCores.Content = cores;
            }

            int? physicalProcessors = cpu.getNumberOfPhysicalProcessors();
            if (physicalProcessors == null)
            {
                cpuNumberOfPhysicalProcessors.Content = "Unknown";
            }
            else
            {
                cpuNumberOfPhysicalProcessors.Content = physicalProcessors;
            }

            string plugAndPlay = cpu.getPnpDeviceId();
            if (plugAndPlay == null)
            {
                cpuPlugAndPlayDeviceId.Content = "Unknown";
            }
            else
            {
                cpuPlugAndPlayDeviceId.Content = plugAndPlay;
            }

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

            UInt16? revision = cpu.getRevision();
            cpuRevision.Content = revision != null ? revision.ToString() : "Unknown";

            cpuCurrentLoad.Content = loadPercentage.NextValue().ToString() + "%";

            bool? cpuVirtualizationStatus = cpu.getVirtualizationEnabled();
            if (cpuVirtualizationStatus != null)
            {
                cpuVirtualization.Content = cpuVirtualizationStatus == true ? "Yes" : "No";
            }
            else
            {
                cpuVirtualization.Content = "Unknown";
            }

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
