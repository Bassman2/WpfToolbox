namespace WpfToolbox.Controls.Primitives;

public abstract class ValueControl<T> : Control where T : IFormattable
{
    private RepeatButton? incRepeatButton;
    private RepeatButton? decRepeatButton;
    
    public ValueControl()
    {
        this.Text = ValueToString();
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        this.incRepeatButton = GetTemplateChild("incRepeatButton") as RepeatButton;
        this.decRepeatButton = GetTemplateChild("decRepeatButton") as RepeatButton;

        if (this.incRepeatButton != null)
        {
            this.incRepeatButton.Click += (s, e) => IncrementValue(); 
        }
        if (this.decRepeatButton != null)
        {
            this.decRepeatButton.Click += (s, e) => DecrementValue(); 
        }
    }

   
    protected void IncrementValue()
    {
        this.Value += (dynamic)this.Increment;
    }

    protected void DecrementValue()
    {
        this.Value -= (dynamic)this.Increment;
    }

    #region CultureInfo

    public static readonly DependencyProperty CultureInfoProperty =
        DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(ValueControl<T>), new UIPropertyMetadata(CultureInfo.CurrentCulture,
            (o, e) => ((ValueControl<T>)o).OnCultureInfoChanged((CultureInfo)e.OldValue, (CultureInfo)e.NewValue)
            ));
    
    public CultureInfo CultureInfo
    {
        get => (CultureInfo)GetValue(CultureInfoProperty);
        set => SetValue(CultureInfoProperty, value);
    }

    
    protected virtual void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
    { }

    #endregion //CultureInfo



    #region StringFormat

    
    public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(ValueControl<T>), 
        new UIPropertyMetadata(String.Empty,
            (o, e) => ((ValueControl<T>)o).OnStringFormatChanged()));

    public string StringFormat
    {
        get =>  (string)GetValue(StringFormatProperty);
        set => SetValue(StringFormatProperty, value);
    }

    private void OnStringFormatChanged()
    {
        this.Text = ValueToString();
    }

    #endregion

    #region IsReadOnly

    public static readonly DependencyProperty IsReadOnlyProperty =
       DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(ValueControl<T>), new PropertyMetadata(false));

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    #endregion

    #region Text

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(ValueControl<T>), 
            new FrameworkPropertyMetadata(default(String), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (o, e) => ((ValueControl<T>)o).OnTextChanged((string)e.OldValue, (string)e.NewValue),
                //OnTextChanged, 
                null, false, UpdateSourceTrigger.LostFocus));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    protected virtual void OnTextChanged(string oldValue, string newValue)
    { }

    #endregion 

    #region Increment

    public static readonly DependencyProperty IncrementProperty =
        DependencyProperty.Register("Increment", typeof(T), typeof(ValueControl<T>), new PropertyMetadata(default(T),
            (d, e) => ((ValueControl<T>)d).OnIncrementChanged((T)e.OldValue, (T)e.NewValue),
            (d, b) => ((ValueControl<T>)d).OnCoerceIncrement((T)b)));

    public T Increment
    {
        get => (T)GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    protected virtual void OnIncrementChanged(T oldValue, T newValue)
    {
        if (this.IsInitialized)
        {
            //SetValidSpinDirection();
        }
    }

    protected virtual T OnCoerceIncrement(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Maximum

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(T), typeof(ValueControl<T>), new UIPropertyMetadata(default(T),
            (o, e) => ((ValueControl<T>)o).OnMaximumChanged((T)e.OldValue, (T)e.NewValue),
            (o, b) => ((ValueControl<T>)o).OnCoerceMaximum((T)b)));
   
    public T Maximum
    {
        get => (T)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    protected virtual void OnMaximumChanged(T oldValue, T newValue)
    {
        if (this.IsInitialized)
        {
            //SetValidSpinDirection();
        }
    }

    //private static object? OnCoerceMaximum(DependencyObject d, object baseValue)
    //{
    //    if (d is NumericBase<T> upDown)
    //    {
    //        return upDown.OnCoerceMaximum((T)baseValue);
    //    }
    //    return baseValue;
    //}

    protected virtual T OnCoerceMaximum(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Minimum

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(T), typeof(ValueControl<T>), new UIPropertyMetadata(default(T),
            (d, e) => ((ValueControl<T>)d).OnMinimumChanged((T)e.OldValue, (T)e.NewValue),
            (d, b) => ((ValueControl<T>)d).OnCoerceMinimum((T)b)));

    public T Minimum
    {
        get => (T)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    protected virtual void OnMinimumChanged(T oldValue, T newValue)
    {
        if (this.IsInitialized)
        {
            //SetValidSpinDirection();
        }
    }

    //private static object OnCoerceMinimum(DependencyObject d, object baseValue)
    //{
    //    if (d is NumericBase<T> upDown)
    //        return upDown!.OnCoerceMinimum((T)baseValue)!;

    //    return baseValue;
    //}

    protected virtual T OnCoerceMinimum(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Value

    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(T?), typeof(ValueControl<T>), 
            new FrameworkPropertyMetadata(default(T?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (d, e) => ((ValueControl<T>)d).OnValueChanged((T)e.OldValue, (T)e.NewValue),
                (d, b) => ((ValueControl<T>)d).OnCoerceValue((T)b),
                //OnValueChanged, OnCoerceValue, 
                false, UpdateSourceTrigger.PropertyChanged));

    public T? Value
    {
        get => (T?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    //private static object OnCoerceValue(DependencyObject o, object basevalue) => ((NumericBase<T>)o).OnCoerceValue(basevalue);

    protected virtual object OnCoerceValue(object newValue)
    {
        //return newValue;

        dynamic value = newValue;
        value = Math.Max(value, (dynamic)this.Minimum);
        value = Math.Min(value, (dynamic)this.Maximum);
        //double x = value / (dynamic)this.Increment;
        //var res = Math.Round(x) * (dynamic)this.Increment;
        return value;
    }

    //private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnValueChanged((T?)e.OldValue, (T?)e.NewValue);

    protected virtual void OnValueChanged(T? oldValue, T? newValue)
    {
        this.Text = ValueToString();
    }

    protected virtual string ValueToString()
    {
        return this.Value!.ToString(this.StringFormat, this.CultureInfo);
    }

    #endregion

    public static readonly DependencyProperty TickFrequencyProperty =
        DependencyProperty.Register("TickFrequency", typeof(T), typeof(ValueControl<T>), new UIPropertyMetadata(default(T)));

    public T TickFrequency
    {
        get => (T)GetValue(TickFrequencyProperty);
        set => SetValue(TickFrequencyProperty, value);
    }

    public static readonly DependencyProperty TickPlacementProperty =
        DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(ValueControl<T>), new UIPropertyMetadata(TickPlacement.None));

    public TickPlacement TickPlacement
    {
        get => (TickPlacement)GetValue(TickPlacementProperty);
        set => SetValue(TickPlacementProperty, value);
    }

    public static readonly DependencyProperty SliderWidthProperty =
        DependencyProperty.Register("SliderWidth", typeof(double), typeof(ValueControl<T>), new UIPropertyMetadata(160.0));

    public double SliderWidth
    {
        get => (double)GetValue(SliderWidthProperty);
        set => SetValue(SliderWidthProperty, value);
    }

    public static readonly DependencyProperty TextWidthProperty =
        DependencyProperty.Register("TextWidth", typeof(double), typeof(ValueControl<T>), new UIPropertyMetadata(50.0));

    public double TextWidth
    {
        get => (double)GetValue(TextWidthProperty);
        set => SetValue(TextWidthProperty, value);
    }
}
