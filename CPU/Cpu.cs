using System;
using System.Collections.Generic;
using System.Management;
using System.Diagnostics;

namespace Sharp_Profiler.CPU
{
    class Cpu
    {
        private PerformanceCounter[] performance;

        public Cpu()
        {
            performance = new PerformanceCounter[countCores()];
        }

        //Return all performance counters
        public PerformanceCounter[] getCounters()
        {
            return performance;
        }

        //Return number of cores (with hyperthreading)
        public int countCores()
        {
            return Environment.ProcessorCount;
        }

        //Return number of cores (without hyperthreading)
        public int countCpu()
        {
            int coreCount = 0;
            foreach (var item in new ManagementObjectSearcher("SELECT NumberOfCores FROM Win32_Processor").Get())
            {
                coreCount += int.Parse(item["NumberOfCores"].ToString());
            }
            return coreCount;
        }
    }
}
