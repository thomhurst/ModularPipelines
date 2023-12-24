using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wafv2", "list-mobile-sdk-releases")]
public record AwsWafv2ListMobileSdkReleasesOptions(
[property: CommandSwitch("--platform")] string Platform
) : AwsOptions
{
    [CommandSwitch("--next-marker")]
    public string? NextMarker { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}