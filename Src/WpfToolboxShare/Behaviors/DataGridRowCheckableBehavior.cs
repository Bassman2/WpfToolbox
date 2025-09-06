namespace WpfToolbox.Behaviors;

public class DataGridRowCheckableBehavior : Behavior<DataGrid>
{
    //    private ObservableCollection<CheckViewModel> list;
    //    private IEnumerable availables;
    //    private IEnumerable checkedItems;

    //    public CheckableDataGrid()
    //    {
    //        this.list = new ObservableCollection<CheckViewModel>();
    //        this.ItemsSource = list;
    //    }

    //    #region AvailableItems

    //    public static readonly DependencyProperty AvailableItemsProperty =
    //       DependencyProperty.Register("AvailableItems", typeof(IEnumerable), typeof(CheckableDataGrid),
    //           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnAvailableItemsPropertyChanged)));

    //    public IEnumerable AvailableItems
    //    {
    //        get { return (IEnumerable)GetValue(AvailableItemsProperty); }
    //        set { SetValue(AvailableItemsProperty, value); }
    //    }

    //    protected static void OnAvailableItemsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
    //    {
    //        ((CheckableDataGrid)source).OnAvailableItemsChanged(args);
    //    }

    //    protected void OnAvailableItemsChanged(DependencyPropertyChangedEventArgs args)
    //    {
    //        // unregister CollectionChanged event from old list
    //        if (args.OldValue is INotifyCollectionChanged oldNotify)
    //        {
    //            oldNotify.CollectionChanged -= OnNotifyCollectioChanged;
    //        }

    //        // clear list
    //        this.list.Clear();

    //        if (args.NewValue is IEnumerable availables)
    //        {
    //            this.availables = availables;
    //            foreach (object obj in this.availables)
    //            {
    //                this.list.Add(CreateViewModel(obj));
    //            }
    //        }

    //        // register CollectionChanged event to new list
    //        if (args.NewValue is INotifyCollectionChanged newNotify)
    //        {
    //            newNotify.CollectionChanged += OnNotifyCollectioChanged;
    //        }
    //    }

    //    protected void OnNotifyCollectioChanged(object sender, NotifyCollectionChangedEventArgs e)
    //    {
    //        switch (e.Action)
    //        {
    //        case NotifyCollectionChangedAction.Add:
    //            this.list.Insert(e.NewStartingIndex, CreateViewModel(e.NewItems[0]));
    //            break;
    //        case NotifyCollectionChangedAction.Move:
    //            this.list.Move(e.OldStartingIndex, e.NewStartingIndex);
    //            break;
    //        case NotifyCollectionChangedAction.Remove:
    //            this.list.RemoveAt(e.OldStartingIndex);
    //            break;
    //        case NotifyCollectionChangedAction.Replace:
    //            this.list[e.OldStartingIndex] = CreateViewModel(e.NewItems[0]);
    //            break;
    //        case NotifyCollectionChangedAction.Reset:
    //            this.list.Clear();
    //            break;
    //        }
    //    }

    //    #endregion

    //    #region CheckedItems

    //    public static readonly DependencyProperty CheckedItemsProperty =
    //       DependencyProperty.Register("CheckedItems", typeof(IEnumerable), typeof(CheckableDataGrid),
    //           new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnCheckedItemsPropertyChanged)));

    //    public IEnumerable CheckedItems
    //    {
    //        get { return (IEnumerable)GetValue(CheckedItemsProperty); }
    //        set { SetValue(CheckedItemsProperty, value); }
    //    }

    //    protected static void OnCheckedItemsPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs args)
    //    {
    //        ((CheckableDataGrid)source).OnCheckedItemsChanged(args);
    //    }

    //    protected void OnCheckedItemsChanged(DependencyPropertyChangedEventArgs args)
    //    {
    //        this.checkedItems = args.NewValue as IEnumerable;
    //        if (this.checkedItems != null)
    //        {
    //            foreach (var i in this.list)
    //            {
    //                i.IsSelectedInternal = this.checkedItems.Cast<object>().Any(o => o.Equals(i.Item));
    //                Debug.WriteLine($"Item {i.Item} : {i.isSelected}");
    //            }
    //        }
    //        else
    //        {
    //            this.list.ForEach(i => i.isSelected = false);
    //            //foreach (var i in this.list)
    //            //{
    //            //    i.isSelected = false;
    //            //}
    //        }
    //    }

    //    #endregion

    //    private CheckViewModel CreateViewModel(object item)
    //    {
    //        var cvm = new CheckViewModel(item);
    //        cvm.CheckChanged += OnCheckChanged;
    //        return cvm;
    //    }

    //    private void OnCheckChanged(object sender, CheckEventArgs e)
    //    {
    //        CheckViewModel cvm = sender as CheckViewModel;
    //        if (this.checkedItems is IList list)
    //        {
    //            Type avat = this.availables.GetType().GenericTypeArguments[0];
    //            Type type = this.checkedItems.GetType();
    //            Type gent = type.GenericTypeArguments[0];

    //            //ConstructorInfo ci = gent.GetConstructors().First(c => c.GetParameters().Count() == 1);
    //            ConstructorInfo ci = gent.GetConstructor(new Type[] { avat });
    //            if (ci == null)
    //            {
    //                throw new Exception($"The class {gent.Name} needs a constructor with a single parameter of the type {avat.Name}!");
    //            }
    //            object item = ci.Invoke(new object[] { cvm.Item });
    //            if (e.IsChecked)
    //            {
    //                list.Add(item);
    //            }
    //            else
    //            {
    //                list.Remove(item);
    //            }
    //        }
    //    }
    //}

    //public delegate void NotifyCollectionChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e);

    //public class CheckEventArgs : EventArgs
    //{
    //    public CheckEventArgs(bool check)
    //    {
    //        this.IsChecked = check;
    //    }

    //    public bool IsChecked { get; set; }
    //}

    //public partial class CheckViewModel : ObservableObject
    //{
    //    //internal bool isSelected;

    //    public event EventHandler<CheckEventArgs> CheckChanged;

    //    public CheckViewModel(object item)
    //    {
    //        this.Item = item;
    //    }

    //    // call for initial setting without event feedback
    //    [ObservableProperty] 
    //    private bool isSelectedInternal;

    //    [ObservableProperty]
    //    private bool isSelected;

    //    //CheckChanged?.Invoke(this, new CheckEventArgs(value));

    //    public object Item { get; set; }

    //    //public override bool Equals(object obj)
    //    //{
    //    //    CheckViewModel? cvm = obj as CheckViewModel;
    //    //    return this.Item.Equals(cvm!.Item);
    //    //}

    //    //public override int GetHashCode()
    //    //{
    //    //    return this.Item.GetHashCode();
    //    //}
}
