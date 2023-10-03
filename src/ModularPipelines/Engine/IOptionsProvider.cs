namespace ModularPipelines.Engine;

internal interface IOptionsProvider
{
    IEnumerable<object?> GetOptions();
}