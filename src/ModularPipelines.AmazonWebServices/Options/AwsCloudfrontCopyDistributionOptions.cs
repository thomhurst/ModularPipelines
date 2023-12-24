using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "copy-distribution")]
public record AwsCloudfrontCopyDistributionOptions(
[property: CommandSwitch("--primary-distribution-id")] string PrimaryDistributionId,
[property: CommandSwitch("--caller-reference")] string CallerReference
) : AwsOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}