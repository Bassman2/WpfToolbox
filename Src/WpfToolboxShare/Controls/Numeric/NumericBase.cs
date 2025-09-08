namespace WpfToolbox.Controls;


/// <summary>
/// Represents a numeric slider control that allows users to select a value of type <typeparamref name="T"/> by sliding a thumb along a track.
/// Used for themes selection and provides base functionality for numeric slider controls.
/// </summary>
/// <typeparam name="T">The numeric type that implements <see cref="IFormattable"/>.</typeparam>
public abstract class NumericSlider<T> : NumericBase<T> where T : IFormattable
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static NumericSlider()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericSlider<T>), new FrameworkPropertyMetadata(typeof(NumericSlider<T>)));
    }
}

/// <summary>
/// Represents a numeric spin control that allows users to increment or decrement a value of type <typeparamref name="T"/>.
/// Used for themes selection and provides base functionality for numeric spin controls.
/// </summary>
/// <typeparam name="T">The numeric type that implements <see cref="IFormattable"/>.</typeparam>
/// <remarks>used for themes selection</remarks>
public abstract class NumericSpin<T> : NumericBase<T> where T : IFormattable
{
    /// <summary>
    /// Static constructor. Overrides the default style key to associate the control with its style in themes.
    /// </summary>
    static NumericSpin()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericSpin<T>), new FrameworkPropertyMetadata(typeof(NumericSpin<T>)));
    }
}

/// <summary>
/// Base class for numeric controls (slider/spin) supporting generic numeric types.
/// Provides dependency properties and logic for value editing, incrementing, formatting, and theming.
/// </summary>
public abstract class NumericBase<T> : Control where T : IFormattable
{
    private RepeatButton? incRepeatButton;
    private RepeatButton? decRepeatButton;

    /// <summary>
    /// Initializes a new instance of the <see cref="NumericBase{T}"/> class.
    /// Sets the initial Text property based on the Value.
    /// </summary>
    public NumericBase()
    {
        this.Text = ValueToString();
    }

    /// <summary>
    /// Called when the control template is applied. Hooks up increment/decrement button events.
    /// </summary>
    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        //if (Spinner != null)
        //    Spinner.Spin -= OnSpinnerSpin;

        this.incRepeatButton = GetTemplateChild("incRepeatButton") as RepeatButton;
        this.decRepeatButton = GetTemplateChild("decRepeatButton") as RepeatButton;

        if (this.incRepeatButton != null)
        {
            this.incRepeatButton.Click += OnIncRepeatButtonClick;
        }
        if (this.decRepeatButton != null)
        {
            this.decRepeatButton.Click += OnDecRepeatButtonClick;
        }
        //if (Spinner != null)
        //    Spinner.Spin += OnSpinnerSpin;
        //Slider
    }

    /// <summary>
    /// Handles the increment button click event.
    /// </summary>
    protected void OnIncRepeatButtonClick(object sender, RoutedEventArgs e)
    {
        IncrementValue();
    }

    /// <summary>
    /// Handles the decrement button click event.
    /// </summary>
    protected void OnDecRepeatButtonClick(object sender, RoutedEventArgs e)
    {
        DecrementValue();
    }

    /// <summary>
    /// Increments the Value property by the Increment amount.
    /// </summary>
    protected void IncrementValue()
    {
        this.Value += (dynamic)this.Increment;
    }

    /// <summary>
    /// Decrements the Value property by the Increment amount.
    /// </summary>
    protected void DecrementValue()
    {
        this.Value -= (dynamic)this.Increment;
    }

    #region CultureInfo

    /// <summary>
    /// Identifies the CultureInfo dependency property.
    /// </summary>
    public static readonly DependencyProperty CultureInfoProperty =
        DependencyProperty.Register("CultureInfo", typeof(CultureInfo), typeof(NumericBase<T>), new UIPropertyMetadata(CultureInfo.CurrentCulture,
            (o, e) => ((NumericSlider<T>)o).OnCultureInfoChanged((CultureInfo)e.OldValue, (CultureInfo)e.NewValue)
            ));

    /// <summary>
    /// Gets or sets the culture used for formatting and parsing values.
    /// </summary>
    public CultureInfo CultureInfo
    {
        get => (CultureInfo)GetValue(CultureInfoProperty);
        set => SetValue(CultureInfoProperty, value);
    }

    /// <summary>
    /// Called when the <see cref="CultureInfo"/> property changes.
    /// Allows derived classes to react to changes in culture, such as updating formatting or parsing logic.
    /// </summary>
    /// <param name="oldValue">The previous <see cref="CultureInfo"/> value.</param>
    /// <param name="newValue">The new <see cref="CultureInfo"/> value.</param>
    protected virtual void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
    {

    }

    #endregion //CultureInfo



    #region StringFormat

    /// <summary>
    /// Identifies the StringFormat dependency property.
    /// </summary>
    public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register("StringFormat", typeof(string), typeof(NumericBase<T>), 
        new UIPropertyMetadata(String.Empty,
            (o, e) => ((NumericBase<T>)o).OnStringFormatChanged((string)e.OldValue, (string)e.NewValue)
            //OnStringFormatChanged
            ));

    /// <summary>
    /// Gets or sets the string format used to display the value.
    /// </summary>
    public string StringFormat
    {
        get =>  (string)GetValue(StringFormatProperty);
        set => SetValue(StringFormatProperty, value);
    }

    //private static void OnStringFormatChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnStringFormatChanged((string)e.OldValue, (string)e.NewValue); 

    /// <summary>
    /// Called when the StringFormat property changes. Updates the Text property.
    /// </summary>
#pragma warning disable IDE0060 // Remove unused parameter
    private void OnStringFormatChanged(string oldValue, string newValue)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        this.Text = ValueToString();
    }

    #endregion

    #region IsReadOnly

    /// <summary>
    /// Identifies the IsReadOnly dependency property.
    /// </summary>
    public static readonly DependencyProperty IsReadOnlyProperty =
       DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(NumericBase<T>), new PropertyMetadata(false));

    /// <summary>
    /// Gets or sets a value indicating whether the control is read-only.
    /// </summary>
    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    #endregion

    #region Text

    /// <summary>
    /// Identifies the Text dependency property.
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(NumericBase<T>), 
            new FrameworkPropertyMetadata(default(String), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (o, e) => ((NumericBase<T>)o).OnTextChanged((string)e.OldValue, (string)e.NewValue),
                //OnTextChanged, 
                null, false, UpdateSourceTrigger.LostFocus));

    /// <summary>
    /// Gets or sets the text representation of the value.
    /// </summary>
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    //private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnTextChanged((string)e.OldValue, (string)e.NewValue);

    /// <summary>
    /// Called when the Text property changes.
    /// </summary>
    protected virtual void OnTextChanged(string oldValue, string newValue)
    {

    }

    #endregion //Text

    #region Increment

    /// <summary>
    /// Identifies the Increment dependency property.
    /// </summary>
    public static readonly DependencyProperty IncrementProperty =
        DependencyProperty.Register("Increment", typeof(T), typeof(NumericBase<T>), new PropertyMetadata(default(T),
            (d, e) => ((NumericBase<T>)d).OnIncrementChanged((T)e.OldValue, (T)e.NewValue),
            (d, b) => ((NumericBase<T>)d).OnCoerceIncrement((T)b)));

    /// <summary>
    /// Gets or sets the amount to increment or decrement the value.
    /// </summary>
    public T Increment
    {
        get => (T)GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    /// <summary>
    /// Called when the Increment property changes.
    /// </summary>
    protected virtual void OnIncrementChanged(T oldValue, T newValue)
    {
        if (this.IsInitialized)
        {
            //SetValidSpinDirection();
        }
    }

    /// <summary>
    /// Called to coerce the Increment property value.
    /// </summary>
    protected virtual T OnCoerceIncrement(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Maximum

    /// <summary>
    /// Identifies the Maximum dependency property.
    /// </summary>
    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(T), typeof(NumericBase<T>), new UIPropertyMetadata(default(T),
            (o, e) => ((NumericBase<T>)o).OnMaximumChanged((T)e.OldValue, (T)e.NewValue),
            (o, b) => ((NumericBase<T>)o).OnCoerceMaximum((T)b)));
   

    /// <summary>
    /// Gets or sets the maximum allowed value.
    /// </summary>
    public T Maximum
    {
        get => (T)GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    //private static void OnMaximumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnMaximumChanged((T)e.OldValue, (T)e.NewValue);

    /// <summary>
    /// Called when the Maximum property changes.
    /// </summary>
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

    /// <summary>
    /// Called to coerce the Maximum property value.
    /// </summary>
    protected virtual T OnCoerceMaximum(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Minimum

    /// <summary>
    /// Identifies the Minimum dependency property.
    /// </summary>
    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(T), typeof(NumericBase<T>), new UIPropertyMetadata(default(T),
            (d, e) => ((NumericBase<T>)d).OnMinimumChanged((T)e.OldValue, (T)e.NewValue),
            (d, b) => ((NumericBase<T>)d).OnCoerceMinimum((T)b)));

    /// <summary>
    /// Gets or sets the minimum allowed value.
    /// </summary>
    public T Minimum
    {
        get => (T)GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    //private static void OnMinimumChanged(DependencyObject o, DependencyPropertyChangedEventArgs e) => ((NumericBase<T>)o).OnMinimumChanged((T)e.OldValue, (T)e.NewValue);

    /// <summary>
    /// Called when the Minimum property changes.
    /// </summary>
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

    /// <summary>
    /// Called to coerce the Minimum property value.
    /// </summary>
    protected virtual T OnCoerceMinimum(T baseValue)
    {
        return baseValue;
    }

    #endregion

    #region Value

    /// <summary>
    /// Identifies the Value dependency property.
    /// </summary>
    public static readonly DependencyProperty ValueProperty =
        DependencyProperty.Register("Value", typeof(T?), typeof(NumericBase<T>), 
            new FrameworkPropertyMetadata(default(T?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                (d, e) => ((NumericBase<T>)d).OnValueChanged((T)e.OldValue, (T)e.NewValue),
                (d, b) => ((NumericBase<T>)d).OnCoerceValue((T)b),
                //OnValueChanged, OnCoerceValue, 
                false, UpdateSourceTrigger.PropertyChanged));

    /// <summary>
    /// Gets or sets the current value.
    /// </summary>
    public T? Value
    {
        get => (T?)GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    //private static object OnCoerceValue(DependencyObject o, object basevalue) => ((NumericBase<T>)o).OnCoerceValue(basevalue);

    /// <summary>
    /// Coerces the Value property to be within the Minimum and Maximum range.
    /// </summary>
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

    /// <summary>
    /// Called when the Value property changes. Updates the Text property.
    /// </summary>
    protected virtual void OnValueChanged(T? oldValue, T? newValue)
    {
        this.Text = ValueToString();
    }

    /// <summary>
    /// Converts the Value property to a string using the current StringFormat and CultureInfo.
    /// </summary>
    protected virtual string ValueToString()
    {
        return this.Value!.ToString(this.StringFormat, this.CultureInfo);
    }

    #endregion

    /// <summary>
    /// Identifies the TickFrequency dependency property. Used for slider tick intervals.
    /// </summary>
    public static readonly DependencyProperty TickFrequencyProperty =
        DependencyProperty.Register("TickFrequency", typeof(T), typeof(NumericBase<T>), new UIPropertyMetadata(default(T)));

    /// <summary>
    /// Gets or sets the frequency of ticks for a slider.
    /// </summary>
    public T TickFrequency
    {
        get => (T)GetValue(TickFrequencyProperty);
        set => SetValue(TickFrequencyProperty, value);
    }

    /// <summary>
    /// Identifies the TickPlacement dependency property. Used for slider tick placement.
    /// </summary>
    public static readonly DependencyProperty TickPlacementProperty =
        DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(NumericBase<T>), new UIPropertyMetadata(TickPlacement.None));

    /// <summary>
    /// Gets or sets the placement of ticks for a slider.
    /// </summary>
    public TickPlacement TickPlacement
    {
        get => (TickPlacement)GetValue(TickPlacementProperty);
        set => SetValue(TickPlacementProperty, value);
    }

    /// <summary>
    /// Identifies the SliderWidth dependency property. Used for setting the width of the slider part.
    /// </summary>
    public static readonly DependencyProperty SliderWidthProperty =
        DependencyProperty.Register("SliderWidth", typeof(double), typeof(NumericBase<T>), new UIPropertyMetadata(160.0));

    /// <summary>
    /// Gets or sets the width of the slider part.
    /// </summary>
    public double SliderWidth
    {
        get => (double)GetValue(SliderWidthProperty);
        set => SetValue(SliderWidthProperty, value);
    }

    /// <summary>
    /// Identifies the TextWidth dependency property. Used for setting the width of the text box part.
    /// </summary>
    public static readonly DependencyProperty TextWidthProperty =
        DependencyProperty.Register("TextWidth", typeof(double), typeof(NumericBase<T>), new UIPropertyMetadata(50.0));

    /// <summary>
    /// Gets or sets the width of the text box part.
    /// </summary>
    public double TextWidth
    {
        get => (double)GetValue(TextWidthProperty);
        set => SetValue(TextWidthProperty, value);
    }
}
