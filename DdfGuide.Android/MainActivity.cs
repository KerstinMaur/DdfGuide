﻿using System;
using Android.App;
using Android.Content.PM;
using Android.Content.Res;
using Android.OS;
using DdfGuide.Core;

namespace DdfGuide.Android
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation)]
    public class MainActivity : Activity, IRootView
    {
        private Core.DdfGuide _ddfGuide;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
        }

        protected override void OnStart()
        {
            base.OnStart();

            var dtoProvider = new AndroidDtoProvider();
            var userDataProvider = new AndroidUserDataProvider();

            var audioDramaListView = new AudioDramaListView();
            var audioDramaView = new AudioDramaView();
            var rootView = this;

            _ddfGuide = new Core.DdfGuide(
                dtoProvider,
                userDataProvider,
                audioDramaListView,
                audioDramaView,
                rootView);

            _ddfGuide.Start();
        }

        public override void OnBackPressed()
        {
            BackClicked?.Invoke(this, EventArgs.Empty);
        }

        public void Show(IView view)
        {
            if (!(view is Fragment fragment))
            {
               throw new Exception("View needs to be of type Android.Views.View");
            }

            var transaction = FragmentManager.BeginTransaction();
            transaction.Replace(Resource.Id.rootLayout, fragment);
            transaction.Commit();
            FragmentManager.ExecutePendingTransactions();
        }

        public event EventHandler ViewDestroyed;

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);

            ViewDestroyed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler BackClicked;
    }
}