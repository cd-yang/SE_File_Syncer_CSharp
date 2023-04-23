using System.Management;

namespace SE_File_Syncer
{
    public class FileSyncer
    {
        public static void AutoCopyFiles(EventArrivedEventArgs e) {
            FileSyncer.logs.Add(e.ToString()); // TODO: 处理 log

            var usbDriverSource = AppHelper.ReadAppSettings("FilePath", "UsbDriverSource");
            var copyTarget = AppHelper.ReadAppSettings("FilePath", "CopyTarget");

            Console.WriteLine("UsbDriverSource: " + usbDriverSource + ", CopyTarget: " + copyTarget);
            Console.WriteLine(usbDriverSource);

            var usbDriverName = e.NewEvent.Properties["DriveName"]?.Value?.ToString();
            if (usbDriverName == null)
            {
                Console.WriteLine("USB 名称未找到");
                return;
            }
            var sourcePath = Path.Combine(usbDriverName, usbDriverSource);
            var files = Directory.GetFiles(sourcePath);
            foreach (string file in files)
            {
                var fileName = Path.GetFileName(file);
                var destPath = Path.Combine(copyTarget, fileName);
                FileInfo fileInfo = new FileInfo(file);
                double fileSizeInMB = (double)fileInfo.Length / (1024 * 1024);
                var log = "fileName: " + fileName + ", fileSize(MB): " + fileSizeInMB;
                FileSyncer.logs.Add(log);
                try
                {
                    File.Copy(file, destPath, true);
                    Console.WriteLine(log);
                }
                catch
                {
                    Console.WriteLine(fileName + " 拷贝失败");
                }

            }
        }

        private static List<string> logs = new List<string>();
    }
}