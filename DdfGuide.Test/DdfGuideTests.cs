﻿using System.Collections.Generic;
using DdfGuide.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DdfGuide.Test
{
    [TestClass]
    public class DdfGuideTests
    {
        private Core.DdfGuide _systemUnderTest;

        private Mock<IAudioDramaListView> _listView;
        private Mock<IProvider<IEnumerable<AudioDramaDto>>> _dtoProvider;
        private Mock<IProvider<IEnumerable<AudioDramaUserData>>> _userDataProvider;
        private Mock<IAudioDramaView> _view;
        private Mock<IRootView> _rootView;

        [TestInitialize]
        public void CreateNewDdfGuide()
        {
            _dtoProvider = new Mock<IProvider<IEnumerable<AudioDramaDto>>>();
            _userDataProvider = new Mock<IProvider<IEnumerable<AudioDramaUserData>>>();
            _listView = new Mock<IAudioDramaListView>();
            _view = new Mock<IAudioDramaView>();
            _rootView = new Mock<IRootView>();

            _systemUnderTest = new Core.DdfGuide(
                _dtoProvider.Object,
                _userDataProvider.Object,
                _listView.Object,
                _view.Object,
                _rootView.Object);
        }

        [TestMethod]
        public void CreationSuccessfull()
        {
            Assert.IsNotNull(_systemUnderTest);
        }

        [TestMethod]
        public void Startup_ShowAudioDramaListView()
        {
            _systemUnderTest.Start();

            _rootView.Verify(x => x.Show(It.IsAny<IAudioDramaListView>()));
        }
    }
}
