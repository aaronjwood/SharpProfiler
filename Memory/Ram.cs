using Sharp_Profiler.Hardware;

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
            return (string)this.queryWmi("SELECT BankLabel FROM Win32_PhysicalMemory", "BankLabel");
        }
    }
}
