using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public Vector2Int numberMinMax;
    public int numChoices;
    public int currentNumber;
    int[] currentChoices;
    List<int> numbersLeft;
    List<int> allChoices;

    public GameObject choiceContainer;
    public Object numberButtonPrefab;
    List<NumberButton> numberButtons = new List<NumberButton>();

    public TextMeshProUGUI messageText;
    public string correctMessage, incorrectMessage;
    public float messageDuration = 5f;
    public AudioClip correctSound, incorrectSound;

    public GameObject endGameObject;

    ARObjectManager arManager;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        arManager = GetComponent<ARObjectManager>();
        audioSource = GetComponent<AudioSource>();

        currentChoices = new int[numChoices];

        for (int i = 0; i < numChoices; i++)
        {
            GameObject button = Instantiate(numberButtonPrefab, choiceContainer.transform) as GameObject;

            numberButtons.Add(button.GetComponent<NumberButton>());
        }

        SetupGame();
    }

    public void SetupGame()
    {
        choiceContainer.SetActive(true);
        endGameObject.SetActive(false);

        allChoices = new List<int>();
        numbersLeft = new List<int>();
        for (int i = numberMinMax.x; i <= numberMinMax.y; i++)
        {
            allChoices.Add(i);
            numbersLeft.Add(i);
        }

        numbersLeft.Sort((a, b) => 1 - 2 * Random.Range(0, allChoices.Count));

        GenerateNumber();
    }

    [ContextMenu("Generate number")]
    private void GenerateNumber()
    {
        if (numbersLeft.Count == 0)
        {
            GameWon();
            return;
        }
        currentNumber = numbersLeft[0];

        //Generate a set of random numbers that will not include dupicates
        allChoices.Sort((a, b) => 1 - 2 * Random.Range(0, allChoices.Count));
        
        GenerateChoices();

        //Spawn number in world
        arManager.SetNumber(currentNumber);
    }

    private void GenerateChoices()
    {
        bool alreadyContains = false;

        for (int i = 0; i < numChoices; i++)
        {
            currentChoices[i] = allChoices[i];
            numberButtons[i].SetNumber(currentChoices[i]);
            if (currentChoices[i] == currentNumber)
                alreadyContains = true;
        }

        if (!alreadyContains)
        {
            int choiceIndex = Random.Range(0, numChoices);
            currentChoices[choiceIndex] = currentNumber;
            numberButtons[choiceIndex].SetNumber(currentNumber);
        }
    }

    public void ButtonPressed(bool correct)
    {
        if (correct)
        {
            numbersLeft.RemoveAt(0);
            GenerateNumber();
        }

        //Show text
        StopAllCoroutines();
        messageText.enabled = true;
        messageText.text = correct ? correctMessage : incorrectMessage;
        audioSource.PlayOneShot(correct ? correctSound : incorrectSound);
        StartCoroutine(IResetMessage(messageDuration));
    }

    IEnumerator IResetMessage(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        messageText.enabled = false;
    }

    void GameWon()
    {
        choiceContainer.SetActive(false);
        endGameObject.SetActive(true);
    }
}
