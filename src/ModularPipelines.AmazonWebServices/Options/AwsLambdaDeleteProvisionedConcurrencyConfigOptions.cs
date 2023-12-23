using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("lambda", "delete-provisioned-concurrency-config")]
public record AwsLambdaDeleteProvisionedConcurrencyConfigOptions(
[property: CommandSwitch("--function-name")] string FunctionName,
[property: CommandSwitch("--qualifier")] string Qualifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}