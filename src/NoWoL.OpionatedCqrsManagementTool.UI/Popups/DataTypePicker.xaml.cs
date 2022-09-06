using System.Collections;
using System.Collections.ObjectModel;
using CodeGen.UI.Models;
using CommunityToolkit.Maui.Views;

namespace CodeGen.UI.Popups
{
    public partial class DataTypePicker : ContentView
    {
        public DataTypePicker()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IReadOnlyList<IHasName>), typeof(DataTypePicker), null);

        public IReadOnlyList<IHasName>? ItemsSource
        {
            get => (IReadOnlyList<IHasName>?)GetValue(DataTypePicker.ItemsSourceProperty);
            set => SetValue(DataTypePicker.ItemsSourceProperty, value);
        }

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(IHasName), typeof(DataTypePicker), null);
        public IHasName? SelectedItem
        {
            get => (IHasName?)GetValue(DataTypePicker.SelectedItemProperty);
            set => SetValue(DataTypePicker.SelectedItemProperty, value);
        }

        private async void ChangeDataType_OnClicked(object? sender, EventArgs e)
        {
            var popup = new DataTypePopup(ItemsSource ?? new List<IHasName>());
            popup.Size = new Size(400, 235);

            var page = ControlHelpers.FindParent<Page>(this);

            var r = await page!.ShowPopupAsync(popup);

            if (r != null)
            {
                SelectedItem = r as IHasName;
            }
        }

        private void Clear_OnClicked(object? sender, EventArgs e)
        {
            SelectedItem = null;
        }
    }
}