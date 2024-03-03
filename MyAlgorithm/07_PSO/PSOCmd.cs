using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _07_PSO
{
    [Transaction(TransactionMode.Manual)]
    public class PSOCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uidoc = commandData.Application.ActiveUIDocument;
            Document doc = uidoc.Document;

            int numParticles = 100;//粒子数量
            int numDimensions = 20;//变量个数
            int iterations = 200;//迭代次数

            PSO pso = new PSO(numParticles, numDimensions);
            double[] solution = pso.Main(iterations);
            var result=pso.Evaluate(solution);

            return Result.Succeeded;
        }
    }
}
