using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
public record AwsOptions() : CommandLineToolOptions("aws")
{
    [CliFlag("--debug")]
    public bool? Debug { get; set; }

    [CliOption("--endpoint-url")]
    public string? EndpointUrl { get; set; }

    [CliFlag("--no-verify-ssl")]
    public bool? NoVerifySsl { get; set; }

    [CliFlag("--no-paginate")]
    public bool? NoPaginate { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliOption("--query")]
    public string? Query { get; set; }

    [CliOption("--profile")]
    public string? Profile { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }

    [CliOption("--color")]
    public string? Color { get; set; }

    [CliFlag("--no-sign-request")]
    public bool? NoSignRequest { get; set; }

    [CliOption("--ca-bundle")]
    public string? CaBundle { get; set; }

    [CliOption("--cli-read-timeout")]
    public int? CliReadTimeout { get; set; }

    [CliOption("--cli-connect-timeout")]
    public int? CliConnectTimeout { get; set; }

    [CliOption("--cli-binary-format")]
    public string? CliBinaryFormat { get; set; }

    [CliFlag("--no-cli-pager")]
    public bool? NoCliPager { get; set; }

    [CliFlag("--cli-auto-prompt")]
    public bool? CliAutoPrompt { get; set; }

    [CliFlag("--no-cli-auto-prompt")]
    public bool? NoCliAutoPrompt { get; set; }
}
