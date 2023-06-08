namespace ModularPipelines.Exceptions;

internal class ModuleNotInitializedException : Exception
{
    public ModuleNotInitializedException(Type moduleType) : base($"Module {moduleType.Name} has not been initialized")
    {
    }
}