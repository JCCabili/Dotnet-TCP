using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SC.LAN;

namespace SC
{
    public partial class Form1 : Form
    {

        private StationComputerClient SCClient = null;
        private StationComputerClient SenderClient = null;

        

        public Form1()
        {
            InitializeComponent();
            LoadInit();
        }

        public void LoadInit()
        {
            SenderClient = new StationComputerClient();
            SCClient = new StationComputerClient();

            SCClient.Online += new OnlineEventHandler(SenderClient_Online);
            SCClient.OutofService += new OutofServiceEventHandler(SenderClient_OutofService);
            SCClient.Offline += new OfflineEventHandler(SenderClient_Offline);
            SCClient.InService += new InServiceEventHandler(SCClient_InService);
            SCClient.Maintenance += new MaintenanceEventHandler(SCClient_Maintenance);
            SCClient.Invalid += new InvalidEventHandler(SCClient_Invalid);
        }

        private void SCClient_Invalid(object sender, StationComputerEventArgs e)
        {
            
        }

        private void SCClient_Maintenance(object sender, StationComputerEventArgs args)
        {
            try
            {
                this.BeginInvoke(new RunControllerProcess(DisplayResponse), args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SCClient_InService(object sender, StationComputerEventArgs args)
        {
            try
            {
                this.BeginInvoke(new RunControllerProcess(DisplayResponse), args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SenderClient_Offline(object sender, StationComputerEventArgs args)
        {
            try
            {
                this.BeginInvoke(new RunControllerProcess(DisplayResponse), args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SenderClient_OutofService(object sender, StationComputerEventArgs args)
        {
            try
            {
                this.BeginInvoke(new RunControllerProcess(DisplayResponse), args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void SenderClient_Online(object sender, StationComputerEventArgs args)
        {
            try
            {
                this.BeginInvoke(new RunControllerProcess(DisplayResponse), args);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }


        public void DisplayResponse(StationComputerEventArgs args)
        {
            txtRecieve.Text = string.Format("{1} \n {0}", txtRecieve.Text, args.Response.ToString());
        }


        public void LoadSC()
        {
            SCClient.StartListeningEvents();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SCClient.RequestStop();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            SenderClient.SendCommand(textBox2.Text, Constants.OUTOFSERVICE);
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
            Global.IPaddress = textBox1.Text;
            LoadSC();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SenderClient.SendCommand(textBox2.Text, Constants.ONLINE);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SenderClient.SendCommand(textBox2.Text, Constants.OFFLINE);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SenderClient.SendCommand(textBox2.Text, Constants.MAINTENANCE);
        }
    }
}
