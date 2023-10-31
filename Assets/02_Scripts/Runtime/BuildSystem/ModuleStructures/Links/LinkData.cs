public class LinkData {
	public LinkDirection linkDirection;
	public Module[] sourceModules;
	public Tube[] sourceTubes;

	public LinkData(GridDirection direction, GridObject source) {
		AddLink(direction, source);
	}

	public void AddLink(GridDirection direction, GridObject source) {
		linkDirection |= direction.ToLinkDirection();
		AddSource(direction, source);
	}

	private void AddSource(GridDirection direction, GridObject source) {
		switch (source) {
			case Module module:
				sourceModules ??= new Module[2];
				if ( sourceModules[0] != null ) {
					sourceModules[1] = module;
				} else {
					sourceModules[0] = module;
				}
				break;
			case Tube tube:
				sourceTubes ??= new Tube[6];
				sourceTubes[(int)direction] = tube;
				break;
		}
	}

	public void RemoveLink(GridDirection direction) {
		int idx = (int)direction;
		if ( sourceTubes?[idx] == null ) {
			sourceModules = null;
		} else {
			sourceTubes[idx] = null;
		}

		linkDirection ^= direction.ToLinkDirection();
	}
}