using UnityEngine;

public static class AttachmentBehaviourPool {
	private static readonly Vector3 HoldPosition = Vector3.one * 32000f;
	private static AttachmentBehaviour _attachmentPrefab;

	public static void Setup(AttachmentBehaviour gridPrefab) {
		_attachmentPrefab = gridPrefab;
	}
		
	public static AttachmentBehaviour Rent(AttachmentData attachmentData, GridObject gridObject, Building building, SlotDefiniton slotDefinition) {
		AttachmentBehaviour attachment = Object.Instantiate(_attachmentPrefab, HoldPosition, Quaternion.identity);
		slotDefinition.AttachmentBehaviours.Add(attachment);
		attachment.Setup(attachmentData, gridObject, building, (int)slotDefinition.SlotID, slotDefinition.Used);
		return attachment;
	}

	public static void Return(AttachmentBehaviour attachmentBehaviour) {
		attachmentBehaviour.TearDown();
		Object.Destroy(attachmentBehaviour.gameObject);
	}
}