#if UNITY_EDITOR
using UnityEditor;
#endif
using SolarAscension;
using UnityEngine;

public class Player : MonoBehaviour {
	public PlayerState State { get; private set; }
	public PlayerInput Input { get; private set; }
	public BuildingGrid BuildingGrid { get; private set; }

	public void Setup(BuildVisualizer visualizer, RotationMarkerVisualizer markerVisualizer, Camera cam, LayerMask buildMask, LayerMask moduleMask, PlayerBilanz playerEconomy, ModuleData startingModuleData, GridCoordinate startingCoordinate, ModulePathfinder pathfinder) {
		Input = new PlayerInput(cam, buildMask, moduleMask);
			
		GridObject startObject = new Module();
		BuildingGrid = new BuildingGrid(playerEconomy.Player, startObject);
		pathfinder.SetUp(BuildingGrid);
		BlockHandler.SetGrid(BuildingGrid);

		BuildingHelper.TrySetBuildingID(startingModuleData.buildingId, out Building mainBuilding);
		mainBuilding.BuildingPlaced(playerEconomy.Player);
		startObject.Setup(mainBuilding, startingModuleData, startingCoordinate, GridOrientation.Identity);

		GridBehaviour startGridBehaviour = startObject.gridBehaviour;
			
		State = new PlayerState(this, startGridBehaviour, visualizer, markerVisualizer);
			
		BuildingGrid.SetPlayerState(State);
			
		cam.transform.GetComponent<OrbitalCamera>().SetInitialFocusTransform(startGridBehaviour.RenderTransform);
	}

	private void Update() {
		State.ProcessUpdate();
	}

	#if UNITY_EDITOR
	[Header("Gizmos")]
	public Vector3Int previewSize;
	public bool drawGridCubes;
	public bool drawGridWires;
	public bool drawLockCubes;
	public bool drawLockWires;
	public bool drawLinkCubes;
	public bool drawLinkWires;
	public bool drawLinkLabels;
	public bool drawStabilizerCubes;
	public bool drawStabilizerNumber;
	public bool drawBufferCubes;
	public bool drawBufferNumber;

	private void OnDrawGizmos() {
		if ( Input == null ) {
			return;
		}

		if ( drawGridCubes || drawGridWires ) {
			Gizmos.color = GizmoUtils.VolumeFillColor;
			for (int z = -previewSize.z; z <= previewSize.z; z++) {
				for (int y = -previewSize.y; y <= previewSize.y; y++) {
					for (int x = -previewSize.x; x <= previewSize.x; x++) {
						Vector3 pos = new GridCoordinate(x, y, z).ToWorldPositionCentered();
						Vector3 size = Vector3.one * BuildSystem.VolumeScale;

						if ( drawGridCubes ) {
							Gizmos.DrawCube(pos, size);
						}

						if ( drawGridWires ) {
							Gizmos.DrawWireCube(pos, size);
						}
					}
				}
			}
		}

		if ( drawLockCubes || drawLockWires ) {
			Gizmos.color = GizmoUtils.LockFillColor;
			for (int z = -previewSize.z; z <= previewSize.z; z++) {
				for (int y = -previewSize.y; y <= previewSize.y; y++) {
					for (int x = -previewSize.x; x <= previewSize.x; x++) {
						GridCoordinate coord = new GridCoordinate(x, y, z);
						if ( BuildingGrid.LockGrid.IsLocked(coord) ) {
							Vector3 pos = coord.ToWorldPositionCentered();
							Vector3 size = Vector3.one * BuildSystem.VolumeScale;
								
							if ( drawLockCubes ) {
								Gizmos.DrawCube(pos, size);
							}

							if ( drawLockWires ) {
								Gizmos.DrawWireCube(pos, size);
							}
						}
					}
				}
			}
		}

		if ( drawLinkCubes || drawLinkWires || drawLinkLabels ) {
			Gizmos.color = GizmoUtils.LinkFillColor;
			for (int z = -previewSize.z; z <= previewSize.z; z++) {
				for (int y = -previewSize.y; y <= previewSize.y; y++) {
					for (int x = -previewSize.x; x <= previewSize.x; x++) {
						GridCoordinate coord = new GridCoordinate(x, y, z);
						if ( BuildingGrid.LinkMap.HasLink(coord) ) {
							LinkDirection dir = BuildingGrid.LinkMap.GetLinkData(coord).linkDirection;

							if ( dir == 0 ) {
								continue;
							}
								
							Vector3 pos = coord.ToWorldPositionCentered();
							Vector3 size = Vector3.one * BuildSystem.VolumeScale;
								
							if ( drawLinkCubes ) {
								Gizmos.DrawCube(pos, size);
							}

							if ( drawLinkWires ) {
								Gizmos.DrawWireCube(pos, size);
							}

							if ( drawLinkLabels ) {
								Handles.Label(pos, dir.ToString(), GizmoUtils.LabelStyle);
							}
						}
					}
				}
			}
		}

		if ( drawStabilizerCubes || drawStabilizerNumber ) {
			Gizmos.color = GizmoUtils.StabilizerFillColor;
			for (int z = -previewSize.z; z <= previewSize.z; z++) {
				for (int y = -previewSize.y; y <= previewSize.y; y++) {
					for (int x = -previewSize.x; x <= previewSize.x; x++) {
						GridCoordinate coord = new GridCoordinate(x, y, z);
						if ( BuildingGrid.StabilizerMap.DebugTryGetStabilizer(coord, out Stabilizer stabilizer) ) {
							if ( stabilizer.DebugStatus == 0 ) {
								continue;
							}
								
							Vector3 pos = coord.ToWorldPositionCentered();
							Vector3 size = Vector3.one * BuildSystem.VolumeScale;
								
							if ( drawStabilizerCubes ) {
								Gizmos.DrawCube(pos, size);
							}

							if ( drawStabilizerNumber ) {
								Handles.Label(pos, stabilizer.DebugStatus.ToString(), GizmoUtils.StabilizerLabelStyle);
							}
						}
					}
				}
			}
		}
			
		if ( drawBufferCubes || drawBufferNumber ) {
			Gizmos.color = GizmoUtils.BufferFillColor;
				
			for (int z = -previewSize.z; z <= previewSize.z; z++) {
				for (int y = -previewSize.y; y <= previewSize.y; y++) {
					for (int x = -previewSize.x; x <= previewSize.x; x++) {
						GridCoordinate coord = new GridCoordinate(x, y, z);
							
						if ( BuildingGrid.BufferMap.DebugTryGetBufferCount(coord, out byte count) ) {
							if ( count == 0 ) {
								continue;
							}
							Vector3 pos = coord.ToWorldPositionCentered();
							Vector3 size = Vector3.one * BuildSystem.VolumeScale;
								
							if ( drawBufferCubes ) {
								Gizmos.DrawCube(pos, size);
							}

							if ( drawBufferNumber ) {
								Handles.Label(pos, count.ToString(), GizmoUtils.BufferLabelStyle);
							}
						}
					}
				}
			}
		}
	}
	#endif
}