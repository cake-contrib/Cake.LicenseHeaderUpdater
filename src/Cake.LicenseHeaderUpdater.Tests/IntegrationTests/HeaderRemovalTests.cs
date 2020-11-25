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
    public class HeaderRemovalTests
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

        /// <summary>
        /// Not sure why one would *want* to remove a header,
        /// but we'll support it.
        /// </summary>
        [Test]
        public void RemoveHeaderTest()
        {
            const string originalFile =
@"//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Generic;
using System.Text;

namespace Cake.LicenseHeaderUpdater.Tests.IntegrationTests
{
    class Class1
    {
    }
}
";
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
            const string regex =
@"//
// Copyright Seth Hendrick 2020\.
// Distributed under the MIT License\.
// \(See accompanying file LICENSE in the root of the repository\)\.
//

";

            CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings
            {
                LicenseString = null
            };

            settings.OldHeaderRegexPatterns.Add( regex );


            ModifyHeaderResult result = this.testFrame.DoModifyHeaderTest( originalFile, expectedFile, settings );
            // No to adding a header, yes to replacing a header with nothing.
            result.WasSuccess( false, true );
        }
    }
}
