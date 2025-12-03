using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("memorydb", "create-user")]
public record AwsMemorydbCreateUserOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--authentication-mode")] string AuthenticationMode,
[property: CliOption("--access-string")] string AccessString
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}