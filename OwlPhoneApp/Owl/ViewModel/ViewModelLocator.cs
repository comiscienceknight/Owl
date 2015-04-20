/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Owl"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace Owl.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<PivotViewModel>();
            SimpleIoc.Default.Register<PostsViewModel>();
            SimpleIoc.Default.Register<ChatHistoryViewModel>();
        }

        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        public PivotViewModel Pivot
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PivotViewModel>();
            }
        }

        public PostsViewModel Posts
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PostsViewModel>();
            }
        }

        public ChatHistoryViewModel Chats
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ChatHistoryViewModel>();
            }
        }
        
        

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}