//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Articy.Sola.Features;
using Articy.Unity;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Sola
{
    
    
    public class DefaultFirmCharacterTemplate : Entity, IEntity, IPropertyProvider, IObjectWithFeatureDefaultExtendedCharacterTemplate, IObjectWithFeatureDefaultBasicCharacterTemplate
    {
        
        [SerializeField()]
        private ArticyValueDefaultFirmCharacterTemplateTemplate mTemplate = new ArticyValueDefaultFirmCharacterTemplateTemplate();
        
        private static Articy.Sola.Templates.DefaultFirmCharacterTemplateTemplateConstraint mConstraints = new Articy.Sola.Templates.DefaultFirmCharacterTemplateTemplateConstraint();
        
        public Articy.Sola.Templates.DefaultFirmCharacterTemplateTemplate Template
        {
            get
            {
                return mTemplate.GetValue();
            }
            set
            {
                mTemplate.SetValue(value);
            }
        }
        
        public static Articy.Sola.Templates.DefaultFirmCharacterTemplateTemplateConstraint Constraints
        {
            get
            {
                return mConstraints;
            }
        }
        
        public DefaultExtendedCharacterTemplateFeature GetFeatureDefaultExtendedCharacterTemplate()
        {
            return Template.DefaultExtendedCharacterTemplate;
        }
        
        public DefaultBasicCharacterTemplateFeature GetFeatureDefaultBasicCharacterTemplate()
        {
            return Template.DefaultBasicCharacterTemplate;
        }
        
        protected override void CloneProperties(object aClone, Articy.Unity.ArticyObject aFirstClassParent)
        {
            DefaultFirmCharacterTemplate newClone = ((DefaultFirmCharacterTemplate)(aClone));
            if ((Template != null))
            {
                newClone.Template = ((Articy.Sola.Templates.DefaultFirmCharacterTemplateTemplate)(Template.CloneObject(newClone, aFirstClassParent)));
            }
            base.CloneProperties(newClone, aFirstClassParent);
        }
        
        public override bool IsLocalizedPropertyOverwritten(string aProperty)
        {
            return base.IsLocalizedPropertyOverwritten(aProperty);
        }
        
        #region property provider interface
        public override void setProp(string aProperty, object aValue)
        {
            if (aProperty.Contains("."))
            {
                Template.setProp(aProperty, aValue);
                return;
            }
            base.setProp(aProperty, aValue);
        }
        
        public override Articy.Unity.Interfaces.ScriptDataProxy getProp(string aProperty)
        {
            if (aProperty.Contains("."))
            {
                return Template.getProp(aProperty);
            }
            return base.getProp(aProperty);
        }
        #endregion
    }
}
