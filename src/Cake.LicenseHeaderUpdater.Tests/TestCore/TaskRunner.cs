//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using Cake.Common;
using Cake.Common.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting;

namespace Cake.LicenseHeaderUpdater.Tests.TestCore
{
    /// <summary>
    /// Executes the license header updater inside of the cake environment.
    /// </summary>
    [TaskName( TaskName )]
    public class TaskRunner : FrostingTask
    {
        internal const string TaskName = "run";

        internal const int MaximumRegexes = 5;

        public override void Run( ICakeContext context )
        {
            CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings
            {
                // Need the "my_" in front because otherwise Cake things its the dry run for *it*
                // not for us.
                DryRun = context.Argument<bool>( $"my_{nameof( CakeLicenseHeaderUpdaterSettings.DryRun )}", false ),
                LicenseString = context.Argument( nameof( CakeLicenseHeaderUpdaterSettings.LicenseString ), string.Empty ),
                Threads = 1,
                Verbosity = Verbosity.Diagnostic
            };

            for( int i = 1; i <= MaximumRegexes; ++i )
            {
                string argName = $"{nameof( CakeLicenseHeaderUpdaterSettings.OldHeaderRegexPatterns )}_{i}";
                if( context.HasArgument( argName ) )
                {
                    settings.OldHeaderRegexPatterns.Add( context.Arguments.GetArgument( argName ) );
                }
            }

            string globPattern = context.Argument( "files", string.Empty );
            if( string.IsNullOrWhiteSpace( globPattern ) )
            {
                throw new ArgumentException( "'files' can not be empty" );
            }

            context.UpdateLicenseHeaders( context.GetFiles( globPattern ), settings );
        }
    }
}
