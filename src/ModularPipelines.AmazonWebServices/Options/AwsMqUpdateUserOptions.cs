using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "update-user")]
public record AwsMqUpdateUserOptions(
[property: CliOption("--broker-id")] string BrokerId,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--groups")]
    public string[]? Groups { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}