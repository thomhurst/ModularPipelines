using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("inspector2", "disable-delegated-admin-account")]
public record AwsInspector2DisableDelegatedAdminAccountOptions(
[property: CommandSwitch("--delegated-admin-account-id")] string DelegatedAdminAccountId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}