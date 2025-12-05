using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connect", "delete-user")]
public record AwsConnectDeleteUserOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}