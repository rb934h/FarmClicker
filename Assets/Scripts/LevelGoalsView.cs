using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class LevelGoalsView : Component
    {
        private Transform _parentTransform;
        private TMP_Text _levelGoalsText;
        
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
}