namespace GameDriveSample
{
    public class ClientHolder
    {
        public GameDrive.Client Client { get; private set; }
        public ClientHolder(string id)
        {
            Client = new GameDrive.Client(id);
        }
    }
}