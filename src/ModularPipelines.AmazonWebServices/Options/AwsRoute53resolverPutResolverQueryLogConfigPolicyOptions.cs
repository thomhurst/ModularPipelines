using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "put-resolver-query-log-config-policy")]
public record AwsRoute53resolverPutResolverQueryLogConfigPolicyOptions(
[property: CommandSwitch("--arn")] string Arn,
[property: CommandSwitch("--resolver-query-log-config-policy")] string ResolverQueryLogConfigPolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}