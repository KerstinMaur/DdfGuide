﻿using System;
using System.Linq;
using Android.App;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using DdfGuide.Core;
using FFImageLoading.Views;

namespace DdfGuide.Android
{
    public class AudioDramaView : Fragment, IAudioDramaView
    {
        private readonly IImageViewFiller _imageViewFiller;
        private View _view;

        public AudioDramaView(IImageViewFiller imageViewFiller)
        {
            _imageViewFiller = imageViewFiller;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _view = inflater.Inflate(Resource.Layout.audiodramalayout, container, false);

            SetupToolbar();
            SetupHeardCheckbox();
            SetupIsFavoriteCheckbox();

            return _view;
        }

        private void SetupIsFavoriteCheckbox()
        {
            var checkBoxIsFavorite = _view.FindViewById<CheckBox>(Resource.Id.checkboxfavorite);
            checkBoxIsFavorite.Click += (sender, args) => IsFavoriteClicked?.Invoke(this, EventArgs.Empty);
        }

        private void SetupHeardCheckbox()
        {
            var checkBoxHeard = _view.FindViewById<CheckBox>(Resource.Id.checkboxheard);
            checkBoxHeard.Click += (sender, args) => { HeardClicked?.Invoke(this, EventArgs.Empty); };
        }

        private void SetupToolbar()
        {
            var toolbar = _view.FindViewById<Toolbar>(Resource.Id.toolbarsingleview);
            toolbar.SetNavigationIcon(Resource.Mipmap.arrow_back);
            toolbar.NavigationOnClick += (sender, args) => BackClicked?.Invoke(this, EventArgs.Empty);
        }

        public void SetAudioDrama(AudioDrama audioDrama)
        {
            var interpreterView = _view.FindViewById<TextView>(Resource.Id.textviewinterpreter);
            interpreterView.Text = audioDrama.AudioDramaDto.Interpreter;

            var titleView = _view.FindViewById<TextView>(Resource.Id.textviewtitelsingle);
            titleView.Text = audioDrama.AudioDramaDto.ToString();

            var descriptionView = _view.FindViewById<TextView>(Resource.Id.textViewDescription);
            descriptionView.Text = string.Empty;
            if (audioDrama.AudioDramaDto.Description != null)
            {
                descriptionView.Append(Html.FromHtml(audioDrama.AudioDramaDto.Description, FromHtmlOptions.ModeLegacy));
            }

            var releaseDateView = _view.FindViewById<TextView>(Resource.Id.textviewreleasesingle);
            releaseDateView.Text = audioDrama.AudioDramaDto.ReleaseDate.ToShortDateString();

            var heardCheckbox = _view.FindViewById<CheckBox>(Resource.Id.checkboxheard);
            heardCheckbox.Checked = audioDrama.AudioDramaUserData.Heard;

            var favoriteCheckbox = _view.FindViewById<CheckBox>(Resource.Id.checkboxfavorite);
            favoriteCheckbox.Checked = audioDrama.AudioDramaUserData.IsFavorite;

            var imageView = _view.FindViewById<ImageViewAsync>(Resource.Id.coverviewsingle);

            _imageViewFiller.FillImageViewFromUrl(imageView, audioDrama.AudioDramaDto.CoverUrl);
           
            var rolesView = _view.FindViewById<TextView>(Resource.Id.textViewRoles);

            rolesView.Text = string.Empty;
            foreach (var roleDto in audioDrama.AudioDramaDto.Roles)
            {
                rolesView.Append(Html.FromHtml($"<b>{roleDto.Character}:</b><br>", FromHtmlOptions.ModeLegacy));
                rolesView.Append(Html.FromHtml($"{roleDto.Speaker}", FromHtmlOptions.ModeLegacy));

                if (roleDto != audioDrama.AudioDramaDto.Roles.Last())
                {
                    rolesView.Append(Html.FromHtml("<br><br>", FromHtmlOptions.ModeLegacy));
                }
            }
        }
        
        public event EventHandler BackClicked;
        public event EventHandler HeardClicked;
        public event EventHandler IsFavoriteClicked;
    }
}