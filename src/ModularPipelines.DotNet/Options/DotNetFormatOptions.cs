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

    [CliArgument(Name = "[<PROJECT | SOLUTION>]")]
    public string? ProjectSolution { get; set; }

    [CliOption("--diagnostics")]
    public virtual string? Diagnostics { get; set; }

    [CliFlag("--severity")]
    public virtual string? Severity { get; set; }

    [CliFlag("--no-restore")]
    public virtual bool? NoRestore { get; set; }

    [CliFlag("--verify-no-changes")]
    public virtual bool? VerifyNoChanges { get; set; }

    [CliOption("--include")]
    public virtual string? Include { get; set; }

    [CliOption("--exclude")]
    public virtual string? Exclude { get; set; }

    [CliFlag("--include-generated")]
    public virtual bool? IncludeGenerated { get; set; }

    [CliOption("--verbosity")]
    public virtual string? Verbosity { get; set; }

    [CliOption("--binarylog")]
    public virtual string? Binarylog { get; set; }

    [CliOption("--report")]
    public virtual string? Report { get; set; }

    [CliFlag("--help")]
    public virtual bool? Help { get; set; }
}
