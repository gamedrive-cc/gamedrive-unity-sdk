using System;
using UnityEngine;

public class PlayerDataHolder
{
    public static PlayerDataHolder Instance { get; private set; } = new PlayerDataHolder();

    public GameDrive.Player Player { get; private set; }
    public GameDrive.Client Client { get; private set; }

    public Action ActionPlayerDataUpdated { get; set; }
    public void SetPlayerData(GameDrive.Player player, GameDrive.Client client)
    {
        Player = player;
        Client = client;
        ActionPlayerDataUpdated?.Invoke();
    }
}
