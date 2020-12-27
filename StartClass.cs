using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Architecture;
using Autodesk.Revit.Attributes;
namespace BuildLintels

{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class StartClass : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            FilteredElementCollector newFilterDoors = new FilteredElementCollector(doc);
            FilteredElementCollector newFilterWindows = new FilteredElementCollector(doc);

            //Create collections

            ICollection<Element> allDoors = newFilterDoors.OfCategory(BuiltInCategory.OST_Doors).WhereElementIsNotElementType().ToElements();
            ICollection<Element> allWindows = newFilterWindows.OfCategory(BuiltInCategory.OST_Windows).WhereElementIsNotElementType().ToElements();


            // Create enpty dictionary
            var dictOpensByWall = new Dictionary<string, Dictionary<string, List<FamilyInstance>>>();

            // Iterate over the elements of each category and fill in the dictionary

            FillDictionary(allDoors, dictOpensByWall);
            FillDictionary(allWindows, dictOpensByWall);


            UserWind userWind = new UserWind(dictOpensByWall);
            userWind.Show();
            return Result.Succeeded;



        }

        //Method to iterate over the elements of each category and fill in the dictionary
        private void FillDictionary(ICollection<Element> collectionElements, Dictionary<string, Dictionary<string, List<FamilyInstance>>> dict)
        {
            foreach (Element element in collectionElements)
            {

                FamilyInstance el = element as FamilyInstance;
                Wall wall = (Wall)el.Host;
                WallType sWall = wall.WallType;
                string modelGroup = sWall.get_Parameter(BuiltInParameter.ALL_MODEL_MODEL).AsString();
                string cat = el.Category.Name;
                double width = el.get_Parameter(BuiltInParameter.FAMILY_WIDTH_PARAM).AsDouble();



                if (!dict.ContainsKey(modelGroup))
                {
                    var newDict = new Dictionary<string, List<FamilyInstance>>();
                    var newList = new List<FamilyInstance>();
                    newList.Add(el);
                    newDict[cat] = newList;

                    dict.Add(modelGroup, newDict);
                }
                else
                {

                    if (!dict[modelGroup].ContainsKey(cat))
                    {
                        var newListInstance = new List<FamilyInstance>();
                        newListInstance.Add(el);
                        dict[modelGroup][cat] = newListInstance;
                    }
                    else
                    {
                        dict[modelGroup][cat].Add(el);
                    }


                }

            }

        }

    }


}