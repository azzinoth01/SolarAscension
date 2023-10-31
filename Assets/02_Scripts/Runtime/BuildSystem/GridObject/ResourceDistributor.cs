public class ResourceDistributor : Module {
	public void BeginDistribution(uint dispenseId) {
		if ( lastDistributionId < dispenseId ) {
			lastDistributionId = dispenseId;
		}
			
		var distribution = economyBuilding.OutputValue;

		if ( clusters == null ) {
			return;
		}
		foreach (TubeCluster cluster in clusters) {
			cluster?.HandleResourceDistribution(this, this, distribution, dispenseId);
		}
	}

	public override void SubmitDistributionForRendering() {
		if ( !economyBuilding.RequiresDistributionRessource ) {
			return;
		}

		if ( economyBuilding.MissingDistributionRessource ) {
			BuildVisualizer.AddOverlayMissingDrawCommand(this);
			return;
		}

		BuildVisualizer.AddOverlaySourceDrawCommand(this);
	}
}