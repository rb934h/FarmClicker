using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameScreen : ScreenBase
    {
        [SerializeField] private Animator _levelGoalsPanelAnimator;
        [SerializeField] private Transform _levelGoalsUITransform;
        [SerializeField] private TMP_Text _levelGoalsText;
        
        private LevelGoalsView _levelGoalsView{ get; set; }
        
        private void Start()
        {
            _levelGoalsView = new LevelGoalsView(_levelGoalsUITransform, _levelGoalsText);
        }

        public void SetLevelGoals(List<LevelGoal> levelGoals)
        {
            _levelGoalsPanelAnimator.enabled = true;
            foreach (var levelDataGoal in levelGoals)
            {
                _levelGoalsView.SetGoal(levelDataGoal.itemData.name, levelDataGoal.requiredCount);
            }
        }
    }
}