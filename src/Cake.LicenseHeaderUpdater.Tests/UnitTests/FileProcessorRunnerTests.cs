//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Concurrent;
using System.Threading;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Moq;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.UnitTests
{
    [TestFixture]
    public class FileProcessorRunnerTests
    {
        // ---------------- Fields ----------------

        private Mock<ICakeLog> mockLog;

        private ConcurrentQueue<FilePath> files;

        private CakeLicenseHeaderUpdaterSettings settings;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            // Don't care about calling the mock.
            this.mockLog = new Mock<ICakeLog>( MockBehavior.Loose );

            this.files = new ConcurrentQueue<FilePath>();

            this.settings = new CakeLicenseHeaderUpdaterSettings();
        }

        [TearDown]
        public void TestTeardown()
        {
        }

        // ---------------- Tests ----------------

        [Test]
        public void StartTwiceTest()
        {
            using( FileProcessorRunner uut = new FileProcessorRunner( 1, this.mockLog.Object, this.files, this.settings ) )
            {
                uut.Start();
                Assert.Throws<InvalidOperationException>( () => uut.Start() );
                uut.Wait();
            }
        }

        [Test]
        public void WaitBeforeStartTest()
        {
            using( FileProcessorRunner uut = new FileProcessorRunner( 1, this.mockLog.Object, this.files, this.settings ) )
            {
                Assert.Throws<InvalidOperationException>( () => uut.Wait() );
                uut.Start();
                uut.Wait();
            }
        }

        [Test]
        public void DisposeWithoutStartingTest()
        {
            using( FileProcessorRunner uut = new FileProcessorRunner( 1, this.mockLog.Object, this.files, this.settings ) )
            {
                Thread.Sleep( 0 );
            }
        }
    }
}
