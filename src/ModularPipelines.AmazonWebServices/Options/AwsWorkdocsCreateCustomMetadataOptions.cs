using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "create-custom-metadata")]
public record AwsWorkdocsCreateCustomMetadataOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--custom-metadata")] IEnumerable<KeyValue> CustomMetadata
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}