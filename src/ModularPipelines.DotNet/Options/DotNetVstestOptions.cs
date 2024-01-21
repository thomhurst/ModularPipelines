using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetVstestOptions : DotNetOptions
{
    [PositionalArgument(PlaceholderName = "[<TEST_FILE_NAMES>]")]
    public string? TestFileNames { get; set; }

    [BooleanCommandSwitch("--Blame")]
    public bool? Blame { get; set; }

    [CommandSwitch("--Diag")]
    public string? Diag { get; set; }

    [CommandSwitch("--Framework")]
    public string? Framework { get; set; }

    [BooleanCommandSwitch("--InIsolation")]
    public bool? InIsolation { get; set; }

    [CommandSwitch("--ListTests")]
    public string? ListTests { get; set; }

    [CommandSwitch("--logger")]
    public string? LoggerUriOrFriendlyName { get; set; }

    [BooleanCommandSwitch("--Parallel")]
    public bool? Parallel { get; set; }

    [CommandSwitch("--ParentProcessId")]
    public string? ParentProcessId { get; set; }

    [BooleanCommandSwitch("--Platform")]
    public bool? Platform { get; set; }

    [PositionalArgument(PlaceholderName = "<PLATFORM_TYPE>")]
    public string? PlatformType { get; set; }

    [CommandSwitch("--Port")]
    public string? Port { get; set; }

    [CommandSwitch("--ResultsDirectory<PATH>")]
    public string? ResultsDirectoryPATH { get; set; }

    [CommandSwitch("--Settings")]
    public string? SettingsFile { get; set; }

    [CommandSwitch("--TestAdapterPath")]
    public string? TestAdapterPath { get; set; }

    [CommandSwitch("--TestCaseFilter")]
    public string? TestCaseFilter { get; set; }

    [CommandSwitch("--Tests")]
    public string? Tests { get; set; }
}
