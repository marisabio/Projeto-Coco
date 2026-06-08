using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GrimRatDialogue : MonoBehaviour
{
    public GameObject dialogueObj;
    public TextMeshProUGUI speechText;
    public TextMeshProUGUI actorNameText;
    public Image profile;
    public PlayerController playerController;
    public GrimRatController grimRatController;

    public string[] lines;
    public string[] names;
    public Sprite[] profiles;
    
    public float textSpeed;

    private int index;
    private Collider2D col;
    private Animator animator;
    
    void Start()
    {
        col = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }

    public void StartDialogue()
    {
        col.isTrigger = true;

        if (!dialogueObj.activeSelf)
        {
            dialogueObj.SetActive(true);
            playerController.DisableCharacterControl();
            animator.Play("Grim Rat Death");
            grimRatController.isAlive = false;
            gameObject.tag = "Interactable";
            PlayerPrefs.SetFloat("temBilhete", 1);

            speechText.text = string.Empty;
            index = 0;
            StartCoroutine(TypeLine());
        }
        else if (speechText.text == lines[index])
        {
            NextLine();
        }
        else
        {
            StopAllCoroutines();
            speechText.text = lines[index];
        }
    }

    private IEnumerator TypeLine()
    {
        actorNameText.text = names[index];
        profile.sprite = profiles[index];
        foreach (char c in lines[index].ToCharArray())
        {
            speechText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index <lines.Length -1)
        {
            index++;
            speechText.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            
            playerController.EnableCharacterControl();
            dialogueObj.SetActive(false);
            col.enabled = false;
            SceneManager.LoadScene(2);
        }
    }
}
