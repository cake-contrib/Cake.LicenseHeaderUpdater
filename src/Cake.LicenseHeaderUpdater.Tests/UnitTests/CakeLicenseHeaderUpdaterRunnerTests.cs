//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Moq;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.UnitTests
{
    [TestFixture]
    public class CakeLicenseHeaderUpdaterRunnerTests
    {
        // ---------------- Fields ----------------

        private Mock<ICakeContext> mockContext;
        private List<FilePath> files;
        private CakeLicenseHeaderUpdaterSettings settings;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            // Don't care about calling the mock.
            this.mockContext = new Mock<ICakeContext>( MockBehavior.Loose );

            this.files = new List<FilePath>();

            this.settings = new CakeLicenseHeaderUpdaterSettings();
        }

        [TearDown]
        public void TestTeardown()
        {
        }

        // ---------------- Tests ----------------

        [Test]
        public void NullContextTest()
        {
            Assert.Throws<ArgumentNullException>(
                () => new CakeLicenseHeaderUpdaterRunner( null )
             );
        }

        [Test]
        public void NullFileListTest()
        {
            CakeLicenseHeaderUpdaterRunner uut = new CakeLicenseHeaderUpdaterRunner( this.mockContext.Object );
            Assert.Throws<ArgumentNullException>(
                () => uut.Run( null, this.settings )
            );
        }

        [Test]
        public void NullSettingsTest()
        {
            CakeLicenseHeaderUpdaterRunner uut = new CakeLicenseHeaderUpdaterRunner( this.mockContext.Object );
            Assert.Throws<ArgumentNullException>(
                () => uut.Run( this.files, null )
            );
        }
    }
}
