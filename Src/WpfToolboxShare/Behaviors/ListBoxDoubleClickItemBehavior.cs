namespace WpfToolbox.Behaviors;

public class ListBoxDoubleClickItemBehavior : Behavior<ListBox>
{
    /// <summary>
    /// Dependency property for the command to execute on double-click.
    /// </summary>
    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register(
            nameof(Command),
            typeof(ICommand),
            typeof(ListBoxDoubleClickItemBehavior),
            new PropertyMetadata(null));

    /// <summary>
    /// Gets or sets the command to execute when a ListBoxItem is double-clicked.
    /// The command parameter will be the data item associated with the clicked ListBoxItem.
    /// </summary>
    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// Attaches the behavior to the ListBox and subscribes to the MouseDoubleClick event.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject.MouseDoubleClick += OnMouseDoubleClick;
    }

    /// <summary>
    /// Detaches the behavior from the ListBox and unsubscribes from the MouseDoubleClick event.
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject.MouseDoubleClick -= OnMouseDoubleClick;
    }

    /// <summary>
    /// Handles the MouseDoubleClick event on the ListBox.
    /// Finds the clicked ListBoxItem and executes the command with its data context.
    /// </summary>
    private void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (Command is null || !Command.CanExecute(null))
        {
            return;
        }

        // Find the ListBoxItem that was clicked
        var item = FindAncestor<ListBoxItem>((DependencyObject)e.OriginalSource);
        if (item is not null)
        {
            // Execute the command with the item's DataContext
            Command.Execute(item.DataContext);
        }
    }

    /// <summary>
    /// Walks up the visual tree to find an ancestor of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of ancestor to find.</typeparam>
    /// <param name="current">The starting dependency object.</param>
    /// <returns>The ancestor of type T, or null if not found.</returns>
    private static T? FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current is not null)
        {
            if (current is T ancestor)
            {
                return ancestor;
            }
            current = System.Windows.Media.VisualTreeHelper.GetParent(current);
        }
        return null;
    }
}
