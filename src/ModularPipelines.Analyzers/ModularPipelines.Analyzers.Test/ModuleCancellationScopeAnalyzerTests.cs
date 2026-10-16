using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationScopeAnalyzerTests
{
    [TestMethod]
    [DataRow("OnBeforeExecuteAsync", false)]
    [DataRow("OnBeforeExecuteAsync", true)]
    [DataRow("OnAfterExecuteAsync", false)]
    [DataRow("OnAfterExecuteAsync", true)]
    [DataRow("OnSkippedAsync", false)]
    [DataRow("OnSkippedAsync", true)]
    [DataRow("OnCachedResultAsync", false)]
    [DataRow("OnCachedResultAsync", true)]
    [DataRow("OnFailedAsync", false)]
    [DataRow("OnFailedAsync", true)]
    public async Task Checks_Token_Forwarding_In_Lifecycle_Overrides(string hook, bool forwardsToken)
    {
        var extraParameter = hook switch
        {
            "OnAfterExecuteAsync" or "OnCachedResultAsync" => "ModuleResult<List<string>> result, ",
            "OnSkippedAsync" => "SkipDecision skipDecision, ",
            "OnFailedAsync" => "Exception exception, ",
            _ => string.Empty,
        };
        var hasResult = hook == "OnAfterExecuteAsync";
        var call = forwardsToken ? "Task.Delay(1, cancellationToken)" : "{|#0:Task.Delay(1)|}";
        var source = ModuleSource($$"""
            protected override async {{(hasResult ? "Task<ModuleResult<List<string>>?>" : "Task")}} {{hook}}(
                IModuleContext context, {{extraParameter}}CancellationToken cancellationToken)
            {
                await {{call}};
                {{(hasResult ? "return null;" : string.Empty)}}
            }
            """);

        await VerifyTokenAsync(source, forwardsToken);
    }

    [TestMethod]
    public async Task Checks_Token_In_NonAsync_Lifecycle_Override()
    {
        var source = ModuleSource("""
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                {|#0:Task.Delay(1)|};
            """);

        await VerifyTokenAsync(source, false);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Checks_Token_In_Lifecycle_Helper(bool forwardsToken)
    {
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var source = ModuleSource($$"""
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) =>
                WorkAsync(cancellationToken);

            private static async Task WorkAsync(CancellationToken token)
            {
                await {{call}};
            }
            """);

        await VerifyTokenAsync(source, forwardsToken);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Checks_Token_In_Override_Of_Inherited_Lifecycle_Hook(bool forwardsToken)
    {
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public abstract class BaseModule : Module<List<string>>
            {
                protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken token) =>
                    Task.CompletedTask;
            }

            public class BuildModule : BaseModule
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}

                protected override async Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken token)
                {
                    await {{call}};
                }
            }
            """;

        await VerifyTokenAsync(source, forwardsToken);
    }

    [TestMethod]
    [DataRow("WithSkipWhen", false)]
    [DataRow("WithSkipWhen", true)]
    [DataRow("WithIgnoreFailuresWhen", false)]
    [DataRow("WithIgnoreFailuresWhen", true)]
    public async Task Checks_Token_In_Async_Configuration_Callback(string configurationMethod, bool forwardsToken)
    {
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var parameters = configurationMethod == "WithIgnoreFailuresWhen" ? "context, exception, token" : "context, token";
        var source = ModuleSource($$"""
            protected override void Configure(ModuleConfigurationBuilder module) =>
                module.{{configurationMethod}}(async ({{parameters}}) =>
                {
                    await {{call}};
                    return false;
                });
            """);

        await VerifyTokenAsync(source, forwardsToken);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Checks_Token_In_Method_Group_Configuration_Callback(bool forwardsToken)
    {
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var source = ModuleSource($$"""
            protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(SkipAsync);

            private static async ValueTask<bool> SkipAsync(IModuleContext context, CancellationToken token)
            {
                await {{call}};
                return false;
            }
            """);

        await VerifyTokenAsync(source, forwardsToken);
    }

    [TestMethod]
    public async Task Checks_Token_In_Callback_Registered_Outside_Module()
    {
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public static class Registration
            {
                public static void Configure(ModuleConfigurationBuilder module) =>
                    module.WithSkipWhen(async (context, token) =>
                    {
                        await {|#0:Task.Delay(1)|};
                        return false;
                    });
            }
            """;

        await VerifyTokenAsync(source, false);
    }

    [TestMethod]
    public async Task Does_Not_Report_Callback_Without_Available_Token()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module) =>
                module.WithSkipWhen(context =>
                {
                    _ = Task.Delay(1);
                    return false;
                });
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Unrelated_SameNamed_Callback_Method()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module) =>
                new OtherBuilder().WithSkipWhen(async (context, token) =>
                {
                    await Task.Delay(1);
                    return false;
                });

            private sealed class OtherBuilder
            {
                public void WithSkipWhen(Func<IModuleContext, CancellationToken, ValueTask<bool>> callback) { }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Unrelated_SameNamed_Override()
    {
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public class OtherBase
            {
                protected virtual Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken token) =>
                    Task.CompletedTask;
            }

            public class Other : OtherBase
            {
                protected override async Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken token)
                {
                    await Task.Delay(1);
                }
            }
            """;

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Report_Uninvoked_Local_Function_In_Lifecycle_Hook()
    {
        var source = ModuleSource("""
            protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
            {
                async Task UnusedAsync() { await Task.Delay(1); }
                return Task.CompletedTask;
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    public async Task Does_Not_Treat_Uninvoked_Nested_Callback_As_Configuration_Root()
    {
        var source = ModuleSource("""
            protected override void Configure(ModuleConfigurationBuilder module) =>
                module.WithSkipWhen(async (context, token) =>
                {
                    Func<CancellationToken, Task> unused = async otherToken => { await Task.Delay(1); };
                    await Task.Delay(1, token);
                    return false;
                });
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Checks_Token_In_Combined_Skip_Callbacks(bool forwardsToken)
    {
        var call = forwardsToken ? "Task.Delay(1, token)" : "{|#0:Task.Delay(1)|}";
        var source = ModuleSource($$"""
            protected override void Configure(ModuleConfigurationBuilder module) =>
                module.WithSkipWhenAll(async (context, token) =>
                {
                    await {{call}};
                    return SkipDecision.DoNotSkip;
                });
            """);

        await VerifyTokenAsync(source, forwardsToken);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Checks_Token_In_NonAsync_Configuration_Callback(bool forwardsToken)
    {
        var call = forwardsToken ? "ReadAsync(token)" : "{|#0:ReadAsync()|}";
        var source = ModuleSource($$"""
            protected override void Configure(ModuleConfigurationBuilder module) =>
                module.WithSkipWhen((context, token) => {{call}});

            private static ValueTask<bool> ReadAsync(CancellationToken token = default) =>
                new ValueTask<bool>(false);
            """);

        if (forwardsToken)
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(
                source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0)
                    .WithArguments("ReadAsync"));
        }
    }

    private static string ModuleSource(string members) => $$"""
        {{TestSourceConstants.StandardUsings}}
        using ModularPipelines.Models;

        {{TestSourceConstants.ExamplesNamespace}}

        public class BuildModule : Module<List<string>>
        {
            {{TestSourceConstants.SimpleAsyncExecuteBody}}

            {{members}}
        }
        """;

    private static Task VerifyTokenAsync(string source, bool forwardsToken) =>
        forwardsToken
            ? VerifyCS.VerifyAnalyzerAsync(source)
            : VerifyCS.VerifyAnalyzerAsync(
                source,
                VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                    .WithLocation(0)
                    .WithArguments("Delay"));
}
