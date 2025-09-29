using System;
using System.Collections.Generic;
using System.Text;

namespace WpfToolbox.Converter;

public class CheckListBoxMultiValueConverter : IMultiValueConverter
{
    private CheckListBox? control = null;
    private object? currentItem = null;

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values is null || values.Length < 2 || values[0] is not CheckListBox checkedListBox || values[1] is not object item)
        {
            return false;
        }

        control = checkedListBox;
        currentItem = values[1];

        //if (control != null && control != checkedListBox)
        //{
        //    throw new Exception("CheckListBoxMultiValueConverter different control");
        //}
        //control = checkedListBox;


        return checkedListBox.CheckedItems.Contains(item);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        bool isChecked = value is bool b && b;

        if (!(parameter is object[] originalValues) || originalValues.Length < 2)
        {
            throw new ArgumentException("ConverterParameter must be an object array containing the original checked items and the current item.", nameof(parameter));
        }
        

        if (!(originalValues[0] is IList checkedItems))
        {
            throw new ArgumentException("The first element of ConverterParameter must be an IList representing the checked items.", nameof(parameter));
        }

        object item = originalValues[1];

        if (isChecked)
        {
            if (!checkedItems.Contains(item))
            {
                checkedItems.Add(item);
            }
        }
        else
        {
            if (checkedItems.Contains(item))
            {
                checkedItems.Remove(item);
            }
        }

        // Return the updated checked items and the item unchanged.
        return new object[] { checkedItems, item };
    }
}
