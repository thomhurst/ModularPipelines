using System.Reflection;
using ModularPipelines.Context;
using ModularPipelines.Helm.Options;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Helm.UnitTests;

public class HelmGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Version_Renders_Inherited_Settings_Once_After_Subcommand()
    {
        var command = await GetService<ICommandContext>();
        var result = await command.ExecuteCommandLineToolAsync(new HelmVersionOptions
        {
            Namespace = "build",
            KubeContext = "ci",
            KubeConfig = "config.yaml",
            KubeAsGroup = ["developers", "operators"],
            Short = true,
        }, new CommandExecutionOptions { InternalDryRun = true });

        await Assert.That(result.CommandInput).IsEqualTo(
            "helm version --short --kube-as-group=developers --kube-as-group=operators --kubeconfig=config.yaml --kube-context=ci --namespace=build");
    }

    [Test]
    public async Task Commands_Inherit_Global_Properties_Instead_Of_Declaring_Copies()
    {
        await Assert.That(typeof(HelmInstallOptions).GetProperty(nameof(HelmOptions.Namespace))!.DeclaringType)
            .IsEqualTo(typeof(HelmOptions));
        await Assert.That(typeof(HelmInstallOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
            .Any(property => property.Name == nameof(HelmOptions.KubeContext))).IsFalse();
    }
}
