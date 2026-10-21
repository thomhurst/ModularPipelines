using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Testing.UnitTests;

public class ModuleTestBuilderTests
{
    [Test]
    public async Task BuildersAreSealedAndHaveNoPublicConstructors()
    {
        foreach (var type in new[]
        {
            typeof(ModuleTestBuilder<ValueModule>),
            typeof(ModuleTestBuilder<ValueModule, string>),
        })
        {
            using (Assert.Multiple())
            {
                await Assert.That(type.IsSealed).IsTrue();
                await Assert.That(type.GetConstructors()).IsEmpty();
            }
        }
    }

    [Test]
    public async Task SharedConfigurationPreservesTypedExecution()
    {
        var builder = ModuleTester.For<ValueModule, string>();
        ModuleTestBuilderBase<ValueModule, ModuleTestBuilder<ValueModule, string>> configuration = builder;

        var configured = configuration.WithTestInput();
        var run = await configured.ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(configured).IsSameReferenceAs(builder);
            await Assert.That(run.Value!.ToUpperInvariant()).IsEqualTo("FROM EXTENSION");
        }
    }

    [Test]
    public async Task SharedConfigurationSupportsTypeErasedExecution()
    {
        var builder = ModuleTester.For<ValueModule>();
        ModuleTestBuilderBase<ValueModule, ModuleTestBuilder<ValueModule>> configuration = builder;

        var configured = configuration.WithTestInput();
        var run = await configured.ExecuteAsync();

        using (Assert.Multiple())
        {
            await Assert.That(configured).IsSameReferenceAs(builder);
            await Assert.That(run.Value).IsEqualTo("from extension");
        }
    }

    [Test]
    public async Task SharedBaseCannotBeConstructedOrSubclassedOutsideTheAssembly()
    {
        var type = typeof(ModuleTestBuilderBase<,>);
        var constructors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        using (Assert.Multiple())
        {
            await Assert.That(type.IsAbstract).IsTrue();
            await Assert.That(constructors).Count().IsEqualTo(1);
            await Assert.That(constructors.Single().IsFamilyAndAssembly).IsTrue();
        }
    }

    public sealed class ValueModule : Module<string>
    {
        protected override Task<string> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            => context.Files.GetFile("input.txt").ReadAsync(cancellationToken);
    }
}

internal static class ModuleTestBuilderTestExtensions
{
    internal static TBuilder WithTestInput<TModule, TBuilder>(this ModuleTestBuilderBase<TModule, TBuilder> builder)
        where TModule : class, IModule
        where TBuilder : ModuleTestBuilderBase<TModule, TBuilder>
        => builder.ConfigurePipeline(_ => { }).WithFile("input.txt", "from extension");
}
