using System.Management;

namespace Sharp_Profiler.Hardware
{
    abstract class Component
    {
        /// <summary>
        /// Performs queries against WMI and returns an object with the specified property
        /// </summary>
        /// <param name="query">The WMI query</param>
        /// <param name="property">The property that you want returned</param>
        /// <returns>WMI object</returns>
        protected object queryWmi(string query, string property)
        {
            foreach (ManagementObject item in new ManagementObjectSearcher(query).Get())
            {
                return item[property];
            }
            return null;
        }
    }
}
