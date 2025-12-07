using TMPro;
using UnityEngine;

public class LevelGoalsView : Component
{
    private readonly Transform _parentTransform;
    private readonly TMP_Text _levelGoalsText;
        
    public LevelGoalsView(Transform parentTransform, TMP_Text levelGoalsText)
    {
        _parentTransform = parentTransform;
        _levelGoalsText = levelGoalsText;
    }
    
    public void SetGoal(string goalName, int count)
    {
        _levelGoalsText.text = goalName + " " + count;
        Instantiate(_levelGoalsText, _parentTransform);
    }
}