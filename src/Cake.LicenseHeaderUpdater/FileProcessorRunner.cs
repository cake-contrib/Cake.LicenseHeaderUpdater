//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Concurrent;
using System.Threading;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.LicenseHeaderUpdater
{
    internal class FileProcessorRunner : IDisposable
    {
        // ---------------- Fields ----------------

        private bool isDisposed;

        private readonly int threadNumber;
        private readonly ICakeLog log;
        private readonly ConcurrentQueue<FilePath> files;
        private readonly CakeLicenseHeaderUpdaterSettings settings;

        private readonly FileProcessor processor;
        private readonly FileProcessorRunnerResult result;

        private Thread thread;

        // ---------------- Constructor ----------------

        public FileProcessorRunner(
            int threadNumber,
            ICakeLog log,
            ConcurrentQueue<FilePath> files,
            CakeLicenseHeaderUpdaterSettings settings
        )
        {
            this.threadNumber = threadNumber;
            this.isDisposed = false;
            this.log = log;
            this.files = files;
            this.settings = settings;

            this.processor = new FileProcessor( this.threadNumber, this.log, this.settings );
            this.result = new FileProcessorRunnerResult();
        }

        ~FileProcessorRunner()
        {
            this.Dispose( false );
        }

        // ---------------- Functions ----------------

        public void Start()
        {
            if( this.thread != null )
            {
                throw new InvalidOperationException( "Already started!" );
            }
            this.thread = new Thread( this.ThreadEntry );
            this.thread.Start();
        }

        public FileProcessorRunnerResult Wait()
        {
            if( this.thread == null )
            {
                throw new InvalidOperationException(
                    "Thread not started, can not wait"
                );
            }
            this.thread.Join();

            return this.result;
        }

        public void Dispose()
        {
            this.Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected void Dispose( bool fromDispose )
        {
            if( this.isDisposed )
            {
                return;
            }

            try
            {
                if( fromDispose )
                {
                    this.thread?.Join();
                }
                else
                {
                    this.thread?.Abort();
                }
            }
            finally
            {
                this.isDisposed = true;
            }
        }

        private void ThreadEntry()
        {
            bool keepGoing = true;
            FilePath currentFile = null;

            while( keepGoing )
            {
                try
                {
                    if( this.files.IsEmpty )
                    {
                        keepGoing = false;
                        this.log.Information(
                            Verbosity.Diagnostic,
                            $"{threadNumber}> Done procesing"
                        );
                        continue;
                    }

                    if( this.files.TryDequeue( out currentFile ) )
                    {
                        Verbosity startVerbosity = this.settings.DryRun ? Verbosity.Quiet : Verbosity.Minimal;

                        if( settings.FileFilter( currentFile ) )
                        {

                            this.log.Information(
                                startVerbosity,
                                $"{threadNumber}> Processing '{currentFile}'..."
                            );

                            this.processor.ProcessFile( currentFile );

                            this.log.Information(
                                Verbosity.Diagnostic,
                                $"{threadNumber}> Processing '{currentFile}'... Done!"
                            );
                        }
                        else
                        {
                            this.log.Information(
                                Verbosity.Verbose,
                                $"{threadNumber}> Skipping '{currentFile}'."
                            );
                        }
                    }
                    else
                    {
                        this.log.Warning(
                            Verbosity.Verbose,
                            $"{threadNumber}> Could not dequeue from queue, trying again."
                        );
                        Thread.Sleep( 100 );
                    }
                }
                catch( ThreadAbortException )
                {
                    throw;
                }
                catch( Exception e )
                {
                    this.log.Error(
                        Verbosity.Minimal,
                        $"{threadNumber}> Error when processing {currentFile ?? "[null]"}: {e.Message}"
                    );

                    if( currentFile != null )
                    {
                        this.result.Exceptions.Add( currentFile, e );
                    }
                }
            }

            this.log.Information(
                Verbosity.Diagnostic,
                $"{threadNumber}> Thread Exiting"
            );
        }
    }
}
