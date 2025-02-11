using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _09_Intersection
{
    [Transaction(TransactionMode.Manual)]
    internal class IntersectionCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            var region1 = doc.GetElement(new ElementId(2537)) as FilledRegion;
            var region2 = doc.GetElement(new ElementId(2791)) as FilledRegion;

            List<XYZ> s1 = region1.GetBoundaries()[0].Select(p => p.GetEndPoint(0)).ToList();
            List<XYZ> s2 = region2.GetBoundaries()[0].Select(p => p.GetEndPoint(0)).ToList();

            List<Pt2> p1 = s1.Select(p => new Pt2(p.X, p.Y)).ToList();
            List<Pt2> p2 = s2.Select(p => new Pt2(p.X, p.Y)).ToList();

            bool intersect = GJK.CheckCollision(p1, p2);
            if (intersect)
            {
                TaskDialog.Show("提示", "两个图形相交！");
            }

            return Result.Succeeded;
        }
    }
}
