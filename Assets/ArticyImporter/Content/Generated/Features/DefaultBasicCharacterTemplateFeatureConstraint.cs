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
using Articy.Unity.Constraints;
using Articy.Unity.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Articy.Sola.Features
{
    
    
    public class DefaultBasicCharacterTemplateFeatureConstraint
    {
        
        private Boolean mLoadedConstraints;
        
        private NumberConstraint mAge;
        
        private TextConstraint mNationality;
        
        private EnumConstraint mAccent;
        
        private TextConstraint mFaction;
        
        private TextConstraint mVoiceActor;
        
        private TextConstraint mPersonality;
        
        private TextConstraint mShortInfo;
        
        public NumberConstraint Age
        {
            get
            {
                EnsureConstraints();
                return mAge;
            }
        }
        
        public TextConstraint Nationality
        {
            get
            {
                EnsureConstraints();
                return mNationality;
            }
        }
        
        public EnumConstraint Accent
        {
            get
            {
                EnsureConstraints();
                return mAccent;
            }
        }
        
        public TextConstraint Faction
        {
            get
            {
                EnsureConstraints();
                return mFaction;
            }
        }
        
        public TextConstraint VoiceActor
        {
            get
            {
                EnsureConstraints();
                return mVoiceActor;
            }
        }
        
        public TextConstraint Personality
        {
            get
            {
                EnsureConstraints();
                return mPersonality;
            }
        }
        
        public TextConstraint ShortInfo
        {
            get
            {
                EnsureConstraints();
                return mShortInfo;
            }
        }
        
        public virtual void EnsureConstraints()
        {
            if ((mLoadedConstraints == true))
            {
                return;
            }
            mLoadedConstraints = true;
            mAge = new Articy.Unity.Constraints.NumberConstraint(-3.40282346638529E+38D, 3.40282346638529E+38D, 0, 0, 0, null);
            mNationality = new Articy.Unity.Constraints.TextConstraint(2048, "", null, true, false);
            mAccent = new Articy.Unity.Constraints.EnumConstraint(true, "BySortIndex");
            mFaction = new Articy.Unity.Constraints.TextConstraint(2048, "", null, true, false);
            mVoiceActor = new Articy.Unity.Constraints.TextConstraint(2048, "", null, true, false);
            mPersonality = new Articy.Unity.Constraints.TextConstraint(2048, "", null, true, true);
            mShortInfo = new Articy.Unity.Constraints.TextConstraint(2048, "", null, true, true);
        }
    }
}
