using System.Collections;
using System.Collections.Generic;
using Strategies;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

public class TutorialManager : MonoBehaviour
{
    [Header("Setup")] 
    [FormerlySerializedAs("arrowPrefab")] [SerializeField] private GameObject _arrowPrefab;
    [FormerlySerializedAs("steps")] [SerializeField] private TutorialStep[] _steps;

    private int _currentStepIndex = -1;
    private GameObject _currentArrow;
    private DialogueManager _dialogueManager;
    private IEnumerable<IPointerObjectInteractStrategy> _interactStrategy;

    [Inject]
    public void Construct(DialogueManager dialogueManager, IEnumerable<IPointerObjectInteractStrategy> interactStrategies)
    {
        _dialogueManager = dialogueManager;
        _interactStrategy = interactStrategies;
    }

    private void Start()
    {
        foreach (var pointerObjectInteractStrategy in _interactStrategy)
        {
            pointerObjectInteractStrategy.OnComplete += NextStep;
        }
    }

    public void StartTutorial()
    {
        _currentStepIndex = -1;
        NextStep();
    }

    private void NextStep()
    {
        if (_currentArrow) Destroy(_currentArrow);

        _currentStepIndex++;
        if (_currentStepIndex >= _steps.Length)
        {
            _dialogueManager.HideDialogueWindow();
            return;
        }

        var step = _steps[_currentStepIndex];
        step.OnStepStart?.Invoke();

        if (_currentStepIndex == 0)
        {
            _dialogueManager.ShowDialogue(step.Dialogue);
        }
        else
        {
            _dialogueManager.NextLine();
        }
        
        if (step.Target != null && _arrowPrefab != null)
            CreateArrow(step.Target);

        if (!step.WaitForPlayerAction)
        {
            StartCoroutine(AutoAdvanceAfterDialogue(step));
        }
    }

    private IEnumerator AutoAdvanceAfterDialogue(TutorialStep step)
    {
        while (_dialogueManager != null &&
               step.Dialogue != null &&
               _dialogueManager.gameObject.activeSelf)
        {
            yield return null; 
        }

        CompleteCurrentStep();
    }

    private void CreateArrow(Transform target)
    {
        _currentArrow = Instantiate(_arrowPrefab, target.transform.parent);
        UpdateArrowPosition(target);
    }

    private void UpdateArrowPosition(Transform target)
    {
        _currentArrow.transform.position = target.position + new Vector3(0, 1.5f, 0);
    }

    private void CompleteCurrentStep()
    {
        if (_currentStepIndex < 0 || _currentStepIndex >= _steps.Length) return;

        var step = _steps[_currentStepIndex];
        step.OnStepComplete?.Invoke();

        if (_currentArrow) Destroy(_currentArrow);
        _dialogueManager.HideDialogueWindow();

        NextStep();
    }
}
