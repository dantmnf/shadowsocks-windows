using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using Shadowsocks.Controller;

namespace Shadowsocks.Util
{
    public class Utils
    {
        private static string TempPath = null;

        // return path to store temporary files
        public static string GetTempPath()
        {
            if (TempPath == null)
            {
                if (File.Exists(Path.Combine(Application.StartupPath, "shadowsocks_portable_mode.txt")))
                    try
                    {
                        Directory.CreateDirectory(Path.Combine(Application.StartupPath, "temp"));
                    }
                    catch (Exception e)
                    {
                        TempPath = Path.GetTempPath();
                        Logging.LogUsefulException(e);
                    }
                    finally
                    {
                        // don't use "/", it will fail when we call explorer /select xxx/temp\xxx.log
                        TempPath = Path.Combine(Application.StartupPath, "temp");
                    }
                else
                    TempPath = Path.GetTempPath();
            }
            return TempPath;
        }

        // return a full path with filename combined which pointed to the temporary directory
        public static string GetTempPath(string filename)
        {
            return Path.Combine(GetTempPath(), filename);
        }

        public static void ReleaseMemory(bool removePages)
        {
            
        }

        public static string UnGzip(byte[] buf)
        {
            byte[] buffer = new byte[1024];
            int n;
            using (MemoryStream sb = new MemoryStream())
            {
                using (GZipStream input = new GZipStream(new MemoryStream(buf),
                    CompressionMode.Decompress, false))
                {
                    while ((n = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        sb.Write(buffer, 0, n);
                    }
                }
                return System.Text.Encoding.UTF8.GetString(sb.ToArray());
            }
        }

        public static string FormatBandwidth(long n)
        {
            float f = n;
            string unit = "B";
            if (f > 1024)
            {
                f = f / 1024;
                unit = "KiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                unit = "MiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                unit = "GiB";
            }
            if (f > 1024)
            {
                f = f / 1024;
                unit = "TiB";
            }
            return $"{f:0.##}{unit}";
        }


    }
}
