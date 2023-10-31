using UnityEngine;

[CreateAssetMenu(fileName = "Description Data", menuName = "Nebra/DescriptionData", order = 0)]
public class DescriptionData : ScriptableObject {
    public string moduleName;
    [TextArea(2, 20)]
    public string moduleDescription;
    public Sprite moduleIcon;
}