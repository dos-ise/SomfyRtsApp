using System;
using IIscAppLoaderInterfaces;
using DayOfWeek = IIscAppLoaderInterfaces.DayOfWeek;

namespace SampleApp
{
  /// <summary>
  ///   Writes on selected data points types to give an example how to write on the bus.
  /// </summary>
  internal class DataPointsUsageExample
  {
    private readonly IIscAppConnector _appHost;

    public DataPointsUsageExample(IIscAppConnector appHost)
    {
      _appHost = appHost;
    }

    public void WriteOneBit()
    {
      // Write on a 1 bit CO
      _appHost.WriteValue(31, true);
    }

    public void WriteTwoBits()
    {
      // Write on a 2 bits CO using the class DptControl to avoid sending invalid values.
      _appHost.WriteValue(32, new DptControl(true, false));
    }

    public void WriteFourBits()
    {
      // Write on a 4 bits control dimming CO.
      _appHost.WriteValue(33, new DptControlDimmingBlinds(true, 0x01));
    }

    public void WriteOneByte()
    {
      // Write on a 1 byte scaling CO 
      _appHost.WriteValue(34, (byte)123);

      // Write on a 1 byte scene number CO 
      _appHost.WriteValue(35, 45);

      // Write on a 1 byte scene control CO using the class DptSceneControl to avoid sending invalid values.
      _appHost.WriteValue(36, new DptSceneControl(false, 63));

      // Write on a 1 byte scene info CO using the class DptSceneInfo to avoid sending invalid values.
      _appHost.WriteValue(37, new DptSceneInfo(false, 2));
    }

    public void WriteTwoBytes()
    {
      // Write on 2 bytes brightness CO
      _appHost.WriteValue(38, 0xaffe);

      // Write on 2 bytes Lux CO
      _appHost.WriteValue(39, (short)122);

      // Write on 2 bytes temperature CO
      _appHost.WriteValue(40, (float)23.3);
    }

    public void WriteThreeBytes()
    {
      // Write on 3 bytes time CO
      _appHost.WriteValue(41, new DptTimeOfDay(10, 02, 45, DayOfWeek.Thursday));

      // Write on 3 bytes date CO
      _appHost.WriteValue(42, new DateTime(2015, 6, 1));

      // Write on 3 bytes colour RGB CO using the class DptColourRGB to avoid sending invalid values.
      _appHost.WriteValue(47, new DptColourRGB(0x12, 0x34, 0x56));
    }

    public void WriteFourBytes()
    {
      // Write on 4 bytes 4 count CO
      _appHost.WriteValue(43, 330);

      // Write on 4 bytes accelerator angular CO
      _appHost.WriteValue(44, (uint)2456);

      // Write on 4 bytes energy CO
      _appHost.WriteValue(45, (uint)2244);
    }

    public void WriteFourteenBytes()
    {
      // Write on 14 bytes string CO
      _appHost.WriteValue(46, "Hallo");
    }

    public void WriteExceptions()
    {
      try
      {
        const int maxSupportedCo = 64;
        _appHost.WriteValue(maxSupportedCo + 1, 0);
      }
      catch (ArgumentOutOfRangeException)
      {
        //COs can be only in the range of 1 to 64.
      }

      try
      {
        _appHost.WriteValue(46, null);
      }
      catch (ArgumentNullException)
      {
        //null is not a valid value.
      }
    }
  }
}