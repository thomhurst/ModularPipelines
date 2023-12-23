using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("servicecatalog", "create-constraint")]
public record AwsServicecatalogCreateConstraintOptions(
[property: CommandSwitch("--portfolio-id")] string PortfolioId,
[property: CommandSwitch("--product-id")] string ProductId,
[property: CommandSwitch("--parameters")] string Parameters,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--accept-language")]
    public string? AcceptLanguage { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}