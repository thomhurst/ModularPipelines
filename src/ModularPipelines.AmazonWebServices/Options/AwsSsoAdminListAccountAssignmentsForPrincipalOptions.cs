using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sso-admin", "list-account-assignments-for-principal")]
public record AwsSsoAdminListAccountAssignmentsForPrincipalOptions(
[property: CommandSwitch("--instance-arn")] string InstanceArn,
[property: CommandSwitch("--principal-id")] string PrincipalId,
[property: CommandSwitch("--principal-type")] string PrincipalType
) : AwsOptions
{
    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}