using System.Collections.Generic;
using UnityEngine;

namespace MVC.Controllers
{
    /// <summary>
    /// Root controller responsible for changing game phases with SubControllers.
    /// </summary>
    public class RootController : Singleton<RootController>
    {
        // SubControllers types.
        public enum ControllerTypeEnum
        {
            TopPanel,
            VictoryPanel,
            FailPanel,
            TapToBeginPanel,
            LevelChooseController
        }

        // References to the subcontrollers.
        [Header("Controllers")]
        [SerializeField]
        private TopPanelController topPanelController;
        [SerializeField]
        private VictoryPanelController victoryPanelController;
        [SerializeField]
        private FailPanelController failPanelController;
        [SerializeField]
        private TapToBeginController tapToBeginController;
        [SerializeField]
        private LevelChooseController levelChooseController;

        /// <summary>
        /// Unity method called on first frame.
        /// </summary>
        private void Start()
        {
            topPanelController.root = this;
            victoryPanelController.root = this;
            failPanelController.root = this;
            tapToBeginController.root = this;
            levelChooseController.root = this;
            
            EventManager.Instance.chooseLevelButtonEvent.AddListener(delegate { SwitchToController(ControllerTypeEnum.LevelChooseController); });
            EventManager.Instance.nextLevelButtonEvent.AddListener(delegate { LevelManager.Instance.ChangeLevel(LevelManager.Instance.getData.Level + 1); });
            EventManager.Instance.retryLevelButtonEvent.AddListener(delegate { LevelManager.Instance.ChangeLevel(LevelManager.Instance.getData.Level); });
            
            SetupLevelChoosePanel();

            
            DisengageController(ControllerTypeEnum.VictoryPanel);
            DisengageController(ControllerTypeEnum.FailPanel);
            DisengageController(ControllerTypeEnum.LevelChooseController);
        }

        /// <summary>
        /// Method used by subcontrollers to change UI state.
        /// </summary>
        /// <param name="controller">Controller type.</param>
        public void EngageController(ControllerTypeEnum controller)
        {
            // Enabling subcontroller based on type.
            switch (controller)
            {
                case ControllerTypeEnum.TopPanel:
                    topPanelController.EngageController();
                    break;
                case ControllerTypeEnum.VictoryPanel:
                    victoryPanelController.EngageController();
                    break;
                case ControllerTypeEnum.FailPanel:
                    failPanelController.EngageController();
                    break;
                case ControllerTypeEnum.TapToBeginPanel:
                    tapToBeginController.EngageController();
                    break;
                case ControllerTypeEnum.LevelChooseController:
                    levelChooseController.EngageController();
                    break;
                default:
                    break;
            }
        }
        
        /// <summary>
        /// Method used by subcontrollers to change UI state.
        /// </summary>
        /// <param name="controller">Controller type.</param>
        public void DisengageController(ControllerTypeEnum controller)
        {
            // Enabling subcontroller based on type.
            switch (controller)
            {
                case ControllerTypeEnum.TopPanel:
                    topPanelController.DisengageController();
                    break;
                case ControllerTypeEnum.VictoryPanel:
                    victoryPanelController.DisengageController();
                    break;
                case ControllerTypeEnum.FailPanel:
                    failPanelController.DisengageController();
                    break;
                case ControllerTypeEnum.TapToBeginPanel:
                    tapToBeginController.DisengageController();
                    break;
                case ControllerTypeEnum.LevelChooseController:
                    levelChooseController.DisengageController();
                    break;
                default:
                    break;
            }
        }

        public void SwitchToController(ControllerTypeEnum controller)
        {
            DisengageAllControllers();
            
            EngageController(controller);
        }

        private void DisengageAllControllers()
        {
            topPanelController.DisengageController();
            victoryPanelController.DisengageController();
            failPanelController.DisengageController();
            tapToBeginController.DisengageController();
            levelChooseController.DisengageController();
        }

        public void SetupTopPanel(LevelData levelData)
        {
            topPanelController.SetupView(levelData);
        }
        
        public void SetupVictoryPanel(LevelData levelData)
        {
            victoryPanelController.SetupView(levelData);
        }
        
        public void SetupFailPanel(LevelData levelData)
        {
            failPanelController.SetupView(levelData);
        }

        public void SetupLevelChoosePanel()
        {
            levelChooseController.SetupView();
        }
    }
}
