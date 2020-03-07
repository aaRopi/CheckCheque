namespace CheckCheque.Core
{
    public class ServicesConfig
    {
        public const string VisionApiUri = "https://kvkinvoicescan.cognitiveservices.azure.com/vision/v2.0/ocr";
        public const string VisionApiSubscriptionKey = "41a0a15e78204dd08ef3ad6528595bd3";
        public const string KvkApiUri = "https://pactwrapper.azurewebsites.net/";

        public const string IotaClientApiUri = "https://nodes.devnet.thetangle.org:443";
        public const int IotaClientTimeoutMilliseconds = 5000;

        public const string PrivateKeyPayload = "MIGTAgEAMBMGByqGSM49AgEGCCqGSM49AwEHBHkwdwIBAQQgxsU2tIFx3Ek3iJ/bB6vkSy3g+6qP1eupXvBMdAcKLvGgCgYIKoZIzj0DAQehRANCAATSWzcjVM7SiaYrzf8D6wfYCcSiGHeBj9U9B3ZWZhk77+zH5buMfAa1ehNcvHrzp7TAlw/84Eis4QdvLzUAJkDq";
        public const string PublicKeyPayload = "MFkwEwYHKoZIzj0CAQYIKoZIzj0DAQcDQgAE0ls3I1TO0ommK83/A+sH2AnEohh3gY/VPQd2VmYZO+/sx+W7jHwGtXoTXLx686e0wJcP/OBIrOEHby81ACZA6g==";

        public const string CompanyKvkNumber = "401196200";
    }
}
