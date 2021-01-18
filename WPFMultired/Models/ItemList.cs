using System.ComponentModel;

namespace WPFMultired.Models
{
    public class ItemList : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private string _item1;

        public string Item1
        {
            get
            {
                return _item1;
            }
            set
            {
                if (_item1 != value)
                {
                    _item1 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item1)));
                }
            }
        }

        private string _item2;

        public string Item2
        {
            get
            {
                return _item2;
            }
            set
            {
                if (_item2 != value)
                {
                    _item2 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item2)));
                }
            }
        }

        private string _item3;

        public string Item3
        {
            get
            {
                return _item3;
            }
            set
            {
                if (_item3 != value)
                {
                    _item3 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item3)));
                }
            }
        }

        private decimal _item4;

        public decimal Item4
        {
            get
            {
                return _item4;
            }
            set
            {
                if (_item4 != value)
                {
                    _item4 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item4)));
                }
            }
        }

        private decimal _item5;

        public decimal Item5
        {
            get
            {
                return _item5;
            }
            set
            {
                if (_item5 != value)
                {
                    _item5 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item5)));
                }
            }
        }

        private int _item6;

        public int Item6
        {
            get
            {
                return _item6;
            }
            set
            {
                if (_item6 != value)
                {
                    _item6 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item6)));
                }
            }
        }

        private string _item7;

        public string Item7
        {
            get
            {
                return _item7;
            }
            set
            {
                if (_item7 != value)
                {
                    _item7 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item7)));
                }
            }
        }

        private string _item8;

        public string Item8
        {
            get
            {
                return _item8;
            }
            set
            {
                if (_item8 != value)
                {
                    _item8 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item8)));
                }
            }
        }

        private string _item9;

        public string Item9
        {
            get
            {
                return _item9;
            }
            set
            {
                if (_item9 != value)
                {
                    _item9 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item9)));
                }
            }
        }

        private decimal _item10;

        public decimal Item10
        {
            get
            {
                return _item10;
            }
            set
            {
                if (_item10 != value)
                {
                    _item10 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item10)));
                }
            }
        }

        private decimal _item11;

        public decimal Item11
        {
            get
            {
                return _item11;
            }
            set
            {
                if (_item11 != value)
                {
                    _item11 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item11)));
                }
            }
        }

        private decimal _item12;

        public decimal Item12
        {
            get
            {
                return _item12;
            }
            set
            {
                if (_item12 != value)
                {
                    _item12 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item12)));
                }
            }
        }

        private decimal _item13;

        public decimal Item13
        {
            get
            {
                return _item13;
            }
            set
            {
                if (_item13 != value)
                {
                    _item13 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item13)));
                }
            }
        }

        private string _item14;

        public string Item14
        {
            get
            {
                return _item14;
            }
            set
            {
                if (_item14 != value)
                {
                    _item14 = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Item14)));
                }
            }
        }

        private int _index;

        public int Index
        {
            get
            {
                return _index;
            }
            set
            {
                if (_index != value)
                {
                    _index = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Index)));
                }
            }
        }

        private string _imageSourse;

        public string ImageSourse
        {
            get
            {
                return _imageSourse;
            }
            set
            {
                if (_imageSourse != value)
                {
                    _imageSourse = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageSourse)));
                }
            }
        }

        private object _data;

        public object Data
        {
            get
            {
                return _data;
            }
            set
            {
                if (_data != value)
                {
                    _data = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Data)));
                }
            }
        }

        private string _visibilityHON;

        public string VisibilityHON
        {
            get
            {
                return _visibilityHON;
            }
            set
            {
                if (_visibilityHON != value)
                {
                    _visibilityHON = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityHON)));
                }
            }
        }

        private string _visibilityPAP;

        public string VisibilityPAP
        {
            get
            {
                return _visibilityPAP;
            }
            set
            {
                if (_visibilityPAP != value)
                {
                    _visibilityPAP = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityPAP)));
                }
            }
        }
        private string _visibilityAPO;

        public string VisibilityAPO
        {
            get
            {
                return _visibilityAPO;
            }
            set
            {
                if (_visibilityAPO != value)
                {
                    _visibilityAPO = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityAPO)));
                }
            }
        }
    }
}
