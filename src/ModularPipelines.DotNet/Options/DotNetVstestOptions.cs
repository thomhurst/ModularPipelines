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

    [PositionalArgument(PlaceholderName = "[<TEST_FILE_NAMES>]")]
    public string? TestFileNames { get; set; }

    [BooleanCommandSwitch("--Blame")]
    public virtual bool? Blame { get; set; }

    [CommandSwitch("--Diag")]
    public virtual string? Diag { get; set; }

    [CommandSwitch("--Framework")]
    public virtual string? Framework { get; set; }

    [BooleanCommandSwitch("--InIsolation")]
    public virtual bool? InIsolation { get; set; }

    [CommandSwitch("--ListTests")]
    public virtual string? ListTests { get; set; }

    [CommandSwitch("--logger")]
    public virtual string? LoggerUriOrFriendlyName { get; set; }

    [BooleanCommandSwitch("--Parallel")]
    public virtual bool? Parallel { get; set; }

    [CommandSwitch("--ParentProcessId")]
    public virtual string? ParentProcessId { get; set; }

    [BooleanCommandSwitch("--Platform")]
    public virtual bool? Platform { get; set; }

    [PositionalArgument(PlaceholderName = "<PLATFORM_TYPE>")]
    public string? PlatformType { get; set; }

    [CommandSwitch("--Port")]
    public virtual string? Port { get; set; }

    [CommandSwitch("--ResultsDirectory<PATH>")]
    public virtual string? ResultsDirectoryPATH { get; set; }

    [CommandSwitch("--Settings")]
    public virtual string? SettingsFile { get; set; }

    [CommandSwitch("--TestAdapterPath")]
    public virtual string? TestAdapterPath { get; set; }

    [CommandSwitch("--TestCaseFilter")]
    public virtual string? TestCaseFilter { get; set; }

    [CommandSwitch("--Tests")]
    public virtual string? Tests { get; set; }

    [BooleanCommandSwitch("--Help")]
    public virtual bool? Help { get; set; }
}
