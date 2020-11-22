//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using Cake.Core.IO;

namespace Cake.LicenseHeaderUpdater
{
    /// <summary>
    /// Exception that happens when 
    /// </summary>
    public class FileProcessorException : Exception
    {
        // ---------------- Constructor ----------------

        public FileProcessorException( FilePath file, Exception e ) :
            base(
                $"Exception thrown when processing file {file}: {e.Message}",
                e
            )
        {
        }
    }
}
