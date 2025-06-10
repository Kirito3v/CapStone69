using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ELECTROVIA_AI
{
    public class PyTimeService
    {
        private Process? ProcessCLI { get; set; }
        private string _lastMessage = "";
        public static bool Wait = false;

        public string LastMessage 
        {
            get 
            {
                _logger.LogInformation("Data requested");
                return _lastMessage;
            }

            private set => _lastMessage = value; 

        }

        public readonly Logger<PyTimeService> _logger;
        public  ManualResetEvent waitHandle = new ManualResetEvent(false);


        bool isReady = true;
        public PyTimeService(Logger<PyTimeService> logger)
        {
            _logger = logger;

            string? exeFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            string? pythonFilePath = Path.Combine(exeFolder??"","python/main.py");

            string? PatFIle = Path.Combine(exeFolder??"","python/pyrunCPU.bat");

            if (pythonFilePath == null)
                throw new Exception("Python not found");


            Init(PatFIle, pythonFilePath);

            PyMeth("init_Model");
            PyMeth("datasets\\AllInOne\\train");
            PyMeth("AllInOne");

            Wait = true;
            waituntilProcessEnd().Wait();

        }
        public void Init(string PatFile,string PythonFilePath)
        {

            _logger.LogInformation($"Starting Pat file {PatFile} which will run {PythonFilePath}");



            ProcessCLI = PyTools.StartPythonProgram(PatFile,PythonFilePath);

            ProcessCLI.EnableRaisingEvents = true;

            ProcessCLI.OutputDataReceived += DataReciced;

            ProcessCLI.ErrorDataReceived += errorDataRecived;

            ProcessCLI.BeginOutputReadLine();
            
            ProcessCLI.BeginErrorReadLine();

            ProcessCLI.Exited += (sender, e) =>
            {
                _logger.LogCritical("Python process exited.");
                ProcessCLI?.Dispose();
                ProcessCLI = null;
                isReady = false;
                Wait = false;
            };



        }


        public void DataReciced (object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                _logger.LogInformation("pyTime: " + e.Data);

                LastMessage = e.Data;

                if (e.Data == "ready")
                {
                    SearchWithImageService.Wait = false;

                }
                else if (e.Data == "loaded")
                {
                    Wait = false;
                    isReady = true;
                }
            }
           
          
        }

        public void errorDataRecived(object sender, DataReceivedEventArgs e) 
        {
            _logger.LogWarning("pyTime: "+e.Data);
        }
        public void PyMeth(string methName)
        {
            if (ProcessCLI != null && isReady)
                ProcessCLI.StandardInput.WriteLine(methName);
            else
            {
                throw new Exception("Python not ready");
            }
        }

        public async Task waituntilProcessEnd()
        {
            while (Wait)
            {
                await Task.Delay(10);
            }
        }
    }
}
