using UnityEngine;

public class General : MonoBehaviour
{
    public string tag_relic = "Relic";
    public string tag_player = "Player";
    public string tag_enemy = "Enemy";
    public BasicActions script_player;

    private static General _instance;

    public static General Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }
}
