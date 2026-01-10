using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Level;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using Convert = UI.Convert;

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
        
        public Convert Convert => _convert;
        
        private async void Start()
        {
            var localizationTableName = "CollectableItems";
            _levelGoalsView = new LevelGoalsView(_levelGoalsUITransform, _levelGoalsText);
            _localizationTable = await LocalizationSettings.StringDatabase.GetTableAsync(localizationTableName);
        }

        public void SetLevelGoals(List<LevelGoal> levelGoals, int requiredCoins)
        {
            _levelGoalsPanelAnimator.enabled = true;

            DOVirtual.DelayedCall(
                _levelGoalsPanelAnimator.GetCurrentAnimatorClipInfo(0).Length * 0.5f,
                () => SetGoalsAsync(levelGoals, requiredCoins).Forget()
            );
        }

        private async UniTask SetGoalsAsync(List<LevelGoal> levelGoals, int requiredCoins)
        {
            foreach (var levelDataGoal in levelGoals)
            {
                var itemName = await levelDataGoal.itemData.itemName
                    .GetLocalizedStringAsync();

                _levelGoalsView.SetGoal(itemName, levelDataGoal.requiredCount);
            }

            if (requiredCoins > 0)
            {
                var localizationKeyForCoin = "Coin_Key";
                _levelGoalsView.SetGoal(_localizationTable.GetEntry(localizationKeyForCoin).GetLocalizedString(),
                    requiredCoins);
            }
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
        }
    }
}