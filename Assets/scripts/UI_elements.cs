using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class UI_elements : MonoBehaviour
{
    public InputField text_field;
    public Button attack_btn;
    public Transform lettersContainer;
    public Transform inputContainer;
    public Transform hiddenContainer;
    public Bible bible;
    [SerializeField]
    private WordSupplier wordSupplier;
    private List<LetterEntry> letterEntries;
    private List<LetterEntry> inputEntries;
    [HideInInspector]
    public string last_correct_word;
    private Player player;

    [SerializeField]
    private Transform playerHeartContainer;
    [SerializeField]
    private Transform enemyHeartContainer;

    public List<Image> playerHearts;
    public List<Image> enemyHearts;
    public List<Image> playerHeartsEmpty;
    public List<Image> enemyHeartsEmpty;
    public List<Animation> playerHeartsAnim;
    public List<Animation> enemyHeartsAnim;

    //public Sprite enabled_heart;
    //public Sprite disabled_heart;
    [HideInInspector]
    public string heart_fade_out = "heart_fade_out";
    [HideInInspector]
    public string heart_fade_in = "heart_fade_in";

    public void TextChanged()
    {
        string mod = text_field.text.ToUpperInvariant();

        attack_btn.interactable = wordSupplier.HasWord(mod);
    }

    private void Awake()
    {
        InitLetters();
        InitHearts();
        bible = new Bible();
        wordSupplier = new WordSupplier(this, bible);
    }

    private void InitHearts()
    {
        playerHearts = new List<Image>();
        enemyHearts = new List<Image>();

        for (int i = 0; i < playerHeartContainer.childCount; i++)
        {
            playerHearts.Add(playerHeartContainer.GetChild(i).GetChild(0).GetComponent<Image>());
            playerHeartsEmpty.Add(playerHeartContainer.GetChild(i).GetComponent<Image>());
            playerHeartsAnim.Add(playerHeartContainer.GetChild(i).GetChild(0).GetComponent<Animation>());
        }

        for (int i = 0; i < enemyHeartContainer.childCount; i++)
        {
            enemyHearts.Add(enemyHeartContainer.GetChild(i).GetChild(0).GetComponent<Image>());
            enemyHeartsEmpty.Add(enemyHeartContainer.GetChild(i).GetComponent<Image>());
            enemyHeartsAnim.Add(enemyHeartContainer.GetChild(i).GetChild(0).GetComponent<Animation>());
        }
    }

    private void InitLetters()
    {
        letterEntries = new List<LetterEntry>();
        foreach (Button btn in lettersContainer.GetComponentsInChildren<Button>())
        {
            LetterEntry letter = new LetterEntry(btn, btn.GetComponentInChildren<TextMeshProUGUI>(), btn.transform, this, letterEntries.Count);
            letterEntries.Add(letter);
            btn.onClick.AddListener(()=>LetterButtonClicked(letter));
        }

        inputEntries = new List<LetterEntry>();
        foreach (Button btn in hiddenContainer.GetComponentsInChildren<Button>())
        {
            LetterEntry letter = new LetterEntry(btn, btn.GetComponentInChildren<TextMeshProUGUI>(), btn.transform, this, inputEntries.Count);
            inputEntries.Add(letter);
            btn.onClick.AddListener(() => InputLetterClicked(letter));
        }
    }

    public void InputLetterClicked(LetterEntry letter)
    {
        Transform t = letter.myTransform;
        //put back this letter, including all that follows it
        if (t.GetSiblingIndex() < inputContainer.childCount - 1)
        {
            int diff = inputContainer.childCount - t.GetSiblingIndex();
            for (int i = diff; i > 0; i--)
            {
                int index = inputContainer.childCount - i;
                LetterEntry thisInputEntry = GetInputEntryByTransform(inputContainer.GetChild(index));
                LetterEntry other = GetLetterEntryByIndex(thisInputEntry.index);
                if (other != null)
                {
                    other.myCharacter.enabled = true;
                    other.button.interactable = true;
                }
                else {
                    Debug.Log("NULL = " + index);
                }
                if (text_field.text.Length > index)
                    text_field.text = text_field.text.Remove(index, 1);
                inputContainer.GetChild(index).SetParent(hiddenContainer, false);
            }
        }

        LetterEntry otherPair = GetLetterEntryByIndex(letter.index);
        if (otherPair != null)
        {
            otherPair.myCharacter.enabled = true;
            otherPair.button.interactable = true;
        }

        if(text_field.text.Length > t.GetSiblingIndex())
            text_field.text = text_field.text.Remove(t.GetSiblingIndex(), 1);
        t.SetParent(hiddenContainer, false);        
    }

    public void LetterButtonClicked(LetterEntry letter)
    {
        letter.button.interactable = false;
        letter.myCharacter.enabled = false;

        LetterEntry other = inputEntries[letter.index];
        other.myTransform.SetParent(inputContainer, false);
        other.myCharacter.text = letter.myCharacter.text;

        text_field.text += letter.myCharacter.text;
    }

    private LetterEntry GetLetterEntryByIndex(int index)
    {
        for (int i = 0; i < letterEntries.Count; i++)
        {
            if (index == letterEntries[i].index)
                return letterEntries[i];
        }

        return null;
    }

    private LetterEntry GetInputEntryByTransform(Transform t)
    {
        for (int i = 0; i < inputEntries.Count; i++)
        {
            if (t.Equals(inputEntries[i].myTransform))
                return inputEntries[i];
        }

        return null;
    }

    private void Start()
    {
        char[] newLetters = wordSupplier.Reshuffle(wordSupplier.GenerateRandomLetters());
        wordSupplier.SetCurrentLetters(newLetters.ArrayToString());
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        //letters_area.SetText(newLetters.ArrayToString());
    }

    public void Attack()
    {
        Debug.Log("Attack!");
        wordSupplier.SubtractLetters(text_field.text.ToUpper().ToCharArray());
        RemoveInputLetters();
        last_correct_word = text_field.text;
        text_field.text = "";
        player.BeginAttack();

        char[] newLetters = wordSupplier.Reshuffle(wordSupplier.GenerateRandomLetters());
        wordSupplier.SetCurrentLetters(newLetters.ArrayToString());
        ToggleLettersBlock(false);
    }

    private void RemoveInputLetters()
    {
        int letterCounts = inputContainer.childCount;
        List<Transform> to_remove = new List<Transform>();
        for (int i = 0; i < letterCounts; i++)
        {
            to_remove.Add(inputContainer.GetChild(i));
        }

        foreach (Transform t in to_remove)
        {
            t.SetParent(hiddenContainer, false);
        }
    }

    public void UpdateLetterEntries()
    {
        for (int i = 0; i < wordSupplier.GetCurrentLetters().Length; i++)
        {
            letterEntries[i].SetLetter(wordSupplier.GetCurrentLetters().Substring(i, 1));
            letterEntries[i].button.interactable = true;
            letterEntries[i].myCharacter.enabled = true;
        }
    }

    public void ToggleLettersBlock(bool state)
    {
        for (int i = 0; i < letterEntries.Count; i++)
        {
            letterEntries[i].button.interactable = state;
        }
    }
}
