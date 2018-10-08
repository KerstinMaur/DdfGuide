﻿using System;
using System.Collections.Generic;
using System.Linq;
using DdfGuide.Core.Filtering;
using DdfGuide.Core.Sorting;

namespace DdfGuide.Core
{
    public class AudioDramaListPresenter
    {
        private readonly IAudioDramaListView _audioDramaListView;
        private readonly IAudioDramaView _audioDramaView;
        private readonly IEnumerable<AudioDrama> _audioDramas;
        private readonly IViewer _viewer;
        private readonly IAudioDramaPresenter _audioDramaPresenter;
        private readonly IAudioDramaFilterFactory _audioDramaFilterFactory;
        private readonly IAudioDramaSorterFactory _audioDramaSorterFactory;

        private IAudioDramaSorter _audioDramaSorter;
        private IAudioDramaFilter _audioDramaFilter;

        public AudioDramaListPresenter(
            IAudioDramaListView audioDramaListView, 
            IAudioDramaView audioDramaView,
            IEnumerable<AudioDrama> audioDramas,
            IViewer viewer, 
            IAudioDramaPresenter audioDramaPresenter,
            IAudioDramaFilterFactory audioDramaFilterFactory,
            IAudioDramaSorterFactory audioDramaSorterFactory)
        {
            _audioDramaListView = audioDramaListView;
            _audioDramaView = audioDramaView;
            _audioDramas = audioDramas.ToList();
            _viewer = viewer;
            _audioDramaPresenter = audioDramaPresenter;
            
            _audioDramaSorterFactory = audioDramaSorterFactory;
            _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.ReleaseDateDescending);

            _audioDramaFilterFactory = audioDramaFilterFactory;
            _audioDramaFilter = _audioDramaFilterFactory.Create(EAudioDramaFilterMode.AllAudioDramas);

            _audioDramaListView.HeardChanged += OnHeardChanged();
            _audioDramaListView.IsFavoriteChanged += OnIsFavoriteChanged();
            _audioDramaListView.AudioDramaClicked += OnAudioDramaClicked();
            _audioDramaListView.OrderByHeardFirstClicked += OnOrderByHeardFirstClicked();
            _audioDramaListView.OrderByHeardLastClicked += OnOrderByHeardLastClicked();
            _audioDramaListView.OrderByIsFavoriteFirstClicked += OnOrderByIsFavoriteFirstClicked();
            _audioDramaListView.OrderByIsFavoriteLastClicked += OnOrderByIsFavoriteLastClicked();
            _audioDramaListView.OrderByNumberAscendingClicked += OnOrderByNumberAscendingClicked();
            _audioDramaListView.OrderByNumberDescendingClicked += OnOrderByNumberDescendingClicked();
            _audioDramaListView.OrderByReleaseDateAscendingClicked += OnOrderByReleaseDateAscendingClicked();
            _audioDramaListView.OrderByReleaseDateDescendingClicked += OnOrderByReleaseDateDescendingClicked();
            _audioDramaListView.OrderByNameAscendingClicked += OnOrderByNameAscendingClicked();
            _audioDramaListView.OrderByNameDescendingClicked += OnOrderByNameDescendingClicked();

            _audioDramaListView.AllAudioDramasClicked += OnAllAudioDramasClicked();
            _audioDramaListView.MainAudioDramasOnlyClicked += OnMainAudioDramasOnlyClicked();
            _audioDramaListView.FavoritesOnlyClicked += OnFavoritesOnlyClicked();
            _audioDramaListView.UnheardOnlyClicked += OnUnheardOnlyClicked();

            foreach (var audioDrama in _audioDramas)
            {
                audioDrama.AudioDramaUserData.Changed += OnUserDataChanged();
            }
            
            UpdateView();
        }

        private EventHandler OnUnheardOnlyClicked()
        {
            return (sender, args) =>
            {
                _audioDramaFilter = _audioDramaFilterFactory.Create(EAudioDramaFilterMode.UnheardOnly);
                UpdateView();
            };
        }

        private EventHandler OnFavoritesOnlyClicked()
        {
            return (sender, args) =>
            {
                _audioDramaFilter = _audioDramaFilterFactory.Create(EAudioDramaFilterMode.FavoritesOnly);
                UpdateView();
            };
        }

        private EventHandler OnAllAudioDramasClicked()
        {
            return (sender, args) =>
            {
                _audioDramaFilter = _audioDramaFilterFactory.Create(EAudioDramaFilterMode.AllAudioDramas);
                UpdateView();
            };
        }

        private EventHandler OnMainAudioDramasOnlyClicked()
        {
            return (sender, args) =>
            {
                _audioDramaFilter = _audioDramaFilterFactory.Create(EAudioDramaFilterMode.MainAudioDramasOnly);
                UpdateView();
            };
        }

        private EventHandler OnOrderByNameDescendingClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.NameDescending);
                UpdateView();
            };
        }

        private EventHandler OnOrderByNameAscendingClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.NameAscending);
                UpdateView();
            };
        }

        private EventHandler OnOrderByReleaseDateDescendingClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.ReleaseDateDescending);
                UpdateView();
            };
        }

        private EventHandler OnOrderByReleaseDateAscendingClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.ReleaseDateAscending);
                UpdateView();
            };
        }

        private EventHandler OnOrderByNumberDescendingClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.NumberDescending);
                UpdateView();
            };
        }

        private EventHandler OnOrderByNumberAscendingClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.NumberAscending);
                UpdateView();
            };
        }

        private EventHandler OnOrderByIsFavoriteLastClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.IsFavoriteLast);
                UpdateView();
            };
        }

        private EventHandler OnOrderByIsFavoriteFirstClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.IsFavoriteFirst);
                UpdateView();
            };
        }

        private EventHandler OnOrderByHeardLastClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.HeardLast);
                UpdateView();
            };
        }

        private EventHandler OnOrderByHeardFirstClicked()
        {
            return (sender, args) =>
            {
                _audioDramaSorter = _audioDramaSorterFactory.Create(EAudioDramaSortMode.HeardFirst);
                UpdateView();
            };
        }

        private EventHandler OnUserDataChanged()
        {
            return (sender, args) => { UpdateView(); };
        }

        private EventHandler<Guid> OnAudioDramaClicked()
        {
            return (sender, id) =>
            {
                var audioDrama = _audioDramas.Single(x => x.AudioDramaDto.Id == id);
                _audioDramaPresenter.SetAudioDrama(audioDrama);

                _viewer.Show(_audioDramaView);
            };
        }

        private EventHandler<Guid> OnIsFavoriteChanged()
        {
            return (sender, id) =>
            {
                var audioDrama = _audioDramas.Single(x => x.AudioDramaDto.Id == id);
                audioDrama.AudioDramaUserData.IsFavorite = !audioDrama.AudioDramaUserData.IsFavorite;
            };
        }

        private EventHandler<Guid> OnHeardChanged()
        {
            return (sender, id) =>
            {
                var audioDrama = _audioDramas.Single(x => x.AudioDramaDto.Id == id);
                audioDrama.AudioDramaUserData.Heard = !audioDrama.AudioDramaUserData.Heard;
            };
        }

        private void UpdateView()
        {
            var filtered = _audioDramaFilter.Filter(_audioDramas);
            var filteredAndSorted = _audioDramaSorter.Sort(filtered);

            _audioDramaListView.SetAudioDramaInfos(filteredAndSorted);

            _audioDramaListView.SetFilterInfos(_audioDramaFilter.FilterMode);
            _audioDramaListView.SetSelectedSortMode(_audioDramaSorter.SortMode);
        }
    }
}
