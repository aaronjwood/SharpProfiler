using System;
using System.Windows;
using System.Diagnostics;
using System.Windows.Threading;
using Sharp_Profiler.CPU;

namespace Sharp_Profiler
{
    public partial class SharpProfiler : Window
    {
        private Cpu cpu;
        private DispatcherTimer timer;

        public SharpProfiler()
        {
            InitializeComponent();
            cpu = new Cpu();
            timer = new DispatcherTimer();

            cpuName.Content = cpu.getName();
            cpuAddressWidth.Content = cpu.getAddressWidth();
            cpuArchitecture.Content = cpu.getArchitecture();
            cpuMaxClockSpeed.Content = cpu.getMaxClockSpeed() + "MHz";
            cpuVoltage.Content = cpu.getVoltage();
            cpuDeviceId.Content = cpu.getDeviceId();
            cpuFamily.Content = cpu.getFamily() == "Other" ? cpu.getFamily() + " " + cpu.getOtherFamilyDescription() : cpu.getFamily();
            cpuL2CacheSize.Content = cpu.getL2CacheSize() + "KB";
            cpuL2CacheSpeed.Content = cpu.getL2CacheSpeed() + "MHz";
            cpuL3CacheSize.Content = cpu.getL3CacheSize() + "KB";
            cpuL3CacheSpeed.Content = cpu.getL3CacheSpeed() + "MHz";
            cpuManufacturer.Content = cpu.getManufacturer();
            cpuNumberOfCores.Content = cpu.getNumberOfCores();
            cpuNumberOfLogicalProcessors.Content = cpu.getNumberOfLogicalProcessors();
            cpuNumberOfPhysicalProcessors.Content = cpu.getNumberOfPhysicalProcessors();
            cpuPlugAndPlayDeviceId.Content = cpu.getPnpDeviceId();
            cpuPowerManagementCapabilities.Content = cpu.getPowerManagementCapabilities();
            cpuId.Content = cpu.getProcessorId();
            cpuType.Content = cpu.getProcessorType();
            cpuRevision.Content = cpu.getRevision();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += delegate(object timerSender, EventArgs timerEventArgs)
            {
                updateCpuUsage();
                updateCpuClockSpeed();
            };
            timer.Start();
        }

        private void updateCpuUsage()
        {
            cpuUsageList.Items.Clear();
            for (int i = 0; i < cpu.getNumberOfLogicalProcessors(); i++)
            {
                cpuUsageList.Items.Add("Core #" + i + ":   " + cpu.UsageCounters[i].NextValue().ToString("00.00") + "%");
            }
        }

        private void updateCpuClockSpeed()
        {
            cpuCurrentClockSpeed.Content = cpu.getCurrentClockSpeed() + "MHz";
        }
    }
}
