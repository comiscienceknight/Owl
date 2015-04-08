using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Telerik.UI.Xaml.Controls.Input.AutoCompleteBox;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace OwlWindowsPhoneApp.View
{
    public sealed partial class FirstTimeEnterUserControl : UserControl
    {
        public event EventHandler<EventArgs> GuideFinished;
        public event EventHandler<EventArgs> TakePhotoClick;
        /// <summary>
        /// 1 for man, 0 for woman
        /// </summary>
        private bool? _maleOrFemale = null;

        public FirstTimeEnterUserControl()
        {
            this.InitializeComponent();

            this.Loaded += FirstTimeEnterUserControl_Loaded;
        }

        void FirstTimeEnterUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitAutoTextComplete();
        }

        private async void TextBlock_Indication_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
                TextBlock_Indication.Text = "";
                Rectangle_Indication.Opacity = 0;
                TextBlock_VenueName.Text = RadAutoCompleteBox_Search.Text;
            });
        }

        private async void Image_Profile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (TakePhotoClick != null)
                {
                    TakePhotoClick(this, null);
                }
            });
        }

        public async void ChangeImageProfile(RenderTargetBitmap bmp)
        {
            if (bmp != null)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
                    Image_Profile.Source = bmp;
                    TextBlock_TouchMyWings.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                });
            }
        }

        private async void Image_Boy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_HiSex.Text = "My Gentleman!";
                _maleOrFemale = true;
                FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
            });
        }

        private async void Image_Girls_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_HiSex.Text = "Hi Lady!";
                _maleOrFemale = false;
                FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
            });
        }

        #region avenues search
        private WebServiceTextSearchProvider _provider;
        public List<SearchAvenues> MockAveues = new List<SearchAvenues>() { 
            new SearchAvenues(){ Avenue = "White Room"},
            new SearchAvenues(){ Avenue = "Café Oz Châtelet"},
            new SearchAvenues(){ Avenue = "Café Oz Denfert Rochereau"},
            new SearchAvenues(){ Avenue = "Café Oz Blanche"},
            new SearchAvenues(){ Avenue = "Café Oz Grands Boulevards"},
            new SearchAvenues(){ Avenue = "Club Queen"},
            new SearchAvenues(){ Avenue = "Le Showcase"},
            new SearchAvenues(){ Avenue = "Club 79"},
            new SearchAvenues(){ Avenue = "Mix Club"},
            new SearchAvenues(){ Avenue = "L'Arc"},
            new SearchAvenues(){ Avenue = "Matignon"},
            new SearchAvenues(){ Avenue = "Black Calvados"},
            new SearchAvenues(){ Avenue = "Le Buddah Bar"},
            new SearchAvenues(){ Avenue = "O'Sullivans"},
            new SearchAvenues(){ Avenue = "Le Duplex"},
            new SearchAvenues(){ Avenue = "VIP Room"},
            new SearchAvenues(){ Avenue = "OMantra"},
            new SearchAvenues(){ Avenue = "Chez Raspoutine"},
            new SearchAvenues(){ Avenue = "Le Baron"}
        };

        private void InitAutoTextComplete()
        {
            _provider = new WebServiceTextSearchProvider();
            _provider.InputChanged += WebServiceProvider_InputChanged;
            RadAutoCompleteBox_Search.InitializeSuggestionsProvider(_provider);
            this.ProgressBar_Search.Visibility = Visibility.Collapsed;
        }

        private async void WebServiceProvider_InputChanged(object sender, EventArgs e)
        {
            string inputString = _provider.InputString;
            if (!string.IsNullOrEmpty(inputString))
            {
                this.ProgressBar_Search.Visibility = Visibility.Visible;
                var items = MockAveues.Where(p => p.Avenue.Contains(inputString));
                if (items == null || items.Count() == 0)
                {
                    items = MockAveues.OrderBy(p => p.Avenue).Take(10);
                }
                _provider.LoadItems(items.OrderBy(p => p.Avenue));
                this.ProgressBar_Search.Visibility = Visibility.Collapsed;

                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
                });
            }
            else
            {
                _provider.Reset();
            }
        }

        private void RadAutoCompleteBox_Search_LostFocus(object sender, RoutedEventArgs e)
        {
            this.LostFocusButton.Focus(FocusState.Pointer);
        }
        #endregion
    }

    public class SearchAvenues
    {
        public string Avenue { get; set; }
    }
}
