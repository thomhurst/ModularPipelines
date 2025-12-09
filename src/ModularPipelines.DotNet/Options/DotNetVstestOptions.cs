using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetVstestOptions : DotNetOptions
{
    public DotNetVstestOptions(
        string testFileNames,
        string platformType
    )
    {
        CommandParts = ["vstest", "[<TEST_FILE_NAMES>]"];

        TestFileNames = testFileNames;

        PlatformType = platformType;
    }

    public DotNetVstestOptions(
        string platformType
    )
    {
        CommandParts = ["vstest", "[<TEST_FILE_NAMES>]"];

        PlatformType = platformType;
    }

    public DotNetVstestOptions()
    {
        CommandParts = ["vstest", "[<TEST_FILE_NAMES>]"];
    }

    [CliArgument(Name = "[<TEST_FILE_NAMES>]")]
    public virtual string? TestFileNames { get; set; }

    [CliFlag("--Blame")]
    public virtual bool? Blame { get; set; }

    [CliOption("--Diag")]
    public virtual string? Diag { get; set; }

    [CliOption("--Framework")]
    public virtual string? Framework { get; set; }

    [CliFlag("--InIsolation")]
    public virtual bool? InIsolation { get; set; }

    [CliOption("--ListTests")]
    public virtual string? ListTests { get; set; }

    [CliOption("--logger")]
    public virtual string? LoggerUriOrFriendlyName { get; set; }

    [CliFlag("--Parallel")]
    public virtual bool? Parallel { get; set; }

    [CliOption("--ParentProcessId")]
    public virtual string? ParentProcessId { get; set; }

    [CliFlag("--Platform")]
    public virtual bool? Platform { get; set; }

    [CliArgument(Name = "<PLATFORM_TYPE>")]
    public virtual string? PlatformType { get; set; }

    [CliOption("--Port")]
    public virtual string? Port { get; set; }

    [CliOption("--ResultsDirectory<PATH>")]
    public virtual string? ResultsDirectoryPATH { get; set; }

    [CliOption("--Settings")]
    public virtual string? SettingsFile { get; set; }

    [CliOption("--TestAdapterPath")]
    public virtual string? TestAdapterPath { get; set; }

    [CliOption("--TestCaseFilter")]
    public virtual string? TestCaseFilter { get; set; }

    [CliOption("--Tests")]
    public virtual string? Tests { get; set; }

    [CliFlag("--Help")]
    public virtual bool? Help { get; set; }
}
