using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("UI Elements")]
        [FormerlySerializedAs("dialogueWindow")] [SerializeField] private GameObject _dialogueWindow;
        [FormerlySerializedAs("speakerNameText")] [SerializeField] private TMP_Text _speakerNameText;
        [FormerlySerializedAs("dialogueText")] [SerializeField] private TMP_Text _dialogueText;
        [FormerlySerializedAs("typingSpeed")] [SerializeField] private float _typingSpeed = 0.02f;
        [SerializeField] private AudioSource _dialogueSound;
    
        private Dialogue _currentDialogue;
        private int _currentLineIndex;
        private bool _isTyping;
        private Coroutine _typingCoroutine;
    

        public void ShowDialogue(Dialogue dialogue)
        {
            if (dialogue == null || dialogue.lines.Length == 0) return;

            _currentDialogue = dialogue;
            _currentLineIndex = 0;
            _dialogueWindow.SetActive(true);
            ShowCurrentLine();
        }
    
        public void HideDialogueWindow()
        {
            _dialogueWindow.SetActive(false);
            _currentDialogue = null;
        }
    
        private async void ShowCurrentLine()
        {
            try
            {
                if (_currentDialogue == null) return;

                var line = _currentDialogue.lines[_currentLineIndex];
                var speakerName = await line.SpeakerName.GetLocalizedStringAsync();
        
                _speakerNameText.text = speakerName;
        
                if (_typingCoroutine != null) StopCoroutine(_typingCoroutine);
                var text = await line.Text.GetLocalizedStringAsync();
                _typingCoroutine = StartCoroutine(TypeLine(text));
            }
            catch
            {
                Debug.Log("Show current line error");
            }
        }
    
        private IEnumerator TypeLine(string line)
        {
            _isTyping = true;
            _dialogueText.text = "";
        
            _dialogueSound.Play();

            foreach (char c in line)
            {
                _dialogueText.text += c;
                yield return new WaitForSeconds(_typingSpeed);
            }

            _isTyping = false;
            _dialogueSound.Stop();
        }
    
        public async void NextLine()
        {
            try
            {
                if (_isTyping)
                {
                    StopCoroutine(_typingCoroutine);
                    var text = await _currentDialogue.lines[_currentLineIndex].Text.GetLocalizedStringAsync();
                    _dialogueText.text = text;
                    _isTyping = false;
                    return;
                }

                _currentLineIndex++;

                if (_currentLineIndex >= _currentDialogue.lines.Length)
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
