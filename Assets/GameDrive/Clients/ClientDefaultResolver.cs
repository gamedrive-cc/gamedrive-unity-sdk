namespace GameDrive
{
    public static class ClientDefaultResolver
    {
        public static Client Resolve(Client client)
        {
            if (client == null)
            {
                return Client.GetDefaultClient();
            }

            return client;
        }
    }
}
