using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using IIscAppLoaderInterfaces;
using log4net;
using SampleApp.SomfyRts;
using SomfyRtsApp.SomfyRts;

namespace SomfyRtsApp
{
    public class IscApp : IIscApp
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(IscApp));

        public static IIscAppConnector AppHost;

        private SomfyRtsController rtsController;

        void IIscApp.Init(IIscAppConnector iscAppHost)
        {
            // Put code to initialize stuff managed by this app here.
            // No KNX communication is allowed!
            Logger.Info("Initializing SampleApp.");

            AppHost = iscAppHost;
            rtsController = SomfyRtsController.CreateFromFile();
            Logger.Info("Initializing done.");
        }

        void IIscApp.Run()
        {
            // Put code to get app running here.
            // If values on the KNX bus should be initialized, do this right at the beginning.
            if (rtsController.Devices.Count == 0)
            {
                for (uint i = 1; i <= 10; i++)
                {
                    rtsController.AddDevice("Somfy_" + i, (uint)AppHost.GetNumberParameter(i));
                }

                rtsController.Save();
            }

            // Check Serial port and log it's name if present
            CheckSerialPort();

            Logger.Info("SomfyRtsApp running.");
        }

        void IIscApp.Exit()
        {
            // Put code here that needs to be done to shut-down this app.
            rtsController.Save();
            Logger.Info("SomfyRtsApp exiting.");
        }

        object IIscApp.GetValue(uint coId)
        {
            //This method will only be called in a future version of Programmable, but is already part of the interface.
            //You should currently implement it by throwing NotImplementedException:
            throw new NotImplementedException();
        }

        /// <summary>
        ///   Called by the host when an interesting value changed on KNX.
        /// </summary>
        /// <param name="coId">The app-specific type (from 1 to 64) of the event</param>
        /// <param name="value">The current value from the KNX event. Its type depends on the KNX type of the value.</param>
        void IIscApp.OnValueReceived(uint coId, object value)
        {
            Logger.Info(string.Format("Received new value for CO {0}: {1}", coId, value));
            if (value is bool coValue)
            {      // Put code here to react to a new value of a CO.
                if (coIdMap.Contains(coId))
                {
                    var command = coIdMap.Get(coId);
                    var device = rtsController.Devices[command.Item1 - 1];

                    if (device.Address != (uint)AppHost.GetNumberParameter((uint)command.Item1))
                    {
                        device.Address = (uint)AppHost.GetNumberParameter((uint)command.Item1);
                    }

                    switch (command.Item2)
                    {
                        case SomfyRtsButton.UpDown: 
                            if (coValue)
                            {
                                rtsController.SendCommand(device.Name, SomfyRtsButton.Down);
                            }
                            else
                            {
                                rtsController.SendCommand(device.Name, SomfyRtsButton.Up);
                            }
                            break;
                        default:
                            rtsController.SendCommand(device.Name, command.Item2);
                            break;
                    }
                }
            }


            if (coId == 31)
            {
                if (!rtsController.IsOpen)
                    rtsController.Open();
                rtsController.GetPort().WriteLine(value.ToString());
                var response = rtsController.GetPort().ReadExisting();
                foreach (var r in Split(response, 14))
                {
                    AppHost.WriteValue(coId, r);
                }
            }
        }
        static IEnumerable<string> Split(string str, int chunkSize)
        {
            return Enumerable.Range(0, str.Length / chunkSize)
                .Select(i => str.Substring(i * chunkSize, chunkSize));
        }

        private void CheckSerialPort()
        {
            var isSerialPortPresent = AppHost.IsSerialPortPresent;
            if (isSerialPortPresent == false)
            {
                Logger.Debug("No serial port");
                return;
            }

            // Get the name of the serial port
            var serialPortName = AppHost.SerialPortName;

            // Getting the serial port name and check if it exists
            if (serialPortName == string.Empty)
            {
                // We are sure that a serial port is connected and if we can not find it,
                // it is necessary to reset the usb-subsystem from the linux system to restore the connection
                AppHost.ResetUsbConfiguration();
                // Wait for the USB-subssytem to reinitialize and restore the connection
                Thread.Sleep(TimeSpan.FromSeconds(2.5));
                if (AppHost.SerialPortName == string.Empty)
                {
                    Logger.Error("Unable to find serial device after reset of the USB subsystem");
                    return;
                }
            }

            Logger.Debug("PortName: " + serialPortName);
        }
    }
}