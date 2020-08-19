namespace Core
{
    public class AblageInfos
    {
        public string PathToFile { get; set; }
        public string FileFilter { get; set; }
        public string AblageCode { get; set; }

        public AblageInfos()
        {
            PathToFile = string.Empty;
            FileFilter = string.Empty;
            AblageCode = string.Empty;
        }
    }
}