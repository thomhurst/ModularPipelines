using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "add-permission")]
public record AwsLambdaAddPermissionOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--statement-id")] string StatementId,
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--principal")] string Principal
) : AwsOptions
{
    [CommandSwitch("--source-arn")]
    public string? SourceArn { get; set; }

    [CommandSwitch("--source-account")]
    public string? SourceAccount { get; set; }

    [CommandSwitch("--event-source-token")]
    public string? EventSourceToken { get; set; }

    [CommandSwitch("--qualifier")]
    public string? Qualifier { get; set; }

    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--principal-org-id")]
    public string? PrincipalOrgId { get; set; }

    [CommandSwitch("--function-url-auth-type")]
    public string? FunctionUrlAuthType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}