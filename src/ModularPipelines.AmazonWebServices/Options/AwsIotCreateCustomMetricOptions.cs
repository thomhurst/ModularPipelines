using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "create-custom-metric")]
public record AwsIotCreateCustomMetricOptions(
[property: CliOption("--metric-name")] string MetricName,
[property: CliOption("--metric-type")] string MetricType
) : AwsOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}