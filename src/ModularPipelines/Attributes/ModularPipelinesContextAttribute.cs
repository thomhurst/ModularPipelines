using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ModularPipelines.Attributes;

/// <summary>
/// Identifies a generated integration registrar in an assembly.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class ModularPipelinesContextAttribute : Attribute
{
    private const string RegistrationMethodName = "Register";

    /// <summary>
    /// Initialises a new instance of the <see cref="ModularPipelinesContextAttribute"/> class.
    /// </summary>
    /// <param name="contextType">The generated registrar type.</param>
    public ModularPipelinesContextAttribute(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicMethods)]
        Type contextType)
    {
        ContextType = contextType;
    }

    /// <summary>
    /// Gets the generated registrar type.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.NonPublicMethods)]
    public Type ContextType { get; }

    internal void Register(IServiceCollection services)
    {
        var registrationMethod = ContextType.GetMethod(
            RegistrationMethodName,
            BindingFlags.NonPublic | BindingFlags.Static,
            binder: null,
            types: [typeof(IServiceCollection)],
            modifiers: null);

        if (registrationMethod is null || registrationMethod.ReturnType != typeof(void))
        {
            throw new InvalidOperationException(
                $"Integration registrar '{ContextType.FullName}' must expose "
                + $"a non-public static void {RegistrationMethodName}(IServiceCollection) method.");
        }

        registrationMethod.Invoke(null, [services]);
    }
}
