using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectcases", "search-cases")]
public record AwsConnectcasesSearchCasesOptions(
[property: CommandSwitch("--domain-id")] string DomainId
) : AwsOptions
{
    [CommandSwitch("--fields")]
    public string[]? Fields { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--search-term")]
    public string? SearchTerm { get; set; }

    [CommandSwitch("--sorts")]
    public string[]? Sorts { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}