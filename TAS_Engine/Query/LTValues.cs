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
using BHG = BH.oM.Geometry;
using BHEE = BH.oM.Environment.Elements;

using BH.Engine.Environment;

namespace BH.Engine.Adapters.TAS
{
    public static partial class Query
    {
        public static double LTValue(this TBD.buildingElement tbdBuildingElement, int decimals = 3)
        {
            TBD.Construction aConstruction = tbdBuildingElement.GetConstruction();
            if (aConstruction == null)
                return 0;

            TBD.ConstructionTypes aConstructionTypes = aConstruction.type;
            if (aConstructionTypes == TBD.ConstructionTypes.tcdTransparentConstruction)
            {
                object aObject = aConstruction.GetGlazingValues();
                List<float> aValueList = Convert.ToFloatList(aObject);
                return Math.Round(aValueList[0], decimals);
            }
            return 0;
        }
    }
}


