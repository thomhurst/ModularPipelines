using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "update-ledger-permissions-mode")]
public record AwsQldbUpdateLedgerPermissionsModeOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--permissions-mode")] string PermissionsMode
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}