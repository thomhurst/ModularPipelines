using System.Diagnostics.CodeAnalysis;
using System.Reflection;
namespace ModularPipelines.UnitTests.Api;

public class PipelineBuilderApiAnnotationTests
{
    [Test]
    public async Task Builder_Extensions_Are_In_Root_Namespace()
    {
        await Assert.That(typeof(PipelineBuilderExtensions).Namespace)
            .IsEqualTo("ModularPipelines");
    }

    [Test]
    public async Task Runtime_Type_Registration_Warns_Trim_And_Aot_Callers()
    {
        var method = typeof(PipelineBuilderExtensions).GetMethod(
            nameof(PipelineBuilderExtensions.AddModules),
            [typeof(PipelineBuilder), typeof(Type[])]);

        using (Assert.Multiple())
        {
            await Assert.That(method).IsNotNull();
            await Assert.That(method!.GetCustomAttribute<RequiresUnreferencedCodeAttribute>())
                .IsNotNull();
            await Assert.That(method.GetCustomAttribute<RequiresDynamicCodeAttribute>())
                .IsNotNull();
        }
    }

    [Test]
    public async Task Result_History_Warns_Trim_And_Aot_Callers()
    {
        var method = typeof(PipelineBuilderExtensions)
            .GetMethods()
            .Single(candidate =>
                candidate.Name == nameof(PipelineBuilderExtensions.AddResultsRepository)
                && candidate.IsGenericMethodDefinition);

        using (Assert.Multiple())
        {
            await Assert.That(method.GetCustomAttribute<RequiresUnreferencedCodeAttribute>())
                .IsNotNull();
            await Assert.That(method.GetCustomAttribute<RequiresDynamicCodeAttribute>())
                .IsNotNull();
        }
    }
}
