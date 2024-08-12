using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.DotNet.Options;

[ExcludeFromCodeCoverage]
public record DotNetFormatOptions : DotNetOptions
{
    public DotNetFormatOptions(
        string projectSolution
    )
    {
        CommandParts = ["format", "[<PROJECT | SOLUTION>]"];

        ProjectSolution = projectSolution;
    }

    public DotNetFormatOptions()
    {
        CommandParts = ["format"];
    }

    [PositionalArgument(PlaceholderName = "[<PROJECT | SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CommandSwitch("--diagnostics")]
    public string? Diagnostics { get; set; }

    [BooleanCommandSwitch("--severity")]
    public string? Severity { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public bool? NoRestore { get; set; }

    [BooleanCommandSwitch("--verify-no-changes")]
    public bool? VerifyNoChanges { get; set; }

    [CommandSwitch("--include")]
    public string? Include { get; set; }

    [CommandSwitch("--exclude")]
    public string? Exclude { get; set; }

    [BooleanCommandSwitch("--include-generated")]
    public bool? IncludeGenerated { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [CommandSwitch("--binarylog")]
    public string? Binarylog { get; set; }

    [CommandSwitch("--report")]
    public string? Report { get; set; }

    [BooleanCommandSwitch("--help")]
    public bool? Help { get; set; }
}
