//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System.Text.RegularExpressions;
using Cake.LicenseHeaderUpdater.Tests.TestCore;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.IntegrationTests
{
    [TestFixture]
    public class MissingHeaderTests
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
        public void MissingHeaderWithNewLicenseSpecifiedTest()
        {
            const string originalFile =
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

            const string licenseString =
@"//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

";

            string expectedFile =

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

            CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings
            {
                LicenseString = licenseString
            };

            // Nothing should happen, even if specified.  There are no old licenses hanging around.
            settings.OldHeaderRegexPatterns.Add( Regex.Escape( licenseString ) );

            ModifyHeaderResult result = this.testFrame.DoModifyHeaderTest( originalFile, expectedFile, settings );
            result.WasSuccess( true, false );
        }

        [Test]
        public void MissingHeaderWithNoNewLicenseSpecifiedTest()
        {
            const string originalFile =
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
                // Expect no changes if this isn't specified.
                LicenseString = null
            };

            ModifyHeaderResult result = this.testFrame.DoModifyHeaderTest( originalFile, originalFile, settings );
            result.WasSuccess( false, false );
        }
    }
}
