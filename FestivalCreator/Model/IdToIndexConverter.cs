using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FestivalCreator.Model
{
    [ValueConversion(typeof(string), typeof(int))]
    class IdToIndexConverter:IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<ContactpersoonType> functies = ContactpersoonType.GetContactpersoonTypes();

            int index = 0;

            for (int i = 0; i < functies.Count; i++)
            {
                if (functies[i].ID.Equals((string)value))
                    index = i;
            }

            return index;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            ObservableCollection<ContactpersoonType> functies = ContactpersoonType.GetContactpersoonTypes();

            if ((int)value == -1)
                return "1";
            
            return functies[(int)value].ID;
        }
    }
}
