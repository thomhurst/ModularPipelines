using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "search-resources")]
public record AwsWorkdocsSearchResourcesOptions : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--query-text")]
    public string? QueryText { get; set; }

    [CommandSwitch("--query-scopes")]
    public string[]? QueryScopes { get; set; }

    [CommandSwitch("--organization-id")]
    public string? OrganizationId { get; set; }

    [CommandSwitch("--additional-response-fields")]
    public string[]? AdditionalResponseFields { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--order-by")]
    public string[]? OrderBy { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}