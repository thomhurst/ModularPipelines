using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appintegrations", "create-data-integration")]
public record AwsAppintegrationsCreateDataIntegrationOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--kms-key")] string KmsKey,
[property: CliOption("--source-uri")] string SourceUri
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--schedule-config")]
    public string? ScheduleConfig { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--file-configuration")]
    public string? FileConfiguration { get; set; }

    [CliOption("--object-configuration")]
    public IEnumerable<KeyValue>? ObjectConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}