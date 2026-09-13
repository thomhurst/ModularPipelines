using ModularPipelines.Terraform.Options;
using ModularPipelines.Terraform.Services;

namespace ModularPipelines.Terraform.UnitTests;

public class DeploymentStepOptionsTests
{
    [Test]
    [Arguments(typeof(TerraformStacksDeploymentStep), nameof(TerraformStacksDeploymentStep.ArtifactsAsync))]
    [Arguments(typeof(TerraformStacksDeploymentStep), nameof(TerraformStacksDeploymentStep.ShowAsync))]
    [Arguments(typeof(TerraformStacksDeploymentRun), nameof(TerraformStacksDeploymentRun.ShowAsync))]
    public async Task Commands_With_Required_Selectors_Cannot_Omit_Options(Type serviceType, string methodName)
    {
        var optionsParameter = serviceType.GetMethod(methodName)!.GetParameters()[0];

        await Assert.That(optionsParameter.IsOptional).IsFalse();
    }

    [Test]
    [Arguments(typeof(TerraformStacksDeploymentStepArtifactsOptions), 2)]
    [Arguments(typeof(TerraformStacksDeploymentStepShowOptions), 1)]
    [Arguments(typeof(TerraformStacksDeploymentRunShowOptions), 1)]
    public async Task Selector_Constructor_Arguments_Cannot_Be_Omitted(Type optionsType, int expectedCount)
    {
        var parameters = optionsType.GetConstructors().Single().GetParameters();

        await Assert.That(parameters.Length).IsEqualTo(expectedCount);
        await Assert.That(parameters.All(parameter => !parameter.IsOptional)).IsTrue();
    }
}
