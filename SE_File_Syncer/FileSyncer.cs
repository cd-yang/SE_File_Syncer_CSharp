using System.Management;

namespace SE_File_Syncer
{
    public class FileSyncer
    {
        public static void AutoCopyFiles(EventArrivedEventArgs e) {
            FileSyncer.logs.Add(e.ToString());
            // TODO
        }

        private static List<string> logs = new List<string>();
    }
}