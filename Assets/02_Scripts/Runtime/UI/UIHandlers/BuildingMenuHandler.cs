using UnityEngine;
using UnityEngine.UIElements;

namespace SolarAscension {
    public class BuildingMenuHandler : MonoBehaviour {
        [SerializeField] ModuleData tubeData, scrapGathererData, solarModuleData, iceMinerData, oxygenModuleData;
        [SerializeField] ModuleData populationData, aquaponicData, biopolymereData;
        [Space, Header("Unlock Quests")]
        [SerializeField] Quest unlockScrapGatherer;
        [SerializeField] Quest unlockSolarModule, unlockIceMiner, unlockOxygen, unlockTier2;

        [SerializeField] Color defaultButtonColor;

        static VisualElement _root, _highlightFrame1, _highlightFrame2;
        UIDocumentLocalization _uIDocumentLocalization;

        VisualElement[] _tierDisplays = new VisualElement[2];
        Button[] _tierSelectors = new Button[3];

        Button _tubeButton, _scrapGathererButton, _solarButton, _iceMinerButton, _oxygenButton, _moveModeButton, _destroyModeButton;
        Button _populationButton, _aquaponicButton, _biopolymerButton;
        VisualElement _scrapGathererBlocker, _solarBlocker, _iceMinerBlocker, _oxygenBlocker, _populationBlocker, _aquaponicsBlocker, _biopolymerBlocker;

        bool _scrapGathererBlocked = true, _solarBlocked = true, _iceMinerBlocked = true, _oxygenBlocked = true, _tier2Blocked = true;

        public static VisualElement Root { get { return _root; } }

        void OnEnable() {
            var document = GetComponent<UIDocument>();
            _root = document.rootVisualElement.Q<VisualElement>("build_menu");

            _uIDocumentLocalization = GetComponent<UIDocumentLocalization>();
            _uIDocumentLocalization.onCompleted += Init;
        }

        void OnDisable() => UnregisterCallbacks();

        void Update() => UpdateModeButtons();

        public void Init() {
            var document = GetComponent<UIDocument>();
            _root = document.rootVisualElement.Q<VisualElement>("build_menu");

            _tierDisplays = new VisualElement[3];
            _tierDisplays[0] = _root.Q<VisualElement>("tier1_display");
            _tierDisplays[1] = _root.Q<VisualElement>("tier2_display");
            _tierDisplays[2] = _root.Q<VisualElement>("tier3_display");
            _tierDisplays[2].SetVisible(false);

            _tierSelectors[0] = _root.Q<Button>("button_tier1Selector");
            _tierSelectors[1] = _root.Q<Button>("button_tier2Selector");
            _tierSelectors[2] = _root.Q<Button>("button_tier3Selector");
            _tierSelectors[2].SetVisible(false);

            _highlightFrame1 = _root.Q<VisualElement>("highlightFrame_tier1");
            _highlightFrame1.SetActive(false);
            _highlightFrame2 = _root.Q<VisualElement>("highlightFrame_tier2");
            _highlightFrame2.SetActive(false);
            
            _tubeButton = _root.Q<Button>("button_tube");
            _tubeButton.SetBackgroundSprite(tubeData?.descriptionData.moduleIcon);
            _scrapGathererButton = _root.Q<Button>("button_scrapGatherer");
            _scrapGathererButton.SetBackgroundSprite(scrapGathererData?.descriptionData.moduleIcon);
            _solarButton = _root.Q<Button>("button_solarModule");
            _solarButton.SetBackgroundSprite(solarModuleData?.descriptionData.moduleIcon);
            _iceMinerButton = _root.Q<Button>("button_iceMiner");
            _iceMinerButton.SetBackgroundSprite(iceMinerData?.descriptionData.moduleIcon);
            _oxygenButton = _root.Q<Button>("button_oxygenModule");
            _oxygenButton.SetBackgroundSprite(oxygenModuleData?.descriptionData.moduleIcon);

            _populationButton = _root.Q<Button>("button_populationModule");
            _populationButton.SetBackgroundSprite(populationData?.descriptionData.moduleIcon);
            _aquaponicButton = _root.Q<Button>("button_aquaponic");
            _aquaponicButton.SetBackgroundSprite(aquaponicData?.descriptionData.moduleIcon);
            _biopolymerButton = _root.Q<Button>("button_biopolymere");
            _biopolymerButton.SetBackgroundSprite(biopolymereData?.descriptionData.moduleIcon);

            _moveModeButton = _root.Q<Button>("button_moveModeButton");
            _destroyModeButton = _root.Q<Button>("button_destroyModeButton");

            _scrapGathererBlocker = _root.Q<VisualElement>("scrapGatherer_inputBlocker");
            _scrapGathererBlocker.SetActive(_scrapGathererBlocked);
            _solarBlocker = _root.Q<VisualElement>("solarModule_inputBlocker");
            _solarBlocker.SetActive(_solarBlocked);
            _iceMinerBlocker = _root.Q<VisualElement>("iceMiner_inputBlocker");
            _iceMinerBlocker.SetActive(_iceMinerBlocked);
            _oxygenBlocker = _root.Q<VisualElement>("oxygenModule_inputBlocker");
            _oxygenBlocker.SetActive(_oxygenBlocked);
            _populationBlocker = _root.Q<VisualElement>("populationModule_inputBlocker");
            _populationBlocker.SetActive(_tier2Blocked);
            _aquaponicsBlocker = _root.Q<VisualElement>("aquaponic_inputBlocker");
            _aquaponicsBlocker.SetActive(_tier2Blocked);
            _biopolymerBlocker = _root.Q<VisualElement>("biopolymere_inputBlocker");
            _biopolymerBlocker.SetActive(_tier2Blocked);

            SetTierDisplay(0);
            _tierSelectors[0].RemoveFromClassList("tier-selector-selected");

            UnregisterCallbacks();
            RegisterCallbacks();
        }

        void UpdateModeButtons() {
            if(_destroyModeButton == null || _moveModeButton == null) {
                return;
            }
            switch (MenuManager.Instance.player.State.CurrentStateType)
            {
                case InteractionState.BuildMoving:
                    _moveModeButton.AddToClassList("build-menu-button-selected");
                    _destroyModeButton.RemoveFromClassList("build-menu-button-selected");
                    break;
                case InteractionState.BuildDeleting:
                    _destroyModeButton.AddToClassList("build-menu-button-selected");
                    _moveModeButton.RemoveFromClassList("build-menu-button-selected");
                    break;
                default:
                    _destroyModeButton.RemoveFromClassList("build-menu-button-selected");
                    _moveModeButton.RemoveFromClassList("build-menu-button-selected");
                    break;
            }
        }

        public void SetTierDisplay(int activeIndex) {
            for (int i = 0; i < _tierDisplays.Length; i++) {
                _tierDisplays[i].SetVisible(i == activeIndex);
                if (i == activeIndex) {
                    _tierSelectors[i].AddToClassList("tier-selector-selected");
                }
                else {
                    _tierSelectors[i].RemoveFromClassList("tier-selector-selected");
                }
            }
        }

        public void SkipTutorial()
        {
            _scrapGathererBlocked = false;
            _scrapGathererBlocker.SetActive(false);
            _solarBlocked = false;
            _solarBlocker.SetActive(false);
            _iceMinerBlocked = false;
            _iceMinerBlocker.SetActive(false);
            _oxygenBlocked = false;
            _oxygenBlocker.SetActive(false);
            _tier2Blocked = false;
            _populationBlocker.SetActive(false);
            _aquaponicsBlocker.SetActive(false);
            _biopolymerBlocker.SetActive(false);
        }

        void HandleDeleteState() {
            if (MenuManager.Instance.player.State.CurrentStateType == InteractionState.BuildDeleting) {
                ((BuildDeletingState)MenuManager.Instance.player.State.CurrentState).PerformDeletion();
            }
            else {
                MenuManager.Instance.player.State.OverrideState(InteractionState.BuildDeleting);
            }
        }

        void ToggleHightlightFrame1(IMouseEvent e) => _highlightFrame1.SetActive(e is MouseEnterEvent);
        void ToggleHightlightFrame2(IMouseEvent e) => _highlightFrame2.SetActive(e is MouseEnterEvent);

        void RegisterCallbacks() {
            //Tier1
            _tubeButton.clicked += delegate { MenuManager.Instance.player.State.SetBuildStateWithData(tubeData); };
            _scrapGathererButton.clicked += delegate { if (!_scrapGathererBlocked) { MenuManager.Instance.player.State.SetBuildStateWithData(scrapGathererData); } };
            _solarButton.clicked += delegate {if (!_solarBlocked) {  MenuManager.Instance.player.State.SetBuildStateWithData(solarModuleData); } };
            _iceMinerButton.clicked += delegate { if (!_iceMinerBlocked) { MenuManager.Instance.player.State.SetBuildStateWithData(iceMinerData); } };
            _oxygenButton.clicked += delegate { if (!_oxygenBlocked) { MenuManager.Instance.player.State.SetBuildStateWithData(oxygenModuleData); } };
            //Tier2
            _populationButton.clicked += delegate {if (!_tier2Blocked) { MenuManager.Instance.player.State.SetBuildStateWithData(populationData); } };
            _aquaponicButton.clicked += delegate { if (!_tier2Blocked) { MenuManager.Instance.player.State.SetBuildStateWithData(aquaponicData); } };
            _biopolymerButton.clicked += delegate {if (!_tier2Blocked) { MenuManager.Instance.player.State.SetBuildStateWithData(biopolymereData); } };
            //Tier3

            _moveModeButton.clicked += delegate { MenuManager.Instance.player.State.OverrideState(InteractionState.BuildMoving); };
            _destroyModeButton.clicked += HandleDeleteState;

            _tierSelectors[0].RegisterCallback<MouseEnterEvent>(ToggleHightlightFrame1);
            _tierSelectors[1].RegisterCallback<MouseEnterEvent>(ToggleHightlightFrame2);
            _tierSelectors[0].RegisterCallback<MouseLeaveEvent>(ToggleHightlightFrame1);
            _tierSelectors[1].RegisterCallback<MouseLeaveEvent>(ToggleHightlightFrame2);

            unlockScrapGatherer.goal.OnCompletion += delegate { _scrapGathererBlocked = false; _scrapGathererBlocker.SetActive(false); };
            unlockSolarModule.goal.OnCompletion += delegate { _solarBlocked = false; _solarBlocker.SetActive(false); };
            unlockIceMiner.goal.OnCompletion += delegate { _iceMinerBlocked = false; _iceMinerBlocker.SetActive(false); };
            unlockOxygen.goal.OnCompletion += delegate { _oxygenBlocked = false; _oxygenBlocker.SetActive(false); };
            unlockTier2.goal.OnCompletion += delegate { _tier2Blocked = false; _populationBlocker.SetActive(false); _aquaponicsBlocker.SetActive(false); _biopolymerBlocker.SetActive(false); };
        }

        void UnregisterCallbacks() {
            //Tier1
            _tubeButton.clicked -= delegate { MenuManager.Instance.player.State.SetBuildStateWithData(tubeData); };
            _scrapGathererButton.clicked -= delegate { if (!_scrapGathererBlocked) { MenuManager.Instance.player.State.SetBuildStateWithData(scrapGathererData); } };
            _solarButton.clicked -= delegate { if (!_solarBlocked) { MenuManager.Instance.player.State.SetBuildStateWithData(solarModuleData); } };
            _iceMinerButton.clicked -= delegate { if (!_iceMinerBlocked) { MenuManager.Instance.player.State.SetBuildStateWithData(iceMinerData); } };
            _oxygenButton.clicked -= delegate { if (!_oxygenBlocked) { MenuManager.Instance.player.State.SetBuildStateWithData(oxygenModuleData); } };
            //Tier2
            _populationButton.clicked -= delegate { if (!_tier2Blocked) { MenuManager.Instance.player.State.SetBuildStateWithData(populationData); } };
            _aquaponicButton.clicked -= delegate { if (!_tier2Blocked) { MenuManager.Instance.player.State.SetBuildStateWithData(aquaponicData); } };
            _biopolymerButton.clicked -= delegate { if (!_tier2Blocked) { MenuManager.Instance.player.State.SetBuildStateWithData(biopolymereData); } };
            //Tier3

            _moveModeButton.clicked -= delegate { MenuManager.Instance.player.State.OverrideState(InteractionState.BuildMoving); };
            _destroyModeButton.clicked -= HandleDeleteState;

            _tierSelectors[0].UnregisterCallback<MouseEnterEvent>(ToggleHightlightFrame1);
            _tierSelectors[1].UnregisterCallback<MouseEnterEvent>(ToggleHightlightFrame2);
            _tierSelectors[0].UnregisterCallback<MouseLeaveEvent>(ToggleHightlightFrame1);
            _tierSelectors[1].UnregisterCallback<MouseLeaveEvent>(ToggleHightlightFrame2);
        }
    }
}

