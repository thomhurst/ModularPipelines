using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "create-partner-input")]
public record AwsMedialiveCreatePartnerInputOptions(
[property: CliOption("--input-id")] string InputId
) : AwsOptions
{
    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}