using Sharp_Profiler.Hardware;
using System;
using System.Collections.Generic;

namespace Sharp_Profiler.Memory
{
    class Ram : Component
    {
        /// <summary>
        /// Gets all of the physically labeled banks where the memory is located
        /// </summary>
        /// <returns>Bank labels</returns>
        public List<object> getBankLabels()
        {
            try
            {
                return this.queryWmi("SELECT BankLabel FROM Win32_PhysicalMemory", "BankLabel", 0);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the total capacity of the system's physical memory
        /// </summary>
        /// <returns>Total capacity of the physical memory</returns>
        public UInt64? getCapacity()
        {
            try
            {
                UInt64 size = 0;
                List<object> capacities = this.queryWmi("SELECT Capacity FROM Win32_PhysicalMemory", "Capacity", 0);
                foreach (UInt64 item in capacities)
                {
                    size += item;
                }
                size = size / 1024 / 1024;
                return size;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the memory data width in bits
        /// </summary>
        /// <param name="bank">Specifies which bank of memory to get information for</param>
        /// <returns>Memory data width</returns>
        public UInt16? getDataWidth(string bank)
        {
            try
            {
                return (UInt16)this.queryWmi("SELECT DataWidth FROM Win32_PhysicalMemory WHERE BankLabel = '" + bank + "'", "DataWidth");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the description of the object
        /// </summary>
        /// <param name="bank">Specifies which bank of memory to get information for</param>
        /// <returns>Description</returns>
        public string getDescription(string bank)
        {
            try
            {
                return (string)this.queryWmi("SELECT Description FROM Win32_PhysicalMemory WHERE BankLabel = '" + bank + "'", "Description");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the name of the socket or circuit board that holds the memory
        /// </summary>
        /// <param name="bank">Specifies which bank of memory to get information for</param>
        /// <returns>Label of the socket or circuit board</returns>
        public string getDeviceLocation(string bank)
        {
            try
            {
                return (string)this.queryWmi("SELECT DeviceLocator FROM Win32_PhysicalMemory WHERE BankLabel = '" + bank + "'", "DeviceLocator");
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
