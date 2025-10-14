namespace WpfToolbox.Controls;

public class HideableGridSplitter : GridSplitter
{
    private GridLength splitHeight;
    private GridLength nextHeight;
    
    public HideableGridSplitter()
    { }
    
    public static readonly DependencyProperty HideProperty =
      DependencyProperty.Register("Hide", typeof(bool), typeof(HideableGridSplitter), new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnHideChanged)));

    public bool Hide
    {
        get { return (bool)GetValue(HideProperty); }
        set { SetValue(HideProperty, value); }
    }

    private static void OnHideChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is HideableGridSplitter hgs)
        {
            hgs.UpdateHide(hgs.Hide);
        }
    }

    private void UpdateHide(bool hide)
    {
        if (base.Parent is Grid parent)
        {
            int rowIndex = Grid.GetRow(this);

            if (rowIndex + 1 >= parent.RowDefinitions.Count)
            {
                throw new Exception("HideableGridSplitter must not be last row in definition");
            }

            RowDefinition splitRow = parent.RowDefinitions[rowIndex];
            RowDefinition nextRow = parent.RowDefinitions[rowIndex + 1];

            if (hide)
            {
                this.Visibility = Visibility.Collapsed;
                this.splitHeight = splitRow.Height;
                splitRow.Height = new GridLength(0);
                this.nextHeight = nextRow.Height;
                nextRow.Height = new GridLength(0);
            }
            else
            {
                this.Visibility = Visibility.Visible;
                splitRow.Height = this.splitHeight;
                nextRow.Height = this.nextHeight;
            }
        }
    }
}
