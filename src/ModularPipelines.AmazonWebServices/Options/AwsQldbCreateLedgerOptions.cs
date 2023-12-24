using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qldb", "create-ledger")]
public record AwsQldbCreateLedgerOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--permissions-mode")] string PermissionsMode
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}