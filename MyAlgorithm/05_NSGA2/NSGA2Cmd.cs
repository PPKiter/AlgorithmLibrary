using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _05_NSGA2
{
    /// <summary>
    /// Revit命令窗口
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public class NSGA2Cmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            NSGA2 nsga2=new NSGA2();
            nsga2.Main();


            return Result.Succeeded;
        }
    }
}
