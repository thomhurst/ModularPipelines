using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "update-template-permissions")]
public record AwsQuicksightUpdateTemplatePermissionsOptions(
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--template-id")] string TemplateId
) : AwsOptions
{
    [CliOption("--grant-permissions")]
    public string[]? GrantPermissions { get; set; }

    [CliOption("--revoke-permissions")]
    public string[]? RevokePermissions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}