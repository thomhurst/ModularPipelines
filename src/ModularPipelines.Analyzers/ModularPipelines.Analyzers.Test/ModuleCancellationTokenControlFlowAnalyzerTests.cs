using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = ModularPipelines.Analyzers.Test.Verifiers.CSharpAnalyzerVerifier<ModularPipelines.Analyzers.ModuleAsyncSafetyAnalyzer>;

namespace ModularPipelines.Analyzers.Test;

[TestClass]
public class ModuleCancellationTokenControlFlowAnalyzerTests
{
    public static IEnumerable<object[]> LoopCases()
    {
        foreach (var token in new[] { "token", "cancellationToken" })
        {
            foreach (var exit in new[] { "break;", "return null!;", "throw new System.Exception();" })
            {
                yield return ["exclusive-" + token + exit, $$"""
                    var original = cancellationToken;
                    var token = cancellationToken;
                    for (var i = 0; i < 2; i++)
                    {
                        if (context is null) {{token}} = CancellationToken.None;
                        else await CALL(1, {{token}});
                        {{token}} = original;
                        {{exit}}
                    }
                    """, false];
                yield return ["opposite-tail-" + token + exit, $$"""
                    var original = cancellationToken;
                    var token = cancellationToken;
                    for (var i = 0; i < 2; i++)
                    {
                        if (context is null) { {{token}} = CancellationToken.None; {{exit}} }
                        else { await CALL(1, {{token}}); {{token}} = original; }
                    }
                    """, false];
            }

            yield return ["real-back-edge-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["continue-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; continue; }
                    else await CALL(1, {{token}});
                    {{token}} = original;
                }
                """, true];
            yield return ["head-reset-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    {{token}} = original;
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, false];
            yield return ["tail-reset-" + token, $$"""
                var original = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                    {{token}} = original;
                }
                """, false];
            yield return ["self-reset-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    {{token}} = {{token}};
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["conditional-exit-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) {{token}} = CancellationToken.None;
                    else await CALL(1, {{token}});
                    if (context is null) break;
                }
                """, true];
            yield return ["nested-break-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; do { break; } while (true); }
                    else await CALL(1, {{token}});
                }
                """, true];
            yield return ["nested-continue-outer-break-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    if (context is null) { {{token}} = CancellationToken.None; for (var j = 0; j < 2; j++) continue; }
                    else await CALL(1, {{token}});
                    break;
                }
                """, false];
            yield return ["outer-carry-" + token, $$"""
                var token = cancellationToken;
                for (var i = 0; i < 2; i++)
                {
                    for (var j = 0; j < 2; j++)
                    {
                        if (context is null) {{token}} = CancellationToken.None;
                        else await CALL(1, {{token}});
                        break;
                    }
                }
                """, true];
        }

        foreach (var dependency in new[] { "cancellationToken", "input" })
        {
            yield return ["do-condition-" + dependency, $$"""
                var input = cancellationToken;
                var token = cancellationToken;
                do
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                } while (({{dependency}} = CancellationToken.None).CanBeCanceled);
                """, true];
            yield return ["for-iterator-" + dependency, $$"""
                var input = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++, {{dependency}} = token)
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, true];
            yield return ["for-initializer-" + dependency, $$"""
                var original = cancellationToken;
                var input = cancellationToken;
                var token = cancellationToken;
                for ({{dependency}} = original; context is not null;)
                {
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, false];
            yield return ["independent-refresh-" + dependency, $$"""
                var original = cancellationToken;
                var input = cancellationToken;
                var token = cancellationToken;
                for (var i = 0; i < 2; i++, {{dependency}} = token)
                {
                    {{dependency}} = original;
                    token = {{dependency}};
                    if (context is null) token = CancellationToken.None;
                    else await CALL(1, token);
                }
                """, false];
        }

        yield return ["forty-conditional-copies", string.Join("\n", Enumerable.Range(0, 40).Select(i =>
            $"var copy{i} = context is null ? {(i == 0 ? "cancellationToken" : $"copy{i - 1}")} : {(i == 0 ? "cancellationToken" : $"copy{i - 1}")};"))
            + "for (var i = 0; i < 2; i++) await CALL(1, copy39);", false];
        yield return ["linked-source-in-loop", """
            for (var i = 0; i < 2; i++)
            {
                using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
                var token = linked.Token;
                await CALL(1, token);
            }
            """, false];
        yield return ["conditional-snapshot-safe", """
            var snapshot = context is null ? cancellationToken : cancellationToken;
            var token = cancellationToken;
            for (var i = 0; i < 2; i++)
            {
                token = snapshot;
                if (context is null) token = CancellationToken.None;
                else await CALL(1, token);
            }
            """, false];
        yield return ["conditional-snapshot-unsafe", """
            var snapshot = context is null ? cancellationToken : CancellationToken.None;
            for (var i = 0; i < 2; i++) await CALL(1, snapshot);
            """, true];
    }

    [TestMethod]
    [DynamicData(nameof(LoopCases))]
    public async Task Tracks_Loop_Reaching_Values(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body, warning);
    }

    public static IEnumerable<object[]> ExceptionCases()
    {
        yield return ["return-does-not-throw", """
            var token = cancellationToken;
            try { if (context is null) { token = default; return null!; } }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["throwing-path", """
            var token = cancellationToken;
            try { token = default; throw new System.Exception(); }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["catch-restores", """
            var token = cancellationToken;
            try { token = default; throw new System.Exception(); }
            catch { token = cancellationToken; }
            await CALL(1, token);
            """, false];
        yield return ["sibling-rethrow", """
            var token = cancellationToken;
            try { if (context is null) { token = default; throw new System.InvalidOperationException(); } }
            catch (System.InvalidOperationException) { throw; }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["finally-restores", """
            var token = cancellationToken;
            try { token = default; }
            finally { token = cancellationToken; }
            await CALL(1, token);
            """, false];
        yield return ["finally-throws-to-catch", """
            var token = cancellationToken;
            try { try { token = default; return null!; } finally { throw new System.Exception(); } }
            catch { }
            await CALL(1, token);
            """, true];
        yield return ["sibling-catch-rethrow-does-not-continue", """
            var token = cancellationToken;
            try { _ = context.ToString(); }
            catch (System.InvalidOperationException) { token = default; throw; }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["nested-catch-restores-before-outer", """
            var token = cancellationToken;
            try
            {
                try { token = default; throw new System.Exception(); }
                catch { token = cancellationToken; }
            }
            catch { }
            await CALL(1, token);
            """, false];
        yield return ["nested-finally-restores-before-catch", """
            var token = cancellationToken;
            try
            {
                try { token = default; throw new System.Exception(); }
                finally { token = cancellationToken; }
            }
            catch { }
            await CALL(1, token);
            """, false];
        foreach (var cleanup in new[] { "using (var resource = new Resource())", "await using (var resource = new Resource())",
            "foreach (var item in new Sequence())", "await foreach (var item in new Sequence())" })
        {
            foreach (var handler in new[] { "", "return null!;", "token = cancellationToken;" })
            {
                yield return [cleanup + handler, $$"""
                    var token = cancellationToken;
                    try { {{cleanup}} { token = default; return null!; } }
                    catch { {{handler}} }
                    await CALL(1, token);
                    """, handler.Length == 0];
            }
        }

        foreach (var collection in new[] { "new[] { 1 }", "\"text\"" })
        {
            yield return ["no-dispose-" + collection, $$"""
                var token = cancellationToken;
                try { foreach (var item in {{collection}}) { token = default; return null!; } }
                catch { }
                await CALL(1, token);
                """, false];
        }
    }

    [TestMethod]
    [DynamicData(nameof(ExceptionCases))]
    public async Task Tracks_Exception_And_Cleanup_Values(string name, string body, bool warning)
    {
        _ = name;
        await Verify(body, warning);
    }

    private static async Task Verify(string body, bool warning)
    {
        body = body.Replace("CALL", warning ? "{|#0:Task.Delay|}" : "Task.Delay");
        // Mark the invocation, rather than only its method name.
        body = body.Replace("{|#0:Task.Delay|}(1, token)", "{|#0:Task.Delay(1, token)|}")
            .Replace("{|#0:Task.Delay|}(1, cancellationToken)", "{|#0:Task.Delay(1, cancellationToken)|}")
            .Replace("{|#0:Task.Delay|}(1, snapshot)", "{|#0:Task.Delay(1, snapshot)|}")
            .Replace("{|#0:Task.Delay|}(1, copy39)", "{|#0:Task.Delay(1, copy39)|}");
        var source = $$"""
            {{TestSourceConstants.StandardModuleHeaderWithExtensions}}
            public class BuildModule : Module<List<string>>
            {
                protected override async Task<List<string>> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
                {
                    {{body}}
                    return null!;
                }
            }
            public class Resource : System.IDisposable, System.IAsyncDisposable
            {
                public void Dispose() => throw new System.Exception();
                public System.Threading.Tasks.ValueTask DisposeAsync() => throw new System.Exception();
            }
            public class Sequence : System.Collections.Generic.IEnumerable<int>, System.Collections.Generic.IAsyncEnumerable<int>
            {
                public System.Collections.Generic.IEnumerator<int> GetEnumerator() => throw new System.Exception();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
                public System.Collections.Generic.IAsyncEnumerator<int> GetAsyncEnumerator(CancellationToken cancellationToken = default) => throw new System.Exception();
            }
            """;
        if (warning)
        {
            await VerifyCS.VerifyAnalyzerAsync(source, VerifyCS.Diagnostic(ModuleAsyncSafetyAnalyzer.UnflowedCancellationTokenId)
                .WithLocation(0).WithArguments("Delay"));
        }
        else
        {
            await VerifyCS.VerifyAnalyzerAsync(source);
        }
    }
}
