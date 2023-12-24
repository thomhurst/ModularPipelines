using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "submit-multi-region-access-point-routes")]
public record AwsS3controlSubmitMultiRegionAccessPointRoutesOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--mrap")] string Mrap,
[property: CommandSwitch("--route-updates")] string[] RouteUpdates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}