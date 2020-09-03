using System;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("Test")]

namespace TestСleverence
{
    /// <summary>
    /// "Server"
    /// </summary>
    public static class Server
    {
        private static int _count;
        private static readonly Mutex MutexObj = new Mutex();
        private static bool _isBlock;

        public static int GetCount()
        {
            if (!_isBlock)
                return _count;
            throw new Exception("Выполняет запись, ждите...");
        }

        public static void AddToCount(int value)
        {
            _isBlock = true;
            MutexObj.WaitOne();
            _count += value;
            MutexObj.ReleaseMutex();
            _isBlock = false;
        }

        /// <summary>
        /// OnlyTest
        /// </summary>
        internal static void DefaultCountTestRun()
        {
            _count = default;
        }
    }
}