using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Owl.DataObjects;
using Owl.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;

namespace Owl.ViewModel
{
    public class MessageViewModel : ViewModelBase
    {
        private ObservableCollection<DataObjects.Message> _messageCollection = new ObservableCollection<DataObjects.Message>();
        public ObservableCollection<DataObjects.Message> MessageCollection
        {
            get { return _messageCollection; }
            set
            {
                _messageCollection = value;
                RaisePropertyChanged("MessageCollection");
            }
        }

        public RelayCommand<string> SendMessageCommand { get; set; }
        public RelayCommand ListViewMessageLoadedCommand { get; set; }

        private string _userId;
        private string _userName;
        private string _userProfileUrl;
        private string _pairId;

        public MessageViewModel()
        {
            SendMessageCommand = new RelayCommand<string>(msg =>
            {
                Messenger.Default.Send<SendMsgMessage>(new SendMsgMessage(true));
                MessageCollection.Add(new DataObjects.Message()
                {
                    TextContent = msg,
                    Time = DateTime.Now.ToString("ddd, hh:mm"),
                    UpperLeftVisibility = "Collapsed",
                    LowerRightVisibility = "Visible",
                    BubbleMargin = "100,0,0,0",
                    BackgroundColor = "#990050EF"
                });
                Messenger.Default.Send<SendMsgMessage>(new SendMsgMessage());
            });
            ListViewMessageLoadedCommand = new RelayCommand(() =>
            {
                LoadPosts();
            });
        }

        public async void InitBasicInfo(string userId, string userName, string userProfileUrl, string pairId)
        {
            _userId = userId;
            _userProfileUrl = userProfileUrl;
            _userName = userName;
            _pairId = pairId;
            if(string.IsNullOrWhiteSpace(_pairId))
                _pairId = await JsonReceiver.CreateNewMessagePair(_userId);

            Messenger.Default.Send<EnterIntoChatMessage>(new EnterIntoChatMessage(new ChatEntry()
            {
                UserId = _userId,
                UserName = _userName,
                Message = "",
                Time = "",
                UserProfile = _userProfileUrl,
                PairId = _pairId
            }));
        }

        private void LoadPosts()
        {
            Messenger.Default.Send<SendMsgMessage>(new SendMsgMessage(true));

            MessageCollection.Add(new DataObjects.Message()
            {
                TextContent = "Hi, we can start talking",
                Time = DateTime.Now.ToString("ddd, hh:mm"),
                UpperLeftVisibility = "Visible",
                LowerRightVisibility = "Collapsed",
                BubbleMargin = "0,0,100,0",
                BackgroundColor = "#FF0050EF"
            });
            MessageCollection.Add(new DataObjects.Message()
            {
                TextContent = "Hi, how're you?",
                Time = DateTime.Now.ToString("ddd, hh:mm"),
                UpperLeftVisibility = "Collapsed",
                LowerRightVisibility = "Visible",
                BubbleMargin = "100,0,0,0",
                BackgroundColor = "#990050EF"
            });
            MessageCollection.Add(new DataObjects.Message()
            {
                TextContent = "Hi",
                Time = DateTime.Now.ToString("ddd, hh:mm"),
                UpperLeftVisibility = "Visible",
                LowerRightVisibility = "Collapsed",
                BubbleMargin = "0,0,100,0",
                BackgroundColor = "#FF0050EF"
            });
            Messenger.Default.Send<SendMsgMessage>(new SendMsgMessage());
        }
    }
}
