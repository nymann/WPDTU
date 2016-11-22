using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UMLaut.ViewModel;
using UMLaut.Model.Enum;

namespace UMLaut.View.Drawable.Selectors
{
    public class LineDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SolidLineTemplate { get; set; }
        public DataTemplate DashedLineTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var lineViewModel = item as LineViewModel;

            if (lineViewModel != null)
            {
                switch (lineViewModel.Type)
                {
                    case ELine.Solid:
                        return SolidLineTemplate;
                    case ELine.Dashed:
                        return DashedLineTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }

    }
}
