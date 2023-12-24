using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "modify-document-permission")]
public record AwsSsmModifyDocumentPermissionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--permission-type")] string PermissionType
) : AwsOptions
{
    [CommandSwitch("--account-ids-to-add")]
    public string[]? AccountIdsToAdd { get; set; }

    [CommandSwitch("--account-ids-to-remove")]
    public string[]? AccountIdsToRemove { get; set; }

    [CommandSwitch("--shared-document-version")]
    public string? SharedDocumentVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}