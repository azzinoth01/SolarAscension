using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    [DefaultExecutionOrder(-1000)]
    public class MenuManager : MonoBehaviour {
        private static MenuManager _instance;
        public static MenuManager Instance { get { return _instance; } }

        public Player player;
        public PostProcessController postProcessController;
        [Space]

        public IngameMenuHandler ingameMenuHandler;
        public SettingsMenuHandler settingsMenuHandler;
        public CameraMenuHandler cameraMenuHandler;
        public BuildingMenuHandler buildingMenuHandler;
        public PauseMenuHandler pauseMenuHandler;
        public EconomyDisplayHandler economyDisplayHandler;
        public ModulePopupHandler modulePopupHandler;
        public ModalDisplayHandler modalDisplayHandler;
        public TooltipHandler tooltipHandler;
        public WarningDisplayHandler warningDisplayHandler;
        public StatisticsDisplayHandler statisticsDisplayHandler;
        public FlyingTextHandler flyingTextHandler;
        public QuestUIHandler questUIHandler;
        public DialogueHandler dialogueHandler;
        public DiplomacyDisplayHandler diplomacyDisplayHandler;
        public BuildingCostHandler buildingCostHandler;

        public bool paused;

        private void Awake() {
            if (_instance != null && _instance != this) {
                Destroy(this.gameObject);
            }
            else {
                _instance = this;
            }
        }
    }
}

