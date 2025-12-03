using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "create-plugin")]
public record AwsQbusinessCreatePluginOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--auth-configuration")] string AuthConfiguration,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--server-url")] string ServerUrl,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}