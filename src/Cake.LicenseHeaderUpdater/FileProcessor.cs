//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.LicenseHeaderUpdater
{
    internal class FileProcessor
    {
        // ---------------- Fields ----------------

        private readonly int threadNumber;
        private readonly ICakeLog log;
        private readonly CakeLicenseHeaderUpdaterSettings settings;
        private readonly Regex oldRegexes;

        // ---------------- Constructor ----------------

        public FileProcessor( int threadNumber, ICakeLog log, CakeLicenseHeaderUpdaterSettings settings )
        {
            this.threadNumber = threadNumber;
            this.log = log;
            this.settings = settings;

            StringBuilder builder = new StringBuilder();
            foreach( string pattern in settings.OldHeaderRegexPatterns)
            {
                builder.Append( $"({pattern})|" );
            }
            builder.Remove( builder.Length - 1, 1 );

            this.oldRegexes = new Regex(
                builder.ToString(),
                RegexOptions.Compiled | RegexOptions.ExplicitCapture
            );
        }

        // ---------------- Functions ----------------

        public void ProcessFile( FilePath file )
        {
            string contents = File.ReadAllText( file.ToString() );

            string result = ProcessFile( file.ToString(), contents );

            if( this.settings.DryRun == false )
            {
                File.WriteAllText( file.ToString(), result );
            }
        }

        public string ProcessFile( string fileName, string fileContents )
        {
            string newContents = this.oldRegexes.Replace( fileContents, string.Empty );

            if( newContents.Length == fileContents.Length )
            {
                this.log.Information(
                    Verbosity.Diagnostic,
                    $"{this.threadNumber}> Found no old license headers in '{fileName}'."
                );
            }
            else
            {
                this.log.Information(
                    Verbosity.Diagnostic,
                    $"{this.threadNumber}> Removed old license headers in '{fileName}'."
                );
            }

            if( string.IsNullOrEmpty( this.settings.LicenseString ) )
            {
                this.log.Information(
                    Verbosity.Diagnostic,
                    $"{this.threadNumber}> No header added since none were specified for '{fileName}'."
                );
                return newContents;
            }
            else
            {
                this.log.Information(
                    Verbosity.Diagnostic,
                    $"{this.threadNumber}> Added new header in '{fileName}'."
                );
                return this.settings.LicenseString + newContents;
            }
        }
    }
}
