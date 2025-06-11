using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELECTROVIA_AI
{
    internal static class PyTools
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Interoperability", "CA1416:Validate platform compatibility", Justification = "<Pending>")]
        internal static string? GetPythonEnvPath()
        {
            string pythonPath = "";

            string? pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (pathEnv != null)
            {
                foreach (string path in pathEnv.Split(';'))
                {
                    if (path.Contains("Python") && System.IO.File.Exists(System.IO.Path.Combine(path, "python.exe")))
                    {
                        pythonPath = System.IO.Path.Combine(path, "python.exe");
                        break;
                    }
                }
            }

            if (pythonPath == null)
            {
                try
                {

                    using (RegistryKey? key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Python\PythonCore"))
                    {
                        if (key != null)
                        {
                            foreach (string subkeyName in key.GetSubKeyNames())
                            {
                                using RegistryKey? subkey = key.OpenSubKey(subkeyName + @"\InstallPath");
                                if (subkey != null)
                                {
                                    string? installPath = subkey.GetValue(null) as string;
                                    if (installPath != null && System.IO.File.Exists(System.IO.Path.Combine(installPath, "python.exe")))
                                    {
                                        pythonPath = System.IO.Path.Combine(installPath, "python.exe");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }

            return pythonPath;
        }
        internal static Process StartPythonProgram(string FileName,string Path)
        {
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = $"\"{FileName}\"", 
                Arguments = $"\"{Path}\"", 
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
            };
            Process? process = Process.Start(psi);
            
            if(process != null)
            {
                return process;
            }
            else
            {
                throw new Exception("Can't start Process!");
            }
            
        }
        private static bool IsWarning(string line)
        {
            return line.Contains("Warning") || line.Contains("DeprecationWarning");
        }
    }
}

