using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("finspace", "update-kx-dataview")]
public record AwsFinspaceUpdateKxDataviewOptions(
[property: CommandSwitch("--environment-id")] string EnvironmentId,
[property: CommandSwitch("--database-name")] string DatabaseName,
[property: CommandSwitch("--dataview-name")] string DataviewName
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--changeset-id")]
    public string? ChangesetId { get; set; }

    [CommandSwitch("--segment-configurations")]
    public string[]? SegmentConfigurations { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}