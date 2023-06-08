using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LetterEntry
{
    public Button button;
    public TextMeshProUGUI myCharacter;
    public Transform myTransform;
    public UI_elements myUI;
    public int index;

    public LetterEntry(Button button, TextMeshProUGUI myCharacter, Transform myTransform, UI_elements myUI, int index)
    {
        this.button = button;
        this.myCharacter = myCharacter;
        this.myTransform = myTransform;
        this.myUI = myUI;
        this.index = index;
    }

    public void SetLetter(string letter)
    {
        myCharacter.text = letter;
    }
}