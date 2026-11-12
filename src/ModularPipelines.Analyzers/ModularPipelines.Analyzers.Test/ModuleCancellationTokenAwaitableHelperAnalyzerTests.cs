using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenAwaitableHelperAnalyzerTests
{
    [TestMethod]
    [DataRow("Task.FromResult(cancellationToken)", false)]
    [DataRow("Task.FromResult(CancellationToken.None)", true)]
    [DataRow("Task.FromResult(cancellationToken).ConfigureAwait(false)", false)]
    [DataRow("Task.FromResult(CancellationToken.None).ConfigureAwait(false)", true)]
    [DataRow("ValueTask.FromResult(cancellationToken)", false)]
    [DataRow("ValueTask.FromResult(CancellationToken.None)", true)]
    [DataRow("ValueTask.FromResult(cancellationToken).ConfigureAwait(false)", false)]
    [DataRow("ValueTask.FromResult(CancellationToken.None).ConfigureAwait(false)", true)]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken)", false)]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None)", true)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(cancellationToken))", false)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None))", true)]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken).AsTask()", false)]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None).AsTask()", true)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(cancellationToken)).ConfigureAwait(false)", false)]
    [DataRow("new ValueTask<CancellationToken>(Task.FromResult(CancellationToken.None)).ConfigureAwait(false)", true)]
    public async Task Tracks_Framework_Payloads(string expression, bool replaced)
    {
        await Verify($"var saved = {expression}; cancellationToken = default; cancellationToken = await saved;", replaced);
    }

    [TestMethod]
    [DataRow("Task.FromResult(cancellationToken)", false)]
    [DataRow("Task.FromResult(CancellationToken.None)", true)]
    [DataRow("ValueTask.FromResult(cancellationToken)", false)]
    [DataRow("ValueTask.FromResult(CancellationToken.None)", true)]
    [DataRow("new ValueTask<CancellationToken>(cancellationToken)", false)]
    [DataRow("new ValueTask<CancellationToken>(CancellationToken.None)", true)]
    public async Task Completed_Result_Tracks_Payload_Separately_From_Completion(string expression, bool replaced)
    {
        await Verify($"var saved = {expression}; await saved; cancellationToken = default; cancellationToken = saved.Result;", replaced);
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Condition_Conversion_Does_Not_Replace_Payload(string taskType, bool replaced)
    {
        var returned = replaced ? "CancellationToken.None" : "token";
        var helper = $"static {taskType}<CancellationToken> Preserve(CancellationToken token, Flag flag) => (bool)flag ? {taskType}.FromResult({returned}) : {taskType}.FromResult(token); ";
        await Verify(helper, replaced, """
            public struct Flag { public static explicit operator bool(Flag value) => true; }
            """, "await Preserve(cancellationToken, new Flag())");
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Rejects_Payload_Conversions(string taskType, bool wrapperConversion)
    {
        var expression = wrapperConversion
            ? $"({taskType}<CancellationToken>)(ReplacingAwaitable){taskType}.FromResult(token)"
            : $"{taskType}.FromResult((CancellationToken)(ReplacingToken)token)";
        var helper = $"static {taskType}<CancellationToken> Replace(CancellationToken token) => {expression}; ";
        await Verify(helper + "cancellationToken = await Replace(cancellationToken);", true, $$"""
            public struct ReplacingToken
            {
                public static explicit operator ReplacingToken(CancellationToken value) => new();
                public static explicit operator CancellationToken(ReplacingToken value) => default;
            }
            public struct ReplacingAwaitable
            {
                public static explicit operator ReplacingAwaitable({{taskType}}<CancellationToken> value) => new();
                public static explicit operator {{taskType}}<CancellationToken>(ReplacingAwaitable value) => {{taskType}}.FromResult(CancellationToken.None);
            }
            """);
    }

    [TestMethod]
    [DataRow("Task", false, false)]
    [DataRow("ValueTask", false, false)]
    [DataRow("Task", true, false)]
    [DataRow("ValueTask", true, false)]
    [DataRow("Task", false, true)]
    [DataRow("ValueTask", false, true)]
    [DataRow("Task", true, true)]
    [DataRow("ValueTask", true, true)]
    public async Task Constructed_Generic_Helpers_Track_Scalar_Inputs(string taskType, bool isAsync, bool replaced)
    {
        var mutation = replaced ? "token = default;" : "";
        var helper = isAsync
            ? $"static async {taskType}<T> Pass<T>(T token) where T : struct {{ await Task.Yield(); {mutation} return token; }} "
            : $"static {taskType}<T> Pass<T>(T token) where T : struct {{ {mutation} return {taskType}.FromResult(token); }} ";
        await Verify(helper + "cancellationToken = await Pass(cancellationToken);", replaced);
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Direct_Async_Scalar_Return_Ignores_Unrelated_Collections(string taskType)
    {
        await Verify($"static async {taskType}<CancellationToken> Pass(CancellationToken token) {{ await Task.Yield(); var unused = new CancellationToken[1]; return token; }} cancellationToken = await Pass(cancellationToken);", false);
    }

    [TestMethod]
    public async Task Preserves_Extension_And_Source_Helper_Forwarding()
    {
        await Verify("cancellationToken = await cancellationToken.Forward();", false, """
            public static class Helpers
            {
                public static Task<CancellationToken> Forward(this CancellationToken token) => Pass(token);
                private static Task<CancellationToken> Pass(CancellationToken token) => Task.FromResult(token);
            }
            """);
    }

    [TestMethod]
    public async Task User_Extension_With_Framework_Wrapper_Name_Does_Not_Prove_Payload()
    {
        await Verify("cancellationToken = cancellationToken.ConfigureAwait(false);", true, """
            public static class Helpers
            {
                public static CancellationToken ConfigureAwait(this CancellationToken token, bool value) => default;
            }
            """);
    }

    [TestMethod]
    public async Task Recursive_Helper_Terminates_Without_Proof()
    {
        await Verify("static Task<CancellationToken> Cycle(CancellationToken token) => Cycle(token); cancellationToken = await Cycle(cancellationToken);", true);
    }

    [TestMethod]
    public async Task Collection_Dependent_Helper_Remains_Conservative()
    {
        await Verify("static Task<CancellationToken> Pass(CancellationToken token) { var tokens = new[] { token }; tokens[0] = default; return Task.FromResult(tokens[0]); } cancellationToken = await Pass(cancellationToken);", true);
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Awaitable_Join_Requires_Every_Payload(bool replaced)
    {
        var alternate = replaced ? "CancellationToken.None" : "cancellationToken";
        await Verify($"var saved = context != null ? Task.FromResult(cancellationToken) : Task.FromResult({alternate}); cancellationToken = await saved;", replaced);
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Tuple_Parameter_Writes_Reject_Helper_Proof(string taskType, bool nested)
    {
        var write = nested ? "((token, _), _) = ((CancellationToken.None, 0), 0);" : "(token, _) = (CancellationToken.None, 0);";
        await Verify($"static {taskType}<CancellationToken> Pass(CancellationToken token) {{ {write} return {taskType}.FromResult(token); }} cancellationToken = await Pass(cancellationToken);", true);
        await Verify("cancellationToken = await Helpers.Pass(cancellationToken);", true,
            $"public static class Helpers {{ public static {taskType}<CancellationToken> Pass(CancellationToken token) {{ {write} return {taskType}.FromResult(token); }} }}");
    }

    [TestMethod]
    [DataRow(false)]
    [DataRow(true)]
    public async Task Overridable_Helper_Bodies_Do_Not_Prove_Runtime_Payload(bool throughOverride)
    {
        var type = throughOverride ? "ForwardingHelper" : "Helper";
        await Verify($"{type} helper = new ReplacingHelper(); cancellationToken = await helper.Pass(cancellationToken);", true, """
            public class Helper
            {
                public virtual Task<CancellationToken> Pass(CancellationToken token) => Task.FromResult(token);
            }
            public class ForwardingHelper : Helper
            {
                public override Task<CancellationToken> Pass(CancellationToken token) => Task.FromResult(token);
            }
            public class ReplacingHelper : ForwardingHelper
            {
                public override Task<CancellationToken> Pass(CancellationToken token) => Task.FromResult(CancellationToken.None);
            }
            """);
    }

    [TestMethod]
    public async Task Sealed_Override_Preserves_Payload()
    {
        await Verify("cancellationToken = await new ForwardingHelper().Pass(cancellationToken);", false, """
            public class Helper
            {
                public virtual Task<CancellationToken> Pass(CancellationToken token) => Task.FromResult(CancellationToken.None);
            }
            public class ForwardingHelper : Helper
            {
                public sealed override Task<CancellationToken> Pass(CancellationToken token) => Task.FromResult(token);
            }
            """);
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(true, true)]
    public async Task Custom_Token_Awaiter_Does_Not_Preserve_Payload(bool assign, bool helper)
    {
        var expression = helper ? "await Pass(cancellationToken)" : "await cancellationToken";
        var statements = helper ? "static async Task<CancellationToken> Pass(CancellationToken token) => await token; " : "";
        if (assign)
        {
            statements += $"cancellationToken = {expression};";
            expression = "cancellationToken";
        }

        await Verify(statements, true, """
            public static class Helpers
            {
                public static System.Runtime.CompilerServices.TaskAwaiter<CancellationToken> GetAwaiter(this CancellationToken token)
                    => Task.FromResult(CancellationToken.None).GetAwaiter();
            }
            """, expression);
    }

    [TestMethod]
    [DataRow("Task", false)]
    [DataRow("ValueTask", false)]
    [DataRow("Task", true)]
    [DataRow("ValueTask", true)]
    public async Task Ref_Alias_Writes_Reject_Helper_Proof(string taskType, bool replaced)
    {
        var mutation = replaced ? "alias = default;" : "";
        await Verify($"static {taskType}<CancellationToken> Pass(CancellationToken token) {{ ref var alias = ref token; {mutation} return {taskType}.FromResult(token); }} cancellationToken = await Pass(cancellationToken);", true);
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Conditional_Ref_Alias_Writes_Reject_Helper_Proof(string taskType)
    {
        await Verify($"static {taskType}<CancellationToken> Pass(CancellationToken token, bool choose) {{ ref var alias = ref (choose ? ref token : ref token); alias = default; return {taskType}.FromResult(token); }} cancellationToken = await Pass(cancellationToken, context != null);", true);
    }

    [TestMethod]
    [DataRow("Task", false, false)]
    [DataRow("ValueTask", false, false)]
    [DataRow("Task", true, false)]
    [DataRow("ValueTask", true, false)]
    [DataRow("Task", false, true)]
    [DataRow("ValueTask", false, true)]
    [DataRow("Task", true, true)]
    [DataRow("ValueTask", true, true)]
    public async Task Generic_Awaited_Return_Tracks_Mapped_Payload(string taskType, bool configured, bool replaced)
    {
        var value = replaced ? "default(T)" : "token";
        var configure = configured ? ".ConfigureAwait(false)" : "";
        await Verify($"static async {taskType}<T> Pass<T>(T token) where T : struct => await {taskType}.FromResult({value}){configure}; cancellationToken = await Pass(cancellationToken);", replaced);
    }

    [TestMethod]
    [DataRow("Task", false, false)]
    [DataRow("ValueTask", false, false)]
    [DataRow("Task", true, false)]
    [DataRow("ValueTask", true, false)]
    [DataRow("Task", false, true)]
    [DataRow("ValueTask", false, true)]
    [DataRow("Task", true, true)]
    [DataRow("ValueTask", true, true)]
    public async Task Generic_Awaited_Local_Tracks_Mapped_Payload(string taskType, bool configured, bool replaced)
    {
        var value = replaced ? "default(T)" : "token";
        var configure = configured ? ".ConfigureAwait(false)" : "";
        await Verify($"static async {taskType}<T> Pass<T>(T token) where T : struct {{ var saved = await {taskType}.FromResult({value}){configure}; return saved; }} cancellationToken = await Pass(cancellationToken);", replaced);
    }

    [TestMethod]
    [DataRow("Task")]
    [DataRow("ValueTask")]
    public async Task Generic_Custom_Awaiter_Does_Not_Prove_Mapped_Payload(string taskType)
    {
        await Verify($"static async {taskType}<T> Pass<T>(T token) where T : struct => await token; cancellationToken = await Pass(cancellationToken);", true, """
            public static class Helpers
            {
                public static System.Runtime.CompilerServices.TaskAwaiter<T> GetAwaiter<T>(this T token) where T : struct
                    => Task.FromResult(default(T)).GetAwaiter();
            }
            """);
    }

    [TestMethod]
    [DataRow(false, false)]
    [DataRow(true, false)]
    [DataRow(false, true)]
    [DataRow(true, true)]
    public async Task Captured_Tuple_Writes_Keep_Scalar_Analysis_Conservative(bool nested, bool lambda)
    {
        var write = nested ? "((cancellationToken, _), _) = ((saved, 0), 0);" : "(cancellationToken, _) = (saved, 0);";
        var helper = lambda ? $"System.Action restore = () => {{ {write} }}; restore();" : $"void Restore() {{ {write} }} Restore();";
        await Verify($"var saved = cancellationToken; cancellationToken = default; {helper}", false);
    }

    private static Task Verify(string statements, bool warns, string extra = "", string token = "cancellationToken")
    {
        var call = warns ? "{|#0:Task.Delay(1, " + token + ")|}" : "Task.Delay(1, " + token + ")";
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeader}}
            public class BuildModule : Module<List<string>>
            {
                {{TestSourceConstants.SimpleAsyncExecuteBody}}
                protected override async Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    {{statements}}
                    await {{call}};
                }
            }
            {{extra}}
            """;
        return VerifyCS.VerifyAnalyzerAsync(source, warns
            ? [VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId).WithLocation(0).WithArguments("Delay")]
            : []);
    }
}
