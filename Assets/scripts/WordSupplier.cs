using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

[System.Serializable]
public class WordSupplier
{
    private UI_elements my_ui;
    private Bible bible;
    [SerializeField]
    private string previousLetters;
    [SerializeField]
    private string currentLetters;

    public WordSupplier(UI_elements ui, Bible bb)
    {
        my_ui = ui;
        bible = bb;
    }

    public void SetCurrentLetters(string letters)
    {
        previousLetters = currentLetters;
        currentLetters = letters;
        my_ui.UpdateLetterEntries();
    }

    public char[] GenerateRandomLetters()
    {
        string allLetters = "";
        int prevWordIndex = 0;

        while (allLetters.Length < 16)
        {
            int currentWordIndex = Random.Range(0, bible.All_words.Count);

            if (currentWordIndex != prevWordIndex)
            {
                string selectedWord = bible.All_words.ElementAt(currentWordIndex);
                if (selectedWord.Length <= 16)
                {
                    Debug.Log(selectedWord);
                    allLetters += selectedWord;

                    prevWordIndex = currentWordIndex;
                }
            }
        }

        char[] returnChars = allLetters.ToCharArray(0, 16);
        SetCurrentLetters(returnChars.ArrayToString());

        return returnChars;
    }

    public char[] Reshuffle(char[] texts)
    {
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++)
        {
            char tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }

        SetCurrentLetters(texts.ArrayToString());
        return texts;
    }

    public void SubtractLetters(char[] letters)
    {
        for (int i = 0; i < letters.Length; i++)
        {
            int index = currentLetters.IndexOf(letters[i]);
            string cleanPath = (index < 0)
                ? currentLetters
                : currentLetters.Remove(index, 1);

            SetCurrentLetters(cleanPath);
        }
    }

    public void SupplyNewLetters()
    {
        string tempCurrentLetters = currentLetters;
        Dictionary<string, bool> tempDict = new Dictionary<string, bool>(bible.Bible_words);

        char[] eachLetter = tempCurrentLetters.ToCharArray();
        foreach (var s in tempDict.Where(kv => kv.Key.IndexOfAny(eachLetter) == -1).ToList())
        {
            tempDict.Remove(s.Key);
        }

        int prevWordIndex = 0;

        while (tempCurrentLetters.Length < 16)
        {
            int currentWordIndex = Random.Range(0, tempDict.Count);

            if (currentWordIndex != prevWordIndex)
            {
                string selectedWord = tempDict.ElementAt(currentWordIndex).Key;
                Debug.Log("new word supplied :" + selectedWord);
                tempCurrentLetters += selectedWord;

                prevWordIndex = currentWordIndex;
            }
        }

        char[] returnChars = tempCurrentLetters.ToCharArray(0, 16);

        SetCurrentLetters(Reshuffle(returnChars).ArrayToString());
    }

    public string GetCurrentLetters()
    {
        return currentLetters;
    }

    public bool HasWord(string word)
    {
        if (word.Length < 3 || !my_ui.bible.ValidWord(word))
            return false;

        string tempCurrentLetter = currentLetters;

        for (int i = 0; i < word.Length; i++)
        {
            int indexOccurence = tempCurrentLetter.IndexOf(word[i]);
            if (indexOccurence > -1)
            {
                tempCurrentLetter.Remove(indexOccurence);
                if (i == word.Length - 1)
                    return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }
}