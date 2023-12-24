using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "create-log-subscription")]
public record AwsDsCreateLogSubscriptionOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--log-group-name")] string LogGroupName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}