namespace Converter
{
    public class ConverterInfos
    {
        public string PathToFile { get; set; }
        public string FileFilter { get; set; }
        public string AblageCode { get; set; }

        public ConverterInfos()
        {
            PathToFile = string.Empty;
            FileFilter = string.Empty;
            AblageCode = string.Empty;
        }
    }
}