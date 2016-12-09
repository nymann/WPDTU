using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using UMLaut.Model;
using UMLaut.Model.Enum;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DeleteCommand : IUndoRedoCommand
    {
        private List<ShapeViewModel> _selectedElement = new List<ShapeViewModel>();
        private MainViewModel _mainViewModel;

        public DeleteCommand(List<ShapeViewModel> selectedElement, MainViewModel mainViewModel)
        {
            _selectedElement = selectedElement;
            _mainViewModel = mainViewModel;
        }

        public void Execute()
        {
            foreach (var shape in _selectedElement)
            {
                _mainViewModel.Shapes.Add(shape);
            }
        }

        public void UnExecute()
        {
            foreach (var shape in _selectedElement)
            {
                _mainViewModel.Shapes.Remove(shape);
            }
        }
    }
}