using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-resolver-dnssec-config")]
public record AwsRoute53resolverUpdateResolverDnssecConfigOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--validation")] string Validation
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}