using UnityEngine.Networking;
using Zenject;

namespace GameDrive.Network
{
    public class CertificateManager
    {
        public static void AddCertificate(UnityWebRequest request)
        {
            if (true)
            {
                request.certificateHandler = new AcceptAllCertificateHandler();
            }
            else
            {
                request.certificateHandler = null;//default certificate
            }

        }
    }
}