using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "list-type-versions")]
public record AwsCloudformationListTypeVersionsOptions : AwsOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--type-name")]
    public string? TypeName { get; set; }

    [CommandSwitch("--arn")]
    public string? Arn { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--deprecated-status")]
    public string? DeprecatedStatus { get; set; }

    [CommandSwitch("--publisher-id")]
    public string? PublisherId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}