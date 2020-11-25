//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Generic;
using System.Text;
using Cake.LicenseHeaderUpdater.Tests.TestCore;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.IntegrationTests
{
    [TestFixture]
    public class HeaderReplacementTests
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
        public void DateReplacementTest()
        {
            const string originalFile =
@"//
// Copyright Seth Hendrick 2019.
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
@"//
// Copyright Seth Hendrick 2019-2020.
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
            const string newLicense =

@"//
// Copyright Seth Hendrick 2019-2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

";

            const string oldLicenseRegex =
@"//
// Copyright Seth Hendrick \d+\.
// Distributed under the MIT License\.
// \(See accompanying file LICENSE in the root of the repository\)\.
//

";
            CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings
            {
                LicenseString = newLicense
            };

            // This should erase everything if dry run doesn't work.
            settings.OldHeaderRegexPatterns.Add( oldLicenseRegex );


            // Should be a successful run with the intent of the output to 
            // change everything, but nothing should actually change.
            ModifyHeaderResult result = this.testFrame.DoModifyHeaderTest( originalFile, expectedFile, settings );
            result.WasSuccess( true, true );
        }

        /// <summary>
        /// In the event VS is helpful and puts the license under the using statements.
        /// </summary>
        [Test]
        public void OldLicenseUnderUsingStatementsTest()
        {
            const string originalFile =
@"using System;
using System.Collections.Generic;
using System.Text;

///
/// Copyright Seth Hendrick 2019-2020.
/// Distributed under the MIT License.
/// (See accompanying file LICENSE in the root of the repository).
///

namespace Cake.LicenseHeaderUpdater.Tests.IntegrationTests
{
    class Class1
    {
    }
}
";
            const string expectedFile =
@"//
// Copyright Seth Hendrick 2019-2020.
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
            const string newLicense =

@"//
// Copyright Seth Hendrick 2019-2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

";

            const string oldLicenseRegex =
@"///?
///? Copyright Seth Hendrick 2019-2020\.
///? Distributed under the MIT License\.
///? \(See accompanying file LICENSE in the root of the repository\)\.
///?

";
            CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings
            {
                LicenseString = newLicense
            };

            // This should erase everything if dry run doesn't work.
            settings.OldHeaderRegexPatterns.Add( oldLicenseRegex );


            // Should be a successful run with the intent of the output to 
            // change everything, but nothing should actually change.
            ModifyHeaderResult result = this.testFrame.DoModifyHeaderTest( originalFile, expectedFile, settings );
            result.WasSuccess( true, true );
        }
    }
}
