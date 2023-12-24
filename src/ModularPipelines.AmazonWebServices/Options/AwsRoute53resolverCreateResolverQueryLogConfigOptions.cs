using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "create-resolver-query-log-config")]
public record AwsRoute53resolverCreateResolverQueryLogConfigOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--destination-arn")] string DestinationArn
) : AwsOptions
{
    [CommandSwitch("--creator-request-id")]
    public string? CreatorRequestId { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}