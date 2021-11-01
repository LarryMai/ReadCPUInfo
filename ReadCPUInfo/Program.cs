using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ReadCPUInfo
{
    partial class Program
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
        static void Main(string[] args)
        {
            Console.WriteLine($"Current platform: {System.Environment.OSVersion.Platform}");

            if (System.OperatingSystem.IsWindows())
            {
                WinAction();
            }
            else if (System.OperatingSystem.IsLinux())
            {
                LinuxAction();
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
        static void LinuxAction()
        {
            const string SOURCE_CPU_INFO = @"/proc/cpuinfo";
            string[] cpuInfoLines = File.ReadAllLines(SOURCE_CPU_INFO);
            Dictionary<Regex, Action<string>> rules = new Dictionary<Regex, Action<string>>()
            {
                {
                    new Regex(@"^Serial\s+:\s(.+)"),  serialno =>
                    {
                        _serialNo = serialno;
                        Console.WriteLine($"{nameof(_serialNo)} : {_serialNo}");
                    }
                },
                {
                    new Regex(@"^Hardware\s+:\s(.+)"),  hardware =>
                    {
                        _hardware = hardware;
                        Console.WriteLine($"{nameof(_hardware)} : {_hardware}");
                    }
                },
                {
                    new Regex(@"^vendor_id\s+:\s(.+)"),  vendor_id =>
                    {
                        Console.WriteLine($"{nameof(vendor_id)} : {vendor_id}");
                    }
                },
                {
                    new Regex(@"^cpu family\s+:\s(.+)"),  cpu_family =>
                    {
                        Console.WriteLine($"{nameof(cpu_family)} : {cpu_family}");
                    }
                },
                {
                    new Regex(@"^model\s+:\s(.+)"),  model =>
                    {
                        Console.WriteLine($"{nameof(model)} : {model}");
                    }
                },
                {
                    new Regex(@"^model name\s+:\s(.+)"),  model_name =>
                    {
                        Console.WriteLine($"{nameof(model_name)} : {model_name}");
                    }
                }
            };

            foreach (string cpuInfoLine in cpuInfoLines)
            {
                foreach (var rule in rules)
                {
                    if (!rule.Key.IsMatch(cpuInfoLine))
                    {
                        continue;
                    }

                    Match match = rule.Key.Match(cpuInfoLine);
                    string value = match.Groups[1].Value;
                    rule.Value?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static void WinAction()
        {
            string SOURCE_CPU_INFO = Program.ProcCpuInfoFilePath;
            string[] cpuInfoLines = File.ReadAllLines(SOURCE_CPU_INFO);
            Dictionary<Regex, Action<string>> rules = new Dictionary<Regex, Action<string>>()
            {
                {
                    new Regex(@"^Serial\s+:\s(.+)"),  serialno =>
                    {
                        _serialNo = serialno;
                        Console.WriteLine($"{nameof(serialno)} : {serialno}");
                    }
                },
                {
                new Regex(@"^Hardware\s+:\s(.+)"),  hardware =>
                {
                    _hardware = hardware;
                    Console.WriteLine($"{nameof(hardware)} : {hardware}");
                }
                },
                {
                new Regex(@"^processor\s+:\s(.+)"),  processor =>
                {
                    Console.WriteLine($"{nameof(processor)} : {processor}");
                }
                },
                {
                new Regex(@"^vendor_id\s+:\s(.+)"),  vendor_id =>
                {
                    Console.WriteLine($"{nameof(vendor_id)} : {vendor_id}");
                }
                },
                {
                new Regex(@"^cpu family\s+:\s(.+)"),  cpu_family =>
                {
                    Console.WriteLine($"{nameof(cpu_family)} : {cpu_family}");
                }
                },
                {
                new Regex(@"^model\s+:\s(.+)"),  model =>
                {
                    Console.WriteLine($"{nameof(model)} : {model}");
                }
                },
                {
                new Regex(@"^model name\s+:\s(.+)"),  model_name =>
                {
                    Console.WriteLine($"{nameof(model_name)} : {model_name}");
                }
                }
        };

            foreach (string cpuInfoLine in cpuInfoLines)
            {
                foreach (var rule in rules)
                {
                    if (!rule.Key.IsMatch(cpuInfoLine))
                    {
                        continue;
                    }

                    Match match = rule.Key.Match(cpuInfoLine);
                    string value = match.Groups[1].Value;
                    rule.Value?.Invoke(value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string ProcCpuInfoFilePath
        {
            get
            {
                string currentDir =
                             new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).DirectoryName;

                return System.IO.Path.Combine(currentDir, "proc_cpuinfo.log");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string Version
        {
            get
            {
                return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
}