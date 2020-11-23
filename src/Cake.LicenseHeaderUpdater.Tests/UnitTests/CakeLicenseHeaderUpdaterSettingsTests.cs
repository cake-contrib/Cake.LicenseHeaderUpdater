//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//


using System;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.UnitTests
{
    [TestFixture]
    public class CakeLicenseHeaderUpdaterSettingsTests
    {
        // ---------------- Tests ----------------

        [Test]
        public void DefaultSettingsTest()
        {
            CakeLicenseHeaderUpdaterSettings uut = new CakeLicenseHeaderUpdaterSettings();

            Assert.IsFalse( uut.DryRun );
            Assert.IsNull( uut.FileFilter );
            Assert.AreEqual( null, uut.LicenseString );
            Assert.AreEqual( 0, uut.OldHeaderRegexPatterns.Count );
            Assert.AreEqual( 1, uut.Threads );
            Assert.AreEqual( Verbosity.Normal, uut.Verbosity );
        }

        [Test]
        public void Fixup_OkaySettings()
        {
            CakeLicenseHeaderUpdaterSettings okaySettings = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = true,
                FileFilter = DefaultFileFilter,
                LicenseString = "Hello",
                Threads = 1,
                Verbosity = Verbosity.Diagnostic
            };

            okaySettings.OldHeaderRegexPatterns.Add( "World" );

            CakeLicenseHeaderUpdaterSettings fixedUp = CakeLicenseHeaderUpdaterRunner.FixupSettings( okaySettings );

            // Nothing should be different between the two, as our settings were okay.
            CompareSettings( okaySettings, fixedUp );
            Assert.AreNotSame( okaySettings, fixedUp );
        }

        [Test]
        public void Fixup_EmptyLicenseString()
        {
            CakeLicenseHeaderUpdaterSettings uut = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = true,
                FileFilter = DefaultFileFilter,
                LicenseString = string.Empty,
                Threads = 2,
                Verbosity = Verbosity.Quiet
            };
            uut.OldHeaderRegexPatterns.Add( "World" );

            CakeLicenseHeaderUpdaterSettings expectedSettings = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = uut.DryRun,
                FileFilter = uut.FileFilter,

                // Expect this to turn null after being fixed up.
                LicenseString = null,
                Threads = uut.Threads,
                Verbosity = uut.Verbosity
            };
            expectedSettings.OldHeaderRegexPatterns.Add( uut.OldHeaderRegexPatterns[0] );

            CakeLicenseHeaderUpdaterSettings fixedUp = CakeLicenseHeaderUpdaterRunner.FixupSettings( uut );

            CompareSettings( expectedSettings, fixedUp );
        }

        [Test]
        public void Fixup_NullAndEmptyOldRegexes()
        {
            CakeLicenseHeaderUpdaterSettings uut = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = false,
                FileFilter = DefaultFileFilter,
                LicenseString = "Hello",
                Threads = 3,
                Verbosity = Verbosity.Normal
            };
            uut.OldHeaderRegexPatterns.Add( "World" );
            uut.OldHeaderRegexPatterns.Add( null );
            uut.OldHeaderRegexPatterns.Add( string.Empty );

            CakeLicenseHeaderUpdaterSettings expectedSettings = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = uut.DryRun,
                FileFilter = uut.FileFilter,
                LicenseString = uut.LicenseString,
                Threads = uut.Threads,
                Verbosity = uut.Verbosity
            };
            expectedSettings.OldHeaderRegexPatterns.Add( uut.OldHeaderRegexPatterns[0] );
            // Null and empty string should be purged.

            CakeLicenseHeaderUpdaterSettings fixedUp = CakeLicenseHeaderUpdaterRunner.FixupSettings( uut );

            CompareSettings( expectedSettings, fixedUp );
        }

        [Test]
        public void Fixup_ZeroOrNegativeThreads()
        {
            CakeLicenseHeaderUpdaterSettings uut = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = false,
                FileFilter = DefaultFileFilter,
                LicenseString = "Hello",
                Threads = 0,
                Verbosity = Verbosity.Normal
            };
            uut.OldHeaderRegexPatterns.Add( "World" );

            CakeLicenseHeaderUpdaterSettings expectedSettings = new CakeLicenseHeaderUpdaterSettings
            {
                DryRun = uut.DryRun,
                FileFilter = uut.FileFilter,
                LicenseString = uut.LicenseString,
                Threads = Environment.ProcessorCount,
                Verbosity = uut.Verbosity
            };
            expectedSettings.OldHeaderRegexPatterns.Add( uut.OldHeaderRegexPatterns[0] );
            // Null and empty string should be purged.

            {
                CakeLicenseHeaderUpdaterSettings fixedUp = CakeLicenseHeaderUpdaterRunner.FixupSettings( uut );
                CompareSettings( expectedSettings, fixedUp );
            }

            // While we are here, check if we are negative as well.
            uut.Threads = -1;

            {
                CakeLicenseHeaderUpdaterSettings fixedUp = CakeLicenseHeaderUpdaterRunner.FixupSettings( uut );
                CompareSettings( expectedSettings, fixedUp );
            }
        }

        // ---------------- Test Helpers ----------------

        private static void CompareSettings( CakeLicenseHeaderUpdaterSettings expected, CakeLicenseHeaderUpdaterSettings actual )
        {
            Assert.AreEqual( expected.DryRun, actual.DryRun );
            Assert.AreSame( expected.FileFilter, actual.FileFilter );
            Assert.AreEqual( expected.LicenseString, actual.LicenseString );
            Assert.AreEqual( expected.OldHeaderRegexPatterns.Count, actual.OldHeaderRegexPatterns.Count );
            for( int i = 0; i < expected.OldHeaderRegexPatterns.Count; ++i )
            {
                Assert.AreEqual( expected.OldHeaderRegexPatterns[i], actual.OldHeaderRegexPatterns[i] );
            }
            Assert.AreEqual( expected.Threads, actual.Threads );
            Assert.AreEqual( expected.Verbosity, actual.Verbosity );
        }

        private static bool DefaultFileFilter( FilePath f )
        {
            return true;
        }
    }
}
