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
using Windows.UI.Xaml.Media.Animation;
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

        private double _unitOffset = 0;

        public FirstTimeEnterUserControl()
        {
            this.InitializeComponent();

            this.Loaded += FirstTimeEnterUserControl_Loaded;
        }

        void FirstTimeEnterUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            InitAutoTextComplete();

            _unitOffset = Window.Current.Bounds.Width;
            ScrollViewer_Main.Height = Window.Current.Bounds.Width;
            StackPanel_ScrollViewer.Height = Window.Current.Bounds.Width;
            StackPanel_ScrollViewer.Width = Window.Current.Bounds.Width * 6 - 20; 
            Grid_PickProfile.Width = Window.Current.Bounds.Width - 20;
            Grid_Iam.Width = Grid_PickProfile.Width;
            StackPanel_AgeName.Width = Grid_PickProfile.Width;
            Grid_SearchVenue.Width = Grid_PickProfile.Width;
            Grid_NumberOfGroup.Width = Grid_PickProfile.Width;
            Grid_Description.Width = Grid_PickProfile.Width;

            ScrollViewer_Main.IsDeferredScrollingEnabled = true;

            ListPickerFlyout_AgeRange.ItemsSource = new List<string>() { "avcdeff", "abfdfe" };
            ListPickerFlyout_Description.ItemsSource = new List<string>() { 
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink", 
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink",
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink",
                "Hi, I wanna have fun and go to some night club. But i don't have wings. Any girl can take me into the bar or club, i'm glad to invite her a drink"
            };

            ScrollViewer_Main.ViewChanged += ScrollViewer_Main_ViewChanged;
        }

        void ScrollViewer_Main_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if((int)(ScrollViewer_Main.HorizontalOffset / _unitOffset) == 5)
            {
                TextBlock_Indication.Text = "Start";
                Rectangle_Indication.Opacity = 1;
            }
        }

        private async void TextBlock_Indication_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                //FlipView_Profile.SelectedIndex = FlipView_Profile.SelectedIndex + 1;
                TextBlock_Indication.Text = "";
                Rectangle_Indication.Opacity = 0;
                TextBlock_VenueName.Text = RadAutoCompleteBox_Search.Text;
                //ScrollViewer_Main.ScrollToHorizontalOffset(ScrollViewer_Main.HorizontalOffset + Grid_PickProfile.Width + 20);
                ScrollToNext();
               
            });
        }

        private void ScrollToNext()
        {
            int offsetBrick = (int)(ScrollViewer_Main.HorizontalOffset / _unitOffset);
            ScrollViewer_Main.ChangeView((offsetBrick % 6) * _unitOffset + _unitOffset, null, null, false);
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
                ScrollToNext();
            });
        }

        private async void Image_Girls_Tapped(object sender, TappedRoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_HiSex.Text = "Hi Lady!";
                _maleOrFemale = false;
                ScrollToNext();
            });
        }

        private async void NumericUpDown_WithBoys_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if(e.NewValue > 0)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
                });
            }
        }

        private async void NumericUpDown_WithGirls_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (e.NewValue > 0)
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    TextBlock_Indication.Text = "Next";
                    Rectangle_Indication.Opacity = 1;
                });
            }
        }

        private async void TextBox_Description_TextChanged(object sender, RoutedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (ScrollViewer_Main.HorizontalOffset > 2000)
                {
                    TextBlock_Indication.Text = "Start";
                    Rectangle_Indication.Opacity = 1;
                }
            });
        }

        private async void TextBox_NickName_TextChanged(object sender, TextChangedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_Indication.Text = "Next";
                Rectangle_Indication.Opacity = 1;
            });
        }

        private async void TextBox_AgeRange_TextChanged(object sender, TextChangedEventArgs e)
        {
            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                TextBlock_Indication.Text = "Next";
                Rectangle_Indication.Opacity = 1;
            });
        }

        #region avenues search
        private WebServiceTextSearchProvider _provider;
        public List<SearchAvenues> MockAveues = new List<SearchAvenues>() { 
            new SearchAvenues(){ Avenue = "White Room", Adresse = "15 Avenue Montaigne, 75008 Paris, France"},
            new SearchAvenues(){ Avenue = "Café Oz Châtelet", Adresse = "18 Rue Saint-Denis, 75001, Paris"},
            new SearchAvenues(){ Avenue = "Café Oz Denfert Rochereau", Adresse = "3 Place Denfert-Rochereau, 75014, Paris"},
            new SearchAvenues(){ Avenue = "Café Oz Blanche", Adresse = "1 Rue de Bruxelles, 75009, Paris"},
            new SearchAvenues(){ Avenue = "Café Oz Grands Boulevards", Adresse = "8 Boulevard Montmartre, 75009, Paris"},
            new SearchAvenues(){ Avenue = "Club Queen", Adresse = "102 Avenue des Champs-Élysées, 75008, Paris"},
            new SearchAvenues(){ Avenue = "Le Showcase", Adresse = "Sous le Pont Alexandre III, Port des Champs Élysées, 75008 , Paris"},
            new SearchAvenues(){ Avenue = "Club 79", Adresse = "22 Rue Quentin-Bauchart, 75008, Paris"},
            new SearchAvenues(){ Avenue = "Mix Club", Adresse = "24 Rue de L'arrivée, 75015, PARIS"},
            new SearchAvenues(){ Avenue = "L'Arc", Adresse = "12 Rue de Presbourg, 75016, Paris"},
            new SearchAvenues(){ Avenue = "Matignon", Adresse = "Unknown"},
            new SearchAvenues(){ Avenue = "Black Calvados", Adresse = "40 Avenue Pierre 1er de Serbie, 75008, Paris"},
            new SearchAvenues(){ Avenue = "Le Buddah Bar", Adresse = "8 Rue Boissy d’Anglas, 75008, Paris"},
            new SearchAvenues(){ Avenue = "O'Sullivans", Adresse = "Unknown"},
            new SearchAvenues(){ Avenue = "Le Duplex", Adresse = "Unknown"},
            new SearchAvenues(){ Avenue = "VIP Room", Adresse = "Unknown"},
            new SearchAvenues(){ Avenue = "OMantra", Adresse = "Unknown"},
            new SearchAvenues(){ Avenue = "Chez Raspoutine", Adresse = "Unknown"},
            new SearchAvenues(){ Avenue = "Le Baron", Adresse = "Unknown"}
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
        public string Adresse { get; set; }
    }
}
