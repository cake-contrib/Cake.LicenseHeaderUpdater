//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.TestCore
{
    public class ModifyHeaderResult
    {
        // ---------------- Fields ----------------

        private static readonly Regex startedProcessingFileRegex = new Regex(
            @"\d+\>\s+Begin\s+Processing\s+'.+'",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase
        );

        private static readonly Regex removedOldLicenseHeaders = new Regex(
            @"\d+\>\s+Removed\s+old\s+license\s+headers\s+in\s+'.+'",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase
        );

        private static readonly Regex addedNewHeaderRegex = new Regex(
            @"\d+\>\s+Added\s+new\s+header\s+in\s+'.+'",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase
        );

        private static readonly Regex finishedProcessingFileRegex = new Regex(
            @"\d+\>\s+Finished\s+Processing\s+'.+'",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase
        );

        private static readonly Regex doneProcessingRegex = new Regex(
            @"\d+\>\s+Done\s+processing",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase
        );

        private static readonly Regex threadExitingRegex = new Regex(
            @"\d+\>\s+Thread\s+Exiting",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.IgnoreCase
        );

        // ---------------- Constructor ----------------

        public ModifyHeaderResult( string consoleOut )
        {
            this.StartingProcessingFile = false;
            this.RemovedOldLicenseHeaders = false;
            this.AddedNewHeader = false;
            this.FinishedProcessingFile = false;
            this.FinishedProcesssing = false;
            this.ThreadExited = false;

            using( StringReader reader = new StringReader( consoleOut ) )
            {
                string line = reader.ReadLine();
                while( line != null )
                {
                    if( startedProcessingFileRegex.IsMatch( line ) )
                    {
                        this.StartingProcessingFile = true;
                    }
                    else if( removedOldLicenseHeaders.IsMatch( line ) )
                    {
                        this.RemovedOldLicenseHeaders = true;
                    }
                    else if( addedNewHeaderRegex.IsMatch( line ) )
                    {
                        this.AddedNewHeader = true;
                    }
                    else if( finishedProcessingFileRegex.IsMatch( line ) )
                    {
                        this.FinishedProcessingFile = true;
                    }
                    else if( doneProcessingRegex.IsMatch( line ) )
                    {
                        this.FinishedProcesssing = true;
                    }
                    else if( threadExitingRegex.IsMatch( line ) )
                    {
                        this.ThreadExited = true;
                    }

                    line = reader.ReadLine();
                }
            }
        }

        // ---------------- Properties ----------------

        public bool StartingProcessingFile { get; private set; }

        public bool RemovedOldLicenseHeaders { get; private set; }

        public bool AddedNewHeader { get; private set; }

        public bool FinishedProcessingFile { get; private set; }

        public bool FinishedProcesssing { get; private set; }

        public bool ThreadExited { get; private set; }
    }

    public static class ModifyHeaderResultExtensions
    {
        public static void WasSuccess( this ModifyHeaderResult result, bool expectedToAddHeader, bool expectedToRemoveOldHeaders )
        {
            Assert.IsTrue( result.StartingProcessingFile );
            Assert.AreEqual( expectedToRemoveOldHeaders, result.RemovedOldLicenseHeaders );
            Assert.AreEqual( expectedToAddHeader, result.AddedNewHeader );
            Assert.IsTrue( result.FinishedProcessingFile );
            Assert.IsTrue( result.FinishedProcesssing );
            Assert.IsTrue( result.ThreadExited );
        }
    }
}
