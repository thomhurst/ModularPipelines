using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("lambda", "add-permission")]
public record AwsLambdaAddPermissionOptions(
[property: CliOption("--function-name")] string FunctionName,
[property: CliOption("--statement-id")] string StatementId,
[property: CliOption("--action")] string Action,
[property: CliOption("--principal")] string Principal
) : AwsOptions
{
    [CliOption("--source-arn")]
    public string? SourceArn { get; set; }

    [CliOption("--source-account")]
    public string? SourceAccount { get; set; }

    [CliOption("--event-source-token")]
    public string? EventSourceToken { get; set; }

    [CliOption("--qualifier")]
    public string? Qualifier { get; set; }

    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--principal-org-id")]
    public string? PrincipalOrgId { get; set; }

    [CliOption("--function-url-auth-type")]
    public string? FunctionUrlAuthType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}