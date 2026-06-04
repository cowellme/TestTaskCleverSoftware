using TestTaskCleverSoftware.Worker;

namespace TestTaskCleverSoftware.Services
{
    public interface IFirstTaskService
    {
        public Task<string> CompressStroke(string stroke);
        public Task<string> DecompressStroke(string stroke);
    }
    public class FirstTaskService(ILogger<FirstTask> logger) : IFirstTaskService
    {
        private readonly  FirstTask _firstTask = new(logger);

        public async Task<string> CompressStroke(string stroke) => await _firstTask.Compress(stroke);

        public async Task<string> DecompressStroke(string stroke) => await _firstTask.Decompress(stroke);
    }
}
