// Decompiled with JetBrains decompiler
// Type: MStarBinToolGUI.ComFunc
// Assembly: MStarBinToolGUI, Version=2.5.0.2, Culture=neutral, PublicKeyToken=null
// MVID: F90E615F-21EC-4258-B3C9-F0CFF9D3BC59
// Assembly location: C:\Anatoly\study\MStarBin\MStarBinTool-GUI_25\MStarBinToolGUI.exe

using MStarBinToolGUI.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace MStarBinToolGUI
{
  public static class ComFunc
  {
    [DllImport("ntdll.dll", SetLastError = true)]
    private static extern uint RtlComputeCrc32([In] uint InitialCrc, [In] byte[] Buffer, [In] int Length);

    public static byte[] GetCrc32(string src_File, long src_Offset, long src_Length, bool rev_Bytes = false)
    {
      try
      {
        if (src_Length == 0L)
          return new byte[4];
        byte[] numArray = new byte[Settings.IoBufferSize];
        uint InitialCrc = 0;
        using (FileStream fileStream = new FileStream(src_File, FileMode.Open, FileAccess.Read, FileShare.None, Settings.IoBufferSize, FileOptions.SequentialScan))
        {
          fileStream.Position = src_Offset;
          while (src_Length > (long) Settings.IoBufferSize)
          {
            fileStream.Read(numArray, 0, Settings.IoBufferSize);
            InitialCrc = ComFunc.RtlComputeCrc32(InitialCrc, numArray, Settings.IoBufferSize);
            src_Length -= (long) Settings.IoBufferSize;
          }
          fileStream.Read(numArray, 0, (int) src_Length);
          InitialCrc = ComFunc.RtlComputeCrc32(InitialCrc, numArray, (int) src_Length);
        }
        byte[] bytes = BitConverter.GetBytes((int) InitialCrc);
        if (rev_Bytes)
          Array.Reverse((Array) bytes);
        return bytes;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static byte[] GetCrc32(ref byte[] src_Bytes, int src_Offset, int src_Length, bool rev_Bytes = false)
    {
      try
      {
        if (src_Bytes == null || src_Bytes.Length == 0)
          return new byte[4];
        uint InitialCrc = 0;
        byte[] Buffer = new byte[Settings.IoBufferSize];
        while (src_Length > Settings.IoBufferSize)
        {
          Buffer.BlockCopy((Array) src_Bytes, src_Offset, (Array) Buffer, 0, Settings.IoBufferSize);
          InitialCrc = ComFunc.RtlComputeCrc32(InitialCrc, Buffer, Settings.IoBufferSize);
          src_Offset += Settings.IoBufferSize;
          src_Length -= Settings.IoBufferSize;
        }
        Buffer.BlockCopy((Array) src_Bytes, src_Offset, (Array) Buffer, 0, src_Length);
        byte[] bytes = BitConverter.GetBytes((int) ComFunc.RtlComputeCrc32(InitialCrc, Buffer, src_Length));
        if (rev_Bytes)
          Array.Reverse((Array) bytes);
        return bytes;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static byte[] ReadBytes(string src_File, long src_Offset, int src_Length)
    {
      try
      {
        byte[] buffer = new byte[src_Length];
        int offset = 0;
        using (FileStream fileStream = new FileStream(src_File, FileMode.Open, FileAccess.Read, FileShare.None, Settings.IoBufferSize, FileOptions.SequentialScan))
        {
          fileStream.Position = src_Offset;
          while (src_Length > Settings.IoBufferSize)
          {
            offset += fileStream.Read(buffer, offset, Settings.IoBufferSize);
            src_Length -= Settings.IoBufferSize;
          }
          fileStream.Read(buffer, offset, src_Length);
        }
        return buffer;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void CopyBytes(string src_File, long src_Offset, long src_Length, string dst_File)
    {
      try
      {
        byte[] buffer = new byte[Settings.IoBufferSize];
        using (FileStream fileStream1 = new FileStream(src_File, FileMode.Open, FileAccess.Read, FileShare.None, Settings.IoBufferSize, FileOptions.SequentialScan))
        {
          using (FileStream fileStream2 = new FileStream(dst_File, FileMode.Append, FileAccess.Write, FileShare.None, Settings.IoBufferSize, FileOptions.SequentialScan))
          {
            fileStream1.Position = src_Offset;
            while (src_Length > (long) Settings.IoBufferSize)
            {
              fileStream1.Read(buffer, 0, Settings.IoBufferSize);
              fileStream2.Write(buffer, 0, Settings.IoBufferSize);
              src_Length -= (long) Settings.IoBufferSize;
            }
            fileStream1.Read(buffer, 0, (int) src_Length);
            fileStream2.Write(buffer, 0, (int) src_Length);
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void WriteBytes(byte[] src_Bytes, int src_Offset, int src_Length, string dst_File)
    {
      try
      {
        using (FileStream fileStream = new FileStream(dst_File, FileMode.Append, FileAccess.Write, FileShare.None, Settings.IoBufferSize, FileOptions.SequentialScan))
          fileStream.Write(src_Bytes, src_Offset, src_Length);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void ReWriteBytes(byte[] src_Bytes, string dst_File, long dst_Offset, bool rev_Bytes = false)
    {
      try
      {
        using (FileStream fileStream = new FileStream(dst_File, FileMode.Open, FileAccess.Write, FileShare.None, Settings.IoBufferSize, FileOptions.SequentialScan))
        {
          fileStream.Position = dst_Offset;
          if (rev_Bytes)
            Array.Reverse((Array) src_Bytes);
          fileStream.Write(src_Bytes, 0, src_Bytes.Length);
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void LoadSettings()
    {
      try
      {
        if (!File.Exists(Settings.AppSettingsFile))
          return;
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(Settings.AppSettingsFile);
        XmlNode xmlNode = xmlDocument.SelectSingleNode("MStarBinToolGUI");
        if (xmlNode == null)
          return;
        foreach (XmlNode childNode in xmlNode.ChildNodes)
        {
          string str = childNode.InnerText.Trim();
          if (str == string.Empty)
            throw new Exception("Value cannot be empty: " + childNode.Name);
          string name = childNode.Name;
          if (!(name == "AppLang"))
          {
            if (!(name == "ImgCnkSize"))
            {
              if (!(name == "RamLoadMaxSize"))
              {
                if (name == "IoBufferSize")
                {
                  Settings.IoBufferSize = Convert.ToInt32(str);
                  if (!Settings.IoBufferSizeList.Contains(Settings.IoBufferSize))
                    throw new Exception("Value out of range: " + childNode.Name);
                  Settings.IoBufferSize *= 1024;
                }
              }
              else
              {
                Settings.RamLoadMaxSize = Convert.ToInt32(str);
                if (!Settings.RamLoadMaxSizeList.Contains(Settings.RamLoadMaxSize))
                  throw new Exception("Value out of range: " + childNode.Name);
                Settings.RamLoadMaxSize *= 1048576;
              }
            }
            else
            {
              Settings.ImgCnkSize = Convert.ToInt32(str);
              if (!Settings.ImgCnkSizeList.Contains(Settings.ImgCnkSize))
                throw new Exception("Value out of range: " + childNode.Name);
              Settings.ImgCnkSize *= 1048576;
            }
          }
          else
            Settings.AppLang = str;
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void SaveSettings()
    {
      try
      {
        if (File.Exists(Settings.AppSettingsFile))
          File.Delete(Settings.AppSettingsFile);
        File.WriteAllText(Settings.AppSettingsFile, string.Join(Environment.NewLine, "<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "<MStarBinToolGUI>", "\t<AppLang>" + Settings.AppLang + "</AppLang>", string.Format("\t<ImgCnkSize>{0}</ImgCnkSize>", (object) (Settings.ImgCnkSize / 1048576)), string.Format("\t<RamLoadMaxSize>{0}</RamLoadMaxSize>", (object) (Settings.RamLoadMaxSize / 1048576)), string.Format("\t<IoBufferSize>{0}</IoBufferSize>", (object) (Settings.IoBufferSize / 1024)), "</MStarBinToolGUI>"), Encoding.UTF8);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static string GetLngStr(string str_Name)
    {
      try
      {
        return object.Equals((object) Settings.AppLang, (object) Settings.AppLangList[1]) ? Language.RusStrings[str_Name] : Language.EngStrings[str_Name];
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static string GetStrSizeSfx(long src_Size, bool out_Ceiling = false)
    {
      if (src_Size == 0L)
        return ComFunc.GetLngStr("sts_unknown");
      string[] strArray = new string[4]
      {
        "sts_b",
        "sts_kb",
        "sts_mb",
        "sts_gb"
      };
      int index = 0;
      Decimal d = (Decimal) src_Size;
      if (out_Ceiling)
      {
        while (Math.Round(d, 0) >= new Decimal(1024))
        {
          d /= new Decimal(1024);
          ++index;
        }
        return string.Format("{0} {1}", (object) Math.Ceiling(d), (object) ComFunc.GetLngStr(strArray[index]));
      }
      while (Math.Round(d, 2) >= new Decimal(1024))
      {
        d /= new Decimal(1024);
        ++index;
      }
      return string.Format("{0:0.##} {1}", (object) d, (object) ComFunc.GetLngStr(strArray[index]));
    }

    public static void AlignFile(string src_File)
    {
      try
      {
        long length1 = new FileInfo(src_File).Length;
        if (length1 == 0L)
          throw new Exception(ComFunc.GetLngStr("msg_file_is_empty") + " " + Path.GetFileName(src_File));
        if (length1 % 4096L == 0L)
          return;
        int length2 = 4096 - (int) (length1 % 4096L);
        byte[] src_Bytes = new byte[length2];
        for (int index = 0; index < length2; ++index)
          src_Bytes[index] = byte.MaxValue;
        ComFunc.WriteBytes(src_Bytes, 0, src_Bytes.Length, src_File);
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static ComClasses.MbootInfo GetMbootInfo(string src_File, long src_Offset, int src_Length)
    {
      try
      {
        ComClasses.MbootInfo mbootInfo = new ComClasses.MbootInfo();
        byte[] inArray = ComFunc.ReadBytes(src_File, src_Offset + 1202176L, src_Length - 1202176);
        if (inArray == null || inArray.Length < 1100)
          return mbootInfo;
        int num = 0;
        int index1 = inArray.Length - 1100 - 1;
        while (index1 >= 0 && ((int) inArray[index1] != 83 || (int) inArray[index1 + 1] != 69 || ((int) inArray[index1 + 2] != 67 || (int) inArray[index1 + 3] != 85)))
          --index1;
        for (int index2 = inArray.Length - 21; index2 >= 0; --index2)
        {
          if ((int) inArray[index2] == 77 && (int) inArray[index2 + 1] == 115 && ((int) inArray[index2 + 2] == 116 && (int) inArray[index2 + 3] == 97))
          {
            num = index2;
            break;
          }
        }
        mbootInfo.AesBootKey = Convert.ToBase64String(inArray, num - 32, 16);
        mbootInfo.AesUpgradeKey = Convert.ToBase64String(inArray, num - 16, 16);
        return mbootInfo;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static ComClasses.MpoolInfo GetMpoolInfo(string src_File, long src_Offset)
    {
      try
      {
        ComClasses.MpoolInfo mpoolInfo = new ComClasses.MpoolInfo();
        byte[] bytes1 = ComFunc.ReadBytes(src_File, src_Offset + 1966084L, 16384);
        int index1 = 0;
        for (int index2 = 0; index2 < 16383; ++index2)
        {
          if ((int) bytes1[index2] == 0)
          {
            mpoolInfo.EnvList1.Add(Encoding.UTF8.GetString(bytes1, index1, index2 - index1));
            if ((int) bytes1[index2 + 1] != 0)
              index1 = index2 + 1;
            else
              break;
          }
        }
        byte[] bytes2 = ComFunc.ReadBytes(src_File, src_Offset + 2031620L, 16384);
        int index3 = 0;
        for (int index2 = 0; index2 < 16383; ++index2)
        {
          if ((int) bytes2[index2] == 0)
          {
            mpoolInfo.EnvList2.Add(Encoding.UTF8.GetString(bytes2, index3, index2 - index3));
            if ((int) bytes2[index2 + 1] != 0)
              index3 = index2 + 1;
            else
              break;
          }
        }
        return mpoolInfo;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static ComClasses.TvConfigInfo GetTvConfigInfo(string src_File, long src_Offset)
    {
      try
      {
        ComClasses.TvConfigInfo tvConfigInfo = new ComClasses.TvConfigInfo();
        string str1 = Settings.UserTempDir + "\\tvconfig.img";
        string path1 = Settings.UserTempDir + "\\Customer_1.ini";
        string path2 = Settings.UserTempDir + "\\mmap.ini";
        if (File.Exists(str1))
          File.Delete(str1);
        if (File.Exists(path1))
          File.Delete(path1);
        if (File.Exists(path2))
          File.Delete(path2);
        ComFunc.CopyBytes(src_File, src_Offset, 15728640L, str1);
        ComFunc.SevZipExtract(str1, new string[2]
        {
          "config/model/Customer_1.ini",
          "config/mmap.ini"
        }, Settings.UserTempDir);
        string[] strArray1 = File.ReadAllLines(path1);
        for (int index = 0; index < strArray1.Length; ++index)
        {
          if (strArray1[index].Contains("m_pPanelName"))
          {
            strArray1[index] = Regex.Replace(strArray1[index], "(.*=)", string.Empty).Trim();
            strArray1[index] = Regex.Replace(strArray1[index], "(/.*config/panel/)", string.Empty);
            strArray1[index] = strArray1[index].Replace(";", string.Empty).Replace("\"", string.Empty);
            if (strArray1[index].EndsWith("ini"))
              strArray1[index] = strArray1[index].Remove(strArray1[index].Length - 4, 4);
            tvConfigInfo.PanelName = strArray1[index];
            break;
          }
        }
        string[] strArray2 = File.ReadAllLines(path2);
        for (int index = 0; index < strArray2.Length; ++index)
        {
          if (strArray2[index].Contains("MIU_DRAM_LEN") && !strArray2[index].Contains("MIU_DRAM_LEN0") && (!strArray2[index].Contains("MIU_DRAM_LEN1") && !strArray2[index].Contains("MIU_DRAM_LEN2")))
          {
            string str2 = Regex.Replace(strArray2[index], "(.*MIU_DRAM_LEN)", string.Empty).Trim();
            if (str2.ToLower().StartsWith("0x"))
            {
              tvConfigInfo.RamSize = (long) Convert.ToUInt64(str2.Remove(0, 2), 16);
              break;
            }
          }
        }
        File.Delete(str1);
        File.Delete(path1);
        File.Delete(path2);
        return tvConfigInfo;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static bool IsAndBootImg(string src_File, long src_Offset = 0)
    {
      byte[] numArray = ComFunc.ReadBytes(src_File, src_Offset, 4);
      if ((int) numArray[0] == 65 && (int) numArray[1] == 78 && (int) numArray[2] == 68)
        return (int) numArray[3] == 82;
      return false;
    }

    public static bool IsAndBootImg(ref byte[] src_Bytes)
    {
      if ((int) src_Bytes[0] == 65 && (int) src_Bytes[1] == 78 && (int) src_Bytes[2] == 68)
        return (int) src_Bytes[3] == 82;
      return false;
    }

    public static bool IsUbootImg(string src_File, long src_Offset = 0)
    {
      byte[] numArray = ComFunc.ReadBytes(src_File, src_Offset, 4);
      if ((int) numArray[0] == 39 && (int) numArray[1] == 5 && (int) numArray[2] == 25)
        return (int) numArray[3] == 86;
      return false;
    }

    public static bool IsUbootImg(ref byte[] src_Bytes)
    {
      if ((int) src_Bytes[0] == 39 && (int) src_Bytes[1] == 5 && (int) src_Bytes[2] == 25)
        return (int) src_Bytes[3] == 86;
      return false;
    }

    public static byte[] AesEncDecBytes(byte[] src_Bytes, string src_AesKey, int crypt_Mode = 0)
    {
      try
      {
        if (src_Bytes.Length % 16 != 0)
        {
          int num = 16 - src_Bytes.Length % 16;
          Array.Resize<byte>(ref src_Bytes, src_Bytes.Length + num);
          for (int index = src_Bytes.Length - num - 1; index < src_Bytes.Length; ++index)
            src_Bytes[index] = byte.MaxValue;
        }
        using (Aes aes = Aes.Create())
        {
          aes.KeySize = 128;
          aes.BlockSize = 128;
          aes.Mode = CipherMode.ECB;
          aes.Padding = PaddingMode.None;
          byte[] iv = aes.IV;
          ICryptoTransform transform = crypt_Mode == 0 ? aes.CreateEncryptor(Convert.FromBase64String(src_AesKey), iv) : aes.CreateDecryptor(Convert.FromBase64String(src_AesKey), iv);
          using (MemoryStream memoryStream = new MemoryStream())
          {
            using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, transform, CryptoStreamMode.Write))
              cryptoStream.Write(src_Bytes, 0, src_Bytes.Length);
            return memoryStream.ToArray();
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static byte[] RsaSignBytes(ref byte[] src_Bytes, string src_RsaPrivKey)
    {
      try
      {
        byte[] numArray1 = (byte[]) null;
        using (RSA rsa = RSA.Create())
        {
          rsa.KeySize = 4096;
          rsa.FromXmlString(src_RsaPrivKey);
          numArray1 = rsa.SignData(src_Bytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        if (numArray1.Length != 256)
          throw new Exception(ComFunc.GetLngStr("msg_error_gen_rsa_sign"));
        byte[] numArray2 = new byte[544];
        Array.Copy((Array) Encoding.ASCII.GetBytes("SECURITY"), 0, (Array) numArray2, 0, 8);
        numArray2[8] = (byte) 1;
        Array.Copy((Array) BitConverter.GetBytes(src_Bytes.Length), 0, (Array) numArray2, 12, 4);
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 16, 256);
        Array.Copy((Array) Encoding.ASCII.GetBytes("INTERLVE"), 0, (Array) numArray2, 272, 8);
        numArray2[280] = (byte) 1;
        Array.Copy((Array) BitConverter.GetBytes(src_Bytes.Length), 0, (Array) numArray2, 284, 4);
        Buffer.BlockCopy((Array) numArray1, 0, (Array) numArray2, 288, 256);
        return numArray2;
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void AndImgTool(string src_FileOrDir, string dst_FileOrDir)
    {
      try
      {
        if (!File.Exists(Settings.DependFiles[0]))
          File.WriteAllBytes(Settings.DependFiles[0], Resources.ait_exe);
        using (Process process = new Process())
        {
          process.StartInfo.FileName = Settings.DependFiles[0];
          process.StartInfo.Arguments = "\"" + src_FileOrDir + "\" \"" + dst_FileOrDir + "\"";
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardInput = false;
          process.StartInfo.RedirectStandardOutput = false;
          process.Start();
          process.WaitForExit();
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void Lzop(string src_File, string dst_File, int lzop_Mode = 0)
    {
      try
      {
        if (!File.Exists(Settings.DependFiles[1]))
          File.WriteAllBytes(Settings.DependFiles[1], Resources.lzop_exe);
        using (Process process = new Process())
        {
          process.StartInfo.FileName = Settings.DependFiles[1];
          ProcessStartInfo startInfo = process.StartInfo;
          string str;
          if (lzop_Mode != 0)
            str = "-d -q -o \"" + dst_File + "\" \"" + src_File + "\"";
          else
            str = "-3 -q -o \"" + dst_File + "\" \"" + src_File + "\"";
          startInfo.Arguments = str;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardInput = false;
          process.StartInfo.RedirectStandardOutput = false;
          process.Start();
          process.WaitForExit();
        }
        if (!File.Exists(dst_File) || new FileInfo(dst_File).Length == 0L)
          throw new Exception(lzop_Mode == 0 ? ComFunc.GetLngStr("msg_lzo_comp_error") + "\r\n" + Path.GetFileName(src_File) + "." : ComFunc.GetLngStr("msg_lzo_decomp_error") + "\r\n" + Path.GetFileName(src_File) + ".");
      }
      catch (Exception ex)
      {
        throw;
      }
    }

    public static void SevZipExtract(string src_Img, string[] src_ExtList, string dst_Dir)
    {
      try
      {
        if (!File.Exists(src_Img))
          throw new Exception(ComFunc.GetLngStr("msg_file_not_found") + " " + Path.GetFileName(src_Img));
        if (!File.Exists(Settings.DependFiles[2]))
          File.WriteAllBytes(Settings.DependFiles[2], Resources.sevzip_exe);
        if (!File.Exists(Settings.DependFiles[3]))
          File.WriteAllBytes(Settings.DependFiles[3], Resources.sevzip_dll);
        string str = "e \"" + src_Img + "\" \"-o" + dst_Dir + "\"";
        for (int index = 0; index < src_ExtList.Length; ++index)
          str = str + " " + src_ExtList[index];
        using (Process process = new Process())
        {
          process.StartInfo.FileName = Settings.DependFiles[2];
          process.StartInfo.Arguments = str;
          process.StartInfo.UseShellExecute = false;
          process.StartInfo.CreateNoWindow = true;
          process.StartInfo.RedirectStandardInput = false;
          process.StartInfo.RedirectStandardOutput = false;
          process.Start();
          process.WaitForExit();
        }
      }
      catch (Exception ex)
      {
        throw;
      }
    }
  }
}
