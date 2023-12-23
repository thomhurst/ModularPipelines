using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "delete-multi-region-access-point")]
public record AwsS3controlDeleteMultiRegionAccessPointOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--details")] string Details
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}