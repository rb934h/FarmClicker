using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private GameObject dialogueWindow;
        [SerializeField] private TMP_Text speakerNameText;
        [SerializeField] private TMP_Text dialogueText;
        [SerializeField] private float typingSpeed = 0.02f;
        [SerializeField] private AudioSource _dialogueSound;
    
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
    
        private async void ShowCurrentLine()
        {
            try
            {
                if (currentDialogue == null) return;

                var line = currentDialogue.lines[currentLineIndex];
                var speakerName = await line.speakerName.GetLocalizedStringAsync();
        
                speakerNameText.text = speakerName;
        
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                var text = await line.text.GetLocalizedStringAsync();
                typingCoroutine = StartCoroutine(TypeLine(text));
            }
            catch
            {
                Debug.Log("Show current line error");
            }
        }
    
        private IEnumerator TypeLine(string line)
        {
            isTyping = true;
            dialogueText.text = "";
        
            _dialogueSound.Play();

            foreach (char c in line)
            {
                dialogueText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }

            isTyping = false;
            _dialogueSound.Stop();
        }
    
        public async void NextLine()
        {
            try
            {
                if (isTyping)
                {
                    StopCoroutine(typingCoroutine);
                    var text = await currentDialogue.lines[currentLineIndex].text.GetLocalizedStringAsync();
                    dialogueText.text = text;
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
            catch
            {
                Debug.Log("Show next line error");
            }
        }
    }
}
