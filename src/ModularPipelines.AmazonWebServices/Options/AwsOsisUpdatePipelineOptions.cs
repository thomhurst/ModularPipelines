using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("osis", "update-pipeline")]
public record AwsOsisUpdatePipelineOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CommandSwitch("--min-units")]
    public int? MinUnits { get; set; }

    [CommandSwitch("--max-units")]
    public int? MaxUnits { get; set; }

    [CommandSwitch("--pipeline-configuration-body")]
    public string? PipelineConfigurationBody { get; set; }

    [CommandSwitch("--log-publishing-options")]
    public string? LogPublishingOptions { get; set; }

    [CommandSwitch("--buffer-options")]
    public string? BufferOptions { get; set; }

    [CommandSwitch("--encryption-at-rest-options")]
    public string? EncryptionAtRestOptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}