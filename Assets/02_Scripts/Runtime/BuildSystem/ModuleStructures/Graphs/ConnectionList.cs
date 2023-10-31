using System.Collections.Generic;

[System.Serializable]
public class ConnectionList {
	public List<ModuleConnection> connections;

	public ConnectionList(List<ModuleConnection> list) {
		connections = list;
	}

	public ConnectionList() {
		connections = new List<ModuleConnection>();
	}
}