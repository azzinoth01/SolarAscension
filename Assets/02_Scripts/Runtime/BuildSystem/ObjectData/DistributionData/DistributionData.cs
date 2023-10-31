using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Distribution Data", menuName = "Nebra/DistributionData", order = 0)]
public class DistributionData : ScriptableObject {
	public List<DistributionCost> distributionCostPerResource;

	private DistributionCost[] _costsPerResource;

	public DistributionCost[] GetCosts() {
		if ( _costsPerResource == null || _costsPerResource.Length == 0 ) {
			int maxResourceIndex = (int)Enum.GetValues(typeof(Ressources)).Cast<Ressources>().Max() + 1;
			_costsPerResource = new DistributionCost[maxResourceIndex];

			foreach (DistributionCost distributionCost in distributionCostPerResource) {
				_costsPerResource[(int)distributionCost.resource] = distributionCost;
			}
		}

		return _costsPerResource;
	}
}