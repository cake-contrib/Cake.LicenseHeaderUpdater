//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using Cake.LicenseHeaderUpdater.Tests.TestCore;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.IntegrationTests
{
    [TestFixture]
    public class DryRunTests
    {
        // ---------------- Fields ----------------

        private BaseTestFramework testFrame;

        // ---------------- Setup / Teardown ----------------

        [SetUp]
        public void TestSetup()
        {
            this.testFrame = new BaseTestFramework();
            this.testFrame.DoTestSetup();
        }

        [TearDown]
        public void TestTeardown()
        {
            this.testFrame?.DoTestTeardown();
        }

        // ---------------- Tests ----------------

        [Test]
        public void DryRunTrueTest()
        {
            const string expectedFile =
@"using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.LicenseHeaderUpdater.Tests.IntegrationTests
{
    class Class1
    {
    }
}
";
            CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = true,
                LicenseString = "My License"
            };

            // This should erase everything if dry run doesn't work.
            settings.OldHeaderRegexPatterns.Add( ".*" );

            // Should be a successful run with the intent of the output to 
            // change everything, but nothing should actually change.
            ModifyHeaderResult result = this.testFrame.DoModifyHeaderTest( expectedFile, expectedFile, settings );
            result.WasSuccess( true, true );
        }
    }
}
