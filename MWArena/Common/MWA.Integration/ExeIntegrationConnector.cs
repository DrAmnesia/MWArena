using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace MWA.Integration
{
    public class ExeIntegrationConnector : MWA.Integration.IntegrationConnector
    {
        private bool debug = false;


        public ExeIntegrationConnector()
            : base("")
        {

        }

        public ExeIntegrationConnector(string name)
            : base(name)
        {

        }
        public ExeIntegrationConnector(string name, string connectorSource)
            : base(name, connectorSource)
        {

        }


        public bool IsProcessRunning()
        {
            //here we're going to get a list of all running processes on
            //the computer
            foreach (Process clsProcess in Process.GetProcesses())
            {
                //now we're going to see if any of the running processes
                //match the currently running processes. Be sure to not
                //add the .exe to the name you provide, i.e: NOTEPAD,
                //not NOTEPAD.EXE or false is always returned even if
                //notepad is running.
                //Remember, if you have the process running more than once, 
                //say IE open 4 times the loop thr way it is now will close all 4,
                //if you want it to just close the first one it finds
                //then add a return; after the Kill
                if (clsProcess.ProcessName.Contains(this.Name))
                {
                    //if the process is found to be running then we
                    //return a true
                    return true;
                }
            }
            //otherwise we return a false
            return false;
        }


        public override void Connect()
        {

            if (!IsProcessRunning())
            {
                //AppDomain currentDomain = AppDomain.CurrentDomain;
                //currentDomain.ExecuteAssembly("logwarrior.exe");


                ProcessStartInfo stinfo = new ProcessStartInfo();

                // Assign file name

                stinfo.FileName = this.Name;

                // start the process without creating new window default is false

                stinfo.CreateNoWindow = false;

                // true to use the shell when starting the process; otherwise, the process is created directly from the executable file

                stinfo.UseShellExecute = true;



                // Creating Process class object to start process

                Process myProcess = Process.Start(stinfo);

                // start the process

                myProcess.Start();
            }
        }

        public override void Disconnect()
        {    //Shut it down
            InputManager.Current.ProcessInput(
                new KeyEventArgs(Keyboard.PrimaryDevice,
                    Keyboard.PrimaryDevice.ActiveSource,
                    0, Key.F12)
                {
                    RoutedEvent = Keyboard.KeyDownEvent
                }
                );
        }
    }
}
