using UnityEngine;

public class General : MonoBehaviour
{
    public string tag_relic = "Relic";
    public string tag_player = "Player";
    public string tag_enemy = "Enemy";
    public int layer_ConnetorTail = 7;
    public BasicActions script_player;
    public GetCardManager getCardManager;
    public CardManager cardManager;
    public GameObject[] cardsList;
    public GameObject DamageTextCanvas;
    public bool isPause = false;
    public bool canOpenMenu = true;

    private static General _instance;

    public static General Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        _instance = this;
    }

    public Vector2 MousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
