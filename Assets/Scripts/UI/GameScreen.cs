using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
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
        [SerializeField] private TableReference _tableReferenceForCoinsText;
        private LevelGoalsView _levelGoalsView{ get; set; }
        private StringTable  _localizationTable;
        
        private async void Start()
        {
            var localizationTableName = "CollectableItems";
            _levelGoalsView = new LevelGoalsView(_levelGoalsUITransform, _levelGoalsText);
            _localizationTable = await LocalizationSettings.StringDatabase.GetTableAsync(localizationTableName);
        }

        public void SetLevelGoals(List<LevelGoal> levelGoals, int requiredCoins)
        {
            _levelGoalsPanelAnimator.enabled = true;
            
            DOVirtual.DelayedCall(_levelGoalsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length * .5f, () =>
            {
                foreach (var levelDataGoal in levelGoals)
                {
                    _levelGoalsView.SetGoal(levelDataGoal.itemData.itemName.GetLocalizedString(), levelDataGoal.requiredCount);
                }

                if (requiredCoins > 0)
                {
                    var localizationKeyForCoin = "Coin_Key";
                    _levelGoalsView.SetGoal(_localizationTable.GetEntry(localizationKeyForCoin).GetLocalizedString(), requiredCoins);
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