namespace WpfToolbox.ViewModel;

/// <summary>
/// Represents a base view model for leaf nodes in a view model hierarchy.
/// </summary>
public partial class LeafViewModel : ObservableValidator
{
}

/// <summary>
/// Represents a generic leaf view model with references to its root and parent view models.
/// </summary>
/// <typeparam name="R">The type of the root view model.</typeparam>
/// <typeparam name="P">The type of the parent leaf view model.</typeparam>
public partial class LeafViewModel<R, P> : LeafViewModel where R : IRootValidator where P : class
{
    protected R root;
    protected P parent;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeafViewModel{R, P}"/> class.
    /// </summary>
    /// <param name="root">The root view model.</param>
    /// <param name="parent">The parent leaf view model.</param>
    public LeafViewModel(R root, P parent)
    {
        this.root = root;
        this.parent = parent;
        this.ErrorsChanged += (s, e) => root.ChildHasErrors(this, e.PropertyName);
    }
}