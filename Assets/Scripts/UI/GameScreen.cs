using System.Collections.Generic;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UI;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameScreen : ScreenBase
    {
        [SerializeField] private Animator _levelGoalsPanelAnimator;
        [SerializeField] private Transform _levelGoalsUITransform;
        [SerializeField] private TMP_Text _levelGoalsText;
        [SerializeField] private SelectItemPanel _selectItemPanel;
        
        private LevelGoalsView _levelGoalsView{ get; set; }
        
        private void Start()
        {
            _levelGoalsView = new LevelGoalsView(_levelGoalsUITransform, _levelGoalsText);
        }

        public void SetLevelGoals(List<LevelGoal> levelGoals)
        {
            _levelGoalsPanelAnimator.enabled = true;
            
            DOVirtual.DelayedCall(_levelGoalsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length * .5f, () =>
            {
                foreach (var levelDataGoal in levelGoals)
                {
                    _levelGoalsView.SetGoal(levelDataGoal.itemData.name, levelDataGoal.requiredCount);
                }
            });
        }

        public void SetAvailableItems(List<CollectableItemData> availableItems)
        {
            foreach (var collectableItemData in availableItems)
            {
                _selectItemPanel.AddButton(collectableItemData);
            }
        }
    }
}