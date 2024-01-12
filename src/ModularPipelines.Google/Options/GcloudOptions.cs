using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
public record GcloudOptions() : CommandLineToolOptions("gcloud")
{
    [CommandSwitch("--account")]
    public string? Account { get; set; }

    [CommandSwitch("--billing-project")]
    public string? BillingProject { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--flags-file")]
    public string? FlagsFile { get; set; }

    [CommandSwitch("--flatten")]
    public string[]? Flatten { get; set; }

    [CommandSwitch("--format")]
    public string? Format { get; set; }

    [BooleanCommandSwitch("--help")]
    public bool? Help { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }

    [BooleanCommandSwitch("--quiet")]
    public bool? Quiet { get; set; }

    [CommandSwitch("--verbosity")]
    public string? Verbosity { get; set; }

    [BooleanCommandSwitch("--version")]
    public bool? Version { get; set; }

    [BooleanCommandSwitch("-h")]
    public bool? H { get; set; }
}
