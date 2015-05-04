using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Owl.DataObjects
{
    public class ChatEntry : INotifyPropertyChanged
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

        private string _pairId;
        public string PairId
        {
            get { return _pairId; }
            set
            {
                _pairId = value;
                OnPropertyChanged("PairId");
            }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        private string _isFrom = "Visible";
        public string IsFrom
        {
            get { return _isFrom; }
            set
            {
                _isFrom = value;
                OnPropertyChanged("IsFrom");
            }
        }

        private string _isTo = "Collapsed";
        public string IsTo
        {
            get { return _isTo; }
            set
            {
                _isTo = value;
                OnPropertyChanged("IsTo");
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

        private long _updatedTime;
        public long UpdatedTime
        {
            get { return _updatedTime; }
            set
            {
                _updatedTime = value;
                OnPropertyChanged("UpdatedTime");
            }
        }

        private string _userProfile;
        public string UserProfile
        {
            get { return _userProfile; }
            set
            {
                _userProfile = value;
                OnPropertyChanged("UserProfile");
            }
        }
    }
}
