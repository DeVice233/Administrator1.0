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
    class StatusTourConverter : MarkupExtension, IValueConverter
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
                case "occuped": return 1;
                case "unoccuped": return 2;
                default: return 0;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                value = 0;
            }
            int index = (int)value;
            switch (index)
            {
                case 0: return "notstated";
                case 1: return "occuped";
                case 2: return "unoccuped";
                default: return "";
            }
        }


        static StatusTourConverter instance;
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (instance == null)
                instance = new StatusTourConverter();
            return instance;
        }
    }

}
