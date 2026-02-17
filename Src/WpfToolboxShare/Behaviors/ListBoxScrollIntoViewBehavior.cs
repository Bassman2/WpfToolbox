namespace WpfToolbox.Behaviors;

public class ListBoxScrollIntoViewBehavior : Behavior<ListBox>
{
    protected override void OnAttached()
        => AssociatedObject.SelectionChanged += OnSelectionChanged;

    protected override void OnDetaching()
        => AssociatedObject.SelectionChanged -= OnSelectionChanged;

    void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ListBox listBox && listBox.SelectedItem is not null)
        {
            listBox.Dispatcher.BeginInvoke(() =>
            {
                listBox.UpdateLayout();
                listBox.ScrollIntoView(listBox.SelectedItem);
            });
        }
    }
}