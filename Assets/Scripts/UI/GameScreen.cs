using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
        
        
        private void Start()
        {
            _levelGoalsView = new LevelGoalsView(_levelGoalsUITransform, _levelGoalsText);
        }

        public void SetLevelGoals(List<LevelGoal> levelGoals, int requiredCoins)
        {
            _levelGoalsPanelAnimator.enabled = true;
            
            DOVirtual.DelayedCall(_levelGoalsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length * .5f, () =>
            {
                foreach (var levelDataGoal in levelGoals)
                {
                    _levelGoalsView.SetGoal(levelDataGoal.itemData.itemName, levelDataGoal.requiredCount);
                }
                
                if(requiredCoins > 0)
                    _levelGoalsView.SetGoal("Монеты ", requiredCoins);
            });
        }

        public void SetAvailableItems(List<CollectableItemData> availableItems)
        {
            foreach (var collectableItemData in availableItems)
            {
                _selectItemPanel.AddButton(collectableItemData);
            }
        }

        public async UniTask ShowConvert(string message, string sender)
        {
            _convert.SetMessage(message);
            _convert.SetMessageSender(sender);
            await _convert.Show();
        }
        public async UniTask HideConvert()
        {
            await _convert.Hide(); 
        }
    }
}