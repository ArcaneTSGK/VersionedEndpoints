#tool dotnet:?package=GitVersion.Tool&version=6.0.0

var target = Argument("target", "Pack");
var configuration = Argument("configuration", "Release");

var assemblyVersion = "1.0.0";
var packageVersion = "1.0.0";

var artifactsDir = MakeAbsolute(Directory("artifacts"));
var packagesDir = artifactsDir.Combine(Directory("packages"));
var testResultsDir = artifactsDir.Combine(Directory("test-results"));

var projectPath = "./src/VersionedEndpoints.AspNetCore/VersionedEndpoints.AspNetCore.csproj";

Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory(artifactsDir);
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetRestore();
});

Task("SemVer")
    .IsDependentOn("Restore")
    .Does(() =>
{
    var gitVersionSettings = new GitVersionSettings
    {
        NoFetch = true,
    };

    var gitVersion = GitVersion(gitVersionSettings);

    assemblyVersion = gitVersion.AssemblySemVer;
    packageVersion = gitVersion.NuGetVersion ?? packageVersion;

    Information($"AssemblySemVer: {assemblyVersion}");
    Information($"NuGetVersion: {packageVersion}");
});

Task("SetGitHubVersion")
    .IsDependentOn("Semver")
    .WithCriteria(() => GitHubActions.IsRunningOnGitHubActions)
    .Does(() =>
{
    var gitHubEnvironmentFile = Environment.GetEnvironmentVariable("GITHUB_ENV");
    var packageVersionEnvironmentVariable = $"PACKAGE_VERSION={packageVersion}";
    System.IO.File.WriteAllText(gitHubEnvironmentFile, packageVersionEnvironmentVariable);
});

Task("Build")
    .IsDependentOn("SetGitHubVersion")
    .Does(() =>
{
    var settings = new DotNetBuildSettings
    {
        Configuration = configuration,
        NoIncremental = true,
        NoRestore = true,
        MSBuildSettings = new DotNetMSBuildSettings()
            .SetVersion(assemblyVersion)
            .WithProperty("FileVersion", packageVersion)
            .WithProperty("InformationalVersion", packageVersion)
    };

    DotNetBuild(projectPath, settings);
});

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new DotNetPackSettings
    {
        Configuration = configuration,
        OutputDirectory = packagesDir,
        NoBuild = true,
        NoRestore = true,
        IncludeSymbols = true,
        MSBuildSettings = new DotNetMSBuildSettings()
            .SetVersion(assemblyVersion)
            .WithProperty("PackageVersion", packageVersion)
    };

    DotNetPack(projectPath, settings);
});

RunTarget(target);
