using System;
using System.Runtime.CompilerServices;
using System.Threading;

[assembly: InternalsVisibleTo("Test")]

namespace ТестовоеЗаданиеСleverence
{
    /// <summary>
    /// "Сервер"
    /// </summary>
    public static class Server
    {
        private static int _count;
        private static Mutex mutex = new Mutex();
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
            mutex.WaitOne();
            _count += value;
            mutex.ReleaseMutex();
            _isBlock = false;
        }

        /// <summary>
        /// Используется для теста - сбрасывает count в дефолтное значение
        /// </summary>
        internal static void DefaultCountTestRun()
        {
            _count = default;
        }
    }
}