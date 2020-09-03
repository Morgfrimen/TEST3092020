using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using ТестовоеЗаданиеСleverence;

namespace Test
{
    public sealed class Tests
    {
        /// <summary>
        /// Массив поток (читателей) для чтения параметра с сервера
        /// </summary>
        private Task<int>[] _tasksReaders;

        /// <summary>
        /// Массив задач тех, кто добавляет параметр
        /// </summary>
        private Task[] _tasksAddValue;

        public Tests()
        {
            DefaultTask();
        }

        private void _server_EventChangedCount()
        {
            Console.WriteLine($"Изменение состояния: {Server.GetCount()}");
        }

        private void DefaultTask()
        {
            _tasksReaders = new Task<int>[]
            {
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount),
                new Task<int>(Server.GetCount)
            };

            _tasksAddValue = new Task[]
            {
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5))),
                new Task((() => Server.AddToCount(5)))
            };

        }


        [Test,Order(1)]
        public void Test_ParallelReaders()
        {
            foreach (Task<int> reader in _tasksReaders)
            {
                reader.Start();
            }

            Task.WaitAll(_tasksReaders);
            foreach (Task<int> reader in _tasksReaders)
            {
                Console.WriteLine($"{reader.Id} : {reader.Status}");
                Assert.AreEqual(default(int),reader.Result);
            }
        }


        [Test,Order(2)]
        public void Test_ParallelAddValue()
        {
            int count = default;
            foreach (Task reader in _tasksAddValue)
            {
                reader.Start();
                count += 5;
            }

            Task.WaitAll(_tasksAddValue);
            Assert.AreEqual(count, Server.GetCount());
        }

        [Test,Order(3)]
        public void Test_AsyncGetCount()
        {
            Server.DefaultCountTestRun();
            DefaultTask();
            int count = default;
            for (int taskIndex = 0; taskIndex < _tasksReaders.Length; taskIndex++)
            {
                _tasksReaders[taskIndex].Start();
                Assert.AreEqual(count, _tasksReaders[taskIndex].Result);
                _tasksAddValue[taskIndex].Start();
                count += 5;
                Console.WriteLine($"Было: {Server.GetCount()}");
                Thread.Sleep(1);
                Assert.AreEqual(count,Server.GetCount());
                Console.WriteLine($"Стало: {Server.GetCount()}");
            }
        }


    }
}