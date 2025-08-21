using System;

namespace GameDrive.Network
{
    public class QueryStringManager
    {
        public static void AddQueryString(UriBuilder uriBuilder, HttpQuery query)
        {
            if (GameDriveInitializer.Config.stage == Stage.PREVIEW)
            {
                if (query == null)
                {
                    query = new HttpQuery();
                }
                query.Add("stage", "PREVIEW");
            }

            if (query != null)
                uriBuilder.Query = query.ToQueryString();
        }
    }
}