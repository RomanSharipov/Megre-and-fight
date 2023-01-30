using IJunior.TypedScenes;
using UnityEngine;
using UnityEngine.UI;

namespace MergeAndFight.Merge
{
    [RequireComponent(typeof(Button))]
    public class BattleSceneLoadButton : MonoBehaviour
    {
        [SerializeField] private CellsField _cellsField;
        [SerializeField] private ViewPanel _loadingScreenPanel;

        private Button _loadLevelButton;
        private Vector3 _initialPlacement = new Vector3(-10000f, -10000f, -10000f);

        private void Awake()
        {
            _loadLevelButton = GetComponent<Button>();
            _loadingScreenPanel.DisableView();
        }

        private void OnEnable()
        {
            _loadLevelButton.onClick.AddListener(OnloadLevelButtonClick);
        }

        private void OnDisable()
        {
            _loadLevelButton.onClick.RemoveListener(OnloadLevelButtonClick);
        }

        private void OnloadLevelButtonClick()
        {
            _loadingScreenPanel.EnableView();

            var mergeObjects = _cellsField.GetMergeObjects();

            foreach (var mergeObject in mergeObjects)
            {
                mergeObject.transform.position = _initialPlacement;
                DontDestroyOnLoad(mergeObject.gameObject);
            }

            FightScene.Load(mergeObjects);
        }
    }
}
