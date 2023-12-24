using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "create-outpost-resolver")]
public record AwsRoute53resolverCreateOutpostResolverOptions(
[property: CommandSwitch("--creator-request-id")] string CreatorRequestId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--preferred-instance-type")] string PreferredInstanceType,
[property: CommandSwitch("--outpost-arn")] string OutpostArn
) : AwsOptions
{
    [CommandSwitch("--instance-count")]
    public int? InstanceCount { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}