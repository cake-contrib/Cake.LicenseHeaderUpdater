//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.LicenseHeaderUpdater
{
    public class CakeLicenseHeaderUpdaterRunnerException : AggregateException
    {
        internal CakeLicenseHeaderUpdaterRunnerException( IEnumerable<FileProcessorRunnerResult> results ) :
            base(
                "Errors when processing files.",
                GetExceptions( results )
            )
        {
        }

        private static IEnumerable<Exception> GetExceptions( IEnumerable<FileProcessorRunnerResult> results )
        {
            List<Exception> exceptions = new List<Exception>();
            foreach( FileProcessorRunnerResult r in results)
            {
                foreach( KeyValuePair<FilePath, Exception> e in r.Exceptions )
                {
                    exceptions.Add( new FileProcessorException( e.Key, e.Value ) );
                }
            }

            return exceptions;
        }
    }
}
