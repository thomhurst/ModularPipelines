using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "create-analysis-template")]
public record AwsCleanroomsCreateAnalysisTemplateOptions(
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--format")] string Format,
[property: CommandSwitch("--source")] string Source
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--analysis-parameters")]
    public string[]? AnalysisParameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}