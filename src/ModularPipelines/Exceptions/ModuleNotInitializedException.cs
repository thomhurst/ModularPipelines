namespace ModularPipelines.Exceptions;

internal class ModuleNotInitializedException : Exception
{
    public ModuleNotInitializedException(Type moduleType) : base($"Module {moduleType.Name} has not been initialized. Avoid doing any work in the constructor other than assigning fields via Dependency Injection.")
    {
    }
}
