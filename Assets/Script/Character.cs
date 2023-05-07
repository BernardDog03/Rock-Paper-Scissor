using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Character : MonoBehaviour
{
    [SerializeField] string userName;
    [SerializeField] CharacterType type;
    [SerializeField] int currentHp;
    [SerializeField] int maxHp;
    [SerializeField] int attackPower;
    [SerializeField] TMP_Text overHead;
    [SerializeField] Image avatar;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text typeText;
    [SerializeField] Image healtBar;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Button button;
    private Vector3 initialposition;

    public Button Button { get => button; }
    public CharacterType Type { get => type; }
    public int AttackPower { get => attackPower; }
    public int CurrentHp { get => currentHp; }
    public Vector3 Initialposition { get => initialposition; }
    public int MaxHp { get => maxHp; }

    private void Start(){
        initialposition = this.transform.position;
        overHead.text = userName;
        nameText.text = userName;
        typeText.text = Type.ToString();
        UpdateHp();

        button.interactable = false;
    }

    public void ChangeHP(int amount)
    {
        currentHp += amount;
        currentHp = Mathf.Clamp(currentHp, 0, MaxHp);
        UpdateHp();
    }

    private void UpdateHp()
    {
        healtBar.fillAmount = (float) currentHp/ (float)MaxHp;
        healthText.text = $"{currentHp}/{MaxHp}";

    }
}