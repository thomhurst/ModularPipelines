using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-plugin")]
public record AwsQbusinessUpdatePluginOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--plugin-id")] string PluginId
) : AwsOptions
{
    [CliOption("--auth-configuration")]
    public string? AuthConfiguration { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--server-url")]
    public string? ServerUrl { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}