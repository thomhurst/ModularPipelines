using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudfront", "update-distribution")]
public record AwsCloudfrontUpdateDistributionOptions(
[property: CommandSwitch("--id")] string Id
) : AwsOptions
{
    [CommandSwitch("--distribution-config")]
    public string? DistributionConfig { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--default-root-object")]
    public string? DefaultRootObject { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}