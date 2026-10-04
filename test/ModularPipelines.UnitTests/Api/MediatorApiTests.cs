namespace ModularPipelines.UnitTests.Api;

public class MediatorApiTests
{
    [Test]
    public async Task GeneratedMediatorTypesAreInternal()
    {
        var assembly = typeof(IModuleContext).Assembly;
        var generatedTypeNames = new[]
        {
            "Mediator.Mediator",
            "Mediator.MediatorOptions",
            "Mediator.MediatorOptionsAttribute",
            "Mediator.AssemblyReference",
            "Microsoft.Extensions.DependencyInjection.MediatorDependencyInjectionExtensions",
        };

        foreach (var typeName in generatedTypeNames)
        {
            var type = assembly.GetType(typeName);
            await Assert.That(type).IsNotNull();
            await Assert.That(type!.IsNotPublic).IsTrue();
        }

        await Assert.That(assembly.ExportedTypes)
            .DoesNotContain(type => type.Namespace == "Mediator");
    }
}
