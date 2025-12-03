using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "delete-user")]
public record AwsMqDeleteUserOptions(
[property: CliOption("--broker-id")] string BrokerId,
[property: CliOption("--username")] string Username
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}