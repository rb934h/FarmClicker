using System.Collections;
using System.Collections.Generic;
using PointerObjects;
using Strategies;
using UnityEngine;
using VContainer;

public class TutorialManager : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private GameObject arrowPrefab; // Стрелка (например, UI-объект или 3D-объект)
    [SerializeField] private TutorialStep[] steps;

    private int currentStepIndex = -1;
    private GameObject currentArrow;
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
        currentStepIndex = -1;
        NextStep();
    }

    public void NextStep()
    {
        if (currentArrow) Destroy(currentArrow);

        currentStepIndex++;
        if (currentStepIndex >= steps.Length)
        {
            Debug.Log("Tutorial complete!");
            _dialogueManager.HideDialogueWindow();
            return;
        }

        var step = steps[currentStepIndex];
        step.onStepStart?.Invoke();

        if (currentStepIndex == 0)
        {
            _dialogueManager.ShowDialogue(step.dialogue);
        }
        else
        {
            _dialogueManager.NextLine();
        }
           

        if (step.target != null)
            CreateArrow(step.target);

        if (!step.waitForPlayerAction)
        {
            StartCoroutine(AutoAdvanceAfterDialogue(step));
        }
    }

    private IEnumerator AutoAdvanceAfterDialogue(TutorialStep step)
    {
        while (_dialogueManager != null &&
               step.dialogue != null &&
               _dialogueManager.gameObject.activeSelf)
        {
            yield return null; // ждём окончания диалога
        }

        CompleteCurrentStep();
    }

    private void CreateArrow(Transform target)
    {
        currentArrow = Instantiate(arrowPrefab, target.transform.parent);
        UpdateArrowPosition(target);
    }

    private void Update()
    {
        // if (currentArrow && steps.Length > 0 && currentStepIndex < steps.Length)
        // {
        //     var target = steps[currentStepIndex].target;
        //     if (target)
        //         UpdateArrowPosition(target);
        // }
    }

    private void UpdateArrowPosition(Transform target)
    {
        currentArrow.transform.position = target.position + new Vector3(0, 1.5f, 0);
    }

    private void CompleteCurrentStep()
    {
        if (currentStepIndex < 0 || currentStepIndex >= steps.Length) return;

        var step = steps[currentStepIndex];
        step.onStepComplete?.Invoke();

        if (currentArrow) Destroy(currentArrow);
        _dialogueManager.HideDialogueWindow();

        NextStep();
    }
}
