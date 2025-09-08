namespace WpfToolbox.Behaviors;

/// <summary>
/// Behavior for DataGrid that provides infrastructure for handling custom delete operations via routed events.
/// Intended to allow external handlers to participate in or override row deletion logic.
/// </summary>
public class DataGridRowDeleteBehavior : Behavior<DataGrid>
{

    //// Register a custom routed event using the Bubble routing strategy.
    //public static readonly RoutedEvent CanExecuteDeleteEvent =
    //    EventManager.RegisterRoutedEvent("CanExecuteDelete", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExcecuteDeleteBehavior));

    //// Provide CLR accessors for assigning an event handler.
    //public event RoutedEventHandler CanExecuteDelete
    //{
    //    add => AddHandler(CanExecuteDeleteEvent, value);
    //    remove => RemoveHandler(CanExecuteDeleteEvent, value);
    //}

    //protected override void OnCanExecuteDelete(CanExecuteRoutedEventArgs e)
    //{

    //    base.OnCanExecuteDelete(e);

    //    //ExtendedDataGrid edg = (ExtendedDataGrid)e.OriginalSource;

    //    //IList items = edg.SelectedItems;
    //    //e.Parameter = items;
    //    //CanExecuteRoutedEventArgs routedEventArgs = (CanExecuteRoutedEventArgs)new RoutedEventArgs(CanExecuteDeleteEvent, this);
    //    ////{ RoutedEvent =  };

    //    // Raise the event, which will bubble up through the element tree.

    //    CanExecuteDeleteRoutedEventArgs routedEventArgs = new(CanExecuteDeleteEvent);

    //    RaiseEvent(routedEventArgs);

    //    e.CanExecute = routedEventArgs.CanExecute;
    //    e.ContinueRouting = routedEventArgs.ContinueRouting;
    //}
}

/// <summary>
/// Routed event arguments for the CanExecuteDelete event.
/// Allows handlers to specify whether a delete operation can be executed and if event routing should continue.
/// </summary>
public class CanExecuteDeleteRoutedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CanExecuteDeleteRoutedEventArgs"/> class.
    /// </summary>
    public CanExecuteDeleteRoutedEventArgs()
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CanExecuteDeleteRoutedEventArgs"/> class with the specified routed event.
    /// </summary>
    /// <param name="routedEvent">The routed event identifier.</param>
    public CanExecuteDeleteRoutedEventArgs(RoutedEvent routedEvent) : base(routedEvent)
    { }

    /// <summary>
    /// Initializes a new instance of the <see cref="CanExecuteDeleteRoutedEventArgs"/> class with the specified routed event and source.
    /// </summary>
    /// <param name="routedEvent">The routed event identifier.</param>
    /// <param name="source">The source of the event.</param>
    public CanExecuteDeleteRoutedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source)
    { }

    /// <summary>
    /// Gets or sets a value indicating whether the delete operation can be executed.
    /// </summary>
    public bool CanExecute { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether event routing should continue after this handler.
    /// </summary>
    public bool ContinueRouting { get; set; }
}
