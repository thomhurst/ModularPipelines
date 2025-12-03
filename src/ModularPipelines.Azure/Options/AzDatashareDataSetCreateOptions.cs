using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datashare", "data-set", "create")]
public record AzDatashareDataSetCreateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--data-set")] string DataSet,
[property: CliOption("--data-set-name")] string DataSetName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--share-name")] string ShareName
) : AzOptions;