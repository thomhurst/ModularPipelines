using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lightsail", "set-resource-access-for-bucket")]
public record AwsLightsailSetResourceAccessForBucketOptions(
[property: CommandSwitch("--resource-name")] string ResourceName,
[property: CommandSwitch("--bucket-name")] string BucketName,
[property: CommandSwitch("--access")] string Access
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}