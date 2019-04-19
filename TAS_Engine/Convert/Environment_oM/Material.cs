﻿/*
 * This file is part of the Buildings and Habitats object Model (BHoM)
 * Copyright (c) 2015 - 2018, the respective contributors. All rights reserved.
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
using BHE = BH.oM.Environment.Elements;
using BHM = BH.oM.Environment.Materials;
using BHP = BH.oM.Environment.Properties;
using BHG = BH.oM.Geometry;

using BHPM = BH.oM.Physical.Properties;
using BHPMC = BH.oM.Physical.Properties.Construction;

using BH.oM.Reflection.Attributes;
using System.ComponentModel;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental Material from a TAS TBD Material")]
        [Input("tbdMaterial", "TAS TBD Material")]
        [Output("BHoM Environmental Material")]
        public static BHPMC.Layer ToBHoM(this TBD.material tbdMaterial, TBD.Construction tbdConstruction)
        {
            if (tbdMaterial == null) return null;

            BHPMC.Layer layer = new BHPMC.Layer();
            layer.Thickness = tbdMaterial.width;

            BHPM.Material material = new BHPM.Material();
            material.Name = tbdMaterial.name;
            material.Properties.Add(tbdMaterial.ToBHoMProperties(tbdConstruction));

            layer.Material = material;

            return layer;
        }

        [Description("BH.Engine.TAS.Convert ToBHoM => gets a BHoM Environmental MaterialProperties from a TAS TBD Material")]
        [Input("tbdMaterial", "TAS TBD Material")]
        [Output("BHoM Environmental MaterialProperties")]
        public static BHM.IEnvironmentMaterial ToBHoMProperties(this TBD.material tbdMaterial, TBD.Construction tbdConstruction)
        {
            if (tbdMaterial == null) return null;

            TBD.MaterialTypes matType = (TBD.MaterialTypes)tbdMaterial.type;

            switch (matType)
            {
                case TBD.MaterialTypes.tcdOpaqueLayer:
                case TBD.MaterialTypes.tcdOpaqueMaterial:
                    return new BHM.SolidMaterial
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        Conductivity = tbdMaterial.conductivity,
                        SpecificHeat = tbdMaterial.specificHeat,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                        SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                        LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                        LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                        EmissivityExternal = tbdMaterial.externalEmissivity,
                        EmissivityInternal = tbdMaterial.internalEmissivity,
                        AdditionalHeatTransfer = tbdConstruction.additionalHeatTransfer,
                    };
                case TBD.MaterialTypes.tcdTransparentLayer:
                    return new BHM.SolidMaterial
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        Conductivity = tbdMaterial.conductivity,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                        SolarTransmittance = tbdMaterial.solarTransmittance,
                        SolarReflectanceExternal = tbdMaterial.externalSolarReflectance,
                        SolarReflectanceInternal = tbdMaterial.internalSolarReflectance,
                        LightTransmittance = tbdMaterial.lightTransmittance,
                        LightReflectanceExternal = tbdMaterial.externalLightReflectance,
                        LightReflectanceInternal = tbdMaterial.internalLightReflectance,
                        EmissivityExternal = tbdMaterial.externalEmissivity,
                        EmissivityInternal = tbdMaterial.internalEmissivity,
                        Transparency = 1,
                        AdditionalHeatTransfer = tbdConstruction.additionalHeatTransfer,
                    };
                case TBD.MaterialTypes.tcdGasLayer:
                    return new BHM.GasMaterial
                    {
                        Name = tbdMaterial.name,
                        Description = tbdMaterial.description,
                        ConvectionCoefficient = tbdMaterial.convectionCoefficient,
                        VapourDiffusionFactor = tbdMaterial.vapourDiffusionFactor,
                        AdditionalHeatTransfer = tbdConstruction.additionalHeatTransfer,
                    };
                default:
                    return new BHM.SolidMaterial();
            }
        }

        [Description("BH.Engine.TAS.Convert ToTASType => gets a TAS TBD MaterialType from a BHoM Environmental MaterialType")]
        [Input("material", "BHoM Material")]
        [Output("TAS Material Type", "TAS TBD MaterialType")]
        public static TBD.MaterialTypes ToTASType(this BHPM.Material material)
        {
            BHM.IEnvironmentMaterial envMaterial = material.Properties.Where(x => x.GetType() == typeof(BHM.IEnvironmentMaterial)).FirstOrDefault() as BHM.IEnvironmentMaterial;
            if (envMaterial.GetType() == typeof(BHM.SolidMaterial))
            {
                if ((envMaterial as BHM.SolidMaterial).Transparency != 0)
                    return TBD.MaterialTypes.tcdTransparentLayer;
                else
                    return TBD.MaterialTypes.tcdOpaqueLayer;
            }
            else if (envMaterial.GetType() == typeof(BHM.GasMaterial))
                return TBD.MaterialTypes.tcdGasLayer;

            return TBD.MaterialTypes.tcdOpaqueLayer;
        }

        [Description("BH.Engine.TAS.Convert ToTAS => gets a TAS TBD Material from a BHoM Environmental Material")]
        [Input("material", "BHoM Environmental Material")]
        [Output("TAS TBD Material")]
        public static TBD.material ToTAS(this BHPMC.Layer layer, TBD.material tbdMaterial)
        {
            if (layer == null) return tbdMaterial;
            if (layer.Material != null)
                tbdMaterial.name = layer.Material.Name;
            tbdMaterial.width = (float)layer.Thickness;

            tbdMaterial.type = (int)layer.Material.ToTASType();

            BHM.IEnvironmentMaterial envMat = layer.Material.Properties.Where(x => x.GetType() == typeof(BHM.IEnvironmentMaterial)).FirstOrDefault() as BHM.IEnvironmentMaterial;

            switch ((TBD.MaterialTypes)tbdMaterial.type)
            {
                case TBD.MaterialTypes.tcdGasLayer:
                    tbdMaterial.convectionCoefficient = (float)((BHM.GasMaterial)envMat).ConvectionCoefficient;
                    tbdMaterial.vapourDiffusionFactor = (float)((BHM.GasMaterial)envMat).VapourDiffusionFactor;
                    tbdMaterial.description = ((BHM.GasMaterial)envMat).Description;

                    break;
                case TBD.MaterialTypes.tcdOpaqueLayer: //Thickness not showing
                    tbdMaterial.conductivity = (float)((BHM.SolidMaterial)envMat).Conductivity;
                    tbdMaterial.specificHeat = (float)((BHM.SolidMaterial)envMat).SpecificHeat;
                    tbdMaterial.density = (float)layer.Material.Density;
                    tbdMaterial.vapourDiffusionFactor = (float)((BHM.SolidMaterial)envMat).VapourDiffusionFactor;
                    tbdMaterial.externalSolarReflectance = (float)((BHM.SolidMaterial)envMat).SolarReflectanceExternal;
                    tbdMaterial.internalSolarReflectance = (float)((BHM.SolidMaterial)envMat).SolarReflectanceInternal;
                    tbdMaterial.externalLightReflectance = (float)((BHM.SolidMaterial)envMat).LightReflectanceExternal;
                    tbdMaterial.internalLightReflectance = (float)((BHM.SolidMaterial)envMat).LightReflectanceInternal;
                    tbdMaterial.externalEmissivity = (float)((BHM.SolidMaterial)envMat).EmissivityExternal;
                    tbdMaterial.internalEmissivity = (float)((BHM.SolidMaterial)envMat).EmissivityInternal;
                    tbdMaterial.description = ((BHM.SolidMaterial)envMat).Description;

                    break;
                case TBD.MaterialTypes.tcdTransparentLayer:
                    tbdMaterial.conductivity = (float)((BHM.SolidMaterial)envMat).Conductivity;
                    tbdMaterial.vapourDiffusionFactor = (float)((BHM.SolidMaterial)envMat).VapourDiffusionFactor;
                    tbdMaterial.solarTransmittance = (float)((BHM.SolidMaterial)envMat).SolarTransmittance;
                    tbdMaterial.externalSolarReflectance = (float)((BHM.SolidMaterial)envMat).SolarReflectanceExternal;
                    tbdMaterial.internalSolarReflectance = (float)((BHM.SolidMaterial)envMat).SolarReflectanceInternal;
                    tbdMaterial.lightTransmittance = (float)((BHM.SolidMaterial)envMat).LightTransmittance;
                    tbdMaterial.externalLightReflectance = (float)((BHM.SolidMaterial)envMat).LightReflectanceExternal;
                    tbdMaterial.internalLightReflectance = (float)((BHM.SolidMaterial)envMat).LightReflectanceInternal;
                    tbdMaterial.externalEmissivity = (float)((BHM.SolidMaterial)envMat).EmissivityExternal;
                    tbdMaterial.internalEmissivity = (float)((BHM.SolidMaterial)envMat).EmissivityInternal;
                    tbdMaterial.description = ((BHM.SolidMaterial)envMat).Description;
                    break;
            }
            return tbdMaterial;
        }
    }
}