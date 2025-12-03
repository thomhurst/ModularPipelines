using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "modify-document-permission")]
public record AwsSsmModifyDocumentPermissionOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--permission-type")] string PermissionType
) : AwsOptions
{
    [CliOption("--account-ids-to-add")]
    public string[]? AccountIdsToAdd { get; set; }

    [CliOption("--account-ids-to-remove")]
    public string[]? AccountIdsToRemove { get; set; }

    [CliOption("--shared-document-version")]
    public string? SharedDocumentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}