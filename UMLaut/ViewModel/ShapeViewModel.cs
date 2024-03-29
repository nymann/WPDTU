﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMLaut.Model;
using UMLaut.Model.Enum;

namespace UMLaut.ViewModel
{
    public class ShapeViewModel : BaseViewModel
    {
        private bool _isEditing = false;
        private double _offsetX;
        private double _offsetY;
        public bool IsEditing
        {
            get { return _isEditing; }
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        public double OffsetX
        {
            get { return _offsetX; }
            set
            {
                _offsetX = value;
                OnPropertyChanged();
            }
        }

        public double OffsetY
        {
            get { return _offsetY; }
            set
            {
                _offsetY = value;
                OnPropertyChanged();
            }
        }


        public UMLShape Shape { get; set; }

        private ShapeViewModel()
        {

        }
        
        public ShapeViewModel(UMLShape shape)
        {
            Shape = shape;
        }

        public Guid Id => Shape.Id;

        public string Label
        {
            get { return Shape.Label; }
            set
            {
                Shape.Label = value;
                OnPropertyChanged();
            }
        }

        public double Height
        {
            get { return Shape.Height; }
            set
            {
                Shape.Height = value;
                OnPropertyChanged();
            }
        }

        public EShape Type => Shape.Type;

        public double Width
        {
            get { return Shape.Width; }
            set
            {
                Shape.Width = value;
                OnPropertyChanged();
            }
        }

        public double X
        {
            get { return Shape.X; }
            set
            {
                Shape.X = value;
                OnPropertyChanged();
            }
        }
        public double Y
        {
            get { return Shape.Y; }
            set
            {
                Shape.Y = value;
                OnPropertyChanged();
            }
        }


    }
}
