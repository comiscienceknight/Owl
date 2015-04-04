﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using OwlWindowsPhoneApp.DataObjects;
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

namespace OwlWindowsPhoneApp.ViewModel
{
    public class ChatHistoryViewModel : ViewModelBase
    {
        private ObservableCollection<ChatEntry> _chatEntrytCollection = new ObservableCollection<ChatEntry>();
        public ObservableCollection<ChatEntry> ChatEntrytCollection
        {
            get { return _chatEntrytCollection; }
            set
            {
                _chatEntrytCollection = value;
                RaisePropertyChanged("ChatEntrytCollection");
            }
        }

        public RelayCommand<ChatEntry> ItemSelectedCommand { get; set; }
        public RelayCommand ListViewChatEntriesLoadedCommand { get; set; }

        public ChatHistoryViewModel()
        {
            ItemSelectedCommand = new RelayCommand<ChatEntry>(ChatEntry =>
            {
                Messenger.Default.Send<NavigateToChatMessage>(new NavigateToChatMessage(ChatEntry));
            });
            ListViewChatEntriesLoadedCommand = new RelayCommand(() =>
            {
                LoadChatsEntries();
            });
            Messenger.Default.Register<EnterIntoChatMessage>(this, msg =>
            {
                if(ChatEntrytCollection.All(p=>p.UserId != msg.Message.UserId))
                {
                    ChatEntrytCollection.Add(new ChatEntry()
                    {
                        Time = msg.Message.Time,
                        Message = msg.Message.Message,
                        UserId = msg.Message.UserId,
                        UserName = msg.Message.UserName,
                        UserProfile = msg.Message.UserProfile + "?Width=125"
                    });
                }
            });
            Messenger.Default.Register<LogoutMessage>(this, msg =>
            {
                ChatEntrytCollection.Clear();
            });
        }

        private async void LoadChatsEntries()
        {
            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage(true), LoadingAnimationMessage.ChatToken);

            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage(), LoadingAnimationMessage.ChatToken);
        }
    }
}