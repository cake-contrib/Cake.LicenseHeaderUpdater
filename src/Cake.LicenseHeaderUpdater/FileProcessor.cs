//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System.IO;
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

        // ---------------- Constructor ----------------

        public FileProcessor( int threadNumber, ICakeLog log, CakeLicenseHeaderUpdaterSettings settings )
        {
            this.threadNumber = threadNumber;
            this.log = log;
            this.settings = settings;
        }

        // ---------------- Functions ----------------

        public void ProcessFile( FilePath file )
        {
            string contents = File.ReadAllText( file.ToString() );

            string result = ProcessFile( contents );

            if( this.settings.DryRun == false )
            {
                File.WriteAllText( file.ToString(), result );
            }
        }

        public string ProcessFile( string fileContents )
        {
            return fileContents;
        }
    }
}
