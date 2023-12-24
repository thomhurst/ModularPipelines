using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "get-multi-region-access-point-routes")]
public record AwsS3controlGetMultiRegionAccessPointRoutesOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--mrap")] string Mrap
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}