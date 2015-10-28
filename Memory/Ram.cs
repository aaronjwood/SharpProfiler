using Sharp_Profiler.Hardware;
using System;

namespace Sharp_Profiler.Memory
{
    class Ram : Component
    {
        /// <summary>
        /// Gets the physically labeled bank where the memory is located
        /// </summary>
        /// <returns>Bank label</returns>
        public string getBankLabel()
        {
            try
            {
                return (string)this.queryWmi("SELECT BankLabel FROM Win32_PhysicalMemory", "BankLabel");
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
