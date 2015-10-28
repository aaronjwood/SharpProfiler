using System.Management;

namespace Sharp_Profiler.RAM
{
    class Memory
    {
        /// <summary>
        /// Gets the physically labeled bank where the memory is located
        /// </summary>
        /// <returns>Bank label</returns>
        public string getBankLabel()
        {
            return (string)this.queryWmi("SELECT BankLabel FROM Win32_PhysicalMemory", "BankLabel");
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
