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
    public virtual string? Diagnostics { get; set; }

    [BooleanCommandSwitch("--severity")]
    public virtual string? Severity { get; set; }

    [BooleanCommandSwitch("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [BooleanCommandSwitch("--verify-no-changes")]
    public virtual bool? VerifyNoChanges { get; set; }

    [CommandSwitch("--include")]
    public virtual string? Include { get; set; }

    [CommandSwitch("--exclude")]
    public virtual string? Exclude { get; set; }

    [BooleanCommandSwitch("--include-generated")]
    public virtual bool? IncludeGenerated { get; set; }

    [CommandSwitch("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CommandSwitch("--binarylog")]
    public virtual string? Binarylog { get; set; }

    [CommandSwitch("--report")]
    public virtual string? Report { get; set; }

    [BooleanCommandSwitch("--help")]
    public virtual bool? Help { get; set; }
}
