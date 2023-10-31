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
using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Sola.Features
{
    
    
    [Serializable()]
    public class DefaultFactionTemplateFeature : IArticyBaseObject, IPropertyProvider
    {
        
        [SerializeField()]
        private ArticyValueArticyObject mFactionLogo = new ArticyValueArticyObject();
        
        [SerializeField()]
        private String mFactionName;
        
        [SerializeField()]
        private FactionCharacterContact mFactionCharacterContact = new FactionCharacterContact();
        
        [SerializeField()]
        private String mFactionCharacteristic;
        
        [SerializeField()]
        private UInt64 mOwnerId;
        
        [SerializeField()]
        private UInt32 mOwnerInstanceId;
        
        public ArticyObject FactionLogo
        {
            get
            {
                return mFactionLogo.GetValue();
            }
            set
            {
                var oldValue = mFactionLogo;
                mFactionLogo.SetValue(value);
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "DefaultFactionTemplate.FactionLogo", oldValue.GetValue(), mFactionLogo.GetValue());
            }
        }
        
        public String FactionName
        {
            get
            {
                return mFactionName;
            }
            set
            {
                var oldValue = mFactionName;
                mFactionName = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "DefaultFactionTemplate.FactionName", oldValue, mFactionName);
            }
        }
        
        public FactionCharacterContact FactionCharacterContact
        {
            get
            {
                return mFactionCharacterContact;
            }
            set
            {
                var oldValue = mFactionCharacterContact;
                mFactionCharacterContact = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "DefaultFactionTemplate.FactionCharacterContact", oldValue, mFactionCharacterContact);
            }
        }
        
        public String FactionCharacteristic
        {
            get
            {
                return mFactionCharacteristic;
            }
            set
            {
                var oldValue = mFactionCharacteristic;
                mFactionCharacteristic = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "DefaultFactionTemplate.FactionCharacteristic", oldValue, mFactionCharacteristic);
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
            }
        }
        
        private void CloneProperties(object aClone, Articy.Unity.ArticyObject aFirstClassParent)
        {
            Articy.Sola.Features.DefaultFactionTemplateFeature newClone = ((Articy.Sola.Features.DefaultFactionTemplateFeature)(aClone));
            if ((mFactionLogo != null))
            {
                newClone.mFactionLogo = ((ArticyValueArticyObject)(mFactionLogo.CloneObject(newClone, aFirstClassParent)));
            }
            newClone.FactionName = FactionName;
            newClone.FactionCharacterContact = FactionCharacterContact;
            newClone.FactionCharacteristic = FactionCharacteristic;
            newClone.OwnerId = OwnerId;
        }
        
        public object CloneObject(object aParent, Articy.Unity.ArticyObject aFirstClassParent)
        {
            Articy.Sola.Features.DefaultFactionTemplateFeature clone = new Articy.Sola.Features.DefaultFactionTemplateFeature();
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
            if ((aProperty == "FactionLogo"))
            {
                FactionLogo = Articy.Unity.Interfaces.BaseScriptFragments.ObjectToModelRep(aValue);
                return;
            }
            if ((aProperty == "FactionName"))
            {
                FactionName = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "FactionCharacterContact"))
            {
                FactionCharacterContact = ((FactionCharacterContact)(aValue));
                return;
            }
            if ((aProperty == "FactionCharacteristic"))
            {
                FactionCharacteristic = System.Convert.ToString(aValue);
                return;
            }
        }
        
        public Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if ((aProperty == "FactionLogo"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(FactionLogo);
            }
            if ((aProperty == "FactionName"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(FactionName);
            }
            if ((aProperty == "FactionCharacterContact"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(FactionCharacterContact);
            }
            if ((aProperty == "FactionCharacteristic"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(FactionCharacteristic);
            }
            return null;
        }
        #endregion
    }
}
