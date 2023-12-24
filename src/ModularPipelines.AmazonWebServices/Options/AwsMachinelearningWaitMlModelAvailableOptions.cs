using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("machinelearning", "wait", "ml-model-available")]
public record AwsMachinelearningWaitMlModelAvailableOptions : AwsOptions
{
    [CommandSwitch("--filter-variable")]
    public string? FilterVariable { get; set; }

    [CommandSwitch("--eq")]
    public string? Eq { get; set; }

    [CommandSwitch("--gt")]
    public string? Gt { get; set; }

    [CommandSwitch("--lt")]
    public string? Lt { get; set; }

    [CommandSwitch("--ge")]
    public string? Ge { get; set; }

    [CommandSwitch("--le")]
    public string? Le { get; set; }

    [CommandSwitch("--ne")]
    public string? Ne { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--sort-order")]
    public string? SortOrder { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}