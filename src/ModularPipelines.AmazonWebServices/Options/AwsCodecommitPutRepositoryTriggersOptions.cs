using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codecommit", "put-repository-triggers")]
public record AwsCodecommitPutRepositoryTriggersOptions(
[property: CommandSwitch("--repository-name")] string RepositoryName,
[property: CommandSwitch("--triggers")] string[] Triggers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}