public class Stabilizer {
	private ushort _stabilizerStatus;
	public bool IsActive => _stabilizerStatus > 0;
	public ushort DebugStatus => _stabilizerStatus;

	public void AddConnectingLocation(int combinationIndex) {
		_stabilizerStatus |= (ushort)(1 << combinationIndex);
	}

	public bool HasConnectingLocation(int combinationIndex) {
		return (_stabilizerStatus & 1 << combinationIndex) != 0;
	}

	public void RemoveConnectingLocation(int combinationIndex) {
		_stabilizerStatus ^= (ushort)(1 << combinationIndex);
	}
}