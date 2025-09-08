namespace WpfToolbox.Behaviors;

/// <summary>
/// Behavior that enables persistence of the Quick Access Toolbar (QAT) state for a Ribbon control.
/// Handles saving and loading of QAT items to application settings, allowing user customizations to be retained across sessions.
/// </summary>
public class RibbonPersistenceBehavior : Behavior<Ribbon>
{
    /// <summary>
    /// Called when the behavior is attached to a Ribbon control.
    /// Subscribes to the Initialized event to trigger QAT loading.
    /// </summary>
    protected override void OnAttached()
    {
        AssociatedObject.Initialized += OnInitialized;
    }

    /// <summary>
    /// Called when the behavior is detached from a Ribbon control.
    /// Unsubscribes from the Initialized event.
    /// </summary>
    protected override void OnDetaching()
    {
        AssociatedObject.Initialized -= OnInitialized;
    }

    /// <summary>
    /// Handles the Ribbon's Initialized event and loads QAT items from settings.
    /// </summary>
    private void OnInitialized(object? sender, EventArgs e)
    {
        LoadQatItems();
    }

    /// <summary>
    /// Saves the current state of the Quick Access Toolbar (QAT) items to application settings.
    /// Serializes the QAT item structure, including their positions in the Ribbon and Application Menu,
    /// so that user customizations can be restored in future sessions.
    /// </summary>
    private void SaveQatItems()
    {
        string text = string.Empty;

        if (AssociatedObject.QuickAccessToolBar != null && AssociatedObject.QuickAccessToolBar.Items != null)
        {
            List<QatItem> qatItems = [.. AssociatedObject.QuickAccessToolBar.Items.Cast<object>().
                Select(i => i as FrameworkElement).Where(e => e != null).
                Select(e => RibbonControlService.GetQuickAccessToolBarId(e)).Where(id => id != null).
                Select(q => new QatItem(q.GetHashCode()))];

            List<QatItem> remainingItems = [];
            remainingItems.AddRange(qatItems);

            // add -1 to show from application menu
            remainingItems.ForEach(qat => qat.ControlIndices.Add(-1));
            SaveQatItems(remainingItems, AssociatedObject.ApplicationMenu);
            remainingItems.ForEach(qat => qat.ControlIndices.Clear());
            SaveQatItems(remainingItems, AssociatedObject);

            text = qatItems.Aggregate("", (a, b) => a + "," + b.ControlIndices.ToString()).TrimStart(',');
        }
        Properties.Settings.Default.QuickAccessToolBar = text;
        Properties.Settings.Default.Save();
    }

    /// <summary>
    /// Recursively saves QAT items for a given ItemsControl and its children.
    /// </summary>
    /// <param name="remainingItems">List of QAT items to process.</param>
    /// <param name="itemsControl">The ItemsControl to traverse.</param>
    private void SaveQatItems(List<QatItem> remainingItems, ItemsControl itemsControl)
    {
        if (itemsControl != null && itemsControl.Items != null)
        {
            for (int index = 0; index < itemsControl.Items.Count && remainingItems.Count > 0; index++)
            {
                SaveQatItemsAmongChildren(remainingItems, itemsControl.Items[index], index);
            }
        }
    }

    /// <summary>
    /// Processes a single control and its children for QAT item saving.
    /// </summary>
    /// <param name="remainingItems">List of QAT items to process.</param>
    /// <param name="control">The control to process.</param>
    /// <param name="controlIndex">The index of the control within its parent.</param>
    private void SaveQatItemsAmongChildren(List<QatItem> remainingItems, object control, int controlIndex)
    {
        if (control != null)
        {
            //Add the index control pending
            remainingItems.ForEach(qat => qat.ControlIndices.Add(controlIndex));

            SaveQatItemsAmongChildrenInner(remainingItems, control);

            //Remove the index control and earrings that are not within this control
            remainingItems.ForEach(qat => qat.ControlIndices.RemoveAt(qat.ControlIndices.Count - 1));
        }
    }

    /// <summary>
    /// Recursively traverses the logical and visual tree to save QAT items.
    /// </summary>
    /// <param name="remainingItems">List of QAT items to process.</param>
    /// <param name="parent">The parent object to traverse.</param>
    private void SaveQatItemsAmongChildrenInner(List<QatItem> remainingItems, object parent)
    {
        SaveQatItemsIfMatchesControl(remainingItems, parent);

        if (remainingItems.Count == 0 || IsLeaf(parent))
        {
            return;
        }

        int childIndex = 0;
        if (parent is DependencyObject dependencyObject)
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(dependencyObject);
            foreach (object child in children)
            {
                SaveQatItemsAmongChildren(remainingItems, child, childIndex);
                childIndex++;
            }
        }
        if (childIndex != 0)
        {
            return;
        }

        // if we failed to get any logical children, enumerate the visual ones
        if (parent is Visual visual)
        {
            for (childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(visual); childIndex++)
            {
                Visual? child = VisualTreeHelper.GetChild(visual, childIndex) as Visual;
                SaveQatItemsAmongChildren(remainingItems, child!, childIndex);
            }
        }
    }

    /// <summary>
    /// Determines if the given element is a leaf node (cannot have children) in the Ribbon structure.
    /// </summary>
    /// <param name="element">The element to check.</param>
    /// <returns>True if the element is a leaf node; otherwise, false.</returns>
    private static bool IsLeaf(object element)
    {
        if ((element is RibbonButton) ||
        (element is RibbonToggleButton) ||
        (element is RibbonRadioButton) ||
        (element is RibbonCheckBox) ||
        (element is RibbonTextBox) ||
        (element is RibbonSeparator))
        {
            return true;
        }

        //RibbonMenuItem menuItem = element as RibbonMenuItem;
        //if (menuItem != null && menuItem.Items.Count == 0)
        if (element is RibbonMenuItem menuItem && menuItem.Items.Count == 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes QAT items from the list if they match the given control.
    /// </summary>
    /// <param name="remainingItems">List of QAT items to process.</param>
    /// <param name="control">The control to check for a match.</param>
    /// <returns>True if any items were matched and removed; otherwise, false.</returns>
    private static bool SaveQatItemsIfMatchesControl(List<QatItem> remainingItems, object control)
    {
        bool matched = false;
        if (control is FrameworkElement element)
        {
            object getQuickAccessToolBarId = RibbonControlService.GetQuickAccessToolBarId(element);
            if (getQuickAccessToolBarId != null)
            {
                int remove = remainingItems.RemoveAll(qat => qat.HashCode == getQuickAccessToolBarId.GetHashCode());
                matched = remove > 0;
            }
        }

        return matched;
    }

    /// <summary>
    /// Loads QAT items from application settings and restores them to the Ribbon.
    /// </summary>
    private void LoadQatItems()
    {
        AssociatedObject.QuickAccessToolBar ??= new RibbonQuickAccessToolBar();

        try
        {
            string text = Properties.Settings.Default.QuickAccessToolBar;
            if (!string.IsNullOrEmpty(text))
            {
                List<QatItem> qatItems = [.. text.Split(',').Select(i => Int32Collection.Parse(i)).Select(x => new QatItem() { ControlIndices = x })];
                if ((qatItems != null) && (qatItems.Count > 0))
                {
                    SearchInApplicationMenu(qatItems);
                    SearchInTabs(qatItems);

                    qatItems.Where(qat => qat.Owner != null).ToList().ForEach(qat =>
                    {
                        if (RibbonCommands.AddToQuickAccessToolBarCommand.CanExecute(null, qat.Owner))
                        {
                            RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, qat.Owner);
                        }
                    });

                }
            }
        }
        catch
        {

        }

        AssociatedObject.QuickAccessToolBar.ItemContainerGenerator.ItemsChanged += OnQuickAccessToolBarItemsChanged;
    }

    /// <summary>
    /// Handles changes to the Quick Access Toolbar items and triggers saving.
    /// </summary>
    protected void OnQuickAccessToolBarItemsChanged(object sender, ItemsChangedEventArgs e)
    {
        SaveQatItems();
    }

    /// <summary>
    /// Searches for QAT items in the Ribbon's Application Menu and matches them to their saved positions.
    /// </summary>
    /// <param name="qatItems">List of QAT items to match.</param>
    private void SearchInApplicationMenu(List<QatItem> qatItems)
    {
        if (qatItems != null)
        {

            int remainingItemsCount = qatItems.Count(qat => qat.Owner == null);
            List<QatItem> matchedItems = [];

            for (int index = 0; index < AssociatedObject.ApplicationMenu.Items.Count && remainingItemsCount > 0; index++)
            {
                matchedItems.Clear();
                matchedItems.AddRange(qatItems.Where(qat => qat.ControlIndices[0] == -1)); //-1 is applicationMenu

                //remove -1
                matchedItems.ForEach(qat => qat.ControlIndices.RemoveAt(0));

                object item = AssociatedObject.ApplicationMenu.Items[index];
                if (item != null)
                {
                    if (!IsLeaf(item))
                    {
                        LoadQatItemsAmongChildren(matchedItems, 0, index, item, ref remainingItemsCount);
                    }
                    else
                    {
                        LoadQatItemIfMatchesControl(matchedItems, [], 0, index, item, ref remainingItemsCount);
                    }
                }
                //Add -1
                matchedItems.ForEach(qat => qat.ControlIndices.Insert(0, -1));
            }
        }
    }

    /// <summary>
    /// Searches for QAT items in the Ribbon's tabs and matches them to their saved positions.
    /// </summary>
    /// <param name="qatItems">List of QAT items to match.</param>
    private void SearchInTabs(List<QatItem> qatItems)
    {
        int remainingItemsCount = qatItems.Count(qat => qat.Owner == null);
        List<QatItem> matchedItems = [];

        for (int tabIndex = 0; tabIndex < AssociatedObject.Items.Count && remainingItemsCount > 0; tabIndex++)
        {
            matchedItems.Clear();
            matchedItems.AddRange(qatItems.Where(qat => qat.ControlIndices[0] == tabIndex));

            if (AssociatedObject.Items[tabIndex] is RibbonTab tab)
            {
                LoadQatItemsAmongChildren(matchedItems, 0, tabIndex, tab, ref remainingItemsCount);
            }
        }
    }

    /// <summary>
    /// Recursively traverses the logical and visual tree to match and restore QAT items.
    /// </summary>
    /// <param name="previouslyMatchedItems">List of QAT items matched so far.</param>
    /// <param name="matchLevel">Current depth in the tree.</param>
    /// <param name="_">Control index (unused).</param>
    /// <param name="parent">Parent object to traverse.</param>
    /// <param name="remainingItemsCount">Reference to the count of unmatched items.</param>
    private static void LoadQatItemsAmongChildren(List<QatItem> previouslyMatchedItems, int matchLevel, int _ /*controlIndex*/, object parent, ref int remainingItemsCount)
    {
        if (previouslyMatchedItems.Count == 0)
        {
            return;
        }
        if (IsLeaf(parent))
        {
            return;
        }

        int childIndex = 0;
        if (parent is DependencyObject dependencyObject)
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(dependencyObject);
            foreach (object child in children)
            {
                if (remainingItemsCount == 0)
                {
                    break;
                }

                List<QatItem> matchedItems = [];
                LoadQatItemIfMatchesControl(previouslyMatchedItems, matchedItems, matchLevel + 1, childIndex, child, ref remainingItemsCount);
                LoadQatItemsAmongChildren(matchedItems, matchLevel + 1, childIndex, child, ref remainingItemsCount);
                childIndex++;
            }
        }
        if (childIndex != 0)
        {
            return;
        }

        // if we failed to get any logical children, enumerate the visual ones
        if (parent is Visual visual)
        {

            for (childIndex = 0; childIndex < VisualTreeHelper.GetChildrenCount(visual); childIndex++)
            {
                if (remainingItemsCount == 0)
                {
                    break;
                }

                Visual? child = VisualTreeHelper.GetChild(visual, childIndex) as Visual;
                List<QatItem> matchedItems = [];
                LoadQatItemIfMatchesControl(previouslyMatchedItems, matchedItems, matchLevel + 1, childIndex, child!, ref remainingItemsCount);
                LoadQatItemsAmongChildren(matchedItems, matchLevel + 1, childIndex, child!, ref remainingItemsCount);
            }
        }
    }

    /// <summary>
    /// Matches a QAT item to a control at a specific tree level and updates its owner if matched.
    /// </summary>
    /// <param name="previouslyMatchedItems">List of QAT items matched so far.</param>
    /// <param name="matchedItems">List to collect further matched items.</param>
    /// <param name="matchLevel">Current depth in the tree.</param>
    /// <param name="controlIndex">Index of the control within its parent.</param>
    /// <param name="control">The control to check for a match.</param>
    /// <param name="remainingItemsCount">Reference to the count of unmatched items.</param>
    private static void LoadQatItemIfMatchesControl(List<QatItem> previouslyMatchedItems, List<QatItem> matchedItems, int matchLevel, int controlIndex, object control, ref int remainingItemsCount)
    {
        for (int qatIndex = 0; qatIndex < previouslyMatchedItems.Count; qatIndex++)
        {
            QatItem qatItem = previouslyMatchedItems[qatIndex];
            if (qatItem.ControlIndices[matchLevel] == controlIndex)
            {
                if (qatItem.ControlIndices.Count == matchLevel + 1)
                {
                    qatItem.Owner = control as Control;
                    remainingItemsCount--;
                }
                else
                {
                    matchedItems.Add(qatItem);
                }
            }
        }
    }

    /// <summary>
    /// Represents a Quick Access Toolbar (QAT) item for persistence.
    /// Stores the item's position in the Ribbon structure and its unique hash code.
    /// </summary>
    private class QatItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QatItem"/> class.
        /// </summary>
        public QatItem()
        {
            this.ControlIndices = [];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="QatItem"/> class with a hash code.
        /// </summary>
        /// <param name="hashCode">The unique hash code for the QAT item.</param>
        public QatItem(int hashCode)
            : this()
        {
            this.HashCode = hashCode;
        }

        /// <summary>
        /// Gets or sets the collection of indices representing the item's position in the Ribbon structure.
        /// </summary>
        public Int32Collection ControlIndices { get; set; }

        /// <summary>
        /// Gets or sets the unique hash code for the QAT item.
        /// </summary>
        public int HashCode { get; set; }

        /// <summary>
        /// Gets or sets the owner control for the QAT item (used during loading).
        /// </summary>
        // <remark>only for load</remark>
        public Control? Owner { get; set; }
    }
}
