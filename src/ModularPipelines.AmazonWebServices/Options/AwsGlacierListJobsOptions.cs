using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glacier", "list-jobs")]
public record AwsGlacierListJobsOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--vault-name")] string VaultName
) : AwsOptions
{
    [CommandSwitch("--statuscode")]
    public string? Statuscode { get; set; }

    [CommandSwitch("--completed")]
    public string? Completed { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public string? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public string? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}