using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace SpectrumInformationDistribution
{
    public partial class MainForm : Form
    {
        ProcessMusic processMusic;
        Timer tim;
        int interval = 40;
        bool isSend = true;

        public MainForm()
        {
            InitializeComponent();

            SyncButtonsState();

            processMusic = new ProcessMusic();
            processMusic.Init();

            tim = new Timer();
            tim.Interval = interval;
            tim.Tick += Tim_Tick;
            tim.Start();

        }

        private void SyncButtonsState()
        {
            button_startbroadcast.Enabled = !isSend;
            button_broadband.Enabled = isSend;
        }

        private void Tim_Tick(object sender, EventArgs e)
        {
            if (!isSend)
            {
                return;
            }

            processMusic.OnUpdate();

            SendData sendat = new SendData() { samples = processMusic.samples };
            string json_send = JsonConvert.SerializeObject(sendat);
            UDPSender.Instance.Send(json_send);
        }

        private void button_startbroadcast_Click(object sender, EventArgs e)
        {
            isSend = true;
            SyncButtonsState();
        }

        private void button_broadband_Click(object sender, EventArgs e)
        {
            isSend = false;
            SyncButtonsState();
        }
    }
}
