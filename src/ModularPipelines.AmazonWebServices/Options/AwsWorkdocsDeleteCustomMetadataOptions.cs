using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "delete-custom-metadata")]
public record AwsWorkdocsDeleteCustomMetadataOptions(
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--keys")]
    public string[]? Keys { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}