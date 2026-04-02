using CapFrameX.Contracts.Sensor;
using Prism.Mvvm;

namespace CapFrameX.Sensor
{
    public class CustomSensorEntryWrapper : BindableBase, ICustomSensorEntry
    {
        private string _name;
        private string _formula;
        private string _unit;
        private bool _isActive;

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(Identifier));
            }
        }

        public string Formula
        {
            get => _formula;
            set
            {
                _formula = value;
                RaisePropertyChanged();
            }
        }

        public string Unit
        {
            get => _unit;
            set
            {
                _unit = value;
                RaisePropertyChanged();
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                RaisePropertyChanged();
            }
        }

        public string Identifier => $"Custom/{Name}";
    }
}
