using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qldb", "update-ledger-permissions-mode")]
public record AwsQldbUpdateLedgerPermissionsModeOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--permissions-mode")] string PermissionsMode
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}