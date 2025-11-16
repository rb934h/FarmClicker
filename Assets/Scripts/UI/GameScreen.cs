using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using Convert = DefaultNamespace.Convert;

namespace UI
{
    public class GameScreen : ScreenBase
    {
        [SerializeField] private Animator _levelGoalsPanelAnimator;
        [SerializeField] private Transform _levelGoalsUITransform;
        [SerializeField] private TMP_Text _levelGoalsText;
        [SerializeField] private Convert _convert;
        [SerializeField] private SelectItemPanel _selectItemPanel;
        
        private LevelGoalsView _levelGoalsView{ get; set; }

        public event Action ConvertHided;
        
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

        public void ShowConvert(string message, string sender)
        {
            _convert.SetMessage(message);
            _convert.SetMessageSender(sender);
            _convert.Show();
        }
        public void HideConvert()
        {
            _convert.Hide();
            ConvertHided?.Invoke();
        }
    }
}