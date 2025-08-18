using UnityEngine.Networking;

namespace GameDrive.Network
{
    public class AcceptAllCertificateHandler : CertificateHandler
    {
        protected override bool ValidateCertificate(byte[] certificateData)
        {
            return true;
        }
    }
}