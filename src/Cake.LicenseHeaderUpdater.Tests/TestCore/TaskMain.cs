//
// Copyright Seth Hendrick 2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cake.Frosting;
using NUnit.Framework;

namespace Cake.LicenseHeaderUpdater.Tests.TestCore
{
    /// <summary>
    /// Runs the cake environment.  This is so we don't have to build
    /// an entire mock cake environment, and we don't need to download cake.exe.  Just
    /// use Cake.Frosting instead.
    /// 
    /// ...Yeahhhh, its a tad hacky.  But the alternative is downloading cake and maintaining a bunch
    /// of cake files OR making a mock instance of ICakeContext, and I don't really
    /// want to do either of those.
    /// </summary>
    public static class TaskMain
    {
        public static string RunTask( string globString, CakeLicenseHeaderUpdaterSettings settings )
        {
            List<string> arguments = new List<string>
            {
                $@"--target=""{TaskRunner.TaskName}""",
                $@"--files=""{globString}""",
                // Need the "my_" in front because otherwise Cake things its the dry run for *it*
                // not for us.
                $@"--my_{nameof( CakeLicenseHeaderUpdaterSettings.DryRun )}=""{settings.DryRun}""",
                $@"--{nameof( CakeLicenseHeaderUpdaterSettings.LicenseString )}=""{settings.LicenseString}"""
            };

            if( settings.OldHeaderRegexPatterns.Count > TaskRunner.MaximumRegexes )
            {
                throw new ArgumentException(
                    $"{nameof( CakeLicenseHeaderUpdaterSettings.OldHeaderRegexPatterns )} can not contain more than {TaskRunner.MaximumRegexes} elements"
                );
            }

            for( int i = 1; i <= settings.OldHeaderRegexPatterns.Count; ++i )
            {
                string argName = $"{nameof( CakeLicenseHeaderUpdaterSettings.OldHeaderRegexPatterns )}_{i}";
                arguments.Add( $@"--{argName}=""{settings.OldHeaderRegexPatterns[i - 1]}""" );
            }

            StringBuilder builder = new StringBuilder();
            arguments.ForEach( a => builder.Append( a + ", " ) );
            Console.WriteLine( "Sending arguments: " + builder );

            int exitCode;
            string stdOut;
            using( StringWriter writer = new StringWriter() )
            { 
                var oldCout = Console.Out;
                try
                {
                    // Each time we write to stdout, capture it for processing later.
                    // We'll put it back in the finally block.

                    Console.SetOut( writer );
                    var host = new CakeHostBuilder()
                        .WithArguments( arguments.ToArray() )
                        .ConfigureServices(
                            delegate ( ICakeServices services )
                            {
                                services.UseAssembly( typeof( TaskMain ).Assembly );
                            }
                        )
                        .Build();

                    exitCode = host.Run();
                    stdOut = writer.ToString();
                }
                finally
                {
                    Console.SetOut( oldCout );
                }
            }

            Console.WriteLine( "---- Start STDOUT From Cake.Frosting ----" );
            Console.WriteLine( stdOut );
            Console.WriteLine( "---- End STDOUT From Cake.Frosting ----" );

            Assert.AreEqual( 0, exitCode );

            return stdOut;
        }
    }
}
