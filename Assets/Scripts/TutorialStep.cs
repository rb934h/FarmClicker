using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TutorialStep
{
    public string stepName;
    public Dialogue dialogue;                 // Диалог для этого шага
    public Transform target;                  // Объект, на который указывает стрелка
    public UnityEvent onStepStart;            // Что выполнить при начале шага
    public UnityEvent onStepComplete;         // Что выполнить при завершении шага
    public bool waitForPlayerAction = true;   // Нужно ли ждать действия игрока
}