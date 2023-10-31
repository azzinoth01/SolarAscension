//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Sola;
using Articy.Sola.Features;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Sola.Templates
{
    
    
    [Serializable()]
    public class DefaultFactionTemplateTemplate : IArticyBaseObject, IPropertyProvider
    {
        
        [SerializeField()]
        private ArticyValueFactionTemplateExtendedFeature mFactionTemplateExtended = new ArticyValueFactionTemplateExtendedFeature();
        
        [SerializeField()]
        private ArticyValueDefaultFactionTemplateFeature mDefaultFactionTemplate = new ArticyValueDefaultFactionTemplateFeature();
        
        [SerializeField()]
        private UInt64 mOwnerId;
        
        [SerializeField()]
        private UInt32 mOwnerInstanceId;
        
        public Articy.Sola.Features.FactionTemplateExtendedFeature FactionTemplateExtended
        {
            get
            {
                return mFactionTemplateExtended.GetValue();
            }
            set
            {
                mFactionTemplateExtended.SetValue(value);
            }
        }
        
        public Articy.Sola.Features.DefaultFactionTemplateFeature DefaultFactionTemplate
        {
            get
            {
                return mDefaultFactionTemplate.GetValue();
            }
            set
            {
                mDefaultFactionTemplate.SetValue(value);
            }
        }
        
        public UInt64 OwnerId
        {
            get
            {
                return mOwnerId;
            }
            set
            {
                mOwnerId = value;
                FactionTemplateExtended.OwnerId = value;
                DefaultFactionTemplate.OwnerId = value;
            }
        }
        
        public UInt32 OwnerInstanceId
        {
            get
            {
                return mOwnerInstanceId;
            }
            set
            {
                mOwnerInstanceId = value;
                FactionTemplateExtended.OwnerInstanceId = value;
                DefaultFactionTemplate.OwnerInstanceId = value;
            }
        }
        
        private void CloneProperties(object aClone, Articy.Unity.ArticyObject aFirstClassParent)
        {
            Articy.Sola.Templates.DefaultFactionTemplateTemplate newClone = ((Articy.Sola.Templates.DefaultFactionTemplateTemplate)(aClone));
            if ((FactionTemplateExtended != null))
            {
                newClone.FactionTemplateExtended = ((Articy.Sola.Features.FactionTemplateExtendedFeature)(FactionTemplateExtended.CloneObject(newClone, aFirstClassParent)));
            }
            if ((DefaultFactionTemplate != null))
            {
                newClone.DefaultFactionTemplate = ((Articy.Sola.Features.DefaultFactionTemplateFeature)(DefaultFactionTemplate.CloneObject(newClone, aFirstClassParent)));
            }
            newClone.OwnerId = OwnerId;
        }
        
        public object CloneObject(object aParent, Articy.Unity.ArticyObject aFirstClassParent)
        {
            Articy.Sola.Templates.DefaultFactionTemplateTemplate clone = new Articy.Sola.Templates.DefaultFactionTemplateTemplate();
            CloneProperties(clone, aFirstClassParent);
            return clone;
        }
        
        public virtual bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            return false;
        }
        
        #region property provider interface
        public void setProp(string aProperty, object aValue)
        {
            int featureIndex = aProperty.IndexOf('.');
            if ((featureIndex != -1))
            {
                string featurePath = aProperty.Substring(0, featureIndex);
                string featureProperty = aProperty.Substring((featureIndex + 1));
                if ((featurePath == "FactionTemplateExtended"))
                {
                    FactionTemplateExtended.setProp(featureProperty, aValue);
                }
                if ((featurePath == "DefaultFactionTemplate"))
                {
                    DefaultFactionTemplate.setProp(featureProperty, aValue);
                }
            }
        }
        
        public Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            int featureIndex = aProperty.IndexOf('.');
            if ((featureIndex != -1))
            {
                string featurePath = aProperty.Substring(0, featureIndex);
                string featureProperty = aProperty.Substring((featureIndex + 1));
                if ((featurePath == "FactionTemplateExtended"))
                {
                    return FactionTemplateExtended.getProp(featureProperty);
                }
                if ((featurePath == "DefaultFactionTemplate"))
                {
                    return DefaultFactionTemplate.getProp(featureProperty);
                }
            }
            return null;
        }
        #endregion
    }
}
