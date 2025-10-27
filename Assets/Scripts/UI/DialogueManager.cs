using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject dialogueWindow;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private float typingSpeed = 0.02f;

    private Dialogue currentDialogue;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingCoroutine;
    
    
    public void ShowDialogue(Dialogue dialogue)
    {
        if (dialogue == null || dialogue.lines.Length == 0) return;

        currentDialogue = dialogue;
        currentLineIndex = 0;
        dialogueWindow.SetActive(true);
        ShowCurrentLine();
    }
    
    public void HideDialogueWindow()
    {
        dialogueWindow.SetActive(false);
        currentDialogue = null;
    }
    
    private void ShowCurrentLine()
    {
        if (currentDialogue == null) return;

        var line = currentDialogue.lines[currentLineIndex];
        speakerNameText.text = line.speakerName;
        
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(line.text));
    }
    
    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
    
    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentDialogue.lines[currentLineIndex].text;
            isTyping = false;
            return;
        }

        currentLineIndex++;

        if (currentLineIndex >= currentDialogue.lines.Length)
        {
            HideDialogueWindow();
        }
        else
        {
            ShowCurrentLine();
        }
    }
}
