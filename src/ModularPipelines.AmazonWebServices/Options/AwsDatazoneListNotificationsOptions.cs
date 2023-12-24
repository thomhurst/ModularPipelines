using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datazone", "list-notifications")]
public record AwsDatazoneListNotificationsOptions(
[property: CommandSwitch("--domain-identifier")] string DomainIdentifier,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--after-timestamp")]
    public long? AfterTimestamp { get; set; }

    [CommandSwitch("--before-timestamp")]
    public long? BeforeTimestamp { get; set; }

    [CommandSwitch("--subjects")]
    public string[]? Subjects { get; set; }

    [CommandSwitch("--task-status")]
    public string? TaskStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}