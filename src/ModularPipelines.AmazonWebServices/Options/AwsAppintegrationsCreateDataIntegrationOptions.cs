using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appintegrations", "create-data-integration")]
public record AwsAppintegrationsCreateDataIntegrationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--kms-key")] string KmsKey,
[property: CommandSwitch("--source-uri")] string SourceUri
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--schedule-config")]
    public string? ScheduleConfig { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--file-configuration")]
    public string? FileConfiguration { get; set; }

    [CommandSwitch("--object-configuration")]
    public IEnumerable<KeyValue>? ObjectConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}