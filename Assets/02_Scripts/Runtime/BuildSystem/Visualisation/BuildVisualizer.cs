using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildVisualizer : MonoBehaviour {
	[SerializeField] private Camera cam;
	[SerializeField] private Mesh selectionPlaneMesh;
	[SerializeField] private Mesh moduleLinkMesh, planeLinkMesh, overlayMesh;
	[SerializeField] private Material selectionPlaneMaterial, moduleLinksMaterial, planeLinksMaterial, planeBlocksMaterial, overlayMaterial, overlayBlockMaterial;
	[SerializeField] private float previewFactor, alphaFactor = 0.6f;
	[SerializeField] private Color validColor, invalidColor, moveColor, deleteColor, hoverColor, selectColor, overlayReceiverColor, overlaySourceColor, overlayMissingColor;
	[SerializeField] private LayerMask buildLayer;
	
	private static BuildingGrid _grid;
	private static MaterialPropertyBlock _mpb;

	private static DrawCommandInstanced _linkCommand;
	private static int _linksToDraw;

	private static DrawCommandInstanced _planeLinkCommand;
	private static int _planeLinksToDraw;
		
	private static DrawCommand[] _drawCommands;
	private static int _commandsToDraw;

	private static DrawCommandInstanced _overlayDistributionReceivers;
	private static int _overlayReceiversToDraw;

	private static DrawCommandInstanced _overlayDistributionMissing;
	private static int _overlayMissingToDraw;
		
	private static DrawCommandInstanced _overlayDistributionSource;
	private static int _overlaySourceToDraw;

	private static DrawCommandInstanced _planeBlockedCommand;
	private static int _planeBlockCommandsToDraw;

	private static DrawCommandInstanced _overlayBlockedCommands;
	private static int _overlayBlocksToDraw;

	private static readonly Collider[] NeighborColliders = new Collider[100];
	private static LayerMask _buildLayer;
	
	private static readonly int PreviewColor = Shader.PropertyToID("_PreviewColor");
	private static readonly int PreviewFactor = Shader.PropertyToID("_PreviewFactor");
	private static readonly int OverlayColor = Shader.PropertyToID("_OverlayColor");
	private static readonly int AlphaFactor = Shader.PropertyToID("_AlphaFactor");
	private static readonly int CursorPos = Shader.PropertyToID("_CursorPos");

	private void Awake() {
		_drawCommands = new DrawCommand[50];
		for (int i = 0; i < 50; i++) {
			_drawCommands[i] = new DrawCommand();
		}
		_commandsToDraw = 0;

		_linkCommand = new DrawCommandInstanced();
		_linkCommand.mesh = moduleLinkMesh;
		_linkCommand.worldMatrices = new Matrix4x4[1024];
		_linksToDraw = 0;

		_planeLinkCommand = new DrawCommandInstanced();
		_planeLinkCommand.mesh = planeLinkMesh;
		_planeLinkCommand.worldMatrices = new Matrix4x4[1024];
		_planeLinksToDraw = 0;
		
		_planeBlockedCommand = new DrawCommandInstanced();
		_planeBlockedCommand.mesh = planeLinkMesh;
		_planeBlockedCommand.worldMatrices = new Matrix4x4[1024];
		_planeBlockCommandsToDraw = 0;

		_overlayDistributionReceivers = new DrawCommandInstanced();
		_overlayDistributionReceivers.mesh = overlayMesh;
		_overlayDistributionReceivers.worldMatrices = new Matrix4x4[1024];
		_overlayReceiversToDraw = 0;
			
		_overlayDistributionMissing = new DrawCommandInstanced();
		_overlayDistributionMissing.mesh = overlayMesh;
		_overlayDistributionMissing.worldMatrices = new Matrix4x4[1024];
		_overlayMissingToDraw = 0;
			
		_overlayDistributionSource = new DrawCommandInstanced();
		_overlayDistributionSource.mesh = overlayMesh;
		_overlayDistributionSource.worldMatrices = new Matrix4x4[1024];
		_overlaySourceToDraw = 0;

		_overlayBlockedCommands = new DrawCommandInstanced();
		_overlayBlockedCommands.mesh = overlayMesh;
		_overlayBlockedCommands.worldMatrices = new Matrix4x4[1024];
		_overlayBlocksToDraw = 0;
	}

	private void Start() {
		_mpb = new MaterialPropertyBlock();
	}

	public static void AddLinkDrawCommands(GridCoordinate worldLocation, LinkDirection linkDirections) {
		Span<GridDirection> directions = stackalloc GridDirection[6];
		int linkCount = linkDirections.ToGridDirectionsAll(ref directions);
		Vector3 worldPosition = worldLocation.ToWorldPositionCentered();

		for (int i = 0; i < linkCount && _linksToDraw < 1024; i++) {
			GridOrientation rotation = GridOrientation.FromUpDirection(directions[i]);
			Matrix4x4 worldMatrix = Matrix4x4.TRS(worldPosition, rotation, new Vector3(1f, 0.9f, 1f) * BuildSystem.HalfScale);
			_linkCommand.worldMatrices[_linksToDraw++] = worldMatrix;
		}
	}

	public static void AddPlaneLinkDrawCommands(GridCoordinate worldLocation) {
		if ( _planeLinksToDraw >= 1023 ) {
			return;
		}
		GridCoordinate planeNormal = PlayerStateData.GetPlaneNormalGridVector();
		GridCoordinate linkCoordinateAlongNormal = worldLocation * planeNormal;
		GridCoordinate planeCoordinateAlongNormal = PlayerStateData.GetPlanePositionAlongNormal();

		if ( linkCoordinateAlongNormal == planeCoordinateAlongNormal ) {
			Vector3 worldPosition = worldLocation.ToWorldPositionCentered();
			GridOrientation rotation = PlayerStateData.GetPlaneMarkerOrientation();
			Matrix4x4 worldMatrix = Matrix4x4.TRS(worldPosition, rotation, Vector3.one * BuildSystem.VolumeScale);
			_planeLinkCommand.worldMatrices[_planeLinksToDraw++] = worldMatrix;
		}
	}

	public static void AddPlaneLinkBlockCommands(GridCoordinate worldLocation) {
		if ( _planeBlockCommandsToDraw >= 1023 ) {
			return;
		}
		GridCoordinate planeNormal = PlayerStateData.GetPlaneNormalGridVector();
		GridCoordinate linkCoordinateAlongNormal = worldLocation * planeNormal;
		GridCoordinate planeCoordinateAlongNormal = PlayerStateData.GetPlanePositionAlongNormal();

		if ( linkCoordinateAlongNormal == planeCoordinateAlongNormal ) {
			Vector3 worldPosition = worldLocation.ToWorldPositionCentered();
			GridOrientation rotation = PlayerStateData.GetPlaneMarkerOrientation();
			Matrix4x4 worldMatrix = Matrix4x4.TRS(worldPosition, rotation, Vector3.one * BuildSystem.VolumeScale);
			_planeBlockedCommand.worldMatrices[_planeBlockCommandsToDraw++] = worldMatrix;
		}
	}

	public static void AddTubeDrawCommand(BuildData buildData, GridCoordinate location, LinkDirection linkDir) {
		Vector3 position = location.ToWorldPositionCentered();
		(Mesh m, Quaternion q) = TubeConfigurator.GetTubeState(linkDir);
		Matrix4x4 worldMatrix = Matrix4x4.TRS(position, q, Vector3.one);
		AddDrawCommand(m, buildData.previewMaterials, worldMatrix);
	}

	public static void AddModuleDrawCommand(BuildData buildData, GridCoordinate origin, GridOrientation orientation) {
		Vector3 position = origin.ToWorldPositionCentered() + buildData.GetCenterPoint(orientation);
		Mesh mesh = buildData.mesh;
		Matrix4x4 worldMatrix = Matrix4x4.TRS(position, orientation, Vector3.one);
		AddDrawCommand(mesh, buildData.previewMaterials, worldMatrix);

		Span<GridLink> worldLinks = stackalloc GridLink[6];
		int linkCount = buildData.links.OuterLinksToWorld(ref worldLinks, orientation, origin);
		for (int i = 0; i < linkCount; i++) {
			AddLinkDrawCommands(worldLinks[i].localCoordinate, worldLinks[i].connectorDirection);
		}

		if ( buildData.attachmentData is { Length: > 0 } ) {
			foreach (AttachmentConfig config in buildData.attachmentConfigs) {
				foreach (AttachmentTransform attachmentTransform in config.attachmentTransforms) {
					Vector3 attachmentPosition = (Quaternion)orientation * attachmentTransform.position;
					attachmentPosition += origin.ToWorldPositionCentered();

					worldMatrix = Matrix4x4.TRS(attachmentPosition, orientation * Quaternion.Euler(attachmentTransform.rotation), Vector3.one);
					
					AddDrawCommand(buildData.attachmentData[0].mesh, buildData.attachmentData[0].previewMaterials, worldMatrix);
				}
			}
		}
	}

	public static void AddOverlayReceiverDrawCommand(GridObject receiver) {
		if ( _overlayReceiversToDraw >= 1023 ) {
			return;
		}
		Matrix4x4 matrix = GetOverlayMatrixFromGridObject(receiver);
		_overlayDistributionReceivers.worldMatrices[_overlayReceiversToDraw++] = matrix;
	}

	public static void AddOverlayMissingDrawCommand(GridObject missing) {
		if ( _overlayMissingToDraw >= 1023 ) {
			return;
		}
		Matrix4x4 matrix = GetOverlayMatrixFromGridObject(missing);
		_overlayDistributionMissing.worldMatrices[_overlayMissingToDraw++] = matrix;
	}

	public static void AddOverlaySourceDrawCommand(GridObject source) {
		if ( _overlaySourceToDraw >= 1023 ) {
			return;
		}
		Matrix4x4 matrix = GetOverlayMatrixFromGridObject(source);
		_overlayDistributionSource.worldMatrices[_overlaySourceToDraw++] = matrix;
	}

	private static Matrix4x4 GetOverlayMatrixFromGridObject(GridObject gridObject) {
		GridCoordinate size = gridObject.moduleData.buildData.gridSize.Rotate(gridObject.orientation);
		Vector3 center = gridObject.gridBehaviour.GetMeshLocalToWorld().GetColumn(3);
		return Matrix4x4.TRS(center, Quaternion.identity, size.ToWorldPositionCentered());
	}

	private static void AddDrawCommand(Mesh mesh, Material[] materials, Matrix4x4 worldMatrix) {
		DrawCommand drawCommand = _drawCommands[_commandsToDraw];
		drawCommand.mesh = mesh;
		drawCommand.materials = materials;
		drawCommand.worldMatrix = worldMatrix;
		_commandsToDraw++;
	}

	public static void ClearDrawCommands() {
		_commandsToDraw = 0;
		_linksToDraw = 0;
	}

	public void SetGrids(BuildingGrid grid) {
		_grid = grid;
		_buildLayer = buildLayer;
	}

	public void RenderVisuals(PlayerStateData stateData) {
		ResetPreviewAlpha();
			
		if ( stateData.CheckFlag(StateFlag.Placing | StateFlag.Moving | StateFlag.Deleting)) {
			Vector3 drawPosition = stateData.GetPlanePosition();
			Vector3 normal = stateData.GetPlaneNormal();
			DrawGridViewPlane(drawPosition, normal);
			if ( stateData.CheckFlag(StateFlag.Placing | StateFlag.Moving) ) {
				DrawPlaneLinks();
				DrawPlaneBlocks();
				DrawOverlayBlocks();
			}

			if ( stateData.CheckFlag(StateFlag.ShowPath) ) {
				DrawPlacementContinuous(stateData.pathfinderPath);
			}
		}

		if ( stateData.selectedGridBehaviour != null ) {
			if ( stateData.selectedGridBehaviour.GridObject is ResourceDistributor distributor ) {
				AddOverlaySourceDrawCommand(distributor);
			}
				
			if ( stateData.CheckFlag(StateFlag.Selected) && !stateData.CheckFlag(StateFlag.Deleting) ) {
				_mpb.SetColor(PreviewColor, selectColor);
				DrawGridObject(stateData.selectedGridBehaviour);
			}

			if ( stateData.CheckFlag(StateFlag.Deleting) && stateData.selectedGridBehaviour.GridObject.isMarkedForTeardown ) {
				_mpb.SetColor(PreviewColor, deleteColor);
				DrawGridObject(stateData.selectedGridBehaviour);
			}
		}

		if ( stateData.CheckFlag(StateFlag.Moving) && BuildMovingState.GridBehaviourToMove != null ) {
			_mpb.SetColor(PreviewColor, moveColor);
			DrawGridObject(BuildMovingState.GridBehaviourToMove);
		}
			
		if ( stateData.CheckFlag(StateFlag.Hover) && stateData.hoveredGridBehaviour != null && stateData.hoveredGridBehaviour != stateData.selectedGridBehaviour ) {
			_mpb.SetColor(PreviewColor, hoverColor);
			DrawGridObject(stateData.hoveredGridBehaviour);
		}
			
		if ( stateData.CheckFlag(StateFlag.Deleting) ) {
			_mpb.SetColor(PreviewColor, deleteColor);
			foreach (GridObject gridObject in BuildDeletingState.ObjectsToDelete) {
				DrawGridObject(gridObject.gridBehaviour);
			}
		}
			
		if ( stateData.CheckFlag(StateFlag.Placing) ) {
			if ( stateData.lastRequestResponse != RequestResponse.None ) {
				_mpb.SetColor(PreviewColor, stateData.lastRequestResponse == RequestResponse.Valid ? validColor : invalidColor);
				ProcessDrawCommands();
			}
		}
			
		if ( stateData.CheckFlag(StateFlag.OpenLinks) ) {
			DrawLinks();
		}

		DrawOverlays();

		_commandsToDraw = 0;
		_linksToDraw = 0;
		_planeLinksToDraw = 0;
		_overlayReceiversToDraw = 0;
		_overlayMissingToDraw = 0;
		_overlaySourceToDraw = 0;
		_planeBlockCommandsToDraw = 0;
		_overlayBlocksToDraw = 0;
	}

	public static void TryRenderNeighboringBlockZones(GridCoordinate location) {
		_mpb.SetVector(CursorPos, location.ToWorldPositionCentered());
		int hitCount = Physics.OverlapBoxNonAlloc(location.ToWorldPositionCentered(), Vector3.one * (BuildSystem.VolumeScale * 5f),
			NeighborColliders, Quaternion.identity, _buildLayer);

		for (int i = 0; i < hitCount; i++) {
			if ( NeighborColliders[i].TryGetComponent(out GridBehaviour gridBehaviour) ) {
				if ( gridBehaviour.GridObject.objectType == GridObjectType.Tube ) {
					continue;
				}
				
				AddOverlayBlockedCommand(gridBehaviour);
			}
		}
	}

	private static void AddOverlayBlockedCommand(GridBehaviour gridBehaviour) {
		if ( _overlayBlocksToDraw >= 1023 ) {
			return;
		}
		Matrix4x4 matrix = gridBehaviour.GetBlockedOverlayMatrix();
		_overlayBlockedCommands.worldMatrices[_overlayBlocksToDraw++] = matrix;
	}

	private void DrawGridViewPlane(Vector3 position, Vector3 normal) {
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
		Matrix4x4 planeMatrix = Matrix4x4.TRS(position, rotation, Vector3.one * 50f);
		Graphics.DrawMesh(selectionPlaneMesh, planeMatrix, selectionPlaneMaterial, 0, cam, 0, null, ShadowCastingMode.Off, false);
	}

	private void DrawPlacementContinuous(List<GridCoordinate> coords) {
		_mpb.SetColor(PreviewColor, validColor);
		BuildData tubeData = BuildSystem.GetBuildData(GridObjectType.Tube);
		Material tubeMaterial = tubeData.previewMaterials[0];
		for (int i = 0; i < coords.Count; i++) {
			GridCoordinate current = coords[i];
			
			LinkDirection currentDir = default;
			
			if ( i > 0 ) {
				currentDir = (current - coords[i - 1]).ToLinkDirection().Flip();
			}
			
			LinkDirection nextDir = i == coords.Count - 1 ? default : (coords[i + 1] - current).ToLinkDirection();
			LinkDirection tubeDirection = currentDir | nextDir;

			LinkDirection linksAtCurrent;
				
			if ( _grid.LinkMap.HasLink(current) ) {
				LinkData linkData = _grid.LinkMap.GetLinkData(current);
				linksAtCurrent = linkData.linkDirection.Flip();
			} else {
				linksAtCurrent = 0;
			}

			tubeDirection |= linksAtCurrent;
			(Mesh m, Quaternion q) = TubeConfigurator.GetTubeState(tubeDirection);
			
			Vector3 position = current.ToWorldPositionCentered();

			Graphics.DrawMesh(m, position, q, tubeMaterial, 0, cam, 
				0, _mpb, ShadowCastingMode.Off, false);
		}
	}

	private void DrawGridObject(GridBehaviour gridBehaviour) {
		if ( gridBehaviour.GridObject is Tube tube ) {
			DrawGridObject(tube, gridBehaviour.GetMeshLocalToWorld());
		} else {
			DrawGridObject(gridBehaviour.GridObject, gridBehaviour.GetMeshLocalToWorld());
		}
	}

	private void DrawGridObject(GridObject gridObject, Matrix4x4 localToWorld) {
		Mesh meshToDraw = gridObject.moduleData.buildData.mesh;
		Material[] materials = gridObject.moduleData.buildData.previewMaterials;
		int subMeshCount = meshToDraw.subMeshCount;
		for (int i = 0; i < subMeshCount; i++) {
			Graphics.DrawMesh(meshToDraw, localToWorld, materials[i], 0, cam, i, _mpb, ShadowCastingMode.Off, false);
		}
	}

	private void DrawGridObject(Tube gridObject, Matrix4x4 localToWorld) {
		Mesh meshToDraw = TubeConfigurator.GetTubeState(gridObject.Connections).Item1;
		Material[] materials = BuildSystem.GetBuildData(GridObjectType.Tube).previewMaterials;
		int subMeshCount = meshToDraw.subMeshCount;
		for (int i = 0; i < subMeshCount; i++) {
			Graphics.DrawMesh(meshToDraw, localToWorld, materials[i], 0, cam, i, _mpb, ShadowCastingMode.Off, false);
		}
	}

	private void DrawLinks() {
		Graphics.DrawMeshInstanced(_linkCommand.mesh, 0, moduleLinksMaterial, _linkCommand.worldMatrices, _linksToDraw, null, ShadowCastingMode.Off, false);
	}

	private void DrawPlaneBlocks() {
		foreach (GridCoordinate coordinate in BlockHandler.BlockedCoordinates) {
			AddPlaneLinkBlockCommands(coordinate);
		}
		
		Graphics.DrawMeshInstanced(_planeBlockedCommand.mesh, 0, planeBlocksMaterial, _planeBlockedCommand.worldMatrices, _planeBlockCommandsToDraw, null, ShadowCastingMode.Off, false);
	}

	private void DrawPlaneLinks() {
		Graphics.DrawMeshInstanced(_planeLinkCommand.mesh, 0, planeLinksMaterial, _planeLinkCommand.worldMatrices, _planeLinksToDraw, null, ShadowCastingMode.Off, false);
	}

	private void DrawOverlays() {
		DrawOverlayMissing();
		DrawOverlayReceivers();
		DrawOverlaySources();
	}

	private void DrawOverlayReceivers() {
		_mpb.SetColor(OverlayColor, overlayReceiverColor);
		Graphics.DrawMeshInstanced(_overlayDistributionReceivers.mesh, 0, overlayMaterial, _overlayDistributionReceivers.worldMatrices, _overlayReceiversToDraw, _mpb, ShadowCastingMode.Off, false);
	}

	private void DrawOverlaySources() {
		_mpb.SetColor(OverlayColor, overlaySourceColor);
		Graphics.DrawMeshInstanced(_overlayDistributionSource.mesh, 0, overlayMaterial, _overlayDistributionSource.worldMatrices, _overlaySourceToDraw, _mpb, ShadowCastingMode.Off, false);
	}

	private void DrawOverlayMissing() {
		_mpb.SetColor(OverlayColor, overlayMissingColor);
		Graphics.DrawMeshInstanced(_overlayDistributionMissing.mesh, 0, overlayMaterial, _overlayDistributionMissing.worldMatrices, _overlayMissingToDraw, _mpb, ShadowCastingMode.Off, false);
	}

	private void DrawOverlayBlocks() {
		Graphics.DrawMeshInstanced(_overlayBlockedCommands.mesh, 0, overlayBlockMaterial, _overlayBlockedCommands.worldMatrices, _overlayBlocksToDraw, _mpb, ShadowCastingMode.Off, false);
	}

	private void ResetPreviewAlpha() {
		_mpb.SetFloat(PreviewFactor, previewFactor);
	}

	private void ProcessDrawCommands() {
		_mpb.SetFloat(AlphaFactor, alphaFactor);
		for (int draw = 0; draw < _commandsToDraw; draw++) {
			DrawCommand drawCommand = _drawCommands[draw];

			for (int i = 0; i < drawCommand.mesh.subMeshCount; i++) {
				Graphics.DrawMesh(drawCommand.mesh, drawCommand.worldMatrix, drawCommand.materials[i], 0, cam, i, _mpb, ShadowCastingMode.Off, false);
			}
		}
	}
}