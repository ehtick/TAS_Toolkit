﻿using System.Collections.Generic;
using BHE = BH.oM.Environmental;
using BHG = BH.oM.Geometry;
using TBD;

namespace BH.Engine.TAS
{
    public static partial class Convert
    {
        /***************************************************/
        /**** Public Methods - BHoM Objects             ****/
        /***************************************************/

        public static BHE.Elements.BuildingElement ToBHoM(TBD.Building ITasBuilding, int BuildingElementIndex)
        {
            BHE.Elements.BuildingElement BHoMBuildingElement = new BHE.Elements.BuildingElement()
            {
                BEType = ITasBuilding.GetBuildingElement(BuildingElementIndex).BEType,
                Name = ITasBuilding.GetBuildingElement(BuildingElementIndex).name,
                Ghost = ITasBuilding.GetBuildingElement(BuildingElementIndex).ghost,
                Width = ITasBuilding.GetBuildingElement(BuildingElementIndex).width,
                Ground = ITasBuilding.GetBuildingElement(BuildingElementIndex).ground
            };
            return BHoMBuildingElement;
        }

        /***************************************************/

        public static BHE.Elements.BuildingElement ToBHoM(TBD.zoneSurface ITasSurface)
        {
            BHE.Elements.BuildingElement BHoMBuildingElement = new BHE.Elements.BuildingElement()
            {
                BEType = ITasSurface.buildingElement.BEType,
                Name = ITasSurface.buildingElement.name
            };
            return BHoMBuildingElement;
        }

        /***************************************************/

        public static BHE.Elements.Location ToBHoM(this TBD.Building tasBuilding)
        {
            BHE.Elements.Location bHoMLocation = new BHE.Elements.Location()
            {
                Latitude = tasBuilding.latitude,
                Longitude = tasBuilding.longitude
            };
            return bHoMLocation;
        }

        /***************************************************/

        public static BHE.Elements.Space ToBHoM(this TBD.zone tasZone)
        {
            BHE.Elements.Space bHoMSpace = new BHE.Elements.Space();
            bHoMSpace.Name = tasZone.name;
            return bHoMSpace;
        }

        /***************************************************/

        // A TAS object must correspond just to one BHoMObject. If you feel things don't work and a duplicate appears plausible, the problem is in the BHoM, not in the converter
        public static BHE.Elements.Panel ToBHoM(TBD.zoneSurface ITasSurface, BHG.Polyline edges)
        {
            BHE.Elements.Panel bHoMPanel = new BHE.Elements.Panel();
                   
            //bHoMPanel.Type = ITasSurface.type.ToString();
            bHoMPanel.Edges = edges;
            return bHoMPanel;
        }


        /***************************************************/
        /**** Public Methods - Geometry                 ****/
        /***************************************************/

        public static BHG.Point ToBHoM(this TBD.TasPoint tasPoint)
        {
            BHG.Point bHoMPoint = new BHG.Point()
            {
                X = tasPoint.x,
                Y = tasPoint.y,
                Z = tasPoint.z
            };
            return bHoMPoint;
        }

        /***************************************************/

        public static BHG.Polyline ToBHoM(this TBD.Polygon tasPolygon)  // TODO : When BH.oM.Geometry.Contour is implemented, Polyline can be replaced with Contour
        {
            //
            //  Not sure how this is working but that's a very strange way of getting points for Tas. Are you sure it is the only way?
            //
            List<BHG.Point> bHoMPointList = new List<BHG.Point>();
            int pointIndex = 0;
            while (true)
            {
                TasPoint tasPt= tasPolygon.GetPoint(pointIndex);
                if (tasPt == null) { break; }
                bHoMPointList.Add(tasPt.ToBHoM());
                pointIndex++;
            }
            bHoMPointList.Add(bHoMPointList[0]);
            return new BHG.Polyline { ControlPoints = bHoMPointList };
        }

        /***************************************************/
    }
}
