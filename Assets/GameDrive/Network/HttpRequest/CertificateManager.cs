using UnityEngine.Networking;

namespace GameDrive.Network
{
    public class CertificateManager
    {
        public static void AddCertificate(UnityWebRequest request)
        {
            request.certificateHandler = new AcceptAllCertificateHandler();
        }
    }
}