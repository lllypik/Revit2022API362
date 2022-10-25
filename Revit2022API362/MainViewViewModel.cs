using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Prism.Commands;
using RevitAPILibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit2022API362
{
    class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public List<Level> Levels { get; private set; }
        public DelegateCommand ApplyCommand { get; private set; }
        public XYZ Point { get; private set; }
        public List<FamilySymbol> FurnitureTypes { get; private set; } = new List<FamilySymbol>();

        public FamilySymbol SelectedFurnitureType { get; set; }
        public Level SelectedLevel { get; set; }


        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            Levels = LevelData.GetLevels(_commandData);
            FurnitureTypes = FamilySymbolData.PickAllFurnitureType(_commandData);
            Levels = LevelData.GetLevels(_commandData);
            ApplyCommand = new DelegateCommand(OnApplyCommand);
            Point = SelectionUtils.GetPoint(_commandData, "Выберите точку для вставки мебели", Autodesk.Revit.UI.Selection.ObjectSnapTypes.Endpoints);
        }

        private void OnApplyCommand()
        {
            UIApplication uiapp = _commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            FamilyInstanceData.CreateFamilyInstance(_commandData, SelectedFurnitureType, Point, SelectedLevel);

            RaiseCloseReqest();
        }

        public event EventHandler CloseReqest;
        private void RaiseCloseReqest()
        {
            CloseReqest?.Invoke(this, EventArgs.Empty);
        }


    }
}
