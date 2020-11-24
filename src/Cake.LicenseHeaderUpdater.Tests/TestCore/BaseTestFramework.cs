//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.TestCore
{
    public class BaseTestFramework
    {
        // ---------------- Constructor ----------------

        public BaseTestFramework()
        {
        }

        // ---------------- Setup / Teardown ----------------

        public void DoTestSetup()
        {
            if( System.IO.Directory.Exists( TestConstants.TestDirectory.ToString() ) == false )
            {
                System.IO.Directory.CreateDirectory( TestConstants.TestDirectory.ToString() );
            }

            if( System.IO.File.Exists( TestConstants.FileToModify.ToString() ) )
            {
                System.IO.File.Delete( TestConstants.FileToModify.ToString() );
            }
        }

        public void DoTestTeardown()
        {
            if( System.IO.File.Exists( TestConstants.FileToModify.ToString() ) )
            {
                System.IO.File.Delete( TestConstants.FileToModify.ToString() );
            }
        }

        // ---------------- Tests ----------------

        public ModifyHeaderResult DoModifyHeaderTest( string originalFile, string expectedFile, CakeLicenseHeaderUpdaterSettings settings )
        {
            System.IO.File.WriteAllText(
                TestConstants.FileToModify.ToString(),
                originalFile
            );

            string stdOut = TaskMain.RunTask( TestConstants.FileToModify.ToString(), settings );

            string modifiedFile = System.IO.File.ReadAllText( TestConstants.FileToModify.ToString() );
            Assert.AreEqual( expectedFile, modifiedFile );

            return new ModifyHeaderResult( stdOut );
        }
    }
}
