using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NumberButton : MonoBehaviour
{
    public TextMeshProUGUI text;
    public Button button;
    public float colourResetSpeed = 5f;
    int number;

    public void SetNumber(int number)
    {
        this.number = number;
        text.text = number.ToString();
    }

    public void ButtonPressed()
    {
        bool correct = Manager.instance.currentNumber == number;

        button.image.color = correct ? Color.green : Color.red;
        Manager.instance.ButtonPressed(correct);
    }

    private void Update()
    {
        if (button.image.color == Color.white)
            return;

        button.image.color = LerpColour(button.image.color, Color.white, Time.deltaTime * colourResetSpeed);
    }

    Color LerpColour(Color a, Color b, float t)
    {
        Color newColour = new Color();
        newColour.r = Mathf.Lerp(a.r, b.r, t);
        newColour.g = Mathf.Lerp(a.g, b.g, t);
        newColour.b = Mathf.Lerp(a.b, b.b, t);
        newColour.a = Mathf.Lerp(a.a, b.a, t);

        return newColour;
    }
}
