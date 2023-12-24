using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "delete-resolver-query-log-config")]
public record AwsRoute53resolverDeleteResolverQueryLogConfigOptions(
[property: CommandSwitch("--resolver-query-log-config-id")] string ResolverQueryLogConfigId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}