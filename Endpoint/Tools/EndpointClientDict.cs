using System.Collections.Generic;

namespace GameDrive
{
    public class EndpointClientDict
    {
        Dictionary<Client, Endpoint> _dict = new Dictionary<Client, Endpoint>();
        string _endpointName;
        public EndpointClientDict(string endpointName)
        {
            _endpointName = endpointName;
        }

        public Endpoint GetEndpoint(Client inputClient)
        {
            Client client = ClientDefaultResolver.Resolve(inputClient);
            if (!_dict.ContainsKey(client))
            {
                _dict[client] = new Endpoint(_endpointName, client);
            }

            return _dict[client];
        }
    }

}