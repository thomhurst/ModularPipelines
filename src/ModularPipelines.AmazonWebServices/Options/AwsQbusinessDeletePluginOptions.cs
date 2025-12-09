using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "delete-plugin")]
public record AwsQbusinessDeletePluginOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--plugin-id")] string PluginId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}