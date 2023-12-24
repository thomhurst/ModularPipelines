using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "put-block-public-access-configuration")]
public record AwsEmrPutBlockPublicAccessConfigurationOptions(
[property: CommandSwitch("--block-public-access-configuration")] string BlockPublicAccessConfiguration
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}