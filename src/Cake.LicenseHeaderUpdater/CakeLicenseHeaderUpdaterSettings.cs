//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.LicenseHeaderUpdater
{
    /// <summary>
    /// Configuration for updating the license headers
    /// in various projects.
    /// </summary>
    public class CakeLicenseHeaderUpdaterSettings
    {
        // ---------------- Constructor ----------------

        public CakeLicenseHeaderUpdaterSettings()
        {
            this.LicenseString = null;
            this.OldHeaderRegexPatterns = new List<string>();
            this.Threads = 1;
            this.FileFilter = null;
            this.Verbosity = Verbosity.Normal;
        }

        // ---------------- Properties ----------------

        /// <summary>
        /// The license string to add to the top of all
        /// the files.
        /// 
        /// If this configuration is null or empty,
        /// then all matches in <see cref="OldLicenseRegexes"/> will be purged,
        /// but nothing will be added to the top of the file.
        /// </summary>
        public string LicenseString { get; set; }

        /// <summary>
        /// If there are any "old" licenses that should be removed,
        /// add them to this list.  All matches in each file will be removed.
        /// If left empty, then it is possible for a file to contain an old license
        /// and a new license header, with the newer license at the top.
        /// 
        /// Nulls or empty strings added to this list are purged before running.
        /// </summary>
        public IList<string> OldHeaderRegexPatterns { get; private set; }

        /// <summary>
        /// The number of threads to spawn to process files.
        /// If this is set to 0 or less, then the number of threads will be equal to
        /// <see cref="Environment.ProcessorCount"/>.
        /// 
        /// Defaulted to 1.
        /// </summary>
        public int Threads { get; set; }

        /// <summary>
        /// Override this to filter files to process.
        /// 
        /// The function passes in a <see cref="FilePath"/>,
        /// and if we should add a header to the top of this file, have this function
        /// return true, otherwise have it return false.
        /// 
        /// If this is set to null, it will process ALL files.
        /// </summary>
        public Func<FilePath, bool> FileFilter { get; set; }

        /// <summary>
        /// Set to true to perform a dry run.
        /// 
        /// If set to true, this will simply print the files that will be processed,
        /// but nothing will happen otherwise.
        /// 
        /// Regardless of what <see cref="Verbosity"/> is set to, this will print out
        /// what files are being processed even if <see cref="Verbosity"/> is set to <see cref="Verbosity.Quiet"/>.
        /// </summary>
        public bool DryRun { get; set; }

        /// <summary>
        /// How verbose should the print-outs to stdout be?
        /// 
        /// Defaulted to <see cref="Verbosity.Normal"/>.
        /// 
        /// <see cref="Verbosity.Quiet"/> - Nothing.
        /// <see cref="Verbosity.Minimal"/> - When it starts to process a file.
        /// <see cref="Verbosity.Normal"/>  - Everything lower and when it skips a file because <see cref="FileFilter"/>
        ///                                   returned false.
        /// <see cref="Verbosity.Verbose"/> - Everything lower and when it finishes processing a file.
        /// <see cref="Verbosity.Diagnostic"/> - Everything lower and what changes it makes to the files.
        /// </summary>
        public Verbosity Verbosity { get; set; }
    }
}
