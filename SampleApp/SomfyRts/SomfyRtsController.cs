using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using log4net;
using SampleApp.SomfyRts;

namespace SomfyRtsApp.SomfyRts
{
    public class SomfyRtsController
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SomfyRtsController));
        protected const int cKeepAliveTime = 60;//[s]
        protected bool mRunKeepAlive = false;
        public List<SomfyRtsDevice> Devices { get; set; } = new List<SomfyRtsDevice>();

        public string SignalDuinoAddress = IscApp.AppHost.SerialPortName;

        public readonly Signalduino mSignalduino = new Signalduino();

        public void AddDevice(string name, uint address)
        {
            Devices.Add(new SomfyRtsDevice() { Name = name, Address = address });
        }

        public SerialPort GetPort()
        {
            return mSignalduino.GetPort();
        }

        public bool IsOpen => mSignalduino.IsOpen;

        public static SomfyRtsController CreateFromFile()
        {
            string path = Path.Combine(IscApp.AppHost.SdCardPath, $"{nameof(SomfyRtsController)}.artset");
            if (File.Exists(path))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SomfyRtsController));
                using (StreamReader r = new StreamReader(path))
                {
                    var s = (SomfyRtsController)serializer.Deserialize(r);
                    s.RunKeepAlive();
                    Logger.Info($"Restored SomfyRtsController from " + path);
                    return s;
                }
            }
            else
            {
                Logger.Info($"New SomfyRtsController created");
                return new SomfyRtsController();
            }
        }
        public void SendCommand(string device, SomfyRtsButton command, int repetition = 6)
        {
            if (!mSignalduino.IsOpen)
                mSignalduino.Open(SignalDuinoAddress);

            var dev = Devices.FirstOrDefault(d => d.Name.Equals(device, StringComparison.OrdinalIgnoreCase));
            if (dev != null)
            {
                var frame = dev.CreateFrame(command);
                Logger.Info($"Send command: {command} to device {dev.Name}");
                mSignalduino.SendSomfyFrame(frame, repetition);
                Save();
            }
        }

        public void RunKeepAlive()
        {
            mRunKeepAlive = true;
            Task.Factory.StartNew(RunKeepAliveInternal);
        }

        public void StopKeepAlive()
        {
            mRunKeepAlive = false;
        }

        private void RunKeepAliveInternal()
        {
            do
            {
                Thread.Sleep(cKeepAliveTime * 1000);
                if (!mSignalduino.IsOpen)
                    mSignalduino.Open(SignalDuinoAddress);
                mSignalduino.SendCommand("P");
            } while (mRunKeepAlive);
        }

        public void Open()
        {
            Logger.Info("Open connection");
            mSignalduino.Open(SignalDuinoAddress);
        }

        public void Close()
        {

            Logger.Info("Close connection");
            mSignalduino.Close();
        }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SomfyRtsController));
            string path = Path.Combine(IscApp.AppHost.SdCardPath, $"{nameof(SomfyRtsController)}.artset");
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            using (StreamWriter w = new StreamWriter(path))
            {
                serializer.Serialize(w, this);
            }
        }
    }
}
