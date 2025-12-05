using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "connect-directory")]
public record AwsDsConnectDirectoryOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--password")] string Password,
[property: CliOption("--size")] string Size,
[property: CliOption("--connect-settings")] string ConnectSettings
) : AwsOptions
{
    [CliOption("--short-name")]
    public string? ShortName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}