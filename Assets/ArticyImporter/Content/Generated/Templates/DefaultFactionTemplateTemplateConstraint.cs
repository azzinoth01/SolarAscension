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
    
    
    public class DefaultFactionTemplateTemplateConstraint
    {
        
        private FactionTemplateExtendedFeatureConstraint mFactionTemplateExtended = new FactionTemplateExtendedFeatureConstraint();
        
        private DefaultFactionTemplateFeatureConstraint mDefaultFactionTemplate = new DefaultFactionTemplateFeatureConstraint();
        
        public FactionTemplateExtendedFeatureConstraint FactionTemplateExtended
        {
            get
            {
                return mFactionTemplateExtended;
            }
        }
        
        public DefaultFactionTemplateFeatureConstraint DefaultFactionTemplate
        {
            get
            {
                return mDefaultFactionTemplate;
            }
        }
    }
}
