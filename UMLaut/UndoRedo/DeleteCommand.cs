using System;
using System.Collections.ObjectModel;
using System.Windows;
using UMLaut.Model;
using UMLaut.ViewModel;

namespace UMLaut.UndoRedo
{
    public class DeleteCommand : IUndoRedoCommand
    {
        private ShapeViewModel _shapeViewModel;
        private ObservableCollection<ShapeViewModel> _shapes;

        public DeleteCommand(ShapeViewModel shapeViewModel, ObservableCollection<ShapeViewModel> shapes )
        {
            _shapeViewModel = shapeViewModel;
            _shapes = shapes;
        }

        public void Execute()
        {
            _shapes.Add(_shapeViewModel); // Virker ikke da shapen skal addes til den samme ObersvableCollection som den i MainViewModel, hvad gør jeg i stedet?
            Console.WriteLine(@"Undo requested.");
        }

        public void UnExecute()
        {
            Console.WriteLine(@"Redo requested.");            
        }
    }
}