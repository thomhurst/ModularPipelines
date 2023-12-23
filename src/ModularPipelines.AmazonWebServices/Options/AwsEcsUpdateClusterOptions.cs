using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "update-cluster")]
public record AwsEcsUpdateClusterOptions(
[property: CommandSwitch("--cluster")] string Cluster
) : AwsOptions
{
    [CommandSwitch("--settings")]
    public string[]? Settings { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--service-connect-defaults")]
    public string? ServiceConnectDefaults { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}