using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "remove-source-identifier-from-subscription")]
public record AwsDocdbRemoveSourceIdentifierFromSubscriptionOptions(
[property: CommandSwitch("--subscription-name")] string SubscriptionName,
[property: CommandSwitch("--source-identifier")] string SourceIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}