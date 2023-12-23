using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("osis", "create-pipeline")]
public record AwsOsisCreatePipelineOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--min-units")] int MinUnits,
[property: CommandSwitch("--max-units")] int MaxUnits,
[property: CommandSwitch("--pipeline-configuration-body")] string PipelineConfigurationBody
) : AwsOptions
{
    [CommandSwitch("--log-publishing-options")]
    public string? LogPublishingOptions { get; set; }

    [CommandSwitch("--vpc-options")]
    public string? VpcOptions { get; set; }

    [CommandSwitch("--buffer-options")]
    public string? BufferOptions { get; set; }

    [CommandSwitch("--encryption-at-rest-options")]
    public string? EncryptionAtRestOptions { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}