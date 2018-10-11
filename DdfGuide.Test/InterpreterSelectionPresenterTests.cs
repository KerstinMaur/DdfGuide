﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DdfGuide.Core;
using DdfGuide.Core.Filtering;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.AutoMock;

namespace DdfGuide.Test
{
    [TestClass]
    public class InterpreterSelectionPresenterTests
    {
        [TestMethod]
        public void OnDieDreiFragezeichenClicked_UpdateListPresenterWithCorrectAudioDramas_ShowListView()
        {
            var mocker = new AutoMocker();
            var sut = mocker.CreateInstance<InterpreterSelectionPresenter>();

            mocker
                .Setup<IAudioDramaFilterFactory, IAudioDramaFilter>(x =>
                    x.Create(EAudioDramaFilterMode.DieDreiFragezeichen))
                .Returns(mocker.Get<IAudioDramaFilter>());

            var view = mocker.GetMock<IInterpreterSelectionView>();
            view.Raise(x => x.DieDreiFragezeichenClicked += null, this, EventArgs.Empty);

            mocker.Verify<IAudioDramaFilter>(x => x.Filter(It.IsAny<IEnumerable<AudioDrama>>()));
            mocker.Verify<IAudioDramaListPresenter>(x => x.SetAudioDramas(It.IsAny<IEnumerable<AudioDrama>>()));
            mocker.Verify<IViewer>(x => x.Show(It.IsAny<IAudioDramaListView>()));
        } 
    }
}
