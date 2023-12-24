using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kinesisanalyticsv2", "delete-application-vpc-configuration")]
public record AwsKinesisanalyticsv2DeleteApplicationVpcConfigurationOptions(
[property: CommandSwitch("--application-name")] string ApplicationName,
[property: CommandSwitch("--vpc-configuration-id")] string VpcConfigurationId
) : AwsOptions
{
    [CommandSwitch("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CommandSwitch("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}