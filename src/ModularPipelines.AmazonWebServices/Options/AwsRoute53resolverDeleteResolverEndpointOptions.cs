using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "delete-resolver-endpoint")]
public record AwsRoute53resolverDeleteResolverEndpointOptions(
[property: CommandSwitch("--resolver-endpoint-id")] string ResolverEndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}