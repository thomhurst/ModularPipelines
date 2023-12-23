using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53", "change-cidr-collection")]
public record AwsRoute53ChangeCidrCollectionOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--changes")] string[] Changes
) : AwsOptions
{
    [CommandSwitch("--collection-version")]
    public long? CollectionVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}