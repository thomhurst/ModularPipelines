using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("route53resolver", "update-resolver-config")]
public record AwsRoute53resolverUpdateResolverConfigOptions(
[property: CommandSwitch("--resource-id")] string ResourceId,
[property: CommandSwitch("--autodefined-reverse-flag")] string AutodefinedReverseFlag
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}