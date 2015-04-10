using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace OwlWindowsPhoneApp.DataObjects
{
    public class Post: INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            var eventHandler = this.PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private string _place;
        public string Place
        {
            get { return _place; }
            set
            {
                _place = value;
                OnPropertyChanged("Place");
            }
        }

        private int? _guysNumber;
        public int? GuysNumber
        {
            get { return _guysNumber; }
            set
            {
                _guysNumber = value;
                OnPropertyChanged("GuysNumber");
            }
        }

        private int? _girlsNumber;
        public int? GirlsNumber
        {
            get { return _girlsNumber; }
            set
            {
                _girlsNumber = value;
                OnPropertyChanged("GirlsNumber");
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
        }

        private string _userId;
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChanged("UserId");
            }
        }

        private string _require;
        public string Require
        {
            get { return _require; }
            set
            {
                _require = value;
                OnPropertyChanged("Require");
            }
        }

        private string _time;
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged("UserName");
            }
        }

        private string _profileUrl;
        public string ProfileUrl
        {
            get { return _profileUrl; }
            set
            {
                _profileUrl = value;
                OnPropertyChanged("ProfileUrl");
            }
        }

        private string _placeAddresse;
        public string PlaceAddresse
        {
            get { return _placeAddresse; }
            set
            {
                _placeAddresse = value;
                OnPropertyChanged("PlaceAddresse");
            }
        }

        private string _myPosition;
        public string MyPosition
        {
            get { return _myPosition; }
            set
            {
                _myPosition = value;
                OnPropertyChanged("MyPosition");
            }
        }

        private int _popularity;
        public int Popularity
        {
            get { return _popularity; }
            set
            {
                _popularity = value;
                OnPropertyChanged("Popularity");
            }
        }

        private BitmapImage _profile;
        public BitmapImage Profile
        {
            get { return _profile; }
            set
            {
                _profile = value;
                OnPropertyChanged("Profile");
            }
        }
    }

    public class JPost
    {
        public string Id { get; set; }
        public string PlaceName { get; set; }
        public string UserId { get; set; }
        public string Time { get; set; }
        public int GuysNumber { get; set; }
        public int GirlsNumber { get; set; }
        public string Description { get; set; }
        public string ProfileUrl { get; set; }
        public string ProfileUrl2 { get; set; }
        public string ProfileUrl3 { get; set; }
        public string ProfileUrl4 { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
    }
}
