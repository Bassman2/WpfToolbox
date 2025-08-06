namespace WpfToolbox.Controls;

public class DataGridPasswordColumn : DataGridBoundColumn 
{
    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        MaskedTextBlock maskedTextBlock = new();
        maskedTextBlock.SetBinding(MaskedTextBlock.MaskedTextProperty, this.Binding);
        return maskedTextBlock;
    }

    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        BindablePasswordBox bindablePasswordBox = new();
        bindablePasswordBox.SetBinding(BindablePasswordBox.PasswordProperty, this.Binding);
        return bindablePasswordBox;
    }
}
