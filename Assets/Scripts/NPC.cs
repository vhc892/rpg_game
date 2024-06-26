using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : Singleton<NPC>
{
    [SerializeField] private GameObject staminaGlobe;
    [SerializeField] private GameObject healthGlobe;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public string[] dialogue;
    private int index = 0;

    public GameObject contButton;
    public float wordSpeed;
    public bool playerIsClose;


    void Start()
    {
        dialogueText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerIsClose)
        {
            if (!dialoguePanel.activeInHierarchy)
            {
                dialoguePanel.SetActive(true);
                StartCoroutine(Typing());
            }
            else if (dialogueText.text == dialogue[index])
            {
                NextLine();
            }

        }
        if (Input.GetKeyDown(KeyCode.Q) && dialoguePanel.activeInHierarchy)
        {
            RemoveText();
        }
        if(dialogueText.text == dialogue[index])
        {
            contButton.SetActive(true);
        }
    }

    public void RemoveText()
    {
        dialogueText.text = "";
        index = 0;
        dialoguePanel.SetActive(false);
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed);
        }
    }

    public void NextLine()
    {
        contButton.SetActive(false);
        if (index < dialogue.Length - 1)
        {
            index++;
            dialogueText.text = "";
            StartCoroutine(Typing());
        }
        else
        {
            RemoveText();
            DropItem();

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            RemoveText();
        }
    }
    public void DropItem()
    {
        int randomNum = Random.Range(1, 2);

        
        if (randomNum == 1)
        {
            int RandomAmountOfGold = Random.Range(1, 5);
            for (int i = 0; i < RandomAmountOfGold; i++)
            {
                Instantiate(healthGlobe, transform.position, Quaternion.identity);
            }
        }
        if (randomNum == 2)
        {
            int RandomAmountOfGold = Random.Range(1, 5);
            for (int i = 0; i < RandomAmountOfGold; i++)
            {
                Instantiate(healthGlobe, transform.position, Quaternion.identity);
            }
        }
        EconomyManager.Instance.Buy();
    }
}