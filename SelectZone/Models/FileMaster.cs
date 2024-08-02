using System.Collections.ObjectModel;

namespace SelectZone.Models
{
    public class FileMaster
    {
        public int amdID { get; set; }
        public short mfid { get; set; }
        public short subModuleID { get; set; }
        public string? docPath { get; set; }
        public List<byte[]> data { get; set; } = new List<byte[]>();
        public int autoID { get; set; }

        public ObservableCollection<FileMaster> files = new ObservableCollection<FileMaster>();
    }
}
