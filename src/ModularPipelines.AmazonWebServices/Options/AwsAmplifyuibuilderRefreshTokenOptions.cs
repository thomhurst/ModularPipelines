using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amplifyuibuilder", "refresh-token")]
public record AwsAmplifyuibuilderRefreshTokenOptions(
[property: CommandSwitch("--provider")] string Provider,
[property: CommandSwitch("--refresh-token-body")] string RefreshTokenBody
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}