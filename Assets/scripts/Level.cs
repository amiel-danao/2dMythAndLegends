using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Level : MonoBehaviour
{

    public int level = 0;
    public SpriteRenderer spriteRenderer;
    public Enemy[] enemies;
    [HideInInspector]
    public BattleManager battleManager;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
