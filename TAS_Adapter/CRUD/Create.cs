﻿using System;
using System.Collections.Generic;
using System.Linq;
using BH.oM.Base;
using BHE = BH.oM.Environmental;
using BHG = BH.oM.Geometry;
using System.Runtime.InteropServices;
using BH.Engine.Environment;

namespace BH.Adapter.TAS
{
    public partial class TasAdapter : BHoMAdapter
    {
        /***************************************************/
        /**** Protected Methods                         ****/
        /***************************************************/

        protected override bool Create<T>(IEnumerable<T> objects, bool replaceAll = false)
        {
            bool success = true;

            if (typeof(IBHoMObject).IsAssignableFrom(typeof(T)))
            {
                success = CreateCollection(objects as dynamic);
                //foreach (T obj in objects)
                //{
                //    success &= Create(obj as dynamic);
                //}
            }

            m_TBDDocumentInstance.save();
            m_TBDDocumentInstance.close();
     
            if (m_TBDDocumentInstance != null)
            {
                // issue with closing files and not closing 
                ClearCOMObject(m_TBDDocumentInstance);
                m_TBDDocumentInstance = null;
            }
            return success; 
        }

        /***************************************************/

        public static void ClearCOMObject(object Object)
        {
            if (Object == null) return;
            int intrefcount = 0;
            do
            {
                intrefcount = Marshal.FinalReleaseComObject(Object);
            } while (intrefcount > 0);
            Object = null;
        }




        /***************************************************/
        /**** Create methods                            ****/
        /***************************************************/

        private bool CreateCollection(IEnumerable<IBHoMObject> objects)
        {
            bool success = true;
            foreach (IBHoMObject obj in objects)
            {
                success &= Create(obj as dynamic);
            }
            return success;
        }

        /***************************************************/

        private bool CreateCollection(IEnumerable<BHE.Elements.Space> spaces)
        {
            bool success = true;
            foreach (BHE.Elements.Space space in spaces)
            {
                success &= Create(space, spaces);
                
            }

            
            //Add adjecency stuff here
            return success;
        }

        /***************************************************/

        private bool Create(BHE.Elements.Building bHoMBuilding)
        {
            TBD.Building tasBuilding = m_TBDDocumentInstance.Building;
            tasBuilding.latitude = (float)bHoMBuilding.Latitude;
            tasBuilding.longitude = (float)bHoMBuilding.Longitude;
            tasBuilding.name = bHoMBuilding.Name;

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElement bHoMBuildingElement)
        {
            TBD.buildingElement tasBuildingElement = m_TBDDocumentInstance.Building.AddBuildingElement();
            tasBuildingElement.name = bHoMBuildingElement.Name;
            return true;
        }

        /***************************************************/

        private bool Create(BHE.Properties.BuildingElementProperties bHoMBuildingElementProperties)
        {
            TBD.Construction tasConstruction = m_TBDDocumentInstance.Building.AddConstruction(null);
            tasConstruction.name = bHoMBuildingElementProperties.Name;
            tasConstruction.materialWidth[0] = (float)bHoMBuildingElementProperties.Thickness; //which value in the array shall we use??

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.BuildingElementPanel bHoMBuildingElementPanel)
        {
            throw new NotImplementedException();
        }
        
        /***************************************************/

        private bool Create(BHE.Elements.InternalCondition bHoMInternalCondition)
        {
            TBD.InternalCondition tasInternalCondition = m_TBDDocumentInstance.Building.AddIC(null);
            tasInternalCondition.name = bHoMInternalCondition.Name;

            return true;
        }

        /***************************************************/

        private bool Create(BHE.Elements.Space bHoMSpace, IEnumerable<BHE.Elements.Space> spaces)
        {
            
            TBD.zone tasZone = m_TBDDocumentInstance.Building.AddZone();
            TBD.room tasRoom = tasZone.AddRoom();
            tasZone = Engine.TAS.Convert.ToTas(bHoMSpace, tasZone);


            foreach (BHE.Elements.BuildingElement element in bHoMSpace.BuildingElements)
            {
                //We have to add a building element to the zonesurface before we save the file. Otherwise we end up with a corrupt file!
                TBD.buildingElement be = m_TBDDocumentInstance.Building.AddBuildingElement();

                //Add zoneSrf and convert it
                TBD.zoneSurface tasZoneSrf = tasZone.AddSurface();
                tasZoneSrf = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry, tasZoneSrf);

                //Add roomSrf, create face, get its controlpoints and convert to TAS
                TBD.Polygon tasPolygon = tasRoom.AddSurface().CreatePerimeter().CreateFace();
                tasPolygon = Engine.TAS.Convert.ToTas(element.BuildingElementGeometry.ICurve(), tasPolygon);
                
                //Set the building Element
                tasZoneSrf.buildingElement = Engine.TAS.Convert.ToTas(element, be);

                tasZoneSrf.type = BH.Engine.TAS.Query.GetSurfaceType(element, spaces);
                tasZoneSrf.orientation = BH.Engine.TAS.Query.GetOrientation(element.BuildingElementGeometry, bHoMSpace);
                tasZoneSrf.inclination = BH.Engine.TAS.Query.GetInclination(element.BuildingElementGeometry, bHoMSpace);
            }

           

            return true;
        }

        /***************************************************/
       
    }
}
