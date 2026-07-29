using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ModularPipelines.Engine;

internal static class AssemblyTypeLoader
{
    [RequiresUnreferencedCode("Calls System.Reflection.Assembly.GetTypes()")]
    internal static IEnumerable<Type> GetLoadableTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            return exception.Types.OfType<Type>();
        }
    }
}
