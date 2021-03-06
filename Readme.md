Cake.LicenseHeaderUpdater
=========
A way to mass-update the license header in all source files.

About
--------
This addin to [Cake](https://github.com/cake-build/cake), a C# build system, allows one to modify or even apply their license header in all of their source code files.

If one needs to update their copyright year, changed their name, or even changed their license, this tool can be used to update all of the source code files with the latest license header.

Packages
--------
[![NuGet](https://img.shields.io/nuget/v/Cake.LicenseHeaderUpdater.svg)](https://www.nuget.org/packages/Cake.LicenseHeaderUpdater/) 

How it Works
--------

First, add the addin to your build.cake file by including ```#addin Cake.LicenseHeaderUpdater```.

Second, you need to collect the list of files you want to change the license header of.  There are numerious ways to do this.  You can hard-code them, you can use the [GetFiles()](https://cakebuild.net/api/Cake.Common.IO/GlobbingAliases/7DD7F309) Alias to return a [FilePathCollection](https://www.cakebuild.net/api/Cake.Core.IO/FilePathCollection/), or can even parse the solution file (see Scenario 4 below).

Third, you need to Configure a CakeLicenseHeaderUpdaterSettings object.  Read the object's comments on the property to see how to configure it properly.  The hardest part is figuring out the list of regexes used to replace old licenses with the new one.  It is strongly recommended that you are on a clean git or SVN checkout before running this tool so its easy to revert in case your regex is wrong.

These are the various scenarios that this addin supports, as shown below.  There are also more examples in the [Integration Tests](https://github.com/xforever1313/Cake.LicenseHeaderUpdater/tree/master/src/Cake.LicenseHeaderUpdater.Tests/IntegrationTests).

Scenario 1: Adding a License Header to New Files
---

```C#
#addin Cake.LicenseHeaderUpdater

Task( "update_license" ).
Does(
    ( context ) =>
    {
        CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings();

        // 1.  Need to include the License as part of the LicenseString property.
        // This can be multiline by adding a '@' in front of the string.
        settings.LicenseString =
@"//
// Copyright Seth Hendrick 2019-2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

";    // <- So there is a new line between the license and the start of the rest of the file

        FilePathCollection files = GetFiles( "src/*.cs" );
        UpdateLicenseHeaders( files, settings );
    }
);

RunTarget( "update_license" );
```

Scenario 2: Replacing an Old License Header with a New One
---

Mostly the same as Scenario 1, but adding regex patterns to the OldHeaderRegexPatterns
property.  Note: be mindful of your Regex pattern, as this tool will Replace **ALL MATCHES**
of the Regex with an empty string.

```C#
#addin Cake.LicenseHeaderUpdater

Task( "update_license" ).
Does(
    ( context ) =>
    {
        CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings();

        // 1.  Need to include the License as part of the LicenseString property.
        // This can be multiline by adding a '@' in front of the string.
        settings.LicenseString =
@"//
// Copyright Seth Hendrick 2019-2020.
// Distributed under the MIT License.
// (See accompanying file LICENSE in the root of the repository).
//

";    // <- So there is a new line between the license and the start of the rest of the file

        // Remember, this is a regex, you need to escape characters such as '.'.
        string old2019Regex = 
@"//
// Copyright Seth Hendrick 20\d+\.
// Distributed under the MIT License\.
// \(See accompanying file LICENSE in the root of the repository\)\.
//";
        settings.OldHeaderRegexPatterns.Add( old2019Regex );

        FilePathCollection files = GetFiles( "src/*.cs" );
        UpdateLicenseHeaders( files, settings );
    }
);

RunTarget( "update_license" );
```

Scenario 3: Removing all License Headers
---

Mostly the same as Scenario 2, but simply do not set the LicenseString to anything
Note: be mindful of your Regex pattern, as this tool will Replace **ALL MATCHES**
of the Regex with an empty string.

```C#
#addin Cake.LicenseHeaderUpdater

Task( "update_license" ).
Does(
    ( context ) =>
    {
        CakeLicenseHeaderUpdaterSettings settings = new CakeLicenseHeaderUpdaterSettings();

        // Remember, this is a regex, you need to escape characters such as '.'.
        string old2019Regex = 
@"//
// Copyright Seth Hendrick 20\d+\.
// Distributed under the MIT License\.
// \(See accompanying file LICENSE in the root of the repository\)\.
//";
        settings.OldHeaderRegexPatterns.Add( old2019Regex );

        FilePathCollection files = GetFiles( "src/*.cs" );
        UpdateLicenseHeaders( files, settings );
    }
);

RunTarget( "update_license" );
```

Scenario 4: Updating all files within a Solution
---

One common use-case could be to update all of the .cs files contained within all of the .csproj files within a solution file.
This can be done with Cake's built-in [ParseSolution](https://cakebuild.net/api/Cake.Common.Solution/SolutionAliases/0E73DD36) and 
[ParseProject](https://cakebuild.net/api/Cake.Common.Solution.Project/ProjectAliases/A553D651) functions.
However, note, there are limitations if you are using the new SDK style csproj format.
As in, Cake doesn't really support parsing these yet (see [this](https://github.com/cake-build/cake/issues/1662) issue).  The following example
is something I used to work with the new .csproj format.  Its obviously not perfect, but it works for me.

In this example, everything is the same as the previous scenarios, minus how one gets the files.

```C#
        settings.FileFilter = delegate( FilePath path )
        {
            // Ignore files in obj and bin folders.
            if( Regex.IsMatch( path.ToString(), @"[/\\]obj[/\\]" ) )
            {
                return false;
            }
            if( Regex.IsMatch( path.ToString(), @"[/\\]bin[/\\]" ) )
            {
                return false;
            }
            else
            {
                return true;
            }
        };

        List<FilePath> files = new List<FilePath>();

        SolutionParserResult slnResult = ParseSolution( File( "PathToYourSolution.sln" ) );
        foreach( SolutionProject proj in slnResult.Projects )
        {
            if( proj.Path.ToString().EndsWith( ".csproj" ) )
            {
                string glob = proj.Path.GetDirectory() + "/**/*.cs";
                files.AddRange( GetFiles( glob ) );
            }
        }

        UpdateLicenseHeaders( files, settings );
```

License
--------
To be consistent with Cake itself, Cake.LicenseHeaderUpdater is released under the MIT license.
