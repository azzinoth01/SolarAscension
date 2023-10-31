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
    
    
    [Serializable()]
    public class IncomingConnection : IArticyBaseObject, IIncomingConnection, IObjectWithColor
    {
        
        [SerializeField()]
        private String mLabel;
        
        [SerializeField()]
        private Color mColor;
        
        [SerializeField()]
        private UInt64 mSourcePin;
        
        [SerializeField()]
        private ArticyValueArticyObject mSource = new ArticyValueArticyObject();
        
        public String Label
        {
            get
            {
                return mLabel;
            }
            set
            {
                mLabel = value;
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
                mColor = value;
            }
        }
        
        public UInt64 SourcePin
        {
            get
            {
                return mSourcePin;
            }
            set
            {
                mSourcePin = value;
            }
        }
        
        public ArticyObject Source
        {
            get
            {
                return mSource.GetValue();
            }
            set
            {
                mSource.SetValue(value);
            }
        }
        
        private void CloneProperties(object aClone, Articy.Unity.ArticyObject aFirstClassParent)
        {
            IncomingConnection newClone = ((IncomingConnection)(aClone));
            newClone.Label = Label;
            newClone.Color = Color;
            newClone.SourcePin = SourcePin;
            if ((mSource != null))
            {
                newClone.mSource = ((ArticyValueArticyObject)(mSource.CloneObject(newClone, aFirstClassParent)));
            }
        }
        
        public object CloneObject(object aParent, Articy.Unity.ArticyObject aFirstClassParent)
        {
            IncomingConnection clone = new IncomingConnection();
            CloneProperties(clone, aFirstClassParent);
            return clone;
        }
        
        public virtual bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            return false;
        }
    }
}
