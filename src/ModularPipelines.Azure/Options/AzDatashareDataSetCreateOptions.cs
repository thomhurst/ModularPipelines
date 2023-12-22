using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datashare", "data-set", "create")]
public record AzDatashareDataSetCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--data-set")] string DataSet,
[property: CommandSwitch("--data-set-name")] string DataSetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--share-name")] string ShareName
) : AzOptions;