using SolarAscension;
using System;
using System.Collections.Generic;

[Serializable]
public class GridObject {
    public BuildingGrid grid;
    public Building economyBuilding;
    public ModuleData moduleData;
    public GridBehaviour gridBehaviour;

    public GridObjectType objectType;
    public GridCoordinate origin;
    public GridOrientation orientation;

    public GridDirection[] linkDirections;
    public GridCoordinate[] linkLocations;

    public bool isMarkedForTeardown, isTearingDown;

    protected static uint lastDistributionId;
    protected List<ResourceDistributor> distributorsInLastReceivedDistribution;
    protected uint lastReceivedDistributionId;

    public void Setup(Building building, ModuleData data, GridCoordinate orig, GridOrientation rot) {
        objectType = data.objectType;
        moduleData = data;

        origin = orig;
        orientation = rot;

        economyBuilding = building;

        gridBehaviour = GridBehaviourPool.Rent();
        gridBehaviour.GridObject = this;
        gridBehaviour.ConfigureObject();

        distributorsInLastReceivedDistribution = new List<ResourceDistributor>(4);

        QuestSystem.Instance.InvokeGoalUpdate(QuestSystem.GridObjectToQuestGoal(objectType), 1);

        Connect();
        ProcessLinks();
    }

    public DistributionCost[] GetTraversalCost() => moduleData.distributionData.GetCosts();

    protected void Connect() {
        SetLinks();
        SetLocks();
    }

    protected void SetLinks() {
        GridLink[] localLinks = moduleData.buildData.links;
        linkDirections = new GridDirection[6];
        linkLocations = new GridCoordinate[6];
        int idx = 0;

        Span<GridLink> worldLinks = stackalloc GridLink[6];

        int worldLinkCount = localLinks.OuterLinksToWorld(ref worldLinks, orientation, origin);

        for (int i = 0; i < worldLinkCount; i++) {
            GridDirection worldDirection = worldLinks[i].connectorDirection.ToGridDirection();
            GridCoordinate worldLocation = worldLinks[i].localCoordinate;

            linkDirections[idx] = worldDirection;
            linkLocations[idx++] = worldLocation;

            grid.LinkMap.AddLink(worldLocation, worldDirection, this);
        }

        for (; idx < 6; idx++) {
            linkLocations[idx] = GridCoordinate.Invalid;
        }
    }

    protected virtual void ProcessLinks() {
    }

    protected virtual void SetLocks() {
    }

    public void SubmitRenderingCommands() {
        PlayerStateData stateData = grid.PlayerState.StateData;

        if (stateData.CheckFlag(StateFlag.OpenLinks)) {
            SubmitLinksForRendering();
        }

        if (lastReceivedDistributionId != lastDistributionId) {
            return;
        }

        if (stateData.selectedGridBehaviour != null && stateData.selectedGridBehaviour.GridObject is ResourceDistributor distributor) {
            if (distributor.economyBuilding.IsActive) {
                if (distributorsInLastReceivedDistribution.Contains(distributor)) {
                    BuildVisualizer.AddOverlayReceiverDrawCommand(this);
                }
            }
        }

        if (stateData.CheckFlag(StateFlag.OverlayOxygen)) {
            SubmitDistributionForRendering();
        }
    }

    public void SubmitLinksForRendering() {
        GridLink[] moduleLinks = moduleData.buildData.links;
        Span<GridLink> worldLinks = stackalloc GridLink[6];
        int linkCount = moduleLinks.OuterLinksToWorld(ref worldLinks, orientation, origin);

        for (int i = 0; i < linkCount; i++) {
            GridCoordinate worldPosition = worldLinks[i].localCoordinate;
            if (grid.LocationIsFree(worldPosition)) {
                BuildVisualizer.AddPlaneLinkDrawCommands(worldPosition);
            }
        }
    }

    public virtual void SubmitDistributionForRendering() {
        if (economyBuilding.MissingDistributionRessource) {
            BuildVisualizer.AddOverlayMissingDrawCommand(this);
            return;
        }

        if (distributorsInLastReceivedDistribution.Count > 0) {
            BuildVisualizer.AddOverlayReceiverDrawCommand(this);
        }
    }

    public void TearDown() {
        isTearingDown = true;
        Disconnect();
        OnTearDown();
        economyBuilding.BuildingRemoved();
        //Assert.IsTrue(economyBuilding.BuildingRemoved());

        GridBehaviourPool.Return(gridBehaviour);
        gridBehaviour.GridObject = null;
        gridBehaviour = null;
        grid = null;
    }

    public virtual bool ValidateTearDown(List<GridObject> excludedObjects) {
        return true;
    }

    protected void Disconnect() {
        RemoveLinks();
        RemoveLocks();
    }

    protected virtual void RemoveLinks() {
    }

    protected virtual void RemoveLocks() {
    }

    protected virtual void OnTearDown() {
    }
}