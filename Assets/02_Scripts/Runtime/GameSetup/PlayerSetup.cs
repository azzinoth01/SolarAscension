using SolarAscension;
using UnityEngine;

public class PlayerSetup : MonoBehaviour {
	[Header("Setup Components")]
	[SerializeField] private Player player;
	[SerializeField] private BuildVisualizer visualizer;
	[SerializeField] private RotationMarkerVisualizer markerVisualizer;
	[SerializeField] private Camera playerCamera;
	[SerializeField] private GridBehaviour gridBehaviourPrefab;
	[SerializeField] private AttachmentBehaviour attachmentBehaviourPrefab;
	[SerializeField] private TubeConfigurator tubeConfig;
	[SerializeField] private ModulePathfinder pathfinder;

	[Header("Grid Object Settings")]
	[SerializeField] private ModuleData[] moduleData;
	[SerializeField] private GridCoordinate startingCoordinate;

	[Header("Attachment Object Settings")]
	[SerializeField] private AttachmentData[] attachments;
	
	[Header("Selection Layer Settings")]
	[SerializeField] private LayerMask buildMask = -1;
	[SerializeField] private LayerMask moduleMask = -1;

	[Header("Economy Settings")]
	[SerializeField] private PlayerBilanz playerEconomy;

	private void Start() {
		BuildSystem.Initialize(moduleData, attachments);
		tubeConfig.Setup();
		GridBehaviourPool.Setup(gridBehaviourPrefab);
		AttachmentBehaviourPool.Setup(attachmentBehaviourPrefab);
		
		EconemySystemInfo.Instanz.StartEconomyThread();
		player.Setup(visualizer, markerVisualizer, playerCamera, buildMask, moduleMask, playerEconomy, BuildSystem.GetModuleData(GridObjectType.MainModule), startingCoordinate, pathfinder);
		playerCamera.GetComponent<OrbitalCamera>().SetUp(player);
	}
}