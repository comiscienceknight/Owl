using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlWindowsPhoneApp.DataObjects
{
    public class Message : INotifyPropertyChanged
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

        private string _textContent;
        public string TextContent
        {
            get { return _textContent; }
            set
            {
                _textContent = value;
                OnPropertyChanged("TextContent");
            }
        }

        private string _upperLeftVisibility;
        public string UpperLeftVisibility
        {
            get { return _upperLeftVisibility; }
            set
            {
                _upperLeftVisibility = value;
                OnPropertyChanged("UpperLeftVisibility");
            }
        }

        private string _lowerRightVisibility;
        public string LowerRightVisibility
        {
            get { return _lowerRightVisibility; }
            set
            {
                _lowerRightVisibility = value;
                OnPropertyChanged("LowerRightVisibility");
            }
        }

        private string _bubbleMargin;
        public string BubbleMargin
        {
            get { return _bubbleMargin; }
            set
            {
                _bubbleMargin = value;
                OnPropertyChanged("BubbleMargin");
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

        private string _backgroundColor;
        public string BackgroundColor
        {
            get { return _backgroundColor; }
            set
            {
                _backgroundColor = value;
                OnPropertyChanged("BackgroundColor");
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
    }

}
