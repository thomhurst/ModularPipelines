using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workdocs", "get-resources")]
public record AwsWorkdocsGetResourcesOptions : AwsOptions
{
    [CommandSwitch("--authentication-token")]
    public string? AuthenticationToken { get; set; }

    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--collection-type")]
    public string? CollectionType { get; set; }

    [CommandSwitch("--limit")]
    public int? Limit { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}