using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Owl.DataObjects;
using Owl.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Owl.ViewModel
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
            ItemSelectedCommand = new RelayCommand<ChatEntry>(c =>
            {
                Messenger.Default.Send<NavigateToChatMessage>(new NavigateToChatMessage(c));
            });
            ListViewChatEntriesLoadedCommand = new RelayCommand(() =>
            {
                LoadChatsEntries();
            });
            Messenger.Default.Register<EnterIntoChatMessage>(this, async msg =>
            {
                if(ChatEntrytCollection.All(p=>p.PairId != msg.Message.PairId))
                {
                    ChatEntrytCollection.Add(new ChatEntry()
                    {
                        Time = msg.Message.Time ?? "",
                        Message = msg.Message.Message ?? "",
                        PairId = msg.Message.PairId,
                        UserId = msg.Message.UserId,
                        UserName = msg.Message.UserName,
                        UserProfile = msg.Message.UserProfile + "?Width=90"
                    });
                }
                SaveToLocalFile(_fileName, ChatEntrytCollection.ToList());
            });
            Messenger.Default.Register<LogoutMessage>(this, msg =>
            {
                ChatEntrytCollection.Clear();
            });
        }

        string _fileName = App.MySelf.Id + "_ChatList";

        private async void LoadChatsEntries()
        {
            ChatEntrytCollection.Clear();
            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage(true), LoadingAnimationMessage.ChatToken);
            var chatHistories = await ReadStringFromLocalFile(_fileName);
            foreach (var item in chatHistories)
            {
                ChatEntrytCollection.Add(new ChatEntry()
                {
                    Time = item.Time,
                    PairId = item.PairId,
                    Message = item.Message,
                    UserId = item.UserId,
                    UserName = item.UserName,
                    UserProfile = item.UserProfile,
                    UpdatedTime = DateTime.Now.Ticks
                });
            }
            Messenger.Default.Send<LoadingAnimationMessage>(new LoadingAnimationMessage(), LoadingAnimationMessage.ChatToken);
        }


        public async Task SaveToLocalFile(string filename, List<ChatEntry> chatEntries)
        {
            // create a file with the given filename in the local folder; replace any existing file with the same name
            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            if (chatEntries.Count > 0)
                await Windows.Storage.FileIO.WriteLinesAsync(file, chatEntries.Select(item =>
                    string.Format("{0};{1};{2};{3};{4};{5};{6}", item.Message, item.PairId, item.UserName, item.UserId, item.Time, item.UserProfile, item.UpdatedTime)));
        }

        public static async Task<List<ChatEntry>> ReadStringFromLocalFile(string filename)
        {
            var chatEntries = new List<ChatEntry>();
            try
            {
                // access the local folder
                StorageFolder local = Windows.Storage.ApplicationData.Current.LocalFolder;
                // open the file 'filename' for reading
                Stream stream = await local.OpenStreamForReadAsync(filename);
                string text;
                // copy the file contents into the string 'text'
                using (StreamReader reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        text = await reader.ReadLineAsync();
                        if (!string.IsNullOrWhiteSpace(text) && text.Length > 4)
                        {
                            string[] subStrs = text.Split(';');
                            if (subStrs.Length > 3)
                                chatEntries.Add(new ChatEntry()
                                    {
                                        Message = subStrs[0],
                                        PairId = subStrs[1],
                                        UserName = subStrs[2],
                                        UserId = subStrs[3],
                                        Time = subStrs[4],
                                        UserProfile = subStrs[5],
                                        UpdatedTime = Convert.ToInt64(string.IsNullOrWhiteSpace(subStrs[6]) ? "0" : subStrs[6])
                                    });
                        }
                    }
                }
            }
            catch (FileNotFoundException fnfexp)
            {

            }
            return chatEntries.OrderByDescending(p => p.UpdatedTime).ToList();
        }
    }
}
