using System;
using FreshMvvm;
using LeaveTracker.PageModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LeaveTracker
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = FreshPageModelResolver.ResolvePageModel<LeaveTrackerBoardPageModel>();
            var basicNavContainer = new FreshNavigationContainer(page);
            MainPage = basicNavContainer;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
