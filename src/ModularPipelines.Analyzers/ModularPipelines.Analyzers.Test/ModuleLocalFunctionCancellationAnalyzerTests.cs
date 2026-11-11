using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleLocalFunctionCancellationAnalyzerTests
{
    [TestMethod]
    [DataRow("execute")]
    [DataRow("lifecycle")]
    [DataRow("configuration")]
    public async Task Recognizes_Token_Forwarded_To_Returned_Local_Function_In_Helper(string root)
    {
        var source = ModuleSource(root, """
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                Task RunAsync(CancellationToken localToken) => Task.Run(() => { }, localToken);
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source);
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_In_Local_Function_In_Helper(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                return RunAsync(token);

                async Task RunAsync(CancellationToken localToken)
                {
                    var unrelatedToken = new CancellationToken();
                    await {|#0:Task.Run(() => { }, {{argument}})|};
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Run"));
    }

    [TestMethod]
    [DataRow("execute", "CancellationToken.None")]
    [DataRow("execute", "unrelatedToken")]
    [DataRow("lifecycle", "CancellationToken.None")]
    [DataRow("lifecycle", "unrelatedToken")]
    [DataRow("configuration", "CancellationToken.None")]
    [DataRow("configuration", "unrelatedToken")]
    public async Task Reports_Unrelated_Token_Forwarded_To_Local_Function(string root, string argument)
    {
        var source = ModuleSource(root, $$"""
            private static Task HelperAsync(CancellationToken token)
            {
                var unrelatedToken = new CancellationToken();
                return {|#0:RunAsync({{argument}})|};

                async Task RunAsync(CancellationToken localToken)
                {
                    await {|#1:Task.Run(() => { }, localToken)|};
                }
            }
            """);

        await VerifyCS.VerifyAnalyzerAsync(source,
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("RunAsync"),
            VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(1).WithArguments("Run"));
    }

    private static string ModuleSource(string root, string helper)
    {
        var entry = root switch
        {
            "execute" => "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            "lifecycle" => "protected override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => HelperAsync(cancellationToken);",
            _ => "protected override void Configure(ModuleConfigurationBuilder module) => module.WithSkipWhen(async (context, cancellationToken) => { await HelperAsync(cancellationToken); return false; });",
        };

        return $$"""
            {{TestSourceConstants.StandardModuleHeader}}

            public sealed class BuildModule : Module
            {
                {{(root == "execute" ? "" : "protected override Task ExecuteAsync(IModuleContext context, CancellationToken cancellationToken) => Task.CompletedTask;")}}
                {{entry}}

                {{helper}}
            }
            """;
    }
}
