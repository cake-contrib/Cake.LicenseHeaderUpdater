//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System.Text.RegularExpressions;
using Cake.Core.IO;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.TestCore
{
    public static class TestConstants
    {
        // ---------------- Fields ----------------

        public static readonly DirectoryPath TestDirectory;

        public static readonly FilePath FileToModify;

        public static readonly string MultilineLicenseString;

        public static readonly string RegexEscapedMultilineLicenseString;

        // ---------------- Constructor ----------------

        static TestConstants()
        {
            TestDirectory = new DirectoryPath( TestContext.CurrentContext.TestDirectory );
            TestDirectory = TestDirectory.Combine( new DirectoryPath( "TestOutput" ) );
            FileToModify = TestDirectory.CombineWithFilePath( new FilePath( "uut.cs" ) );

            MultilineLicenseString =
@"//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

";

            RegexEscapedMultilineLicenseString = Regex.Escape( MultilineLicenseString );
        }
    }
}
