using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace Administrator1._0.Other
{
    public class StatusOrderConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                value = "notstated";
            }
            string status = value.ToString();
            switch (value)
            {
                case "notstated": return 0;
                case "done": return 1;
                case "confirmed": return 2;
                case "canceled": return 3;
                default: return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value == null)
            {
                value = 0;
            }
            int index = (int)value;
            switch (index)
            {
                case 0: return "notstated";
                case 1: return "done";
                case 2: return "confirmed";
                case 3: return "canceled";
                default: return "";
            }
        }


        static StatusOrderConverter instance;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (instance == null)
                instance = new StatusOrderConverter();
            return instance;
        }
    }
}
