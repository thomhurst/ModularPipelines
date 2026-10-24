namespace ModularPipelines.Validation;

internal static class TimeoutValidation
{
    private static readonly TimeSpan MaximumTimeout = TimeSpan.FromMilliseconds(uint.MaxValue - 1);

    public static bool IsValid(TimeSpan timeout, bool allowZero = false) =>
        timeout == Timeout.InfiniteTimeSpan
        || (timeout <= MaximumTimeout && (allowZero ? timeout >= TimeSpan.Zero : timeout > TimeSpan.Zero));

    public static string GetErrorMessage(string optionName, bool allowZero = false) =>
        $"{optionName} must be {(allowZero ? "non-negative" : "positive")} and no greater than "
        + $"{uint.MaxValue - 1} milliseconds, or Timeout.InfiniteTimeSpan.";

    public static void ThrowIfInvalid(TimeSpan timeout, string optionName, bool allowZero = false)
    {
        if (!IsValid(timeout, allowZero))
        {
            throw new ArgumentOutOfRangeException(optionName, timeout, GetErrorMessage(optionName, allowZero));
        }
    }
}
