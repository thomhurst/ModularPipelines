namespace ModularPipelines.Engine;

public interface IOptionsProvider
{
    IEnumerable<object?> GetOptions();
}