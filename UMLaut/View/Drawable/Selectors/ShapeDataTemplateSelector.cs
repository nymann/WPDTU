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
    public class ShapeDataTemplateSelector : DataTemplateSelector
    {

        #region Shape Templates
        public DataTemplate InitialTemplate { get; set; }
        public DataTemplate FlowFinalTemplate { get; set; }
        public DataTemplate ActionTemplate { get; set; }
        public DataTemplate ActivityFinalTemplate { get; set; }
        //public DataTemplate DecisionTemplate { get; set; }
        public DataTemplate MergeTemplate { get; set; }
        public DataTemplate ForkTemplate { get; set; }
        //public DataTemplate JoinTemplate { get; set; }
        //public DataTemplate SyncBarHorzTemplate { get; set; }
        //public DataTemplate SyncBarVertTemplate { get; set; }
        //public DataTemplate TimeEventTemplate { get; set; }
        //public DataTemplate SendSignalTemplate { get; set; }
        //public DataTemplate ReceiveSignalTemplate { get; set; }
        //public DataTemplate EdgeTemplate { get; set; }
        #endregion

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var shapeView = item as ShapeViewModel;

            if (shapeView != null)
            {
                switch (shapeView.Type)
                {
                    case EShape.Initial:
                        return InitialTemplate;
                    case EShape.FlowFinal:
                        return FlowFinalTemplate;
                    case EShape.Action:
                        return ActionTemplate;
                    case EShape.ActivityFinal:
                        return ActivityFinalTemplate;
                    case EShape.Merge:
                        return MergeTemplate;
                    case EShape.Fork:
                        return ForkTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
