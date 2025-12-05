using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "update-user")]
public record AwsIamUpdateUserOptions(
[property: CliOption("--user-name")] string UserName
) : AwsOptions
{
    [CliOption("--new-path")]
    public string? NewPath { get; set; }

    [CliOption("--new-user-name")]
    public string? NewUserName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}