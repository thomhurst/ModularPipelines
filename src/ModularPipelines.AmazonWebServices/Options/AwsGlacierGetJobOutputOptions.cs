using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glacier", "get-job-output")]
public record AwsGlacierGetJobOutputOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--vault-name")] string VaultName,
[property: CommandSwitch("--job-id")] string JobId
) : AwsOptions
{
    [CommandSwitch("--range")]
    public string? Range { get; set; }
}