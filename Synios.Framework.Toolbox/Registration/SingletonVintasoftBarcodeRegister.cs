using Vintasoft.Barcode;

namespace Synios.Framework.Toolbox.Registration
{
    public class SingletonVintasoftBarcodeRegister
    {
        private const string className = "SingletonVintasoftBarcodeRegister";

        private static Synios.Framework.Toolbox.Registration.SingletonVintasoftBarcodeRegister instance;
        private static readonly object mutex = new object();

        private SingletonVintasoftBarcodeRegister() { }

        public static Synios.Framework.Toolbox.Registration.SingletonVintasoftBarcodeRegister GetInstance()
        {
            if (instance == null)
            {
                lock (mutex)
                {
                    if (instance == null)
                        instance = new Synios.Framework.Toolbox.Registration.SingletonVintasoftBarcodeRegister();
                }
            }

            return instance;
        }

        private Synios.Framework.Toolbox.Registration.RegCodes GetDesktopCodes()
        {
            Synios.Framework.Toolbox.Registration.RegCodes codes = new Synios.Framework.Toolbox.Registration.RegCodes();

            codes.param.Add(Synios.Framework.Toolbox.Registration.eRegister.Barcode, "JezwHs48OQEedYdlFojOy+7jQaJ6TmiJ7wmYxSvt29SVbaL4Fhj1xbaIdsoqFPKhCRQ9L8M9aAuM7fkellbb2RIWhHe4yc3ZtoZBLYN46L5NJvCKtdMJ9dPCLE83H4GGDwj/cHAAoeMefTlDWEaDcD9R4q4og2DAmuc6ToPy4lxY");
            codes.param.Add(Synios.Framework.Toolbox.Registration.eRegister.User, "Synios GmbH");
            codes.param.Add(Synios.Framework.Toolbox.Registration.eRegister.Email, "lizenz@synios.de");

            return codes;

        }

        private Synios.Framework.Toolbox.Registration.RegCodes GetServerCodes()
        {
            Synios.Framework.Toolbox.Registration.RegCodes codes = new Synios.Framework.Toolbox.Registration.RegCodes();

            codes.param.Add(Synios.Framework.Toolbox.Registration.eRegister.Barcode, "IPUkReWpKWhfJ22Kd0MPWgPs2p/Qgl0u1y1ivMFgOn7Dcp/wXN5GrIN1YBdxio+92GIrWzF7Z0FmoNix4AsbGFNNrpg6kiIld0/DO5hkIdF0iqOR4gKaENqW3MQ80Pg7Y3YM2Sn9MbhVMmQ+9GeHDA2XS1G/2YKsxEugITslfFdQ");
            codes.param.Add(Synios.Framework.Toolbox.Registration.eRegister.User, "Synios GmbH");
            codes.param.Add(Synios.Framework.Toolbox.Registration.eRegister.Email, "synios.de");

            return codes;

        }


        public void Register()
        {


            Synios.Framework.Toolbox.Registration.RegCodes regCodes = string.IsNullOrEmpty(BarcodeGlobalSettings.ServerName) ? GetDesktopCodes() : GetServerCodes();

            BarcodeGlobalSettings.RegisterBarcodeReader(regCodes.param[Synios.Framework.Toolbox.Registration.eRegister.User], regCodes.param[Synios.Framework.Toolbox.Registration.eRegister.Email], regCodes.param[Synios.Framework.Toolbox.Registration.eRegister.Barcode]);
        }
    }
}
