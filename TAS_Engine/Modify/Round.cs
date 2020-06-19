/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2020, the respective contributors. All rights reserved.
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
using System.ComponentModel;
using System.Collections.Generic;

using BH.oM.Base;
using BH.oM.Reflection.Attributes;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Modify
    {
        /***************************************************/
        /**** Public Methods                            ****/
        /***************************************************/

        [Description("Returns a float number rounded to the specified number of decimal places")]
        [Input("number", "A floating point number to round")]
        [Input("decimals", "The number of decimals to round to - default 3 decimal places (e.g. round 3.14159 to 3.141")]
        [Output("number", "The floating point number rounded to the specified number of decimal places")]
        public static float Round(this float number, int decimals = 3)
        {
            return (float)Math.Round(number, decimals);
        }

        [Description("Returns a double number rounded to the specified number of decimal places")]
        [Input("number", "A double to round")]
        [Input("decimals", "The number of decimals to round to - default 3 decimal places (e.g. round 3.14159 to 3.141")]
        [Output("number", "The double number rounded to the specified number of decimal places")]
        public static double Round(this double number, int decimals = 3)
        {
            return Math.Round(number, decimals);
        }

        /***************************************************/
    }
}

