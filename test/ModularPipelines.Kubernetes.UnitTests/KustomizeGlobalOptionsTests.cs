using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Kubernetes.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kubernetes.UnitTests;

public class KustomizeGlobalOptionsTests : TestBase
{
    [Test]
    [Arguments(true, "kustomize --stack-trace build overlays/test")]
    [Arguments(false, "kustomize build overlays/test")]
    [Arguments(null, "kustomize build overlays/test")]
    public async Task Stack_Trace_Precedes_Command_And_Operand(bool? stackTrace, string expected)
    {
        var rendered = await RenderCommand(new KustomizeBuildOptions { StackTrace = stackTrace, Dir = "overlays/test" });
        await Assert.That(rendered).IsEqualTo(expected);
    }

    [Test]
    public async Task Nested_Command_Keeps_Repeated_Values_Separate()
    {
        var builder = await GetService<ICommandLineBuilder>();
        var command = builder.Build(new KustomizeEditAddConfigmapOptions("settings")
        {
            StackTrace = true,
            FromLiteral = ["first=hello world", "second=value"],
        });
        await Assert.That(string.Join("|", command.Arguments))
            .IsEqualTo("--stack-trace|edit|add|configmap|settings|--from-literal=first=hello world|--from-literal=second=value");
    }

    [Test]
    public async Task Only_Stack_Trace_Is_Inherited()
    {
        await Assert.That(typeof(KustomizeOptions)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(property => property.Name))
            .IsEquivalentTo(["StackTrace"]);

        foreach (var type in typeof(KustomizeOptions).Assembly.GetTypes()
                     .Where(type => !type.IsAbstract && typeof(KustomizeOptions).IsAssignableFrom(type)))
        {
            var property = type.GetProperties().Single(property => property.Name == "StackTrace");
            await Assert.That(property.DeclaringType).IsEqualTo(typeof(KustomizeOptions));
        }

        await Assert.That(typeof(KubernetesOptions).GetProperty("StackTrace")).IsNull();
    }
}
