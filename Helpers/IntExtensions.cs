namespace TestTaskCleverSoftware.Helpers;

public static class IntExtensions
{
    public static string ToFormat(this int number) => number == 1 ? "" : $"{number}";
}