using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "list-ca-certificates")]
public record AwsIotListCaCertificatesOptions : AwsOptions
{
    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--template-name")]
    public string? TemplateName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}