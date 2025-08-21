namespace GameDrive
{
    public class StageHelper
    {
        public static string GetStageBody()
        {
            if (GameDriveInitializer.Config.stage == Stage.PREVIEW)
            {
                return "PREVIEW";
            }
            else if (GameDriveInitializer.Config.stage == Stage.LIVE)
            {
                return "LIVE";
            }

            return "";
        }
    }
}