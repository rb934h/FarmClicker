using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialStep
{
    public string StepName;
    public Dialogue Dialogue; // Диалог для этого шага
    public Transform Target; // Объект, на который указывает стрелка
    public UnityEvent OnStepStart; // Что выполнить при начале шага
    public UnityEvent OnStepComplete; // Что выполнить при завершении шага
    public bool WaitForPlayerAction = true; // Нужно ли ждать действия игрока
}
