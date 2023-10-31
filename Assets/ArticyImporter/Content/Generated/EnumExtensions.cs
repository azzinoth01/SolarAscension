namespace Articy.Sola
{
	public static class EnumExtensionMethods
	{
		public static string GetDisplayName(this FactionCharacterContact aFactionCharacterContact)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("FactionCharacterContact").GetEnumValue(((int)(aFactionCharacterContact))).DisplayName;
		}

		public static string GetDisplayName(this RivalLowDiplomacy aRivalLowDiplomacy)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("RivalLowDiplomacy").GetEnumValue(((int)(aRivalLowDiplomacy))).DisplayName;
		}

		public static string GetDisplayName(this RivalHighDiplomacy aRivalHighDiplomacy)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("RivalHighDiplomacy").GetEnumValue(((int)(aRivalHighDiplomacy))).DisplayName;
		}

		public static string GetDisplayName(this AllyLowDiplomacy aAllyLowDiplomacy)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("AllyLowDiplomacy").GetEnumValue(((int)(aAllyLowDiplomacy))).DisplayName;
		}

		public static string GetDisplayName(this AllyHighDiplomacy aAllyHighDiplomacy)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("AllyHighDiplomacy").GetEnumValue(((int)(aAllyHighDiplomacy))).DisplayName;
		}

		public static string GetDisplayName(this Accent aAccent)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("Accent").GetEnumValue(((int)(aAccent))).DisplayName;
		}

		public static string GetDisplayName(this Sex aSex)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("Sex").GetEnumValue(((int)(aSex))).DisplayName;
		}

		public static string GetDisplayName(this ShapeType aShapeType)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("ShapeType").GetEnumValue(((int)(aShapeType))).DisplayName;
		}

		public static string GetDisplayName(this SelectabilityModes aSelectabilityModes)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("SelectabilityModes").GetEnumValue(((int)(aSelectabilityModes))).DisplayName;
		}

		public static string GetDisplayName(this VisibilityModes aVisibilityModes)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("VisibilityModes").GetEnumValue(((int)(aVisibilityModes))).DisplayName;
		}

		public static string GetDisplayName(this OutlineStyle aOutlineStyle)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("OutlineStyle").GetEnumValue(((int)(aOutlineStyle))).DisplayName;
		}

		public static string GetDisplayName(this PathCaps aPathCaps)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("PathCaps").GetEnumValue(((int)(aPathCaps))).DisplayName;
		}

		public static string GetDisplayName(this LocationAnchorSize aLocationAnchorSize)
		{
			return Articy.Unity.ArticyTypeSystem.GetArticyType("LocationAnchorSize").GetEnumValue(((int)(aLocationAnchorSize))).DisplayName;
		}

	}
}

