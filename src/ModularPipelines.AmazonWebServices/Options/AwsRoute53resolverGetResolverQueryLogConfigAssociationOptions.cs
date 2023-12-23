using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "get-resolver-query-log-config-association")]
public record AwsRoute53resolverGetResolverQueryLogConfigAssociationOptions(
[property: CommandSwitch("--resolver-query-log-config-association-id")] string ResolverQueryLogConfigAssociationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}