using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace UMLaut.Resources
{
    public static class Constants
    {
        public static int DuplicateOffset => 30;
        public static int CornerBoxSize => 7;
        public static SolidColorBrush CornerBoxColor => Brushes.DarkCyan;

        public static class Messages
        {
            public static string GenericError => "Der opstod en fejl.";
        }

        public static class Drawables
        {
            public static class Shapes
            {
                public static int DefaultHeight => 50;
                public static int DefaultWidth => 50;

                public static class Action
                {
                    public static int DefaultHeight => 50;
                    public static int DefaultWidth => 100;
                }
                public static class Initial
                {
                    public static int DefaultHeight => 40;
                    public static int DefaultWidth => 40;
                }
                public static class FlowFinal
                {
                    public static int DefaultHeight => 40;
                    public static int DefaultWidth => 40;
                }
                public static class ActivityFinal
                {
                    public static int DefaultHeight => 40;
                    public static int DefaultWidth => 40;
                }
                public static class Decision
                {
                    public static int DefaultHeight => 60;
                    public static int DefaultWidth => 60;
                }
                public static class Merge
                {
                    public static int DefaultHeight => 75;
                    public static int DefaultWidth => 75;
                }
                public static class Fork
                {
                    public static int DefaultHeight => 15;
                    public static int DefaultWidth => 200;
                }
                public static class Join
                {
                    public static int DefaultHeight => 15;
                    public static int DefaultWidth => 200;
                }
                public static class SyncBarHor
                {
                    public static int DefaultHeight => 15;
                    public static int DefaultWidth => 200;
                }
                public static class SyncBarVert
                {
                    public static int DefaultHeight => 200;
                    public static int DefaultWidth => 15;
                }
                public static class TimeEvent
                {
                    public static int DefaultHeight => 75;
                    public static int DefaultWidth => 75;
                }
                public static class SendSignal
                {
                    public static int DefaultHeight => 50;
                    public static int DefaultWidth => 100;
                }
                public static class ReceiveSignal
                {
                    public static int DefaultHeight => 50;
                    public static int DefaultWidth => 100;
                }
            }
            
            public static class Lines
            {
                public static class Solid
                {

                }
                public static class Dashed
                {

                }
            }            
        }
    }
}
