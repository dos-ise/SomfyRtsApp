using System;
using System.IO;
using System.Xml.Serialization;
using SomfyRtsApp;
using SomfyRtsApp.SomfyRts;

namespace SampleApp.SomfyRts
{
  public class SomfyRtsDevice
  {
    public UInt16 RollingCode { get; set; } = 1;
    public UInt32 Address { get; set; }
    public byte EncryptionKey { get; set; } = 0xA1;
    public string Name { get; set; }

    public SomfyRtsFrame CreateFrame(SomfyRtsButton button)
    {
      SomfyRtsFrame frame = new SomfyRtsFrame()
      {
        Address = Address,
        Command = button,
        EncryptionKey = EncryptionKey,
        RollingCode = RollingCode
      };
      //Update Keys
      RollingCode += 1;
      //EncryptionKey += 1;
      //if (EncryptionKey > 0xAF)
      //  EncryptionKey = 0xA0;
      return frame;
    }
  }
}
