using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("guardduty", "create-threat-intel-set")]
public record AwsGuarddutyCreateThreatIntelSetOptions(
[property: CliOption("--detector-id")] string DetectorId,
[property: CliOption("--name")] string Name,
[property: CliOption("--format")] string Format,
[property: CliOption("--location")] string Location
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}