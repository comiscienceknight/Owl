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

            InitAutoTextComplete();
            this.Loaded += FirstTimeEnterUserControl_Loaded;
        }

        void FirstTimeEnterUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //AppBarButton_Next.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private async void AppBarButton_Next_Click(object sender, RoutedEventArgs e)
        {
            if (FlipView_Profile.SelectedIndex == 0)
            {
                if (AppBarButton_Next.Visibility == Windows.UI.Xaml.Visibility.Visible)
                {
                    FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
                }
                else
                {
                    var dialog = new MessageDialog("Pleas pick a profile photo");
                    dialog.Commands.Add(new UICommand("OK"));
                    await dialog.ShowAsync();
                }
            }
            else
            {
                if (FlipView_Profile.SelectedIndex == FlipView_Profile.Items.Count - 1)
                {
                    if (GuideFinished != null)
                        GuideFinished(this, null);
                }
                else
                    FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
            }
            //AppBarButton_Next.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void Image_Profile_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if(TakePhotoClick != null)
            {
                TakePhotoClick(this, null);
            }
        }

        public void ChangeImageProfile(RenderTargetBitmap bmp)
        {
            Image_Profile.Source = bmp;
            if (bmp != null)
            {
                //AppBarButton_Next.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
        }

        private void AppBarButton_Back_Click(object sender, RoutedEventArgs e)
        {
            if (FlipView_Profile.SelectedIndex > 0)
            {
                FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex - 1;
            }
        }

        private void Image_Boy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock_HiSex.Text = "3, Hi Man! Where do you want?";
            _maleOrFemale = true;
            FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
        }

        private void Image_Girls_Tapped(object sender, TappedRoutedEventArgs e)
        {
            TextBlock_HiSex.Text = "3, Hi Girl! Where do you want?";
            _maleOrFemale = false;
            FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
            //AppBarButton_Next.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
            //this.ProgressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        private void WebServiceProvider_InputChanged(object sender, EventArgs e)
        {
            string inputString = _provider.InputString;
            if (!string.IsNullOrEmpty(inputString))
            {
                this.ProgressBar.Visibility = Visibility.Visible;
                var items = MockAveues.Where(p=> p.Avenue.Contains(inputString));
                if(items == null || items.Count() == 0)
                {
                    items = MockAveues.OrderBy(p=>p.Avenue).Take(10);
                }
                _provider.LoadItems(items.OrderBy(p=>p.Avenue));
                this.ProgressBar.Visibility = Visibility.Collapsed;
                //AppBarButton_Next.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            else
            {
                _provider.Reset();
                //AppBarButton_Next.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
