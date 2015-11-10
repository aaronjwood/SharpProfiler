using Sharp_Profiler.Hardware;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

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
            Processor cpu = new Processor();
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

        /// <summary>
        /// Initializes the whole memory section
        /// Retrieves all of the necessary information and populates the UI
        /// </summary>
        private void initMemory()
        {
            Memory memory = new Memory();

            var getMemoryInfo = new Action<string>(bank =>
            {
                memoryDataWidth.Content = (memory.getDataWidth(bank)?.ToString() ?? "Unknown") + " bits";
                memoryDescription.Content = memory.getDescription(bank) ?? "Unknown";
                memoryLocation.Content = memory.getDeviceLocation(bank) ?? "Unknown";
            });

            memoryBankList.ItemsSource = memory.getBankLabels();
            memoryBankList.SelectedIndex = 0;
            getMemoryInfo(memoryBankList.SelectedItem.ToString());
            memoryBankList.SelectionChanged += new SelectionChangedEventHandler(delegate (object sender, SelectionChangedEventArgs e)
            {
                string value = (sender as ComboBox).SelectedItem.ToString();
                getMemoryInfo(value);
            });

            memoryCapacity.Content = (memory.getCapacity()?.ToString() ?? "Unknown") + " MB";

        }
    }
}
