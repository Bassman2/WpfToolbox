namespace WpfToolbox.Controls;

public partial class ExtendedDataGrid : DataGrid
{
    // Register a custom routed event using the Bubble routing strategy.
    public static readonly RoutedEvent CanExecuteDeleteEvent = 
        EventManager.RegisterRoutedEvent("CanExecuteDelete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExtendedDataGrid));

    // Provide CLR accessors for assigning an event handler.
    public event RoutedEventHandler CanExecuteDelete
    {
        add => AddHandler(CanExecuteDeleteEvent, value); 
        remove => RemoveHandler(CanExecuteDeleteEvent, value); 
    }
    
    protected override void OnCanExecuteDelete(CanExecuteRoutedEventArgs e)
    {
        
        base.OnCanExecuteDelete(e);

        //ExtendedDataGrid edg = (ExtendedDataGrid)e.OriginalSource;

        //IList items = edg.SelectedItems;
        //e.Parameter = items;
        //CanExecuteRoutedEventArgs routedEventArgs = (CanExecuteRoutedEventArgs)new RoutedEventArgs(CanExecuteDeleteEvent, this);
        ////{ RoutedEvent =  };

        // Raise the event, which will bubble up through the element tree.

        CanExecuteDeleteRoutedEventArgs routedEventArgs = new(CanExecuteDeleteEvent);

        RaiseEvent(routedEventArgs);

        e.CanExecute = routedEventArgs.CanExecute;
        e.ContinueRouting = routedEventArgs.ContinueRouting;
    }
}

public class CanExecuteDeleteRoutedEventArgs : RoutedEventArgs
{
    public CanExecuteDeleteRoutedEventArgs()
    { }

    public CanExecuteDeleteRoutedEventArgs(RoutedEvent routedEvent) : base(routedEvent)
    { }

    public CanExecuteDeleteRoutedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
    { }
        
    public bool CanExecute { get; set; }
    
    public bool ContinueRouting { get; set; }
}
