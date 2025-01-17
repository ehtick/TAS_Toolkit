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
using System.ComponentModel;
using BH.oM.Base.Attributes;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Query
    {
        [Description("Get Single Upper Limit")]
        [Input("tbdICThermostat", "tbd IC Thermostat")]
        [Output("maxUL", "return max UpperLimit value")]
        public static float GetSingleValueUpperLimit(this TBD.Thermostat tbdICThermostat)
        {
            float maxUL = 150;

            if (tbdICThermostat == null)
                return -1;

            TBD.profile tbdUpperLimitProfile = tbdICThermostat.GetProfile((int)TBD.Profiles.ticUL);
            switch (tbdUpperLimitProfile.type)
            {
                case TBD.ProfileTypes.ticValueProfile:
                    maxUL = tbdUpperLimitProfile.value;
                    break;
                case TBD.ProfileTypes.ticHourlyProfile:
                    for (int i = 1; i <= 24; i++)
                    {
                        if (tbdUpperLimitProfile.hourlyValues[i] <= maxUL)
                            maxUL = tbdUpperLimitProfile.hourlyValues[i];
                    }

                    break;
                case TBD.ProfileTypes.ticYearlyProfile:
                    for (int i = 1; i <= 8760; i++)
                    {
                        if (tbdUpperLimitProfile.yearlyValues[i] >= maxUL)
                            maxUL = tbdUpperLimitProfile.yearlyValues[i];
                    }
                    break;
                    // case other profile types etc.
            }

            return maxUL;
        }
    }
}




