using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SC.LAN
{
    delegate void RunControllerProcess(StationComputerEventArgs args);

    public class StationComputerEventArgs : EventArgs
    {
        public eState Response { get; set; }
        public StationComputerEventArgs(eState response)
        {
            Response = response;
        }

    }

    public delegate void OutofServiceEventHandler(object sender, StationComputerEventArgs e);
    public delegate void InServiceEventHandler(object sender, StationComputerEventArgs e);
    public delegate void OfflineEventHandler(object sender, StationComputerEventArgs e);
    public delegate void OnlineEventHandler(object sender, StationComputerEventArgs e);
    public delegate void MaintenanceEventHandler(object sender, StationComputerEventArgs e);
    public delegate void InvalidEventHandler(object sender, StationComputerEventArgs e);

    public class StationComputerBase
    {
        private Thread workerThread;

        public event OutofServiceEventHandler OutofService = null;
        public event InServiceEventHandler InService = null;
        public event OfflineEventHandler Offline = null;
        public event OnlineEventHandler Online = null;
        public event MaintenanceEventHandler Maintenance = null;
        public event InvalidEventHandler Invalid = null;

        protected void OnOutofService(StationComputerEventArgs e)
        {
            if (this.OutofService != null)
                this.OutofService(this, e);
        }

        protected void OnInService(StationComputerEventArgs e)
        {
            if (this.InService != null)
                this.InService(this,e);
        }

        protected void OnOffline(StationComputerEventArgs e)
        {
            if (this.Offline != null)
                this.Offline(this, e);
        }

        protected void OnOnline(StationComputerEventArgs e)
        {
            if (this.Online != null)
                this.Online(this, e);
        }

        protected void OnMaintenance(StationComputerEventArgs e)
        {
            if (this.Maintenance != null)
               this.Maintenance(this, e);
        }

        protected void OnInvalid(StationComputerEventArgs e)
        {
            if (this.Invalid != null)
                this.Invalid(this, e);
        }

        StationComputerClient client;
        public void Do_Work()
        {


            client = new StationComputerClient();

            if (client.Open())
            {
                while (!_shouldStop)
                {

                    if (!_shouldStop && client.AcceptClient())
                    {
                        System.Diagnostics.Debug.WriteLine("Do_Work");

                        if (client.eStateResponse == eState.OutofService)
                            OnOutofService(new StationComputerEventArgs(client.eStateResponse));
                        else if (client.eStateResponse == eState.Online)
                            OnOnline(new StationComputerEventArgs(client.eStateResponse));
                        else if (client.eStateResponse == eState.InService)
                            OnInService(new StationComputerEventArgs(client.eStateResponse));
                        else if (client.eStateResponse == eState.Offline)
                            OnOffline(new StationComputerEventArgs(client.eStateResponse));
                        else if (client.eStateResponse == eState.Maintenance)
                            OnMaintenance(new StationComputerEventArgs(client.eStateResponse));
                        else
                            OnInvalid(new StationComputerEventArgs(client.eStateResponse));


                    }
                }
            }

          
            
        }


        public void StartListeningEvents()
        {

            workerThread = new Thread(this.Do_Work);
            workerThread.Start();
            System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss") + " main thread: Starting worker thread..");

            while (!workerThread.IsAlive) ;
            Thread.Sleep(1);

        }

        public void RequestStop()
        {
            _shouldStop = true;
        }
        // Keyword volatile is used as a hint to the compiler that this data
        // member is accessed by multiple threads.
        private volatile bool _shouldStop;


       
    }
}
