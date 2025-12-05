using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ds", "reset-user-password")]
public record AwsDsResetUserPasswordOptions(
[property: CliOption("--directory-id")] string DirectoryId,
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--new-password")] string NewPassword
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}