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
    public class ArticyDefaultCharacterBasicFeature : IArticyBaseObject, IPropertyProvider
    {
        
        [SerializeField()]
        private Single mAge;
        
        [SerializeField()]
        private String mLocaKey_Species;
        
        [SerializeField()]
        private String mOverwritten_Species = "";
        
        [SerializeField()]
        private String mLocaKey_BornIn;
        
        [SerializeField()]
        private String mOverwritten_BornIn = "";
        
        [SerializeField()]
        private Sex mSex = new Sex();
        
        [SerializeField()]
        private String mLocaKey_Occupation;
        
        [SerializeField()]
        private String mOverwritten_Occupation = "";
        
        [SerializeField()]
        private String mLocaKey_Accent;
        
        [SerializeField()]
        private String mOverwritten_Accent = "";
        
        [SerializeField()]
        private String mLocaKey_Personality;
        
        [SerializeField()]
        private String mOverwritten_Personality = "";
        
        [SerializeField()]
        private String mLocaKey_Appearance;
        
        [SerializeField()]
        private String mOverwritten_Appearance = "";
        
        [SerializeField()]
        private UInt64 mOwnerId;
        
        [SerializeField()]
        private UInt32 mOwnerInstanceId;
        
        public Single Age
        {
            get
            {
                return mAge;
            }
            set
            {
                var oldValue = mAge;
                mAge = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.Age", oldValue, mAge);
            }
        }
        
        public String LocaKey_Species
        {
            get
            {
                return mLocaKey_Species;
            }
        }
        
        public String Species
        {
            get
            {
                if ((mOverwritten_Species != ""))
                {
                    return mOverwritten_Species;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_Species);
            }
            set
            {
                var oldValue = Species;
                mOverwritten_Species = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.Species", oldValue, mOverwritten_Species);
            }
        }
        
        public String LocaKey_BornIn
        {
            get
            {
                return mLocaKey_BornIn;
            }
        }
        
        public String BornIn
        {
            get
            {
                if ((mOverwritten_BornIn != ""))
                {
                    return mOverwritten_BornIn;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_BornIn);
            }
            set
            {
                var oldValue = BornIn;
                mOverwritten_BornIn = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.BornIn", oldValue, mOverwritten_BornIn);
            }
        }
        
        public Sex Sex
        {
            get
            {
                return mSex;
            }
            set
            {
                var oldValue = mSex;
                mSex = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.Sex", oldValue, mSex);
            }
        }
        
        public String LocaKey_Occupation
        {
            get
            {
                return mLocaKey_Occupation;
            }
        }
        
        public String Occupation
        {
            get
            {
                if ((mOverwritten_Occupation != ""))
                {
                    return mOverwritten_Occupation;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_Occupation);
            }
            set
            {
                var oldValue = Occupation;
                mOverwritten_Occupation = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.Occupation", oldValue, mOverwritten_Occupation);
            }
        }
        
        public String LocaKey_Accent
        {
            get
            {
                return mLocaKey_Accent;
            }
        }
        
        public String Accent
        {
            get
            {
                if ((mOverwritten_Accent != ""))
                {
                    return mOverwritten_Accent;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_Accent);
            }
            set
            {
                var oldValue = Accent;
                mOverwritten_Accent = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.Accent", oldValue, mOverwritten_Accent);
            }
        }
        
        public String LocaKey_Personality
        {
            get
            {
                return mLocaKey_Personality;
            }
        }
        
        public String Personality
        {
            get
            {
                if ((mOverwritten_Personality != ""))
                {
                    return mOverwritten_Personality;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_Personality);
            }
            set
            {
                var oldValue = Personality;
                mOverwritten_Personality = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.Personality", oldValue, mOverwritten_Personality);
            }
        }
        
        public String LocaKey_Appearance
        {
            get
            {
                return mLocaKey_Appearance;
            }
        }
        
        public String Appearance
        {
            get
            {
                if ((mOverwritten_Appearance != ""))
                {
                    return mOverwritten_Appearance;
                }
                return Articy.Unity.ArticyDatabase.Localization.Localize(mLocaKey_Appearance);
            }
            set
            {
                var oldValue = Appearance;
                mOverwritten_Appearance = value;
                Articy.Unity.ArticyDatabase.ObjectNotifications.ReportChanged(OwnerId, OwnerInstanceId, "ArticyDefaultCharacterBasic.Appearance", oldValue, mOverwritten_Appearance);
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
            Articy.Sola.Features.ArticyDefaultCharacterBasicFeature newClone = ((Articy.Sola.Features.ArticyDefaultCharacterBasicFeature)(aClone));
            newClone.Age = Age;
            newClone.mLocaKey_Species = mLocaKey_Species;
            newClone.mOverwritten_Species = mOverwritten_Species;
            newClone.mLocaKey_BornIn = mLocaKey_BornIn;
            newClone.mOverwritten_BornIn = mOverwritten_BornIn;
            newClone.Sex = Sex;
            newClone.mLocaKey_Occupation = mLocaKey_Occupation;
            newClone.mOverwritten_Occupation = mOverwritten_Occupation;
            newClone.mLocaKey_Accent = mLocaKey_Accent;
            newClone.mOverwritten_Accent = mOverwritten_Accent;
            newClone.mLocaKey_Personality = mLocaKey_Personality;
            newClone.mOverwritten_Personality = mOverwritten_Personality;
            newClone.mLocaKey_Appearance = mLocaKey_Appearance;
            newClone.mOverwritten_Appearance = mOverwritten_Appearance;
            newClone.OwnerId = OwnerId;
        }
        
        public object CloneObject(object aParent, Articy.Unity.ArticyObject aFirstClassParent)
        {
            Articy.Sola.Features.ArticyDefaultCharacterBasicFeature clone = new Articy.Sola.Features.ArticyDefaultCharacterBasicFeature();
            CloneProperties(clone, aFirstClassParent);
            return clone;
        }
        
        public virtual bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            if ((mOverwritten_Species != ""))
            {
                return true;
            }
            if ((mOverwritten_BornIn != ""))
            {
                return true;
            }
            if ((mOverwritten_Occupation != ""))
            {
                return true;
            }
            if ((mOverwritten_Accent != ""))
            {
                return true;
            }
            if ((mOverwritten_Personality != ""))
            {
                return true;
            }
            if ((mOverwritten_Appearance != ""))
            {
                return true;
            }
            return false;
        }
        
        #region property provider interface
        public void setProp(string aProperty, object aValue)
        {
            if ((aProperty == "Age"))
            {
                Age = System.Convert.ToSingle(aValue);
                return;
            }
            if ((aProperty == "Species"))
            {
                Species = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "BornIn"))
            {
                BornIn = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "Sex"))
            {
                Sex = ((Sex)(aValue));
                return;
            }
            if ((aProperty == "Occupation"))
            {
                Occupation = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "Accent"))
            {
                Accent = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "Personality"))
            {
                Personality = System.Convert.ToString(aValue);
                return;
            }
            if ((aProperty == "Appearance"))
            {
                Appearance = System.Convert.ToString(aValue);
                return;
            }
        }
        
        public Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if ((aProperty == "Age"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Age);
            }
            if ((aProperty == "Species"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Species);
            }
            if ((aProperty == "BornIn"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(BornIn);
            }
            if ((aProperty == "Sex"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Sex);
            }
            if ((aProperty == "Occupation"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Occupation);
            }
            if ((aProperty == "Accent"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Accent);
            }
            if ((aProperty == "Personality"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Personality);
            }
            if ((aProperty == "Appearance"))
            {
                return new Articy.Unity.Interfaces.ScriptDataProxy(Appearance);
            }
            return null;
        }
        #endregion
    }
}
