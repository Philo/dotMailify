#tool "xunit.runner.console"
#tool "gitversion.commandline"

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target                  = Argument("target", "Default");
var configuration           = Argument("configuration", "Release");
var solutionPath            = MakeAbsolute(File(Argument("solutionPath", "./dotMailify.sln")));

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

var artifacts               = MakeAbsolute(Directory(Argument("artifactPath", "./artifacts")));
var packagesOutput          = MakeAbsolute(Directory(artifacts +"/packages/"));
var buildOutput             = MakeAbsolute(Directory(artifacts +"/build"));
var testResultsPath         = MakeAbsolute(Directory(artifacts + "./tests"));
var versionAssemblyInfo     = MakeAbsolute(File(Argument("versionAssemblyInfo", "VersionAssemblyInfo.cs")));

var testAssemblies          = buildOutput +"/*.Tests.dll";

IEnumerable<FilePath> nugetProjectPaths     = null;
SolutionParserResult solution               = null;
GitVersion versionInfo                      = null;

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////

Setup(() => {
    CreateDirectory(artifacts);
    
    if(!FileExists(solutionPath)) throw new Exception(string.Format("Solution file not found - {0}", solutionPath.ToString()));
    solution = ParseSolution(solutionPath.ToString());

    Information("[Setup] Using Solution '{0}'", solutionPath.ToString());
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifacts);
    var binDirs = GetDirectories(solutionPath.GetDirectory() +@"\src\**\bin");
    var objDirs = GetDirectories(solutionPath.GetDirectory() +@"\src\**\obj");
    CleanDirectories(binDirs);
    CleanDirectories(objDirs);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solutionPath, new NuGetRestoreSettings());
});

Task("Update-Version-Info")
    .IsDependentOn("CreateVersionAssemblyInfo")
    .WithCriteria(() => DirectoryExists(".git"))
    .Does(() => 
{
        versionInfo = GitVersion(new GitVersionSettings {
            UpdateAssemblyInfo = true,
            UpdateAssemblyInfoFilePath = versionAssemblyInfo
        });

    if(versionInfo != null) {
        Information("Version: {0}", versionInfo.FullSemVer);
    } else {
        throw new Exception("Unable to determine version");
    }
});

Task("CreateVersionAssemblyInfo")
    .WithCriteria(() => !FileExists(versionAssemblyInfo))
    .Does(() =>
{
    Information("Creating version assembly info");
    CreateAssemblyInfo(versionAssemblyInfo, new AssemblyInfoSettings {
        Version = "0.0.0.0",
        FileVersion = "0.0.0.0",
        InformationalVersion = "",
    });
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .IsDependentOn("Update-Version-Info")
    .Does(() =>
{
    MSBuild(solutionPath, settings => settings
        .WithProperty("TreatWarningsAsErrors","true")
        .WithProperty("UseSharedCompilation", "false")
        .WithProperty("AutoParameterizationWebConfigConnectionStrings", "false")
        .WithProperty("OutputPath", buildOutput.ToString())
        .SetVerbosity(Verbosity.Quiet)
        .SetConfiguration(configuration)
        .WithTarget("Rebuild")
    );
});

public string GetNugetVersionString() {
    var version = string.Format("0.0.{0}.{1}-dev", DateTime.UtcNow.Date.Day, (DateTime.UtcNow - DateTime.Today).TotalSeconds.ToString("F0"));
    var nugetVersion = versionInfo != null ? versionInfo.NuGetVersionV2 : version;
    return nugetVersion;
}

Task("Package")
    .IsDependentOn("Build")
    .Does(() => 
{
    CreateDirectory(packagesOutput);

    NuGetPack("nuspec/dotMailify.nuspec", new NuGetPackSettings {
        Id = "dotMailify.Core",
        Version = GetNugetVersionString(),
        NoPackageAnalysis = false,     
        Properties = new Dictionary<string, string> { { "Configuration", configuration }},
        Symbols = false,
        OutputDirectory = packagesOutput,
        BasePath = buildOutput,
        Files = new[] {
            new NuSpecContent { Source = "dotMailify.Core.dll", Target = "lib/net46" }
        }
    });
    
    NuGetPack("nuspec/dotMailify.nuspec", new NuGetPackSettings {
        Id = "dotMailify.Smtp",
        NoPackageAnalysis = false,     
        Properties = new Dictionary<string, string> { { "Configuration", configuration }},
        Symbols = false,
        Version = GetNugetVersionString(),
        OutputDirectory = packagesOutput,
        BasePath = buildOutput,
        Files = new[] {
            new NuSpecContent { Source = "dotMailify.Smtp.dll", Target = "lib/net46" }
        },
        Dependencies = new[] {
            new NuSpecDependency { Id="dotMailify.Core", Version=GetNugetVersionString() }
        }
    });
    
});

Task("Copy-Packages-Locally")
    .IsDependentOn("Package")
    .WithCriteria(() => BuildSystem.IsLocalBuild)
    .Does(() => 
{
    var localProfile = Environment.GetEnvironmentVariable("USERPROFILE");
    CreateDirectory(localProfile +"/.nuget");
    CopyFiles(packagesOutput +"/*.nupkg", localProfile +"/.nuget");
    DeleteFiles(localProfile +"/.nuget/*.symbols.nupkg");  
});


Task("Run-Unit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    CreateDirectory(testResultsPath);

    var settings = new XUnit2Settings {
        XmlReportV1 = true,
        NoAppDomain = true,
        OutputDirectory = testResultsPath,
    };
    settings.ExcludeTrait("Category", "Integration");
    
    XUnit2(testAssemblies, settings);
});

Task("Update-AppVeyor-Build-Number")
    .IsDependentOn("Update-Version-Info")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
{
    AppVeyor.UpdateBuildVersion(string.Format("{0}-{1}", versionInfo.FullSemVer, AppVeyor.Environment.Build.Number));
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Local")
    .IsDependentOn("Default")
    .IsDependentOn("Copy-Packages-Locally")
    ;

Task("Default")
    .IsDependentOn("Update-Version-Info")
    .IsDependentOn("Build")
    .IsDependentOn("Package")
    ;

Task("CI")
    .IsDependentOn("Update-Version-Info")
    .IsDependentOn("Update-AppVeyor-Build-Number")
    .IsDependentOn("Build")
    .IsDependentOn("Run-Unit-Tests")
    .IsDependentOn("Package")
    ;
    
//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
