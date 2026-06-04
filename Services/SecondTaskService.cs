namespace TestTaskCleverSoftware.Services
{
    public interface ISecondTaskService
    {
        public bool GetStatus();
        public int GetCount();
        public Task<int> AddToCount(int count, int delay = 0);

    }
    public class SecondTaskService : ISecondTaskService
    {
        public bool GetStatus() => Server.GetStatus();

        public int GetCount() => Server.GetCount();

        public async Task<int> AddToCount(int count, int delay = 0) => await Server.AddToCount(count, delay);
    }

    public static class Server
    {
        private static int _count;
        private static bool _isWriting;
        private static readonly ReaderWriterLockSlim LockSlim = new();

        public static int GetCount()
        {
            LockSlim.EnterReadLock();
            try
            {
                return _count;
            }
            finally
            {
                LockSlim.ExitReadLock();
            }
        }

        public static Task<int> AddToCount(int value, int delay = 0)
        {
            LockSlim.EnterWriteLock();
            try
            {
                _isWriting = true;
                _count += value;

                // Я знаю что в проде на куче юзеров это забъет тхред пул и приложение в лучшем случае встанет, но с семафором городить огород тут не хочу) А это для тестов 
                if (delay > 0) Thread.Sleep(delay);

                return Task.FromResult(_count);
            }
            finally
            {
                _isWriting = false;
                LockSlim.ExitWriteLock();
            }
        }

        public static bool GetStatus() => _isWriting;
    }
}
