using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
public record AwsOptions() : CommandLineToolOptions("aws")
{
    [BooleanCommandSwitch("--debug")]
    public bool? Debug { get; set; }

    [CommandSwitch("--endpoint-url")]
    public string? EndpointUrl { get; set; }

    [BooleanCommandSwitch("--no-verify-ssl")]
    public bool? NoVerifySsl { get; set; }

    [BooleanCommandSwitch("--no-paginate")]
    public bool? NoPaginate { get; set; }

    [CommandSwitch("--output")]
    public string? Output { get; set; }

    [CommandSwitch("--query")]
    public string? Query { get; set; }

    [CommandSwitch("--profile")]
    public string? Profile { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }

    [CommandSwitch("--color")]
    public string? Color { get; set; }

    [BooleanCommandSwitch("--no-sign-request")]
    public bool? NoSignRequest { get; set; }

    [CommandSwitch("--ca-bundle")]
    public string? CaBundle { get; set; }

    [CommandSwitch("--cli-read-timeout")]
    public int? CliReadTimeout { get; set; }

    [CommandSwitch("--cli-connect-timeout")]
    public int? CliConnectTimeout { get; set; }

    [CommandSwitch("--cli-binary-format")]
    public string? CliBinaryFormat { get; set; }

    [BooleanCommandSwitch("--no-cli-pager")]
    public bool? NoCliPager { get; set; }

    [BooleanCommandSwitch("--cli-auto-prompt")]
    public bool? CliAutoPrompt { get; set; }

    [BooleanCommandSwitch("--no-cli-auto-prompt")]
    public bool? NoCliAutoPrompt { get; set; }
}
