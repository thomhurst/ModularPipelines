using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appsync", "update-api-key")]
public record AwsAppsyncUpdateApiKeyOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--expires")]
    public long? Expires { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}