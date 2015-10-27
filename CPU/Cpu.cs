using OpenHardwareMonitor.Hardware;
using System;
using System.Diagnostics;
using System.Management;

namespace Sharp_Profiler.CPU
{
    class Cpu
    {
        /// <summary>
        /// Gets/sets the performance property which holds performance counters for the CPU
        /// </summary>
        public PerformanceCounter[] UsageCounters { get; set; }
        private Computer cpu;

        /// <summary>
        /// Creates a new Cpu object and kicks off performance counters
        /// </summary>
        public Cpu()
        {
            UsageCounters = new PerformanceCounter[this.getNumberOfLogicalProcessors()];
            this.calculateUsage();
            this.cpu = new Computer();
            this.cpu.CPUEnabled = true;
            this.cpu.Open();
        }

        /// <summary>
        /// Gets the CPU address width
        /// </summary>
        /// <returns>CPU address width</returns>
        public UInt16 getAddressWidth()
        {
            try
            {
                return (UInt16)this.queryWmi("SELECT AddressWidth FROM Win32_Processor", "AddressWidth");
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        /// <summary>
        /// Gets the CPU architecture
        /// </summary>
        /// <returns>CPU Architecture</returns>
        public string getArchitecture()
        {
            try
            {
                UInt16 arch = UInt16.Parse(this.queryWmi("SELECT Architecture FROM Win32_Processor", "Architecture").ToString());
                switch (arch)
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
            catch (Exception e)
            {
                return null;
            }

        }

        /// <summary>
        /// Gets the current clock speed of the CPU
        /// </summary>
        /// <returns>Current CPU clock speed in MHz</returns>
        public float? getCurrentClockSpeed()
        {
            foreach (IHardware hardware in this.cpu.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    hardware.Update();
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            return sensor.Value.HasValue ? (float?)Math.Ceiling((double)sensor.Value) : null;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the current voltage of the CPU.
        /// </summary>
        /// <returns>CPU voltage or 0 if the SMBIOS doesn't present a voltage value</returns>
        public Decimal getVoltage()
        {
            //Make sure we can actually obtain the voltage
            try
            {
                UInt16 voltage = (UInt16)this.queryWmi("SELECT CurrentVoltage FROM Win32_Processor", "CurrentVoltage");
                return Convert.ToDecimal(voltage) / 10;
            }
            catch (Exception e)
            {
                return new Decimal(0);
            }
        }

        /// <summary>
        /// Gets the unique identifier of the processor
        /// </summary>
        /// <returns>CPU DeviceID</returns>
        public string getDeviceId()
        {
            try
            {
                return (string)this.queryWmi("SELECT DeviceID FROM Win32_Processor", "DeviceID");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the processor family type
        /// </summary>
        /// <returns>CPU family type</returns>
        public string getFamily()
        {
            try
            {
                UInt16 family = (UInt16)this.queryWmi("SELECT Family FROM Win32_Processor", "Family");
                switch (family)
                {
                    case 1:
                        return "Other";
                    case 2:
                        return "Unknown";
                    case 3:
                        return "8086";
                    case 4:
                        return "80286";
                    case 5:
                        return "Intel386 Processor";
                    case 6:
                        return "Intel486 Processor";
                    case 7:
                        return "8087";
                    case 8:
                        return "80287";
                    case 9:
                        return "80387";
                    case 10:
                        return "80487";
                    case 11:
                        return "Pentium Brand";
                    case 12:
                        return "Pentium Pro";
                    case 13:
                        return "Pentium II";
                    case 14:
                        return "Pentium Processor with MMX Technology";
                    case 15:
                        return "Celeron";
                    case 16:
                        return "Pentium II Xeon";
                    case 17:
                        return "Pentium III";
                    case 18:
                        return "M1 Family";
                    case 19:
                        return "M2 Family";
                    case 24:
                        return "AMD Duron Processor Family";
                    case 25:
                        return "K5 Family";
                    case 26:
                        return "K6 Family";
                    case 27:
                        return "K6-2";
                    case 28:
                        return "K6-3";
                    case 29:
                        return "AMD Athlon Processor Family";
                    case 30:
                        return "AMD2900 Family";
                    case 31:
                        return "K6-2+";
                    case 32:
                        return "Power PC Family";
                    case 33:
                        return "Power PC 601";
                    case 34:
                        return "Power PC 603";
                    case 35:
                        return "Power PC 603+";
                    case 36:
                        return "Power PC 604";
                    case 37:
                        return "Power PC 620";
                    case 38:
                        return "Power PC X704";
                    case 39:
                        return "Power PC 750";
                    case 48:
                        return "Alpha Family";
                    case 49:
                        return "Alpha 21064";
                    case 50:
                        return "Alpha 21066";
                    case 51:
                        return "Alpha 21164";
                    case 52:
                        return "Alpha 21164PC";
                    case 53:
                        return "Alpha 21164a";
                    case 54:
                        return "Alpha 21264";
                    case 55:
                        return "Alpha 21364";
                    case 64:
                        return "MIPS Family";
                    case 65:
                        return "MIPS R4000";
                    case 66:
                        return "MIPS R4200";
                    case 67:
                        return "MIPS R4400";
                    case 68:
                        return "MIPS R4600";
                    case 69:
                        return "MIPS R10000";
                    case 80:
                        return "SPARC Family";
                    case 81:
                        return "SuperSPARC";
                    case 82:
                        return "microSPARC II";
                    case 83:
                        return "microSPARC IIep";
                    case 84:
                        return "UltraSPARC";
                    case 85:
                        return "UltraSPARC II";
                    case 86:
                        return "UltraSPARC IIi";
                    case 87:
                        return "UltraSPARC III";
                    case 88:
                        return "UltraSPARC IIIi";
                    case 96:
                        return "68040";
                    case 97:
                        return "68xx Family";
                    case 98:
                        return "68000";
                    case 99:
                        return "68010";
                    case 100:
                        return "68020";
                    case 101:
                        return "68030";
                    case 112:
                        return "Hobbit Family";
                    case 120:
                        return "Crusoe TM5000 Family";
                    case 121:
                        return "Crusoe TM3000 Family";
                    case 122:
                        return "Efficeon TM8000 Family";
                    case 128:
                        return "Weitek";
                    case 130:
                        return "Itanium Processor";
                    case 131:
                        return "AMD Athlon 64 Processor Family";
                    case 132:
                        return "AMD Opteron Processor Family";
                    case 144:
                        return "PA-RISC Family";
                    case 145:
                        return "PA-RISC 8500";
                    case 146:
                        return "PA-RISC 8000";
                    case 147:
                        return "PA-RISC 7300LC";
                    case 148:
                        return "PA-RISC 7200";
                    case 149:
                        return "PA-RISC 7100LC";
                    case 150:
                        return "PA-RISC 7100";
                    case 160:
                        return "V30 Family";
                    case 176:
                        return "Pentium III Xeon Processor";
                    case 177:
                        return "Pentium III Processor with Intel SpeedStep Technology";
                    case 178:
                        return "Pentium 4";
                    case 179:
                        return "Intel Xeon";
                    case 180:
                        return "AS400 Family";
                    case 181:
                        return "Intel Xeon Processor MP";
                    case 182:
                        return "AMD Athlon XP Family";
                    case 183:
                        return "AMD Athlon MP Family";
                    case 184:
                        return "Intel Itanium 2";
                    case 185:
                        return "Intel Pentium M Processor";
                    case 190:
                        return "K7";
                    case 200:
                        return "IBM390 Family";
                    case 201:
                        return "G4";
                    case 202:
                        return "G5";
                    case 203:
                        return "G6";
                    case 204:
                        return "z/Architecture Base";
                    case 250:
                        return "i860";
                    case 251:
                        return "i960";
                    case 260:
                        return "SH-3";
                    case 261:
                        return "SH-4";
                    case 280:
                        return "ARM";
                    case 281:
                        return "StrongARM";
                    case 300:
                        return "6x86";
                    case 301:
                        return "MediaGX";
                    case 302:
                        return "MII";
                    case 320:
                        return "WinChip";
                    case 350:
                        return "DSP";
                    case 500:
                        return "Video Processor";
                    default:
                        return "Unknown";
                }
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the CPU's L2 cache size
        /// </summary>
        /// <returns>CPU L2 cache size in KB</returns>
        public UInt32 getL2CacheSize()
        {
            try
            {
                return (UInt32)this.queryWmi("SELECT L2CacheSize FROM Win32_Processor", "L2CacheSize");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the CPU's L2 cache speed
        /// </summary>
        /// <returns>CPU L2 cache speed in MHz</returns>
        public UInt32 getL2CacheSpeed()
        {
            try
            {
                return (UInt32)this.queryWmi("SELECT L2CacheSpeed FROM Win32_Processor", "L2CacheSpeed");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the CPU's L3 cache size
        /// </summary>
        /// <returns>CPU L3 cache size in KB</returns>
        public UInt32 getL3CacheSize()
        {
            try
            {
                return (UInt32)this.queryWmi("SELECT L3CacheSize FROM Win32_Processor", "L3CacheSize");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the CPU's L3 cache speed
        /// </summary>
        /// <returns>CPU L3 cache speed in MHz</returns>
        public UInt32 getL3CacheSpeed()
        {
            try
            {
                return (UInt32)this.queryWmi("SELECT L3CacheSpeed FROM Win32_Processor", "L3CacheSpeed");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the CPU's manufacturer
        /// </summary>
        /// <returns>CPU manufacturer</returns>
        public string getManufacturer()
        {
            try
            {
                return (string)this.queryWmi("SELECT Manufacturer FROM Win32_Processor", "Manufacturer");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the minimum CPU clock speed value that has been read
        /// </summary>
        /// <returns>The minimum CPU clock speed value</returns>
        public float? getMinClockSpeed()
        {
            foreach (IHardware hardware in this.cpu.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    hardware.Update();
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            return sensor.Value.HasValue ? (float?)Math.Ceiling((double)sensor.Min) : null;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the CPU's maximum clock speed
        /// </summary>
        /// <returns>The CPU's maximum clock speed in MHz</returns>
        public float? getMaxClockSpeed()
        {
            foreach (IHardware hardware in this.cpu.Hardware)
            {
                if (hardware.HardwareType == HardwareType.CPU)
                {
                    hardware.Update();
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Clock)
                        {
                            return sensor.Value.HasValue ? (float?)Math.Ceiling((double)sensor.Max) : null;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the CPU's name
        /// </summary>
        /// <returns>The full name of the CPU</returns>
        public string getName()
        {
            try
            {
                return (string)this.queryWmi("SELECT Name FROM Win32_Processor", "Name");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the number of cores in the CPU
        /// </summary>
        /// <returns>The number of CPU cores</returns>
        public UInt32 getNumberOfCores()
        {
            try
            {
                return (UInt32)this.queryWmi("SELECT NumberOfCores FROM Win32_Processor", "NumberOfCores");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of logical processors on a system
        /// </summary>
        /// <returns>The number of logical processors</returns>
        public UInt32 getNumberOfLogicalProcessors()
        {
            try
            {
                return (UInt32)this.queryWmi("SELECT NumberOfLogicalProcessors FROM Win32_Processor", "NumberOfLogicalProcessors");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the number of physical processors in a system
        /// </summary>
        /// <returns>The number of physical processors</returns>
        public int getNumberOfPhysicalProcessors()
        {
            try
            {
                return int.Parse(this.queryWmi("SELECT NumberOfProcessors FROM Win32_ComputerSystem", "NumberOfProcessors").ToString());
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets additional family information if the family property is set to 1 (Other)
        /// </summary>
        /// <returns>Additional family information</returns>
        public string getOtherFamilyDescription()
        {
            try
            {
                return (string)this.queryWmi("SELECT OtherFamilyDescription FROM Win32_Processor", "OtherFamilyDescription");
            }
            catch (Exception e)
            {
                return null;
            }

        }

        /// <summary>
        /// Gets the windows plug and play device identifier of the CPU
        /// </summary>
        /// <returns>The windows plug and play device identifier</returns>
        public string getPnpDeviceId()
        {
            try
            {
                return (string)this.queryWmi("SELECT PNPDeviceID FROM Win32_Processor", "PNPDeviceID");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the CPU's power management capabilities
        /// </summary>
        /// <returns>An array of descriptions for the CPU's power management capabilities</returns>
        public string[] getPowerManagementCapabilities()
        {
            try
            {
                string[] powerManagementDescriptions = new string[10];
                UInt16[] powerManagementCodes = (UInt16[])this.queryWmi("SELECT PowerManagementCapabilities FROM Win32_Processor", "PowerManagementCapabilities");
                foreach (UInt16 code in powerManagementCodes)
                {
                    switch (code)
                    {
                        case 0:
                            powerManagementDescriptions[code] = "Unknown";
                            break;
                        case 1:
                            powerManagementDescriptions[code] = "Not Supported";
                            break;
                        case 2:
                            powerManagementDescriptions[code] = "Disabled";
                            break;
                        case 3:
                            powerManagementDescriptions[code] = "Enabled";
                            break;
                        case 4:
                            powerManagementDescriptions[code] = "Power Saving Modes Entered Automatically";
                            break;
                        case 5:
                            powerManagementDescriptions[code] = "Power State Settable";
                            break;
                        case 6:
                            powerManagementDescriptions[code] = "Power Cycling Supported";
                            break;
                        case 7:
                            powerManagementDescriptions[code] = "Timed Power-On Supported";
                            break;
                    }
                }
                return powerManagementDescriptions;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the CPU's ID which describes the processor's features.
        /// </summary>
        /// <returns>The processor ID</returns>
        public string getProcessorId()
        {
            try
            {
                return (string)this.queryWmi("SELECT ProcessorId FROM Win32_Processor", "ProcessorId");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the CPU type
        /// </summary>
        /// <returns>Processor type</returns>
        public string getProcessorType()
        {
            try
            {
                UInt16 processorType = (UInt16)this.queryWmi("SELECT ProcessorType FROM Win32_Processor", "ProcessorType");
                switch (processorType)
                {
                    case 1:
                        return "Other";
                    case 2:
                        return "Unknown";
                    case 3:
                        return "Central Processor";
                    case 4:
                        return "Math Processor";
                    case 5:
                        return "DSP Processor";
                    case 6:
                        return "Video Processor";
                }
                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the CPU revision level
        /// </summary>
        /// <returns>CPU revision level</returns>
        public UInt16 getRevision()
        {
            try
            {
                return (UInt16)this.queryWmi("SELECT Revision FROM Win32_Processor", "Revision");
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets the current CPU load
        /// </summary>
        /// <returns>Performance counter that contains the load percentage</returns>
        public PerformanceCounter getLoadPercentage()
        {
            return new PerformanceCounter("Processor", "% Processor Time", "_Total");
        }

        /// <summary>
        /// Determines if virtualization extensions are enabled or not
        /// </summary>
        /// <returns>Virtualization extensions status</returns>
        public bool? getVirtualizationEnabled()
        {
            try
            {
                return (bool)this.queryWmi("SELECT VirtualizationFirmwareEnabled FROM Win32_Processor", "VirtualizationFirmwareEnabled");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// Instantiates the performance counters for retrieving CPU usage
        /// </summary>
        private void calculateUsage()
        {
            for (int i = 0; i < this.getNumberOfLogicalProcessors(); i++)
            {
                this.UsageCounters[i] = new PerformanceCounter("Processor", "% Processor Time", i.ToString());
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
