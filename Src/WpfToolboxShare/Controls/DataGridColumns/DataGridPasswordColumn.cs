namespace WpfToolbox.Controls;

/// <summary>
/// A DataGrid column for displaying and editing password fields.
/// Uses <see cref="MaskedTextBlock"/> for display and <see cref="BindablePasswordBox"/> for editing.
/// </summary>
public class DataGridPasswordColumn : DataGridBoundColumn 
{
    /// <summary>
    /// Generates the element used to display the cell value (masked).
    /// </summary>
    /// <param name="cell">The parent DataGridCell.</param>
    /// <param name="dataItem">The data item for the row.</param>
    /// <returns>A <see cref="MaskedTextBlock"/> bound to the password property.</returns>
    protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
    {
        MaskedTextBlock maskedTextBlock = new();
        maskedTextBlock.SetBinding(MaskedTextBlock.MaskedTextProperty, this.Binding);
        return maskedTextBlock;
    }

    /// <summary>
    /// Generates the element used to edit the cell value (password input).
    /// </summary>
    /// <param name="cell">The parent DataGridCell.</param>
    /// <param name="dataItem">The data item for the row.</param>
    /// <returns>A <see cref="BindablePasswordBox"/> bound to the password property.</returns>
    protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
    {
        BindablePasswordBox bindablePasswordBox = new();
        bindablePasswordBox.SetBinding(BindablePasswordBox.PasswordProperty, this.Binding);
        return bindablePasswordBox;
    }
}
