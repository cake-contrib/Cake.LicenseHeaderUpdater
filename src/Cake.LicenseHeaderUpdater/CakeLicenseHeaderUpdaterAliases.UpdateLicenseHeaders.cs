//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.LicenseHeaderUpdater
{
    public static partial class CakeLicenseHeaderUpdaterAliases
    {
        /// <summary>
        /// Modifies the License Headers of all the passed in files
        /// based on the given settings.
        /// </summary>
        /// <param name="files">The files to modify</param>
        /// <param name="settings">The settings to use</param>
        /// <example>
        /// <code>
        ///         CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings();
        ///         settings.LicenseString =
        /// @"//
        /// // Copyright Seth Hendrick 2019-2020.
        /// // Distributed under the MIT License.
        /// // (See accompanying file LICENSE in the root of the repository).
        /// //";
        /// 
        ///         string old2019String = 
        /// @"//
        /// // Copyright Seth Hendrick 2019\.
        /// // Distributed under the MIT License\.
        /// // \(See accompanying file LICENSE in the root of the repository\)\.
        /// //";
        ///         settings.OldHeaderRegexPatterns.Add( old2019String );
        ///         settings.Threads = 1;
        ///         settings.DryRun = true;
        ///         settings.Verbosity = Verbosity.Diagnostic;
        ///         settings.FileFilter = delegate( FilePath path )
        ///         {
        ///             if( path.ToString().Contains( "bin" ) )
        ///             {
        ///                 return false;
        ///             }
        ///             else if( path.ToString().Contains( "obj" ) )
        ///             {
        ///                 return false;
        ///             }
        /// 
        ///             return true;
        ///         };
        /// 
        ///         CakeLicenseHeaderUpdaterRunner runner = new CakeLicenseHeaderUpdaterRunner( context );
        /// 
        ///         FilePathCollection files = GetFiles( "*.cs");
        ///         UpdateLicenseHeaders( files, settings );
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory( "UpdateLicenseHeaders" )]
        [CakeNamespaceImport( "Cake.LicenseHeaderUpdater" )]
        public static void UpdateLicenseHeaders(
            this ICakeContext context,
            IEnumerable<FilePath> files,
            CakeLicenseHeaderUpdaterSettings settings
        )
        {
            CakeLicenseHeaderUpdaterRunner runner = new CakeLicenseHeaderUpdaterRunner( context );
            runner.Run( files, settings );
        }
    }
}
