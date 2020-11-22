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
    internal class FileProcessorRunnerResult
    {
        // ---------------- Constructor ----------------

        public FileProcessorRunnerResult()
        {
            this.Exceptions = new Dictionary<FilePath, Exception>();
        }

        // ---------------- Properties ----------------

        public IDictionary<FilePath, Exception> Exceptions { get; private set; }
    }
}
