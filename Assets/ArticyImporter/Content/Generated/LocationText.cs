//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Sola
{
    
    
    public class LocationText : ArticyObject, ILocationText, IPropertyProvider, IObjectWithColor, IObjectWithDisplayName, IObjectWithLocalizableDisplayName, IObjectWithPreviewImage, IObjectWithText, IObjectWithLocalizableText, IObjectWithTransformation, IObjectWithVertices, IObjectWithExternalId, IObjectWithShortId, IObjectWithZIndex, IObjectWithSize
    {
        
        [SerializeField()]
        private String mLocaKey_DisplayName;
        
        [SerializeField()]
        private String mOverwritten_DisplayName = "";
        
        [SerializeField()]
        private PreviewImage mPreviewImage = new PreviewImage();
        
        [SerializeField()]
        private ArticyValueListLocationAnchor mAnchors = new ArticyValueListLocationAnchor();
        
        [SerializeField()]
        private ArticyValueListVector2 mVertices = new ArticyValueListVector2();
        
        [SerializeField()]
        private Transformation mTransform = new Transformation();
        
        [SerializeField()]
        private ShapeType mShapeType = new ShapeType();
        
        [SerializeField()]
        private Color mColor;
        
        [SerializeField()]
        private String mLocaKey_Text;
        
        [SerializeField()]
        private String mOverwritten_Text = "";
        
        [SerializeField()]
        private String mExternalId;
        
        [SerializeField()]
        private Single mZIndex;
        
        [SerializeField()]
        private Vector2 mSize;
        
        [SerializeField()]
        private UInt32 mShortId;
        
        [SerializeField()]
        private VisibilityModes mVisibility = new VisibilityModes();
        
        [SerializeField()]
        private Color mOutlineColor;
        
        [SerializeField()]
        private Single mOutlineSize;
        
        [SerializeField()]
        private OutlineStyle mOutlineStyle = new OutlineStyle();
        
        [SerializeField()]
        private SelectabilityModes mSelectability = new SelectabilityModes();
        
        [SerializeField()]
        private Single mForcedWidth;
        
        [SerializeField()]
        private Single mForcedHeight;
        
        [SerializeField()]
        private Boolean mDropShadow = new Boolean();
        
        public String LocaKey_DisplayName
        {
            get
            {
                return mLocaKey_DisplayName;
            }
        }
        
        public String DisplayName
        {
            get
            {
                if ((mOverwritten_DisplayName != ""))
                {
                    return mOverwritten_DisplayName;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_DisplayName);
            }
            set
            {
                var oldValue = DisplayName;
                mOverwritten_DisplayName = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "DisplayName", oldValue, mOverwritten_DisplayName);
            }
        }
        
        public PreviewImage PreviewImage
        {
            get
            {
                return mPreviewImage;
            }
            set
            {
                var oldValue = mPreviewImage;
                mPreviewImage = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "PreviewImage", oldValue, mPreviewImage);
            }
        }
        
        public List<LocationAnchor> Anchors
        {
            get
            {
                return mAnchors.GetValue();
            }
            set
            {
                var oldValue = mAnchors;
                mAnchors.SetValue(value);
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Anchors", oldValue.GetValue(), mAnchors.GetValue());
            }
        }
        
        public List<Vector2> Vertices
        {
            get
            {
                return mVertices.GetValue();
            }
            set
            {
                var oldValue = mVertices;
                mVertices.SetValue(value);
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Vertices", oldValue.GetValue(), mVertices.GetValue());
            }
        }
        
        public Transformation Transform
        {
            get
            {
                return mTransform;
            }
            set
            {
                var oldValue = mTransform;
                mTransform = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Transform", oldValue, mTransform);
            }
        }
        
        public ShapeType ShapeType
        {
            get
            {
                return mShapeType;
            }
            set
            {
                var oldValue = mShapeType;
                mShapeType = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "ShapeType", oldValue, mShapeType);
            }
        }
        
        public Color Color
        {
            get
            {
                return mColor;
            }
            set
            {
                var oldValue = mColor;
                mColor = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Color", oldValue, mColor);
            }
        }
        
        public String LocaKey_Text
        {
            get
            {
                return mLocaKey_Text;
            }
        }
        
        public String Text
        {
            get
            {
                if ((mOverwritten_Text != ""))
                {
                    return mOverwritten_Text;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_Text);
            }
            set
            {
                var oldValue = Text;
                mOverwritten_Text = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Text", oldValue, mOverwritten_Text);
            }
        }
        
        public String ExternalId
        {
            get
            {
                return mExternalId;
            }
            set
            {
                var oldValue = mExternalId;
                mExternalId = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "ExternalId", oldValue, mExternalId);
            }
        }
        
        public Single ZIndex
        {
            get
            {
                return mZIndex;
            }
            set
            {
                var oldValue = mZIndex;
                mZIndex = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "ZIndex", oldValue, mZIndex);
            }
        }
        
        public Vector2 Size
        {
            get
            {
                return mSize;
            }
            set
            {
                var oldValue = mSize;
                mSize = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Size", oldValue, mSize);
            }
        }
        
        public UInt32 ShortId
        {
            get
            {
                return mShortId;
            }
            set
            {
                var oldValue = mShortId;
                mShortId = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "ShortId", oldValue, mShortId);
            }
        }
        
        public VisibilityModes Visibility
        {
            get
            {
                return mVisibility;
            }
            set
            {
                var oldValue = mVisibility;
                mVisibility = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Visibility", oldValue, mVisibility);
            }
        }
        
        public Color OutlineColor
        {
            get
            {
                return mOutlineColor;
            }
            set
            {
                var oldValue = mOutlineColor;
                mOutlineColor = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "OutlineColor", oldValue, mOutlineColor);
            }
        }
        
        public Single OutlineSize
        {
            get
            {
                return mOutlineSize;
            }
            set
            {
                var oldValue = mOutlineSize;
                mOutlineSize = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "OutlineSize", oldValue, mOutlineSize);
            }
        }
        
        public OutlineStyle OutlineStyle
        {
            get
            {
                return mOutlineStyle;
            }
            set
            {
                var oldValue = mOutlineStyle;
                mOutlineStyle = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "OutlineStyle", oldValue, mOutlineStyle);
            }
        }
        
        public SelectabilityModes Selectability
        {
            get
            {
                return mSelectability;
            }
            set
            {
                var oldValue = mSelectability;
                mSelectability = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "Selectability", oldValue, mSelectability);
            }
        }
        
        public Single ForcedWidth
        {
            get
            {
                return mForcedWidth;
            }
            set
            {
                var oldValue = mForcedWidth;
                mForcedWidth = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "ForcedWidth", oldValue, mForcedWidth);
            }
        }
        
        public Single ForcedHeight
        {
            get
            {
                return mForcedHeight;
            }
            set
            {
                var oldValue = mForcedHeight;
                mForcedHeight = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "ForcedHeight", oldValue, mForcedHeight);
            }
        }
        
        public Boolean DropShadow
        {
            get
            {
                return mDropShadow;
            }
            set
            {
                var oldValue = mDropShadow;
                mDropShadow = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(Id, InstanceId, "DropShadow", oldValue, mDropShadow);
            }
        }
        
        protected override void CloneProperties(object aClone, Articy.Unity.ArticyObject aFirstClassParent)
        {
            LocationText newClone = ((LocationText)(aClone));
            newClone.mLocaKey_DisplayName = mLocaKey_DisplayName;
            newClone.mOverwritten_DisplayName = mOverwritten_DisplayName;
            newClone.PreviewImage = PreviewImage;
            List<LocationAnchor> temp_Anchors = new List<LocationAnchor>();
            int i = 0;
            for (i = 0; (i < Anchors.Count); i = (i + 1))
            {
                temp_Anchors.Add(((LocationAnchor)(Anchors[i].CloneObject(newClone, aFirstClassParent))));
            }
            newClone.Anchors = temp_Anchors;
            List<Vector2> temp_Vertices = new List<Vector2>();
            for (i = 0; (i < Vertices.Count); i = (i + 1))
            {
                temp_Vertices.Add(Vertices[i]);
            }
            newClone.Vertices = temp_Vertices;
            newClone.Transform = Transform;
            newClone.ShapeType = ShapeType;
            newClone.Color = Color;
            newClone.mLocaKey_Text = mLocaKey_Text;
            newClone.mOverwritten_Text = mOverwritten_Text;
            newClone.ExternalId = ExternalId;
            newClone.ZIndex = ZIndex;
            newClone.Size = Size;
            newClone.ShortId = ShortId;
            newClone.Visibility = Visibility;
            newClone.OutlineColor = OutlineColor;
            newClone.OutlineSize = OutlineSize;
            newClone.OutlineStyle = OutlineStyle;
            newClone.Selectability = Selectability;
            newClone.ForcedWidth = ForcedWidth;
            newClone.ForcedHeight = ForcedHeight;
            newClone.DropShadow = DropShadow;
            base.CloneProperties(newClone, aFirstClassParent);
        }
        
        public override bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            if ((mOverwritten_DisplayName != ""))
            {
                return true;
            }
            if ((mOverwritten_Text != ""))
            {
                return true;
            }
            return base.IsLocalizedPropertyOverwritten(aProperty);
        }
        
        #region property provider interface
        public override void setProp(string aProperty, object aValue)
        {
            if ((aProperty == "DisplayName"))
            {
                DisplayName = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "PreviewImage"))
            {
                PreviewImage = ((PreviewImage)(aValue));
                return;
            }
            if ((aProperty == "Anchors"))
            {
                Anchors = ((List<LocationAnchor>)(aValue));
                return;
            }
            if ((aProperty == "Vertices"))
            {
                Vertices = ((List<Vector2>)(aValue));
                return;
            }
            if ((aProperty == "Transform"))
            {
                Transform = ((Transformation)(aValue));
                return;
            }
            if ((aProperty == "ShapeType"))
            {
                ShapeType = ((ShapeType)(aValue));
                return;
            }
            if ((aProperty == "Color"))
            {
                Color = ((Color)(aValue));
                return;
            }
            if ((aProperty == "Text"))
            {
                Text = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "ExternalId"))
            {
                ExternalId = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "ZIndex"))
            {
                ZIndex = System.Convert.ToSingle(aValue);
                return;
            }
            if ((aProperty == "Size"))
            {
                Size = ((Vector2)(aValue));
                return;
            }
            if ((aProperty == "ShortId"))
            {
                ShortId = ((UInt32)(aValue));
                return;
            }
            if ((aProperty == "Visibility"))
            {
                Visibility = ((VisibilityModes)(aValue));
                return;
            }
            if ((aProperty == "OutlineColor"))
            {
                OutlineColor = ((Color)(aValue));
                return;
            }
            if ((aProperty == "OutlineSize"))
            {
                OutlineSize = System.Convert.ToSingle(aValue);
                return;
            }
            if ((aProperty == "OutlineStyle"))
            {
                OutlineStyle = ((OutlineStyle)(aValue));
                return;
            }
            if ((aProperty == "Selectability"))
            {
                Selectability = ((SelectabilityModes)(aValue));
                return;
            }
            if ((aProperty == "ForcedWidth"))
            {
                ForcedWidth = System.Convert.ToSingle(aValue);
                return;
            }
            if ((aProperty == "ForcedHeight"))
            {
                ForcedHeight = System.Convert.ToSingle(aValue);
                return;
            }
            if ((aProperty == "DropShadow"))
            {
                DropShadow = System.Convert.ToBoolean(aValue);
                return;
            }
            base.setProp(aProperty, aValue);
        }
        
        public override Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if ((aProperty == "DisplayName"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(DisplayName);
            }
            if ((aProperty == "PreviewImage"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(PreviewImage);
            }
            if ((aProperty == "Anchors"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Anchors);
            }
            if ((aProperty == "Vertices"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Vertices);
            }
            if ((aProperty == "Transform"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Transform);
            }
            if ((aProperty == "ShapeType"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ShapeType);
            }
            if ((aProperty == "Color"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Color);
            }
            if ((aProperty == "Text"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Text);
            }
            if ((aProperty == "ExternalId"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ExternalId);
            }
            if ((aProperty == "ZIndex"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ZIndex);
            }
            if ((aProperty == "Size"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Size);
            }
            if ((aProperty == "ShortId"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ShortId);
            }
            if ((aProperty == "Visibility"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Visibility);
            }
            if ((aProperty == "OutlineColor"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(OutlineColor);
            }
            if ((aProperty == "OutlineSize"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(OutlineSize);
            }
            if ((aProperty == "OutlineStyle"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(OutlineStyle);
            }
            if ((aProperty == "Selectability"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Selectability);
            }
            if ((aProperty == "ForcedWidth"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ForcedWidth);
            }
            if ((aProperty == "ForcedHeight"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(ForcedHeight);
            }
            if ((aProperty == "DropShadow"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(DropShadow);
            }
            return base.getProp(aProperty);
        }
        #endregion
    }
}
