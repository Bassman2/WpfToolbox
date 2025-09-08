namespace WpfToolbox.Misc;

/// <summary>
/// Provides an attached property to update a binding's source when the Enter key is pressed on a UIElement.
/// </summary>
public static class InputBindingsManager
{
    /// <summary>
    /// Attached property to specify which dependency property should update its source when Enter is pressed.
    /// </summary>
    public static readonly DependencyProperty UpdatePropertySourceWhenEnterPressedProperty = DependencyProperty.RegisterAttached(
            "UpdatePropertySourceWhenEnterPressed", typeof(DependencyProperty), typeof(InputBindingsManager), new PropertyMetadata(null, OnUpdatePropertySourceWhenEnterPressedPropertyChanged));

    static InputBindingsManager()
    {

    }

    /// <summary>
    /// Sets the attached property on the specified DependencyObject.
    /// </summary>
    /// <param name="dp">The target DependencyObject.</param>
    /// <param name="value">The DependencyProperty to update on Enter key press.</param>
    public static void SetUpdatePropertySourceWhenEnterPressed(DependencyObject dp, DependencyProperty value)
    {
        dp.SetValue(UpdatePropertySourceWhenEnterPressedProperty, value);
    }

    /// <summary>
    /// Gets the attached property from the specified DependencyObject.
    /// </summary>
    /// <param name="dp">The target DependencyObject.</param>
    /// <returns>The DependencyProperty to update, or null if not set.</returns>
    public static DependencyProperty? GetUpdatePropertySourceWhenEnterPressed(DependencyObject? dp)
    {
        return (DependencyProperty?)dp?.GetValue(UpdatePropertySourceWhenEnterPressedProperty) ?? null;
    }

    /// <summary>
    /// Handles changes to the attached property, subscribing or unsubscribing to the PreviewKeyDown event as needed.
    /// </summary>
    private static void OnUpdatePropertySourceWhenEnterPressedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
    {
        if (dp is UIElement element)
        {
            if (e.OldValue != null)
            {
                element.PreviewKeyDown -= HandlePreviewKeyDown;
            }

            if (e.NewValue != null)
            {
                element.PreviewKeyDown += new KeyEventHandler(HandlePreviewKeyDown);
            }
        }
    }

    /// <summary>
    /// Handles the PreviewKeyDown event and updates the binding source if Enter is pressed.
    /// </summary>
    static void HandlePreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            DoUpdateSource(e.Source);
        }
    }

    /// <summary>
    /// Updates the binding source for the specified source object and property.
    /// </summary>
    /// <param name="source">The source object (should be a DependencyObject).</param>
    static void DoUpdateSource(object source)
    {
        DependencyProperty? property = GetUpdatePropertySourceWhenEnterPressed(source as DependencyObject);

        if (property == null)
        {
            return;
        }


        if (source is UIElement elt)
        {
            BindingExpression? binding = BindingOperations.GetBindingExpression(elt, property);

            binding?.UpdateSource();
        }
       
    }
}
