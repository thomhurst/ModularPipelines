using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "update-user")]
public record AwsMemorydbUpdateUserOptions(
[property: CliOption("--user-name")] string UserName
) : AwsOptions
{
    [CliOption("--authentication-mode")]
    public string? AuthenticationMode { get; set; }

    [CliOption("--access-string")]
    public string? AccessString { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}