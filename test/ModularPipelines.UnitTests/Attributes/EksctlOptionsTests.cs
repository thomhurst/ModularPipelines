using ModularPipelines.Eksctl.Enums;
using ModularPipelines.Eksctl.Options;
using ModularPipelines.Helpers.Internal;

namespace ModularPipelines.UnitTests.Attributes;

public class EksctlOptionsTests
{
    private readonly CommandModelProvider _modelProvider = new();
    private readonly CommandArgumentBuilder _argumentBuilder = new();

    [Test]
    public async Task CreateCluster_Renders_Scalar_Collections_And_Flags()
    {
        var arguments = BuildArguments(new EksctlCreateClusterOptions
        {
            Name = "production",
            Region = "eu-west-2",
            Zones = ["eu-west-2a", "eu-west-2b"],
            Nodes = 3,
            Managed = true,
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--name=production",
            "--region=eu-west-2",
            "--zones=eu-west-2a",
            "--zones=eu-west-2b",
            "--nodes=3",
            "--managed",
        ]);
    }

    [Test]
    public async Task CreateCapability_Renders_Enum_Wire_Value()
    {
        var arguments = BuildArguments(new EksctlCreateCapabilityOptions
        {
            Name = "gitops",
            Type = EksctlCreateCapabilityType.Argocd,
            Cluster = "production",
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--name=gitops",
            "--type=ARGOCD",
            "--cluster=production",
        ]);
    }

    [Test]
    public async Task UpdateClusterLogging_Renders_Repeated_Enum_Values()
    {
        var arguments = BuildArguments(new EksctlUtilsUpdateClusterLoggingOptions
        {
            Cluster = "production",
            Approve = true,
            EnableTypes =
            [
                EksctlUtilsUpdateClusterLoggingEnableTypes.Api,
                EksctlUtilsUpdateClusterLoggingEnableTypes.Controllermanager,
            ],
        });

        await Assert.That(arguments).IsEquivalentTo(
        [
            "--cluster=production",
            "--approve",
            "--enable-types=api",
            "--enable-types=controllerManager",
        ]);
    }

    private List<string> BuildArguments(object options)
    {
        var model = _modelProvider.GetCommandModel(options.GetType());
        return _argumentBuilder.BuildArguments(model, options);
    }
}
