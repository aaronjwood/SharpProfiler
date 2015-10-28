using System.Collections.Generic;
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

        /// <summary>
        /// Performs queries against WMI and returns a list of objects with the specified property
        /// </summary>
        /// <param name="query">The WMI query</param>
        /// <param name="property">The property that you want from each item</param>
        /// <param name="numItems">How many items you want returned. Specifying 0 will grab everything</param>
        /// <returns>List of items with the specified property</returns>
        protected List<object> queryWmi(string query, string property, int numItems)
        {
            List<object> items = new List<object>();
            foreach (ManagementObject item in new ManagementObjectSearcher(query).Get())
            {
                items.Add(item[property]);
                if (numItems != 0 && items.Count == numItems)
                {
                    break;
                }
            }
            return items;
        }
    }
}
