using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField] Character selectedCharacter;
    [SerializeField] List<Character> charactersList;
    [SerializeField] Transform AtkPosition;
    [SerializeField] bool isBot;
    [SerializeField] UnityEvent onTakeDamage;


    public Character SelectedCharacter { get => selectedCharacter; }
    public List<Character> CharactersList { get => charactersList; }

    private void Start()
    {
        if(isBot)
        {
            foreach (var character in CharactersList)
            {
                character.Button.interactable = false;
            }
        }
    }
    public void Prepare()
    {
        selectedCharacter = null;
    }

    public void SelectCharacter(Character character)
    {
        selectedCharacter = character;
    }

    public void SetPlay(bool value)
    {
        if (isBot)
        {
            List<Character> lotteryList = new List<Character>();
            foreach ( var character in charactersList)
            {
                int ticket = Mathf.CeilToInt(((float)character.CurrentHp/(float)character.MaxHp) * 10);
                for (int i = 0; i < ticket; i++)
                {
                    lotteryList.Add(character);
                }
            }
            
            int index = Random.Range(0,lotteryList.Count);
            selectedCharacter = lotteryList[index];
        }
        else
        {
            foreach (var character in CharactersList)
            {
                character.Button.interactable = value;
            }
        }
    }

    public void Attack()
    {
        selectedCharacter.transform
            .DOMove(AtkPosition.position, 0.35f)
            .SetEase(Ease.Linear);
    }

    public bool isAttackedDone()
    {
        if (selectedCharacter == null)
            return false;
        return DOTween.IsTweening(selectedCharacter.transform, true);
    }

    public void TakeDamage(int value)
    {
        selectedCharacter.ChangeHP(-value);
        var spriteRend = selectedCharacter.GetComponent<SpriteRenderer>();
        spriteRend.DOColor(Color.red, 0.1f).SetLoops(4, LoopType.Yoyo);

        onTakeDamage.Invoke();
    }

    public bool isDamaged()
    {
        if (selectedCharacter == null)
            return false;
        var spriteRend = selectedCharacter.GetComponent<SpriteRenderer>();
        return DOTween.IsTweening(spriteRend);
    }

    public void Remove(Character character)
    {
        if (selectedCharacter == character)
        {
            selectedCharacter = null;
        }
        if (CharactersList.Contains(character) == false)
        {
            return;
        }
        character.Button.interactable = false;
        character.gameObject.SetActive(false);
        CharactersList.Remove(character);
    }

    public void Return()
    {
        selectedCharacter.transform.DOMove(selectedCharacter.Initialposition, 0.35f).SetEase(Ease.Linear);
    }

    public bool isReturning()
    {
        if (selectedCharacter == null)
            return false;

        return DOTween.IsTweening(selectedCharacter.transform);
    }
}