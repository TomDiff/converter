using Vintasoft.Imaging;

namespace Synios.Framework.Toolbox.Registration
{

    public class SingletonVintasoftImagingRegister
    {
        private const string className = "SingletonVintasoftRegister";

        private static SingletonVintasoftImagingRegister instance;
        private static readonly object mutex = new object();

        private SingletonVintasoftImagingRegister() { }

        public static SingletonVintasoftImagingRegister GetInstance()
        {
            if (instance == null)
            {
                lock (mutex)
                {
                    if (instance == null)
                        instance = new SingletonVintasoftImagingRegister();
                }
            }

            return instance;
        }

        private RegCodes GetDesktopCodes()
        {
            RegCodes codes = new RegCodes();

            codes.param.Add(eRegister.ImagingRegcode, "yErmUzpmdpzKk/RDpCvXwk8mdOvEwMVLOmdEQW8nSt6ELEi2mPT0is5+h5Li9KVNh7yH0pKJMLnn44FZl6s2DNSlc60Di47DfGQgMEdhquR6wr59aPfMeQJhUwbn6yTpZ00uwkGKRi1LVc15sm6YGxdjF63mngz4O1xnztl9Xp7Y");
            codes.param.Add(eRegister.ImagingWpfRegcode, "UiFsv2JUGUynSm3LfosV58v5jKA2L1/tauD0vRZ/wkMA7wyzwIOSlNMUyDpmwtRMOFkZGq3EFwCnZnbSU3T/BL7I6vsuEq/XjPJrp8fhi5Ox/YxvG1Aa2eil5PbEoPiwwSI3gqbHqw+u0q1Ks9bQpLhc7CWyaEu7GnIk2CcpiSwU");
            codes.param.Add(eRegister.AnnotationRegcode, "CVx1YBpJrRtv2EK28flfL5b2oSwoiHZPphIwvvg2RFPfU+q43KQ57NoKZKO9cQ6+Ju0yR4PffRAg+U5HIiNg4maDxe1Erj4Q0JGbhceics4BzsWT7pmHoKiR/jgyI7zkn5NqPbrUj3mW7CzObtw3NxwffPS+Zw78w9NDX9EZmDM8");
            codes.param.Add(eRegister.PdfRegcodeReader, "jcNAv75fxpV6OKGliZmCrbxFb7NbvvNNwUjf5OUMWswdJENoeW38ob73UBVXzUIeo7HCovUzASXw/gmIhgLQ78cdyA6uYy+atK6upVOD2lUr9zpeRrr2PRV8rt9uLIAyjqHEcihWNWaGLjwjmbq1E4qxXA2YuGfVDaUPZ8LYQuZ8");
            codes.param.Add(eRegister.PdfRegcodeWriter, "OasRRhZjMn8AIHi94lsshH4tqhnse3xng/6BKftKqZay63SgLS5pKOFNzqhGlmvUrz/XIQJx0nyqte3N7uDs1gKvNVjoMz6t5sMPL0MTzV+hjyQ6+3D9LNWL76hPFdyNjcjgY2NDJ+s/4Ny7RUgxndTkGWwD9bu6M1tL6cSR+PH4");
            codes.param.Add(eRegister.JbigRegcode, "LlID2ALKK4SyLtlawJ/GlGtYVuD7JKIFWYF61Ed15kcp8KjqVRdyPWAas9bFMygIEr7CC1wLPFdbo4Dhezpamt8TsQz8J2zLx83VW6n+ef4C6l2B3mN3qymK6nDZwaFGq0NyntU6Noqn6Kg7vPjZOrZkfsrBGObvPUyTJFtYudhg");
            codes.param.Add(eRegister.J2000Regcode, "VrRyHG5NPsP5M8EG0mRQoxARzUlmK6X0W6UglseRctMNY+3InQXJXXANmcLBB3Yj6CdV61SvMg8aaE1z014LOh1Nc64YAfypiMpz9RFWyrqmgPSTKqMOD9J7me7xvjeWxt1aBsbQpe4RL7RtiuNWovSWP6OhXwSCdeqWTWT4op/Y");
            codes.param.Add(eRegister.DoccleanupRegcode, "Lyga7xmep9w2StArBxclf0VSIvyS0kK1qFNOOlD6+cYQ743IHAQdTT6zQZ4guqaozsaKkyQ6JiwDDZrxlQwA/PlmqN+PXpO/liZg1OsLW+ZiwDS2/0GfUFIPTl9+xZkgoKNxOJEvZRmis3iLxAY1T1OCAm0CZZ7/czF6jeDT+Uak");
            codes.param.Add(eRegister.User, "Lenin Ochoa");
            codes.param.Add(eRegister.Email, "l.ochoa@synios.de");


            return codes;

        }

        private RegCodes GetServerCodes()
        {
            RegCodes codes = new RegCodes();

            codes.param.Add(eRegister.ImagingRegcode, "r+3vDX3H0LbKISipwfEpTY9pnNg5bQIwDVm8JXEBH23C63nuYdMjFywXk+LNOr+oSUcU6pmYs/mRnq7fCjEluVZatCmr68WCGBCgSAgCXFtnSPtD3ddzc1RYYMo+93WpE52cgrWtVnB+IDWHh4m86FOwKDawj2QkS1zUDqkhBsHw");
            codes.param.Add(eRegister.ImagingWpfRegcode, "MpEj/DhkxbrYqnc5laX3FyusVVIZ5SACSOZ+TC1sMwMSvhE7B4dPMbShgVX8mRxsn+JkPLeNWT3RTAZ7ZTb1AS8w1RDfh5QoFjbn9EpbITC0tYifC1t2rgcHPY33WiGk3jgPvTH9ReGnGaKfc1TB/2r2Zbno4t6sgxTRcZjhDDhc");
            codes.param.Add(eRegister.AnnotationRegcode, "XRA6av8G/0GZ/N7BZimJM0h6a+HPsYcvxDE8fi6XVf2nGW5OJPoinNTUdT9so8SB0ujwgdspuQBADq2wIYnbw9YZik6hG5wyIJ+PQskvCnS1wzTFuwLFPY7bhouHQHqZZ3bqvikFBfjaS1ppsCtRpgC1Ce9G7SW02FiHZyljL7M0");
            codes.param.Add(eRegister.PdfRegcodeReader, "QdhnTpMM8Lavqu5V56ym5Rg0fJn5C9HOXLgKeroGik3fze1NQNvnGMG+Q1AOcHMcFN7KELcyRiBB1QPFTqc7sk4Oz7ic0Fm0dibEqq/J/7krRZPvEztm48Qaw2etfq2wXM3mKM9TxN1hIx8PHvLje3W5NLdi1bsO4WFhGSmtaRKo");
            codes.param.Add(eRegister.PdfRegcodeWriter, "YaVL1KB2yWqslqs8o9U6eKsqegBP3AgcrspqGYVcoyJg3mdpHDuyQsS77j6PA7/RE4zIO+OOZmGGcxV3bXbj7sDKJRpVUFdnEHY37AayM0WcsT3jdG3bVv6lyAo/fn2zmnpJQA6Olui7v5W0P4NyVSSaLH9azRyU07uAlHeEdO4A");
            codes.param.Add(eRegister.JbigRegcode, "ZHCpKedLdJjJ/SwnRw/ec29qCwcCvcImsTJmYcnCWe9RAoOHJPkhKGahm/9yCuMWGXS8tFUmxN7VrrPzmhfKhfhdVpKqkLKpNlKm0yT8P2jZ6MYMnYI6k3mI1gyW37w7ie2z/py2HfuC05MX//20I9WN20D7Bc5nKxIRwHHDUkJI");
            codes.param.Add(eRegister.J2000Regcode, "nE+o1D3OZoYb+3f+E7cU8oOkvLdd2M5a1DyTzOAXFxxaYHzOJBtNudhv+TfQxZ7rKvK6PS9LQYKIDa9smos0HJIp8O0yxKtG6ORwnhWN3H3mIKITVklCKL1g0ybpW4y+AyDIw0QVNh9fLMOv/M5ChdUNsEKrHRP28xBuLFA0aVk0");
            codes.param.Add(eRegister.DoccleanupRegcode, "QaehACK2wE9ZhBqSeWkEmueydB5OLZHrF7d8oJuQlvtmeBMKA7NyRIyWdJe4uxygkuj7fmbbxURMYhaDW+Y8qDMHDeKqBUGv3XswfF8MIQKb8wUCHG8b5UEYJM+ezoXraiDNVxTM75rGZLoYs2yNU9HeW4Z0DOmf6fa0i5i2u30w");
            codes.param.Add(eRegister.User, "Lenin Ochoa (Servers)");
            codes.param.Add(eRegister.Email, "l.ochoa@synios.de");



            return codes;

        }


        public void Register()
        {

            RegCodes regCodes = string.IsNullOrEmpty(ImagingGlobalSettings.ServerName) ? GetDesktopCodes() : GetServerCodes();

            ImagingGlobalSettings.RegisterImaging(regCodes.param[eRegister.User], regCodes.param[eRegister.Email], regCodes.param[eRegister.ImagingRegcode]);
            ImagingGlobalSettings.RegisterWpfImaging(regCodes.param[eRegister.ImagingWpfRegcode]);
            ImagingGlobalSettings.RegisterAnnotation(regCodes.param[eRegister.AnnotationRegcode]);
            ImagingGlobalSettings.RegisterPdfReader(regCodes.param[eRegister.PdfRegcodeReader]);
            ImagingGlobalSettings.RegisterPdfWriter(regCodes.param[eRegister.PdfRegcodeWriter]);
            ImagingGlobalSettings.RegisterJbig2(regCodes.param[eRegister.JbigRegcode]);
            ImagingGlobalSettings.RegisterJpeg2000(regCodes.param[eRegister.J2000Regcode]);
            ImagingGlobalSettings.RegisterDocCleanup(regCodes.param[eRegister.DoccleanupRegcode]);
        }
    }
}
