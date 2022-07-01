using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;
using log4net;
using SomfyRtsApp;
using SomfyRtsApp.SomfyRts;

namespace SampleApp.SomfyRts
{
    public class Signalduino
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Signalduino));
        const int cDatabits = 8;
        private SerialPort mSerialPort = null;

        public void Open(string device)
        {
            mSerialPort = new SerialPort(device, 38400, Parity.None, cDatabits, StopBits.One);
            mSerialPort.Handshake = Handshake.XOnXOff;
            mSerialPort.DtrEnable = true;
            mSerialPort.ErrorReceived += MSerialPort_ErrorReceived;
            mSerialPort.PinChanged += MSerialPort_PinChanged;
            mSerialPort.Open();
            mSerialPort.Handshake = Handshake.XOnXOff;
            mSerialPort.DtrEnable = true;
            mSerialPort.ReadTimeout = 5000;
            mSerialPort.WriteTimeout = 500;

            Logger.Info($"Encoding is: {mSerialPort.Encoding}");
            Logger.Info($"Device is open: {mSerialPort.IsOpen}");
            Logger.Info($"DTR:{mSerialPort.DtrEnable} Handshake: {mSerialPort.Handshake}");
        }

        public SerialPort GetPort()
        {
            return mSerialPort;
        }

        public bool IsOpen
        {
            get
            {
                if (null == mSerialPort)
                    return false;
                return mSerialPort.IsOpen;
            }
        }

        private void MSerialPort_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            Logger.Error("MSerialPort_PinChanged: " + e.EventType);
        }

        private void MSerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Logger.Error("MSerialPort_ErrorReceived: " + e.EventType);
        }

        public void Close()
        {
            mSerialPort?.Close();
            mSerialPort = null;
        }
        public void SendCommand(string command)
        {
            Logger.Info($"Send command: '{command}'");
            mSerialPort.WriteLine(command);
            var result = mSerialPort.ReadExisting();
        }

        public void WriteWithBytes(string text)
        {
            var buffer = ToBytes(text);
            Logger.Info($"Data is: {buffer.ToHexDisplayString()}");
            mSerialPort.Write(buffer, 0, buffer.Length);
        }

        public byte[] ToBytes(string text)
        {
            var buffer = new ASCIIEncoding().GetBytes(text);
            List<byte> list = new List<byte>();
            list.Add(0x02);
            list.AddRange(buffer);
            list.Add(0x0A);
            list.Add(0x03);
            return list.ToArray();
        }

        public string Read()
        {
            try
            {
                string message = mSerialPort.ReadLine();
                Logger.Info(message);
                return message;
            }
            catch (TimeoutException) { }

            Logger.Info($"Bytes to read: {mSerialPort.BytesToRead}");
            if (mSerialPort.BytesToRead > 0)
            {
                byte[] buffer = new byte[mSerialPort.BytesToRead];
                var len = mSerialPort.Read(buffer, 0, buffer.Length);
                return new ASCIIEncoding().GetString(buffer);
            }
            return "";
        }

        public void SendSomfyFrame(SomfyRtsFrame frame, int repetition = 6)
        {
            string cmd;
            cmd = $"SC;R={repetition};SR;P0=-2560;P1=2560;P3=-640;D=10101010101010113;SM;C=645;D={frame.GetFrame(true).ToHexString()};F=10AB85550A;";
            Logger.Info($"SEND: {cmd}");
            SendCommand(cmd);
        }
    }
}
