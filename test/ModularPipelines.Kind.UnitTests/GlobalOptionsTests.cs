using System.Reflection;
using ModularPipelines.Kind.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Kind.UnitTests;

public class GlobalOptionsTests : TestBase
{
    [Test]
    [Arguments(true, 2, "kind --quiet --verbosity=2 create cluster --name=integration")]
    [Arguments(false, 0, "kind --verbosity=0 create cluster --name=integration")]
    [Arguments(null, null, "kind create cluster --name=integration")]
    public async Task Root_Settings_Precede_Nested_Commands(bool? quiet, int? verbosity, string expected)
    {
        var rendered = await RenderCommand(new KindCreateClusterOptions
        {
            Quiet = quiet,
            Verbosity = verbosity,
            Name = "integration",
        });
        await Assert.That(rendered).IsEqualTo(expected);
    }

    [Test]
    public async Task Local_Path_Value_Renders_After_Command()
    {
        var rendered = await RenderCommand(new KindCreateClusterOptions
        {
            Quiet = true,
            KubeConfig = "test directory/config",
        });
        await Assert.That(rendered)
            .IsEqualTo("kind --quiet create cluster --kubeconfig=test directory/config");
    }

    [Test]
    public async Task Every_Command_Inherits_Exactly_Two_Settings()
    {
        await Assert.That(typeof(KindOptions)
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(property => property.Name))
            .IsEquivalentTo(["Quiet", "Verbosity"]);

        foreach (var type in typeof(KindOptions).Assembly.GetTypes()
                     .Where(type => !type.IsAbstract && typeof(KindOptions).IsAssignableFrom(type)))
        {
            foreach (var name in new[] { "Quiet", "Verbosity" })
            {
                var property = type.GetProperties().Single(property => property.Name == name);
                await Assert.That(property.DeclaringType).IsEqualTo(typeof(KindOptions));
            }
        }
    }
}
