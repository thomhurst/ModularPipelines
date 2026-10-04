using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.TestHelpers;
using ModularPipelines.UnitTests.Logging;
using ModularPipelines.Context;
using ModularPipelines.Secrets;

namespace ModularPipelines.UnitTests.Context;

public class SecurityContextTests
{
    [Test]
    public async Task Security_Context_Exposes_Secret_Registry()
    {
        var property = typeof(ISecurityContext).GetProperty("Secrets");
        await Assert.That(property).IsNotNull();
        await Assert.That(property!.PropertyType).IsEqualTo(typeof(ISecretRegistry));
    }

    [Test]
    public async Task Runtime_Secrets_Use_The_Pipeline_Registry_And_Configured_Masking()
    {
        var secret = $"runtime-secret-{Guid.NewGuid():N}";
        var output = new StringBuilder();
        var module = new RuntimeSecretModule(secret, registerSecret: true);
        await using var pipeline = await CreateBuilder(module, output).BuildAsync();

        await pipeline.RunAsync();

        await Assert.That(module.Registry).IsSameReferenceAs(pipeline.Services.GetRequiredService<ISecretRegistry>());
        await Assert.That(output.ToString()).Contains("[runtime-mask]");
        await Assert.That(output.ToString()).DoesNotContain(secret);
    }

    [Test]
    public async Task Runtime_Secrets_Are_Isolated_Between_Pipelines()
    {
        var secret = $"isolated-secret-{Guid.NewGuid():N}";
        var firstOutput = new StringBuilder();
        var secondOutput = new StringBuilder();
        var firstModule = new RuntimeSecretModule(secret, registerSecret: true);
        var secondModule = new RuntimeSecretModule(secret, registerSecret: false);
        await using var first = await CreateBuilder(firstModule, firstOutput).BuildAsync();
        await using var second = await CreateBuilder(secondModule, secondOutput).BuildAsync();

        await first.RunAsync();
        await second.RunAsync();

        await Assert.That(firstOutput.ToString()).DoesNotContain(secret);
        await Assert.That(secondOutput.ToString()).Contains(secret);
        await Assert.That(ReferenceEquals(firstModule.Registry, secondModule.Registry)).IsFalse();
    }

    private static PipelineBuilder CreateBuilder(RuntimeSecretModule module, StringBuilder output)
    {
        var builder = TestPipelineBuilder.Create();
        builder.Services.AddSingleton<ILogger<RuntimeSecretModule>>(new StringLogger<RuntimeSecretModule>(output));
        return builder.AddModule(module)
            .ConfigureSecrets(options => options with { MaskValue = "[runtime-mask]" });
    }

    private sealed class RuntimeSecretModule(string secret, bool registerSecret) : Module<bool>
    {
        public ISecretRegistry? Registry { get; private set; }

        protected internal override Task<bool> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            Registry = context.Security.Secrets;
            if (registerSecret)
            {
                Registry.AddSecret(secret);
            }

            context.Logger.LogInformation("Discovered token: {Token}", secret);
            return Task.FromResult(true);
        }
    }
}
