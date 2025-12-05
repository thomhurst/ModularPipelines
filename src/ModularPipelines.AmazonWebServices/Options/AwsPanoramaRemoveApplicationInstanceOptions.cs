using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("panorama", "remove-application-instance")]
public record AwsPanoramaRemoveApplicationInstanceOptions(
[property: CliOption("--application-instance-id")] string ApplicationInstanceId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}