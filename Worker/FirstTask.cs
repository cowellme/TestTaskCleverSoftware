using System.Text;
using TestTaskCleverSoftware.Helpers;

namespace TestTaskCleverSoftware.Worker
{
    public class FirstTask(ILogger<FirstTask> logger)
    {
        public async Task<string> Compress(string stroke)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var array = stroke.ToCharArray();
                    var length = array.Length;
                    if (length <= 0) return "";

                    var result = new StringBuilder(length);
                    Dictionary<char, int>? currentChar = null;

                    for (var i = 0; i < length; i++)
                    {
                        if (currentChar == null)
                        {
                            result.Append($"{array[i]}");
                            currentChar = new Dictionary<char, int> { { array[i], 1 } };
                            continue;
                        }

                        if (currentChar.ContainsKey(array[i]))
                        {
                            currentChar[array[i]]++;
                            // result.Append(currentChar[array[i - 1]] == 1 ? "" : $"{currentChar[array[i - 1]]}");
                            if (i == length - 1) result.Append(currentChar[array[i - 1]].ToFormat());
                            continue;
                        }

                        result.Append($"{currentChar[array[i - 1]].ToFormat()}{array[i]}");
                        currentChar = new Dictionary<char, int> { { array[i], 1 } };
                    }

                    return result.ToString();
                }
                catch (Exception e)
                {
                    logger.LogWarning(e.Message);
                    throw;
                }
            });
        }
        public async Task<string> Decompress(string stroke)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var array = stroke.ToCharArray();
                    var length = array.Length;
                    if (length <= 0) return "";

                    var result = new StringBuilder(stroke.Length);
                    var num = "";
                    var currentChar = ' ';
                    for (var i = 0; i < length; i++)
                    {
                        if (IsDigital(array[i]))
                        {
                            num += array[i];
                            if (i == length - 1) WriteOfNumber(num, result, currentChar);
                            continue;
                        }

                        WriteOfNumber(num, result, currentChar);
                        num = "";

                        result.Append(array[i]);
                        currentChar = array[i];
                    }

                    return result.ToString();
                }
                catch (Exception e)
                {
                    logger.LogWarning(e.Message);
                    throw;
                }
            });
        }
        private static void WriteOfNumber(string num, StringBuilder result, char currentChar)
        {
            if (!int.TryParse(num, out var resultNumber)) return;

            for (var j = 1; j < resultNumber; j++)
                result.Append(currentChar);
        }
        private static bool IsDigital(char ch) => int.TryParse(ch.ToString(), out _);
    }
}

