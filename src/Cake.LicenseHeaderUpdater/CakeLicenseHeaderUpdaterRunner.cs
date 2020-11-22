//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.LicenseHeaderUpdater
{
    public class CakeLicenseHeaderUpdaterRunner
    {
        // ---------------- Fields ----------------

        private readonly ICakeContext context;

        // ---------------- Constructor ----------------

        public CakeLicenseHeaderUpdaterRunner( ICakeContext context )
        {
            this.context = context;
        }

        // ---------------- Functions ----------------

        public void Run( IEnumerable<FilePath> files, CakeLicenseHeaderUpdaterSettings settings )
        {
            settings = FixupSettings( settings );

            ConcurrentQueue<FilePath> filesToProcess = new ConcurrentQueue<FilePath>( files );
            List<FileProcessorRunner> runners = new List<FileProcessorRunner>( settings.Threads );
            List<FileProcessorRunnerResult> results = new List<FileProcessorRunnerResult>( settings.Threads );

            using( this.context.Log.WithVerbosity( settings.Verbosity ) )
            {
                for( int i = 1; i <= settings.Threads; ++i )
                {
                    FileProcessorRunner runner = new FileProcessorRunner( i, this.context.Log, filesToProcess, settings );
                    runners.Add( runner );
                }

                try
                {
                    foreach( FileProcessorRunner runner in runners )
                    {
                        runner.Start();
                    }

                    foreach( FileProcessorRunner runner in runners )
                    {
                        results.Add( runner.Wait() );
                    }

                }
                finally
                {
                    foreach( FileProcessorRunner runner in runners )
                    {
                        try
                        {
                            runner.Dispose();
                        }
                        catch( Exception )
                        {
                        }
                    }
                }
            }

            CakeLicenseHeaderUpdaterRunnerException exception = new CakeLicenseHeaderUpdaterRunnerException( results );
            if( exception.InnerExceptions.Count != 0 )
            {
                throw exception;
            }
        }

        private CakeLicenseHeaderUpdaterSettings FixupSettings( CakeLicenseHeaderUpdaterSettings settings )
        {
            CakeLicenseHeaderUpdaterSettings realSettings = new CakeLicenseHeaderUpdaterSettings();
            realSettings.DryRun = settings.DryRun;

            if( settings.FileFilter == null )
            {
                realSettings.FileFilter = ( FilePath path ) => true;
            }
            else
            {
                realSettings.FileFilter = settings.FileFilter;
            }

            if( string.IsNullOrEmpty( settings.LicenseString ) )
            {
                realSettings.LicenseString = null;
            }

            IEnumerable<string> fixedRegexes = settings.OldHeaderRegexPatterns.Where(
                s => string.IsNullOrEmpty( s ) == false
            );

            foreach( string s in fixedRegexes )
            {
                realSettings.OldHeaderRegexPatterns.Add( s );
            }

            if( settings.Threads <= 0 )
            {
                realSettings.Threads = Environment.ProcessorCount;
            }
            else
            {
                realSettings.Threads = settings.Threads;
            }

            realSettings.Verbosity = settings.Verbosity;

            return realSettings;
        }
    }
}
