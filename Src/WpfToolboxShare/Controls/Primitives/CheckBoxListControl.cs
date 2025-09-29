namespace WpfToolbox.Controls.Primitives;

public class CheckControl : CheckBox
{
    static CheckControl()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(CheckControl), new FrameworkPropertyMetadata(typeof(CheckControl)));
    }

    public static readonly DependencyProperty ParentControlProperty =
               DependencyProperty.Register(nameof(ParentControl), typeof(Control), typeof(CheckControl),
                   new FrameworkPropertyMetadata(null, (d,e) => ((CheckControl)d).OnParentControlChanged(d,e)));

    private void OnParentControlChanged(DependencyObject _, DependencyPropertyChangedEventArgs e)
    {
        if (e.NewValue is CheckListBox checkListBox)
        {
            var item = DataContext;
            this.IsChecked = checkListBox.CheckedItems.Contains(item);
        }
        if (e.NewValue is CheckComboBox checkListBox)
        {
            var item = DataContext;
            this.IsChecked = checkListBox.CheckedItems.Contains(item);
        }
    }

    public Control ParentControl
    {
        get => (Control)GetValue(ParentControlProperty);
        set => SetValue(ParentControlProperty, value);
    }
    
    protected override void OnChecked(RoutedEventArgs e)
    {
        var item = DataContext;
        if (ParentControl is CheckListBox checkListBox && !checkListBox.CheckedItems.Contains(item))
        {  
            checkListBox.CheckedItems.Add(item);
        }
        else if (ParentControl is CheckComboBox checkComboBox && !checkComboBox.CheckedItems.Contains(item))
        {
            checkComboBox.CheckedItems.Add(item);
        }

        base.OnChecked(e);
    }

    protected override void OnUnchecked(RoutedEventArgs e)
    {
        var item = DataContext;
        if (ParentControl is CheckListBox checkListBox && !checkListBox.CheckedItems.Contains(item))
        {
            checkListBox.CheckedItems.Remove(item);
        }
        else if (ParentControl is CheckComboBox checkComboBox && !checkComboBox.CheckedItems.Contains(item))
        {
            checkComboBox.CheckedItems.Remove(item);
        }
    
        base.OnUnchecked(e);
    }
    
}
