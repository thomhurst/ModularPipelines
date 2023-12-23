using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "create-index")]
public record AwsQbusinessCreateIndexOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--display-name")] string DisplayName
) : AwsOptions
{
    [CommandSwitch("--capacity-configuration")]
    public string? CapacityConfiguration { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}