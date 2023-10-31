using System;
using System.Linq;

public static class BuildSystem {
	public const float VolumeScale = 5f;
	public const float HalfScale = VolumeScale / 2f;
	public const float QuarterScale = HalfScale / 2f;
	public const float EighthScale = QuarterScale / 2f;
	public const float ModelScale = 5f;
		
	private static ModuleData[] _moduleConfigs;
	private static AttachmentData[] _attachmentConfigs;

	public static void Initialize(ModuleData[] configs, AttachmentData[] attachments) {
		int maxGridObjectIndex = (int)Enum.GetValues(typeof(GridObjectType)).Cast<GridObjectType>().Max() + 1;
		_moduleConfigs = new ModuleData[maxGridObjectIndex];
		foreach (ModuleData moduleData in configs) {
			int buildId = moduleData.buildingId;
			if ( _moduleConfigs[buildId] == null ) {
				_moduleConfigs[buildId] = moduleData;
			}
		}

		_attachmentConfigs = new AttachmentData[maxGridObjectIndex];
		foreach (AttachmentData attachmentData in attachments) {
			int attachmentId = (int)attachmentData.objectType;
			if ( _attachmentConfigs[attachmentId] == null ) {
				_attachmentConfigs[attachmentId] = attachmentData;
			}
		}
	}

	public static ModuleData GetModuleData(GridObjectType type) {
		return _moduleConfigs[(int)type];
	}

	public static BuildData GetBuildData(GridObjectType type) {
		return _moduleConfigs[(int)type].buildData;
	}

	public static AttachmentData GetAttachmentData(int id) {
		return _attachmentConfigs[id];
	}
}