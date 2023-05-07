using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [SerializeField] State state;
    [SerializeField] GameObject battleResult;
    [SerializeField] TMP_Text battleResultText;
    [SerializeField] Player player;
    [SerializeField] Player enemy;

    //temporary
    [SerializeField] bool isReturn;
    [SerializeField] bool isPlayerDefeated;

    enum State
    {
        Preparation,
        PlayerSelect,
        EnemySelect,
        AttackAnimation,
        DamageAnimation,
        ReturnAnimation,
        BattleOver

    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Preparation:
                player.Prepare();
                enemy.Prepare();

                player.SetPlay(true);
                enemy.SetPlay(false);
                state = State.PlayerSelect;
                break;
            case State.PlayerSelect:
                if (player.SelectedCharacter != null)
                {
                    player.SetPlay(false);
                    enemy.SetPlay(true);
                    state = State.EnemySelect;   
                }
                break;
            case State.EnemySelect:
                if (enemy.SelectedCharacter != null)
                {
                    enemy.SetPlay(false);
                    player.Attack();
                    enemy.Attack();
                    state = State.AttackAnimation;
                }
                break;
            case State.AttackAnimation:
                if (player.isAttackedDone() == false && enemy.isAttackedDone() == false)
                {
                    CalculateBattle(player, enemy, out Player winner, out Player loser);
                    if(loser == null)
                    {
                        player.TakeDamage(enemy.SelectedCharacter.AttackPower);
                        enemy.TakeDamage(player.SelectedCharacter.AttackPower);
                    }
                    else
                    {
                        loser.TakeDamage(winner.SelectedCharacter.AttackPower);
                    }
                    state = State.DamageAnimation;
                }
                break;
            case State.DamageAnimation:
                if (player.isDamaged() == false && enemy.isDamaged() == false)
                {
                    if(player.SelectedCharacter.CurrentHp == 0)
                    {
                        player.Remove(player.SelectedCharacter);
                    }
                    if(enemy.SelectedCharacter.CurrentHp == 0)
                    {
                        enemy.Remove(enemy.SelectedCharacter);
                    }
                    
                    if(player.SelectedCharacter != null)
                    {
                        player.Return();
                    }
                    if(enemy.SelectedCharacter != null)
                    {
                        enemy.Return();
                    }
                    
                    state = State.ReturnAnimation;
                }
                break;
            case State.ReturnAnimation:
                if (player.isReturning() ==  false && enemy.isReturning() == false)
                {
                    if (player.CharactersList.Count == 0 && enemy.CharactersList.Count == 0)
                    {
                        battleResult.SetActive(true);
                        battleResultText.text = "Game Over!\nDraw";
                        state = State.BattleOver;
                    }
                    else if(player.CharactersList.Count == 0)
                    {
                        battleResult.SetActive(true);
                        battleResultText.text = "Game Over!\nPlayer 2 Win!";
                        state = State.BattleOver;
                    }
                    else if(enemy.CharactersList.Count == 0)
                    {
                        battleResult.SetActive(true);
                        battleResultText.text = "Game Over!\nPlayer 1 Win!";
                        state = State.BattleOver;
                    }
                    else
                    {
                        state = State.Preparation;
                    }
                }
                break;
            case State.BattleOver:
                break;
        }
    }

    private void CalculateBattle(Player player, Player enemy, out Player winner, out Player loser)
    {
        var type1 = player.SelectedCharacter.Type;
        var type2 = enemy.SelectedCharacter.Type;

        if(type1 == CharacterType.Rock && type2 == CharacterType.Paper)
        {
            winner = enemy;
            loser = player;
        }
        else if(type1 == CharacterType.Rock && type2 == CharacterType.Scissor)
        {
            winner = player;
            loser = enemy;
        }
        else if(type1 == CharacterType.Paper && type2 == CharacterType.Rock)
        {
            winner = player;
            loser = enemy;
        }
        else if(type1 == CharacterType.Paper && type2 == CharacterType.Scissor)
        {
            winner = enemy;
            loser = player;
        }
        else if(type1 == CharacterType.Scissor && type2 == CharacterType.Rock)
        {
            winner = enemy;
            loser = player;
        }
        else if(type1 == CharacterType.Scissor && type2 == CharacterType.Paper)
        {
            winner = player;
            loser = enemy;
        }
        else
        {
            winner = null;
            loser = null;
        }

    }

    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void quit()
    {
        SceneManager.LoadScene("Main");
    }
}
