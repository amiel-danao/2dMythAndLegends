using UnityEngine;
using System.Collections;
//using System;

public class Enemy : MonoBehaviour
{
    [HideInInspector]
    public Level level;
    public int damage = 1;
    public int total_hp = 12;
    public int hp;
    public Animation anim;

    private string idle_anim_string;
    private string hurt_anim_string;

    private void OnDisable()
    {
        
    }

    void Start()
    {
        level = transform.parent.GetComponent<Level>();
        hp = total_hp;
        idle_anim_string = gameObject.name + "_idle";
        hurt_anim_string = gameObject.name + "_hurt";
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        hp += damage;
        anim.Play(hurt_anim_string);
        UpdateHealthUI();
        if (hp <= 0)
            Die();
    }


    private void Die()
    {
        level.battleManager.enemy = null;
        gameObject.SetActive(false);
    }

    public void BeginAttack()
    {
        string clip_name = gameObject.name + "_attack";
        anim.Play(clip_name);
    }

    public void DoneAttacking()
    {
        anim.Play(idle_anim_string);
        level.battleManager.DoneEnemyDamage();
    }

    public void DamagePlayer()
    {
        damage = Mathf.RoundToInt(damage * (Random.value * 4));
        level.battleManager.player.TakeDamage(-damage);
    }

    private void UpdateHealthUI()
    {
        int max_hearts = level.battleManager.my_ui.enemyHearts.Count;
        int rounded_hp = Mathf.RoundToInt(hp / 4);
        int rounded_total_hp = Mathf.RoundToInt(total_hp / 4);

        for (int i = level.battleManager.my_ui.enemyHearts.Count - 1; i >= 0; i--)
        {
            //disable unused hearts
            if (hp / 4 > i)
            {
                level.battleManager.my_ui.enemyHearts[i].enabled = true;
                level.battleManager.my_ui.enemyHeartsEmpty[i].enabled = true;
            }
            else
            {
                level.battleManager.my_ui.enemyHearts[i].enabled = false;
                if (rounded_total_hp <= i)
                    level.battleManager.my_ui.enemyHeartsEmpty[i].enabled = false;
            }

            if (rounded_hp >= i)
            {
                if (rounded_hp == i)
                {
                    level.battleManager.my_ui.enemyHearts[i].fillAmount = (hp % 4) * 0.25f;
                }

                if (rounded_hp > i)
                {
                    level.battleManager.my_ui.enemyHearts[i].enabled = true;
                    level.battleManager.my_ui.enemyHeartsEmpty[i].enabled = true;
                }

                level.battleManager.my_ui.enemyHeartsAnim[i].Play(level.battleManager.my_ui.heart_fade_in);
            }
            else
            {
                level.battleManager.my_ui.enemyHearts[i].enabled = false;
                if (rounded_total_hp < i)
                    level.battleManager.my_ui.enemyHeartsEmpty[i].enabled = false;
            }
        }
    }
}
