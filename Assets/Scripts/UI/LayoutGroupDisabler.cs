using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LayoutGroupDisabler : MonoBehaviour
    {
        [SerializeField] private LayoutGroup[] _layoutGroups;

        public void DisableLayoutGroups()
        {
            foreach (var layoutGroup in _layoutGroups)
            {
                layoutGroup.enabled = false;
            }
        }
    }
}