using System;
using System.Collections.Generic;
using System.Management;
using System.Diagnostics;

namespace Sharp_Profiler.CPU
{
    class Cpu
    {
        /// <summary>
        /// Gets/sets the performance property which holds performance counters for the CPU
        /// </summary>
        public PerformanceCounter[] cpuUsageCounters { get; set; }

        /// <summary>
        /// Creates a new Cpu object and kicks off performance counters
        /// </summary>
        public Cpu()
        {
            cpuUsageCounters = new PerformanceCounter[this.getLogicalProcessorCount()];
            this.calculateCpuUsage();
        }

        /// <summary>
        /// Return all performance counters
        /// </summary>
        /// <returns>Performance counters</returns>
        public PerformanceCounter[] getCpuUsageCounters()
        {
            return this.cpuUsageCounters;
        }

        public string getCpuArch()
        {
            int cpuArch = int.Parse(this.queryWmi("SELECT Architecture FROM Win32_Processor", "Architecture").ToString());
            switch (cpuArch)
            {
                case 0:
                    return "x86";
                case 1:
                    return "MIPS";
                case 2:
                    return "Alpha";
                case 3:
                    return "PowerPC";
                case 5:
                    return "ARM";
                case 6:
                    return "Itanium";
                case 9:
                    return "x64";
            }
            return null;
        }

        /// <summary>
        /// Gets the CPU's name
        /// </summary>
        /// <returns>The full name of the CPU</returns>
        public string getCpuName()
        {
            return (string)this.queryWmi("SELECT Name FROM Win32_Processor", "Name");
        }

        /// <summary>
        /// Gets the number of logical processors on a system
        /// </summary>
        /// <returns>The number of logical processors</returns>
        public int getLogicalProcessorCount()
        {
            return Environment.ProcessorCount;
        }

        /// <summary>
        /// Gets the number of physical processors on a system
        /// </summary>
        /// <returns>The number of physical processors</returns>
        public int getPhysicalProcessorCount()
        {
            return int.Parse(this.queryWmi("SELECT NumberOfProcessors FROM Win32_ComputerSystem", "NumberOfProcessors").ToString());
        }

        /// <summary>
        /// Gets the number of physical cores on a system
        /// </summary>
        /// <returns>The number of physical cores</returns>
        public int getPhysicalCoreCount()
        {
            return int.Parse(this.queryWmi("SELECT NumberOfCores FROM Win32_Processor", "NumberOfCores").ToString());
        }

        /// <summary>
        /// Instantiates the performance counters for retrieving CPU usage
        /// </summary>
        private void calculateCpuUsage()
        {
            for (int i = 0; i < this.getLogicalProcessorCount(); i++)
            {
                this.cpuUsageCounters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
            }
        }

        /// <summary>
        /// Performs queries against WMI and returns an object with the specified property
        /// </summary>
        /// <param name="query">The WMI query</param>
        /// <param name="property">The property that you want returned</param>
        /// <returns>WMI object</returns>
        private object queryWmi(string query, string property)
        {
            foreach (ManagementObject item in new ManagementObjectSearcher(query).Get())
            {
                return item[property];
            }
            return null;
        }


    }
}
