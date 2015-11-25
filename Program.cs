using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Speech.Synthesis;

namespace CPU_and_Memory_monitor
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Performance counters
            // This will pull the current CPU load in percentage
            PerformanceCounter perfCountCPU = new PerformanceCounter("Processor Information", "% Processor Time", "_Total");
            perfCountCPU.NextValue();

            // This will pull the current available memory in Megabytes
            PerformanceCounter perfCountMem = new PerformanceCounter("Memory", "Available MBytes");
            perfCountMem.NextValue();

            // This will get us the system up time (in seconds)
            PerformanceCounter perfCountUpTime = new PerformanceCounter("System", "System Up Time");
            perfCountUpTime.NextValue();

            #endregion
            // This will greet the user in the default voice
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.Speak("This is CPU and Memory monitor");
            synth.Speak("Hello, how are you today?");

            synth.SelectVoiceByHints(VoiceGender.Female);
            TimeSpan uptime = TimeSpan.FromSeconds(perfCountUpTime.NextValue());
            string UpTimeMessage = String.Format("The current system up time is {0} days, {1} hours, {2} minutes and {3} seconds", (int)uptime.TotalDays, (int)uptime.Hours, (int)uptime.Minutes, (int)uptime.Seconds);
            Console.WriteLine(UpTimeMessage);
            synth.Speak(UpTimeMessage);

            while (true)
            {
                int currentCpuPercentage = (int)perfCountCPU.NextValue();
                int currentAvailableMemory = (int)perfCountMem.NextValue();


                // Every 1 second print the CPU load in percentage to the screen
                Console.WriteLine("CPU load %: {0}", currentCpuPercentage);
                Console.WriteLine("Available Memory: {0}MB", currentAvailableMemory);


                if (currentCpuPercentage > 80)
                {
                    if (currentCpuPercentage == 100)
                    {
                        synth.SelectVoiceByHints(VoiceGender.Female);
                        synth.Speak("Warning, warning CPU load is 100 percent!");
                    }
                    else
                    {
                        string cpuLoadMessage = String.Format("The current CPU load is {0} percent", currentCpuPercentage);
                        synth.Speak(cpuLoadMessage);
                    }

                }

                if (currentAvailableMemory < 1024)
                {
                    string MemoryMessage = String.Format("The current available memory is {0} megabytes", currentAvailableMemory);
                    synth.Speak(MemoryMessage);
                }

                Thread.Sleep(1000);
            }
        }
    }
}
