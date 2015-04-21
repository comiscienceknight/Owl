using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml.Media.Imaging;

namespace Owl.DataObjects
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

        private string _venueId;
        public string VenueId
        {
            get { return _venueId; }
            set
            {
                _venueId = value;
                OnPropertyChanged("VenueId");
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

        private string _dressCode;
        public string DressCode
        {
            get { return _dressCode; }
            set
            {
                _dressCode = value;
                OnPropertyChanged("DressCode");
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
        public string OtherInfo
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

        private string _lookingFor;
        public string LookingFor
        {
            get { return _lookingFor; }
            set
            {
                _lookingFor = value;
                OnPropertyChanged("LookingFor");
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

        private string _profileUrl2;
        public string ProfileUrl2
        {
            get { return _profileUrl2; }
            set
            {
                _profileUrl2 = value;
                OnPropertyChanged("ProfileUrl2");
            }
        }

        private string _profileUrl3;
        public string ProfileUrl3
        {
            get { return _profileUrl3; }
            set
            {
                _profileUrl3 = value;
                OnPropertyChanged("ProfileUrl3");
            }
        }

        private string _venuePhotoUrl1;
        public string VenuePhotoUrl1
        {
            get { return _venuePhotoUrl1; }
            set
            {
                _venuePhotoUrl1 = value;
                OnPropertyChanged("VenuePhotoUrl1");
            }
        }

        private string _venuePhotoUrl2;
        public string VenuePhotoUrl2
        {
            get { return _venuePhotoUrl2; }
            set
            {
                _venuePhotoUrl2 = value;
                OnPropertyChanged("VenuePhotoUrl2");
            }
        }

        private string _venuePhotoUrl3;
        public string VenuePhotoUrl3
        {
            get { return _venuePhotoUrl3; }
            set
            {
                _venuePhotoUrl3 = value;
                OnPropertyChanged("VenuePhotoUrl3");
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
        public string VenuePosition
        {
            get { return _myPosition; }
            set
            {
                _myPosition = value;
                OnPropertyChanged("MyPosition");
            }
        }

        private int _userPopularity;
        public int UserPopularity
        {
            get { return _userPopularity; }
            set
            {
                _userPopularity = value;
                OnPropertyChanged("UserPopularity");
            }
        }

        private int _venuePopularity;
        public int VenuePopularity
        {
            get { return _venuePopularity; }
            set
            {
                _venuePopularity = value;
                OnPropertyChanged("VenuePopularity");
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

        private BitmapImage _profile2;
        public BitmapImage Profile2
        {
            get { return _profile2; }
            set
            {
                _profile2 = value;
                OnPropertyChanged("Profile2");
            }
        }

        private BitmapImage _profile3;
        public BitmapImage Profile3
        {
            get { return _profile3; }
            set
            {
                _profile3 = value;
                OnPropertyChanged("Profile3");
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
