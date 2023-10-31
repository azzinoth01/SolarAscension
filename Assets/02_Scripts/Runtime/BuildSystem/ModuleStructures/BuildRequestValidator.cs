using System;

public class BuildRequestValidator {
	private readonly BuildingGrid _buildingGrid;

	public BuildRequestValidator(BuildingGrid buildingGrid) {
		_buildingGrid = buildingGrid;
	}

	public BuildRequestResponse ValidateBuildRequestFromLink(ModuleData data, bool enactOnValidation,
		GridCoordinate linkLocation, GridOrientation orientation) {
		return !TryFindValidOriginFromLink(data, linkLocation, orientation, out GridCoordinate origin) ? 
			new BuildRequestResponse(RequestResponse.IncompatibleLink, default, GridCoordinate.Zero) : 
			ValidateBuildRequestFromOrigin(data, enactOnValidation, origin, orientation);
	}

	public BuildRequestResponse ValidateBuildRequestFromOrigin(ModuleData data, bool enactOnValidation, GridCoordinate origin, GridOrientation orientation) {
		return data.objectType == GridObjectType.Tube ? 
			ValidateTubeBuildRequest(data, enactOnValidation, origin) : 
			ValidateModuleBuildRequest(data, enactOnValidation, origin, orientation);
	}

	private bool TryFindValidOriginFromLink(ModuleData data, GridCoordinate linkLocation, GridOrientation orientation, out GridCoordinate origin) {
		origin = GridCoordinate.Invalid;

		if ( _buildingGrid.LockGrid.IsLocked(linkLocation) ) {
			return false;
		}
			
		if ( !_buildingGrid.LinkMap.TryGetLinkData(linkLocation, out LinkData linkData) ) {
			return false;
		}

		if ( !TryFindValidLink(data.buildData, orientation, linkData.linkDirection, out GridLink rotatedLink) ) {
			return false;
		}

		origin = linkLocation - rotatedLink.localCoordinate;
		return true;
	}

	private bool TryFindValidLink(BuildData buildData, GridOrientation orientation, LinkDirection linkDirection, out GridLink rotatedLink) {
		GridLink[] links = buildData.links;

		for (int i = 0; i < links.Length; i++) {
			rotatedLink = links[i].Rotate(orientation);
			if ( rotatedLink.TryLink(linkDirection) ) {
				return true;
			}
		}

		rotatedLink = default;
		return false;
	}

	private BuildRequestResponse ValidateTubeBuildRequest(ModuleData data, bool enactOnValidation, GridCoordinate origin) {
		GridLink worldLink = new GridLink(origin, LinkDirection.All);
		BuildData tubeBuildData = BuildSystem.GetBuildData(GridObjectType.Tube);
			
		if ( !ValidateLink(worldLink, out LinkDirection linkDirection) ) {
			BuildVisualizer.AddTubeDrawCommand(tubeBuildData, origin, 0);
			return new BuildRequestResponse(RequestResponse.MissingLink, default, GridCoordinate.Zero);
		}
			
		BuildVisualizer.AddTubeDrawCommand(tubeBuildData, origin, linkDirection.Flip());

		if ( !_buildingGrid.LocationIsFree(origin) || !IsBuildableCoordinate(origin) ) {
			return new BuildRequestResponse(RequestResponse.BlockedSpace, default, GridCoordinate.Zero);
		}

		if ( !BuildingHelper.CheckHasBuildRessources(data.buildingId, _buildingGrid.PlayerID) ) {
			return new BuildRequestResponse(RequestResponse.MissingResources, default, GridCoordinate.Zero);
		}

		if ( enactOnValidation ) {
			if ( !BuildingHelper.TrySetBuildingID(data.buildingId, out Building tubeBuilding) ) {
				return new BuildRequestResponse(RequestResponse.MissingResources, default, GridCoordinate.Zero);
			}

			if ( !tubeBuilding.BuildingPlaced(_buildingGrid.BalanceInfo) ) {
				return new BuildRequestResponse(RequestResponse.MissingResources, default, GridCoordinate.Zero);
			}
			
			BuildTube(tubeBuilding, origin, linkDirection);
		}
			
		return new BuildRequestResponse(RequestResponse.Valid, origin, GridCoordinate.Zero);
	}

	private BuildRequestResponse ValidateModuleBuildRequest(ModuleData data, bool enactOnValidation, GridCoordinate origin, GridOrientation orientation) {
		if ( !TryFindValidLink(data.buildData.links, origin, orientation, out GridLink validatedLink, out LinkDirection validatedLinkDirection) ) {
			BuildVisualizer.AddModuleDrawCommand(data.buildData, origin, orientation);
			return new BuildRequestResponse(RequestResponse.MissingLink, default, GridCoordinate.Zero);
		}
		
		GridCoordinate buildOffset = GridCoordinate.Zero;

		bool canBePlacedWithoutAdditionalTube = ValidateSpaceRequirements(data.buildData, orientation, origin);
		GridDirection buildDirection = (validatedLinkDirection & validatedLink.connectorDirection.Flip()).ToGridDirection();
			
		if ( !canBePlacedWithoutAdditionalTube ) {
			BuildData tubeBuildData = BuildSystem.GetBuildData(GridObjectType.Tube);
			BuildVisualizer.AddTubeDrawCommand(tubeBuildData, validatedLink.localCoordinate, validatedLinkDirection.Flip());
			origin += buildDirection.ToGridVector();
			buildOffset = buildDirection.ToGridVector();

			bool canBePlacedWithAdditionalTube = ValidateSpaceRequirements(data.buildData, orientation, origin);

			if ( !canBePlacedWithAdditionalTube ) {
				BuildVisualizer.AddModuleDrawCommand(data.buildData, origin, orientation);
				return new BuildRequestResponse(RequestResponse.BlockedSpace, default, buildOffset);
			}
		}

		BuildVisualizer.AddModuleDrawCommand(data.buildData, origin, orientation);

		bool hasResources = canBePlacedWithoutAdditionalTube ? 
			BuildingHelper.CheckHasBuildRessources(data.buildingId, _buildingGrid.BalanceInfo) :
			BuildingHelper.CheckHasBuildRessources(_buildingGrid.BalanceInfo, data.buildingId, BuildSystem.GetModuleData(GridObjectType.Tube).buildingId );
			
		if ( !hasResources ) {
			return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
		}

		GridCoordinate linkCenter = default;

		if ( enactOnValidation ) {
			if ( !canBePlacedWithoutAdditionalTube ) {
				ModuleData tubeData = BuildSystem.GetModuleData(GridObjectType.Tube);
				if ( !BuildingHelper.TrySetBuildingID(tubeData.buildingId, out Building tubeBuilding) || 
				     !BuildingHelper.TrySetBuildingID(data.buildingId, out Building moduleBuilding) ) {
					return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
				}

				if ( !BuildingHelper.PlaceMultipleBuildings( new[] {moduleBuilding, tubeBuilding} , _buildingGrid.BalanceInfo ) ) {
					return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
				}
					
				BuildTube(tubeBuilding, validatedLink.localCoordinate, validatedLinkDirection);
				linkCenter = BuildModule(moduleBuilding, data, origin, orientation);
			} else {
				if ( !BuildingHelper.TrySetBuildingID(data.buildingId, out Building moduleBuilding) ) {
					return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
				}

				if ( !moduleBuilding.BuildingPlaced(_buildingGrid.BalanceInfo) ) {
					return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
				}
					
				linkCenter = BuildModule(moduleBuilding, data, origin, orientation);
			}
		}
			
		return new BuildRequestResponse(RequestResponse.Valid, linkCenter, buildOffset);
	}

	public BuildRequestResponse ValidateMoveRequestFromLink(Module module, bool enactOnValidation, GridCoordinate linkLocation,
		GridOrientation orientation) {
		return !TryFindValidOriginFromLink(module.moduleData, linkLocation, orientation, out GridCoordinate origin) ? 
			new BuildRequestResponse(RequestResponse.IncompatibleLink, default, GridCoordinate.Zero) : 
			ValidateMoveRequestFromOrigin(module, enactOnValidation, origin, orientation);
	}

	public BuildRequestResponse ValidateMoveRequestFromOrigin(Module module, bool enactOnValidation, GridCoordinate origin, GridOrientation orientation) {
		BuildData buildData = module.moduleData.buildData;
			
		if ( !TryFindValidLink(buildData.links, origin, orientation, out GridLink validatedLink, out LinkDirection validatedLinkDirection) ) {
			BuildVisualizer.AddModuleDrawCommand(buildData, origin, orientation);
			return new BuildRequestResponse(RequestResponse.MissingLink, default, GridCoordinate.Zero);
		}
			
		bool canBeMovedWithoutAdditionalTube = ValidateSpaceRequirements(buildData, orientation, origin);
		GridDirection buildDirection = (validatedLinkDirection & validatedLink.connectorDirection.Flip()).ToGridDirection();
		GridCoordinate buildOffset = GridCoordinate.Zero;

		if ( !canBeMovedWithoutAdditionalTube ) {
			BuildData tubeBuildData = BuildSystem.GetBuildData(GridObjectType.Tube);
			BuildVisualizer.AddTubeDrawCommand(tubeBuildData, validatedLink.localCoordinate, validatedLinkDirection.Flip());
			
			origin += buildDirection.ToGridVector();
			buildOffset = buildDirection.ToGridVector();

			if ( !BuildingHelper.CheckHasBuildRessources((int)GridObjectType.Tube, _buildingGrid.PlayerID) ) {
				BuildVisualizer.AddModuleDrawCommand(buildData, origin, orientation);
				return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
			}
			
			bool canBeMovedWithAdditionalTube = ValidateSpaceRequirements(buildData, orientation, origin);
				
			if ( !canBeMovedWithAdditionalTube ) {
				BuildVisualizer.AddModuleDrawCommand(buildData, origin, orientation);
				return new BuildRequestResponse(RequestResponse.BlockedSpace, default, buildOffset);
			}
		}
			
		BuildVisualizer.AddModuleDrawCommand(buildData, origin, orientation);
			
		if ( enactOnValidation ) {
			if ( !canBeMovedWithoutAdditionalTube ) {
				ModuleData tubeData = BuildSystem.GetModuleData(GridObjectType.Tube);
				if ( !BuildingHelper.TrySetBuildingID(tubeData.buildingId, out Building tubeBuilding) ) {
					return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
				}

				if ( !tubeBuilding.BuildingPlaced(_buildingGrid.BalanceInfo) ) {
					return new BuildRequestResponse(RequestResponse.MissingResources, default, buildOffset);
				}
				BuildTube(tubeBuilding, validatedLink.localCoordinate, validatedLinkDirection);
			}
			
			module.Relocate(origin, orientation);
		}
			
		return new BuildRequestResponse(RequestResponse.Valid, module.gridBehaviour.GetLinkCenter(), buildOffset);
	}

	private void BuildTube(Building building, GridCoordinate origin, LinkDirection connectionDirections) {
		Tube tube = new Tube();
		ModuleData tubeData = BuildSystem.GetModuleData(GridObjectType.Tube);
		tube.grid = _buildingGrid;
		tube.Setup(building, tubeData, origin, connectionDirections);
	}

	private GridCoordinate BuildModule(Building building, ModuleData moduleData, GridCoordinate origin, GridOrientation orientation) {
		Module module;

		if ( moduleData.isDistributor ) {
			ResourceDistributor distributor = new ResourceDistributor();
			_buildingGrid.BalanceInfo.startRedistributionEvent += distributor.BeginDistribution;
			module = distributor;
		} else {
			module = new Module();
		}
			
		module.grid = _buildingGrid;
		module.Setup(building, moduleData, origin, orientation);
		return module.gridBehaviour.GetLinkCenter();
	}

	private bool TryFindValidLink(GridLink[] links, GridCoordinate origin, GridOrientation orientation, out GridLink validLink, out LinkDirection validDirection) {
		Span<GridLink> worldLinks = stackalloc GridLink[6];
		int linkCount = links.InnerLinksToWorld(ref worldLinks, orientation, origin);

		validLink = default;
		validDirection = 0;
		int i = 0;
			
		for (; i < linkCount; i++) {
			if ( ValidateLink(worldLinks[i], out validDirection) ) {
				validLink = worldLinks[i];
				return true;
			}
		}

		return false;
	}

	private bool ValidateSpaceRequirements(BuildData buildData, GridOrientation orientation, GridCoordinate origin) {
		GridCoordinate gridSize = buildData.gridSize + new GridCoordinate(2);
		GridCoordinate lockOrigin = origin - new GridCoordinate(1).Rotate(orientation);

		GridLink[] links = buildData.links;
		Span<GridLink> worldLinks = stackalloc GridLink[6];
		links.OuterLinksToWorld(ref worldLinks, orientation, origin);

		for (int z = 0; z < gridSize.z; z++) {
			for (int y = 0; y < gridSize.y; y++) {
				for (int x = 0; x < gridSize.x; x++) {
					GridCoordinate localCoordinate = new GridCoordinate(x, y, z).Rotate(orientation);
					GridCoordinate worldCoordinate = lockOrigin + localCoordinate;

					if ( !IsBuildableCoordinate(worldCoordinate) ) return false;

					if ( worldLinks.Contains(worldCoordinate) ) {
						continue;
					}

					if ( x == 0 || y == 0 || z == 0 || x == gridSize.x - 1 || y == gridSize.y - 1 || z == gridSize.z - 1 ) {
						if ( !ValidateLocationIsNotLocked(worldCoordinate) ) {
							return false;
						}
					} else {
						if ( !_buildingGrid.LocationIsFree(worldCoordinate) ) {
							return false;
						}
					}
				}
			}
		}

		return true;
	}

	private static bool IsBuildableCoordinate(GridCoordinate worldCoordinate) {
		if ( worldCoordinate.y < -30 ) {
			return false;
		}

		return worldCoordinate.x is > 6 or < -6 || worldCoordinate.z is > 6 or < -6 || worldCoordinate.y >= 0;
	}

	private bool ValidateLink(GridLink worldLink, out LinkDirection linkDirection) {
		GridCoordinate worldCoordinate = worldLink.localCoordinate;
		linkDirection = 0;

		if ( _buildingGrid.LockGrid.IsLocked(worldCoordinate) ) {
			return false;
		}
			
		if ( _buildingGrid.LinkMap.HasLink(worldCoordinate) ) {
			LinkData linkData = _buildingGrid.LinkMap.GetLinkData(worldCoordinate);
			linkDirection = linkData.linkDirection;
				
			if ( worldLink.TryLink(linkDirection) ) {
				return true;
			}
		}
			
		return false;
	}

	private bool ValidateLocationIsNotLocked(GridCoordinate location) {
		return !_buildingGrid.LockGrid.IsLocked(location);
	}
}