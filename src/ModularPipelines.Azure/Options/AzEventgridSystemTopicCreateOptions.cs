using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("eventgrid", "system-topic", "create")]
public record AzEventgridSystemTopicCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--source")] string Source,
[property: CliOption("--topic-type")] string TopicType
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--mi-system-assigned")]
    public string? MiSystemAssigned { get; set; }

    [CliOption("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}