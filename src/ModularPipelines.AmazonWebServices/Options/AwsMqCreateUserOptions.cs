using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "create-user")]
public record AwsMqCreateUserOptions(
[property: CliOption("--broker-id")] string BrokerId,
[property: CliOption("--password")] string Password,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--groups")]
    public string[]? Groups { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}