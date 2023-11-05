using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Grant2FW.ViewModel
{
    internal class ListOfHousesVM : INotifyPropertyChanged
    {
        private Models.ListOfHousesModel model;

        public ListOfHousesVM()
        {
            model = new Models.ListOfHousesModel();
        }
        public string TextValue
        {
            get { return model.Text; }
            set
            {
                if (model.Text != value)
                {
                    model.Text = value;
                    RaisePropertyChanged("TextValue");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
