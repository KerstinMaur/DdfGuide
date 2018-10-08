﻿using System.Linq;
using DdfGuide.Core;
using DdfGuide.Core.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DdfGuide.Test.Sorting
{
    [TestClass]
    public class ReleaseDateAscendingSorterTests
    {
        [TestMethod]
        public void CorrectMode()
        {
            var sorter = new ReleaseDateAscendingSorter();
            Assert.AreEqual(EAudioDramaSortMode.ReleaseDateAscending, sorter.SortMode);
        }

        [TestMethod]
        public void SortByReleaseDateAscending()
        {
            var provider = new MultipleAudioDramaProvider();
            var sorter = new ReleaseDateAscendingSorter();
            var audioDramas = provider.Get().ToList();
            var expectedSort = audioDramas.OrderBy(x => x.AudioDramaDto.ReleaseDate).ToList();

            var sort = sorter.Sort(audioDramas).ToList();

            CollectionAssert.AreEqual(expectedSort, sort);
        }
    }
}
