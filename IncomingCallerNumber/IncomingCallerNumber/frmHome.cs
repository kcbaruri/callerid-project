using System;
using System.IO.Ports;
using System.Windows.Forms;

namespace IncomingCallerNumber
{
    public partial class frmHome : Form
    {
        private string phoneNumber = "There is no call";

        public frmHome()
        {
            InitializeComponent();
        }

        #region "User defined function"

        public void EnablePort()
        {
            if (!serialPort.IsOpen)
                serialPort.Open();
        }
      
        public void DisablePort()
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        #endregion

        #region "Event handler"

        private void frmHome_Load(object sender, System.EventArgs e)
        {
            DisablePort();
            serialPort.PortName = CommonConstant.ComportName;
            serialPort.BaudRate = Convert.ToInt32(CommonConstant.baudRate);
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.None;
            serialPort.ReadTimeout = 0;

            EnablePort(); // Enable port before writing command.
            string command = CommonConstant.portCommand + "\r\n";
            serialPort.Write(command); // Ready to receive call.
            serialPort.DataReceived += SerialPort_DataReceived;
            tmrCaller.Start();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string dataFromPort = serialPort.ReadExisting();
            string[] callerData = new string[0];
            callerData = dataFromPort.Split('=');
            string[] retrievedNumber = new string[0];
            if (callerData.Length > 3)
            {
                retrievedNumber = callerData[4].ToString().Split('\r');
                phoneNumber = Convert.ToString(retrievedNumber[0]);
            }
        }

        private void tmrCaller_Tick(object sender, EventArgs e)
        {
            if ((phoneNumber.Replace(" ", "").ToUpper() == "There is no call".Replace(" ", "").ToUpper()) || (phoneNumber == String.Empty)) //If there is no call
            {
                txtPhoneNumber.Text = "There is no call in progress.";
            }
            else
            {
                txtPhoneNumber.Text = phoneNumber;
            }
        }

        #endregion
    }
}
