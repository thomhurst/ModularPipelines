using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
public record GcloudOptions() : CommandLineToolOptions("gcloud")
{
    [CliOption("--account")]
    public string? Account { get; set; }

    [CliOption("--billing-project")]
    public string? BillingProject { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--flags-file")]
    public string? FlagsFile { get; set; }

    [CliOption("--flatten")]
    public string[]? Flatten { get; set; }

    [CliOption("--format")]
    public string? Format { get; set; }

    [CliFlag("--help")]
    public bool? Help { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliFlag("--quiet")]
    public bool? Quiet { get; set; }

    [CliOption("--verbosity")]
    public string? Verbosity { get; set; }

    [CliFlag("--version")]
    public bool? Version { get; set; }

    [CliFlag("-h")]
    public bool? H { get; set; }
}
