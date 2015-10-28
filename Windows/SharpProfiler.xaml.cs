using Sharp_Profiler.CPU;
using Sharp_Profiler.RAM;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Diagnostics;

namespace Sharp_Profiler
{
    public partial class SharpProfiler : Window
    {
        /// <summary>
        /// Window initialization
        /// </summary>
        public SharpProfiler()
        {
            InitializeComponent();

            initCpu();
            initMemory();
        }

        /// <summary>
        /// Initializes the whole CPU section
        /// Retrieves all of the necessary information and populates the UI
        /// </summary>
        private void initCpu()
        {
            Cpu cpu = new Cpu();
            int numberLogicalProcessors = cpu.getNumberOfLogicalProcessors();
            PerformanceCounter loadPercentage = cpu.getLoadPercentage();

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
            cpuId.Content = cpu.getProcessorId() ?? "Unknown";
            cpuType.Content = cpu.getProcessorType() ?? "Unknown";
            cpuRevision.Content = cpu.getRevision()?.ToString() ?? "Unknown";
            cpuCurrentLoad.Content = loadPercentage.NextValue().ToString() + "%";
            cpuVirtualization.Content = ((cpu.getVirtualizationEnabled()?.ToString() ?? "Unknown") == "True" ? "Yes" : "No");
            cpuStepping.Content = cpu.getStepping() ?? "Unknown";
            cpuUniqueId.Content = cpu.getUniqueId() ?? "Unknown";

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

            //Delegate used for continually updating the CPU usage for each core
            var updateCpuUsage = new Action<bool>(init =>
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
            });

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += delegate (object sender, EventArgs e)
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
            };
            timer.Start();

            //Add CPU usage counters for the first time
            updateCpuUsage(true);
        }

        private void initMemory()
        {
            Memory memory = new Memory();
            memoryBankLabel.Content = memory.getBankLabel() ?? "Unknown";
        }
    }
}
