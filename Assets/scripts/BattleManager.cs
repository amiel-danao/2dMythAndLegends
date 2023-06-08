using UnityEngine;
using System.Collections;

public class BattleManager : MonoBehaviour
{
    public UI_elements my_ui;
    public Level current_level;
    public Player player;
    public Enemy enemy;

    private int enemy_index = 0;

    public bool players_turn = true;
    
    public void GameOver()
    {

    }

    void Start()
    {
        current_level.battleManager = this;
        player.battleManager = this;
        enemy = current_level.enemies[0];
    }

    private void Advance()
    {
        enemy_index++;
        enemy = current_level.enemies[enemy_index];
    }

    private void ToggleTurns(bool player)
    {
        players_turn = player;
        if (player == true)
            my_ui.ToggleLettersBlock(true);
        else
        {
            my_ui.ToggleLettersBlock(false);
            if (enemy)
                enemy.BeginAttack();
        }
    }

    public void DoneEnemyDamage()
    {
        ToggleTurns(true);
    }

    public void DonePlayerDamage()
    {
        ToggleTurns(false);
    }
}
