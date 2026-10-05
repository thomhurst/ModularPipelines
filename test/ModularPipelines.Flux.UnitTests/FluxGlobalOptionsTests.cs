using System.Globalization;
using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Flux.Options;
using ModularPipelines.Secrets;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.Flux.UnitTests;

public class FluxGlobalOptionsTests : TestBase
{
    [Test]
    public async Task Inherited_Settings_Follow_The_Command_Path()
    {
        var rendered = await RenderCommand(new FluxGetSourcesGitOptions { Namespace = "platform" });
        await Assert.That(rendered).IsEqualTo("flux get sources git --namespace=platform");
        await Assert.That(typeof(FluxOptions).GetProperty(nameof(FluxOptions.Namespace))!
            .GetCustomAttribute<CliOptionAttribute>()!.ShortForm).IsEqualTo("-n");
    }

    [Test]
    public async Task Repeated_Values_Keep_Their_Token_Boundaries()
    {
        var arguments = OptionsRenderingTestHelper.BuildArguments(new FluxGetSourcesGitOptions
        {
            AsGroup = ["team one", "readers"],
            AsUserExtra = ["team=platform", "team=delivery"],
        });
        await OptionsRenderingTestHelper.AssertArguments(arguments,
            ["--as-group=team one", "--as-group=readers", "--as-user-extra=team=platform", "--as-user-extra=team=delivery"]);
    }

    [Test]
    [Arguments(null)]
    [Arguments(false)]
    public async Task Unset_And_False_Presence_Flags_Are_Omitted(bool? value)
    {
        var rendered = await RenderCommand(new FluxGetSourcesGitOptions
        {
            DisableCompression = value,
            InsecureSkipTlsVerify = value,
            NsFollowsKubeContext = value,
            Verbose = value,
        });
        await Assert.That(rendered).IsEqualTo("flux get sources git");
    }

    [Test]
    public async Task Numeric_Zero_And_Fractional_Qps_Use_Invariant_Formatting()
    {
        var previous = CultureInfo.CurrentCulture;
        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("fr-FR");
            var rendered = await RenderCommand(new FluxGetSourcesGitOptions { KubeApiBurst = 0, KubeApiQps = 12.5 });
            await Assert.That(rendered).IsEqualTo("flux get sources git --kube-api-burst=0 --kube-api-qps=12.5");
        }
        finally
        {
            CultureInfo.CurrentCulture = previous;
        }
    }

    [Test]
    [Arguments(false)]
    [Arguments(true)]
    public async Task Receiver_Token_Set_Through_Base_Uses_Local_Metadata_And_One_Value(bool trigger)
    {
        FluxOptions options = trigger
            ? new FluxTriggerReceiverOptions { Name = "receiver" }
            : new FluxCreateSecretReceiverOptions { Name = "receiver" };
        options.Token = "webhook-fixture";
        var property = options.GetType().GetProperty(nameof(FluxOptions.Token))!;
        await Assert.That(property.DeclaringType).IsEqualTo(options.GetType());
        await Assert.That(property.GetCustomAttribute<SecretValueAttribute>()).IsNotNull();
        await Assert.That(property.GetValue(options)).IsEqualTo("webhook-fixture");
        var path = trigger ? "trigger receiver" : "create secret receiver";
        await Assert.That(await RenderCommand(options)).IsEqualTo($"flux {path} receiver --token=webhook-fixture");
        property.SetValue(options, "updated-webhook");
        await Assert.That(options.Token).IsEqualTo("updated-webhook");
    }

    [Test]
    public async Task All_Commands_Inherit_Settings_Without_Promoting_Group_Flags()
    {
        var globals = typeof(FluxOptions).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        await Assert.That(globals.Length).IsEqualTo(23);
        var commands = typeof(FluxOptions).Assembly.GetTypes()
            .Where(type => !type.IsAbstract && typeof(FluxOptions).IsAssignableFrom(type)).ToArray();
        await Assert.That(commands.Length).IsEqualTo(173);
        foreach (var command in commands)
        {
            foreach (var global in globals)
            {
                var property = command.GetProperty(global.Name)!;
                var isReceiverToken = global.Name == nameof(FluxOptions.Token)
                    && (command == typeof(FluxCreateSecretReceiverOptions) || command == typeof(FluxTriggerReceiverOptions));
                await Assert.That(property.DeclaringType).IsEqualTo(isReceiverToken ? command : typeof(FluxOptions));
            }
        }

        await Assert.That(typeof(FluxOptions).GetProperty("AllNamespaces")).IsNull();
        await Assert.That(typeof(FluxOptions).GetProperty("Branch")).IsNull();
        await Assert.That(typeof(FluxOptions).GetProperty(nameof(FluxOptions.Token))!
            .GetCustomAttribute<SecretValueAttribute>()).IsNotNull();
    }

    [Test]
    public async Task Derived_Override_Renders_Once()
    {
        await Assert.That(await RenderCommand(new CustomOptions { Namespace = "custom" }))
            .IsEqualTo("flux get sources git --namespace=custom");
    }

    private record CustomOptions : FluxGetSourcesGitOptions
    {
        public override string? Namespace { get; set; }
    }
}
