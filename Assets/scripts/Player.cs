using UnityEngine;

public class Player : MonoBehaviour
{
    public int total_hp = 12;
    public int hp;
    private int damage;
    [HideInInspector]
    public BattleManager battleManager;
    public Animator anim;

    void Start()
    {
        hp = total_hp;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        hp += damage;
        anim.SetTrigger("Hurt");
        UpdateHealthUI();
        if (hp <= 0)
            battleManager.GameOver();
        
    }

    private void UpdateHealthUI()
    {
        int max_hearts = battleManager.my_ui.playerHearts.Count;
        int rounded_hp = Mathf.RoundToInt(hp / 4);
        int rounded_total_hp = Mathf.RoundToInt(total_hp / 4);

        for (int i = battleManager.my_ui.playerHearts.Count - 1; i >= 0; i--)
        {
            //disable unused hearts
            if (hp / 4 > i)
            {
                battleManager.my_ui.playerHearts[i].enabled = true;
                battleManager.my_ui.playerHeartsEmpty[i].enabled = true;
            }
            else
            {
                battleManager.my_ui.playerHearts[i].enabled = false;
                if (rounded_total_hp <= i)
                    battleManager.my_ui.playerHeartsEmpty[i].enabled = false;
            }

            if (rounded_hp >= i)
            {
                if (rounded_hp == i)
                {
                    battleManager.my_ui.playerHearts[i].fillAmount = (hp % 4) * 0.25f;
                }

                if (rounded_hp > i)
                {
                    battleManager.my_ui.playerHearts[i].enabled = true;
                    battleManager.my_ui.playerHeartsEmpty[i].enabled = true;
                }

                battleManager.my_ui.playerHeartsAnim[i].Play(battleManager.my_ui.heart_fade_in);
            }
            else
            {
                battleManager.my_ui.playerHearts[i].enabled = false;
                if(rounded_total_hp < i)
                    battleManager.my_ui.playerHeartsEmpty[i].enabled = false;
            }
        }
    }

    public void BeginAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void DoneAttacking()
    {
        battleManager.DonePlayerDamage();
    }


    private void DamageEnemy()
    {
        CalculateDamage();
        battleManager.enemy.TakeDamage(-damage);
    }

    private void CalculateDamage()
    {
        damage = battleManager.my_ui.last_correct_word.Length;
    }
}
