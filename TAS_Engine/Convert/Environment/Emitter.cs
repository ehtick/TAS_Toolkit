/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2023, the respective contributors. All rights reserved.
 *
 * Each contributor holds copyright over their respective contributions.
 * The project versioning (Git) records all such contribution source information.
 *                                           
 *                                                                              
 * The BHoM is free software: you can redistribute it and/or modify         
 * it under the terms of the GNU Lesser General Public License as published by  
 * the Free Software Foundation, either version 3.0 of the License, or          
 * (at your option) any later version.                                          
 *                                                                              
 * The BHoM is distributed in the hope that it will be useful,              
 * but WITHOUT ANY WARRANTY; without even the implied warranty of               
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the                 
 * GNU Lesser General Public License for more details.                          
 *                                                                            
 * You should have received a copy of the GNU Lesser General Public License     
 * along with this code. If not, see <https://www.gnu.org/licenses/lgpl-3.0.html>.      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BHA = BH.oM.Architecture;
using BHE = BH.oM.Environment.SpaceCriteria;
using BHG = BH.oM.Geometry;

using BH.oM.Base.Attributes;
using System.ComponentModel;
using BH.oM.Adapters.TAS.Fragments;

using BH.Engine.Base;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Convert
    {
        [Description("Gets BHoM Emitter from TAS TBD Emitter")]
        [Input("tbdEmitter", "TAS TBD Emitter")]
        [Output("BHoM Environmental Emitter object")]
        public static BHE.Equipment FromTAS(this TBD.Emitter tbdEmitter)
        {
            if (tbdEmitter == null) return null;

            BHE.Equipment emitter = new BHE.Equipment();
            emitter.Name = tbdEmitter.name;
            emitter.RadiantFraction = tbdEmitter.radiantProportion;


            TASDescription tasData = new TASDescription();
            tasData.Description = tbdEmitter.description.RemoveBrackets();
            emitter.Fragments.Add(tasData);

            return emitter;            
        }

        [Description("Gets BHoM EmitterType from TAS TBD EmitterTypes")]
        [Input("tbdEmitterType", "TAS TBD EmitterTypes object")]
        [Output("BHoM Environmental EmitterType enum value")]
        public static BHE.EmitterType FromTAS(this TBD.EmitterTypes tbdEmitterType)
        {
            switch(tbdEmitterType)
            {
                case TBD.EmitterTypes.ticCooling:
                case TBD.EmitterTypes.ticCompensatedCooling:
                    return BHE.EmitterType.Cooling;
                case TBD.EmitterTypes.ticHeating:
                case TBD.EmitterTypes.ticCompensatedHeating:
                    return BHE.EmitterType.Heating;
                default:
                    return BHE.EmitterType.Undefined;
            }
        }

        [Description("Gets TAS TBD Emitter from BH.oM.Environment.Elements.Emitter")]
        [Input("emitter", "BHoM Environmental Emitter object")]
        [Output("TAS TBD Emitter")]
        public static TBD.Emitter ToTAS(this BHE.Equipment emitter, TBD.Emitter tbdEmitter)
        {
            //TODO:Gain list (Heating and Cooling factors) are not pushed. The View Coefficient, Radiant Proportion, Temperatures are pushed. Capacity Compensation is not pushed.

            if (emitter == null) return tbdEmitter;

            tbdEmitter.name = emitter.Name;
            tbdEmitter.radiantProportion = (float)emitter.RadiantFraction;

            TASDescription tasFragment = emitter.FindFragment<TASDescription>(typeof(TASDescription));
            if (tasFragment != null)
            {
                tbdEmitter.description = tasFragment.Description;
            }

            return tbdEmitter;
        }

        [Description("Gets TAS TBD EmitterTypes from BHoM EmitterType")]
        [Input("emitterType", "BHoM Environmental EmitterType enum value")]
        [Output("TAS TBD EmitterType enum value")]
        public static TBD.EmitterTypes ToTAS(this BHE.EmitterType emitterType)
        {
            switch(emitterType)
            {
                case BHE.EmitterType.Cooling:
                    return TBD.EmitterTypes.ticCooling;
                case BHE.EmitterType.Heating:
                    return TBD.EmitterTypes.ticHeating;
                default:
                    return TBD.EmitterTypes.ticHeating;
            }
        }
    }
}




