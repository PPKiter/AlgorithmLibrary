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
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="commandData">外部数据</param>
        /// <param name="message">信息</param>
        /// <param name="elements">元素集合</param>
        /// <returns></returns>
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
