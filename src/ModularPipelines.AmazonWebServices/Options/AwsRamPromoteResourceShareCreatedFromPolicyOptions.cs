using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "promote-resource-share-created-from-policy")]
public record AwsRamPromoteResourceShareCreatedFromPolicyOptions(
[property: CommandSwitch("--resource-share-arn")] string ResourceShareArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}