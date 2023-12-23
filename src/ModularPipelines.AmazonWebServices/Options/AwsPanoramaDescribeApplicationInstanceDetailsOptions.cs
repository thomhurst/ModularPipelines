using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("panorama", "describe-application-instance-details")]
public record AwsPanoramaDescribeApplicationInstanceDetailsOptions(
[property: CommandSwitch("--application-instance-id")] string ApplicationInstanceId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}