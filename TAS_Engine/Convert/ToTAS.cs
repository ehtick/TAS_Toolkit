﻿using System;
using System.Collections;
using System.Collections.Generic;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHS = BH.oM.Structural;
using BH.oM.Environmental.Elements;
using BH.oM.Environmental.Properties;
using BH.oM.Environmental.Interface;
using BHG = BH.oM.Geometry;
using BH.Engine;
using TBD;
using BH.Adapter.TAS;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {

        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static TBD.TasPoint ToTas(this BHG.Point bHoMPoint, TasPoint tasPoint)
        {
            tasPoint.x = (float)(bHoMPoint.X);
            tasPoint.y = (float)(bHoMPoint.Y);
            tasPoint.z = (float)(bHoMPoint.Z);
            return tasPoint;
        }

        /***************************************************/

        public static TBD.RoomSurface ToTas(this BuildingElementPanel bHoMPanel, RoomSurface tasRoomSurface)
        {
            tasRoomSurface.area = 120;
            TBD.Perimeter tasPerimeter = tasRoomSurface.CreatePerimeter();
            return tasRoomSurface;
        }

        /***************************************************/

        public static TBD.zoneSurface ToTas(this BuildingElementPanel bHoMPanel, zoneSurface tasZoneSrf)
        {
            tasZoneSrf.orientation = BH.Engine.TAS.Query.Query.GetOrientation(bHoMPanel);
            tasZoneSrf.inclination = 4;
            tasZoneSrf.altitude = 3;
            tasZoneSrf.altitudeRange = 2;
            tasZoneSrf.GUID = bHoMPanel.BHoM_Guid.ToString();
            tasZoneSrf.area = (float)BH.Engine.Geometry.Query.Area(bHoMPanel.PolyCurve);
            
            return tasZoneSrf;
        }


        /***************************************************/
        /**** Public Methods - Objects                  ****/
        /***************************************************/

        //public static TBD.material ToTas(this BHE.Elements.OpaqueMaterial bHoMOpaqueMaterial)
        //{
        //    TBD.material tasMaterial = new TBD.material
        //    {
        //        name = bHoMOpaqueMaterial.Name,
        //        description = bHoMOpaqueMaterial.Description,
        //        width = (float)bHoMOpaqueMaterial.Thickness,
        //        conductivity = (float)bHoMOpaqueMaterial.Conductivity,
        //        vapourDiffusionFactor = (float)bHoMOpaqueMaterial.VapourDiffusionFactor,
        //        externalSolarReflectance = (float)bHoMOpaqueMaterial.SolarReflectanceExternal,
        //        internalSolarReflectance = (float)bHoMOpaqueMaterial.SolarReflectanceInternal,
        //        externalLightReflectance = (float)bHoMOpaqueMaterial.LightReflectanceExternal,
        //        internalLightReflectance = (float)bHoMOpaqueMaterial.LightReflectanceInternal,
        //        externalEmissivity = (float)bHoMOpaqueMaterial.EmissivityExternal,
        //        internalEmissivity = (float)bHoMOpaqueMaterial.EmissivityInternal
        //    };

        //    return tasMaterial;
        //}

        /***************************************************/

        public static TBD.ConstructionClass ToTas(this BuildingElementProperties bHoMBuildingElementProperties)
        {

            TBD.ConstructionClass tasConstruction = new TBD.ConstructionClass
            {
                name = bHoMBuildingElementProperties.Name
            };

            return tasConstruction;
        }

    
        /***************************************************/


        /***************************************************/
        /**** Public Methods - Enums                    ****/
        /***************************************************/

        public static TBD.MaterialTypes ToTas(this BHE.Elements.MaterialType bHoMMaterialType)
        {
            switch (bHoMMaterialType)
            {
                case BHE.Elements.MaterialType.Opaque:
                    return MaterialTypes.tcdOpaqueMaterial;
                case BHE.Elements.MaterialType.Transparent:
                    return MaterialTypes.tcdTransparentLayer;
                case BHE.Elements.MaterialType.Gas:
                    return MaterialTypes.tcdGasLayer;
                default:
                    return MaterialTypes.tcdOpaqueMaterial;
            }
        }

        /***************************************************/

        public static TBD.BuildingElementType ToTas(this BHE.Elements.BuidingElementType bHoMBuildingElementType)
        {
            switch (bHoMBuildingElementType) // This is just a test, it doeas not match. We have more BETypes in Tas than in BHoM
            {
                case BHE.Elements.BuidingElementType.Wall:
                    return BuildingElementType.EXTERNALWALL; //What about the other TBD Wall types??
                case BHE.Elements.BuidingElementType.Roof:
                    return BuildingElementType.ROOFELEMENT;
                case BHE.Elements.BuidingElementType.Ceiling:
                    return BuildingElementType.UNDERGROUNDCEILING;
                case BHE.Elements.BuidingElementType.Floor:
                    return BuildingElementType.INTERNALFLOOR;
                default:
                    return BuildingElementType.EXTERNALWALL;
            }
        }





    }
}
