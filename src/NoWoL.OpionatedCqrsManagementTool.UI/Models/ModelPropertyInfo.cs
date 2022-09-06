﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace CodeGen.UI.Models
{
    public partial class ModelPropertyInfo : ObservableValidator
    {
        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private bool _isNullable;

        [ObservableProperty]
        private bool _initialize;

        [ObservableProperty]
        private bool _isList;

        [ObservableProperty]
        private int? _maxLength;

        [ObservableProperty]
        private ModelInfo? _dataType;
    }
}