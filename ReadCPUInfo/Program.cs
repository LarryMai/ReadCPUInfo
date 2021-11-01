using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ReadCPUInfo
{
    class Program
    {
        /// <summary>
        /// 
        /// </summary>
        static string _hardware = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        static string _serialNo = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine($"Current platform: {System.Environment.OSVersion.Platform}");

            if (System.OperatingSystem.IsWindows())
            {
                WinMain();
            }
            else if(System.OperatingSystem.IsLinux())
            {
                LinuxMain();
            }
            else
            {
                // do nothing
            }
            Console.WriteLine("All over ...");
        }

        /// <summary>
        /// 
        /// </summary>
        static void LinuxMain()
        {
            const string SOURCE_CPU_INFO = @"/proc/cpuinfo";
            string[] cpuInfoLines = File.ReadAllLines(SOURCE_CPU_INFO);
            Dictionary<Regex, Action<string>> rules = new Dictionary<Regex, Action<string>>()
            {
                {
                    new Regex(@"^Serial\s+:\s + (.+)"),  serialno =>
                    {
                        _serialNo = serialno;
                        Console.WriteLine($"{nameof(_serialNo)} : {_serialNo}");
                    }
                },
                {
                    new Regex(@"^Hardware\s+:\s + (.+)"),  hardware =>
                    {
                        _hardware = hardware;
                        Console.WriteLine($"{nameof(_hardware)} : {_hardware}");
                    }
                },
                {
                    new Regex(@"^vendor_id\s+:\s + (.+)"),  vendor_id =>
                    {
                        Console.WriteLine($"{nameof(vendor_id)} : {vendor_id}");
                    }
                },
                {
                    new Regex(@"^cpu family\s+:\s + (.+)"),  cpu_family =>
                    {
                        Console.WriteLine($"{nameof(cpu_family)} : {cpu_family}");
                    }
                },
                {
                    new Regex(@"^model\s+:\s + (.+)"),  model =>
                    {
                        Console.WriteLine($"{nameof(model)} : {model}");
                    }
                },
                {
                    new Regex(@"^model name\s+:\s + (.+)"),  model_name =>
                    {
                        Console.WriteLine($"{nameof(model_name)} : {model_name}");
                    }
                }
            };

            foreach (string cpuInfoLine in cpuInfoLines)
            {
                foreach (var rule in rules)
                {
                    Match match = rule.Key.Match(cpuInfoLine);
                    if (match.Groups[0].Success)
                    {
                        string value = match.Groups[1].Value;
                        rule.Value.Invoke(value);
                    }
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        static void WinMain()
        {

        }
    }
}
