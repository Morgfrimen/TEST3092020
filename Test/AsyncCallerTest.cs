﻿using System;
using System.Threading;
using System.Threading.Channels;
using NUnit.Framework;

namespace Test
{
    [TestFixture]
    public class AsyncCallerTest
    {
        [Test]
        public void Test()
        {
            EventHandler h = new EventHandler(Test_Handler_4000ms);
            var ac = new AsyncCaller.AsyncCaller(h);
            bool completedOK = ac.Invoke(5000, null, EventArgs.Empty);
            Console.WriteLine("Меньше 5000");
            Assert.AreEqual(true, completedOK);

            h = new EventHandler(Test_Handler_6000ms);
            ac = new AsyncCaller.AsyncCaller(h);
            completedOK = ac.Invoke(5000, null, EventArgs.Empty);
            Console.WriteLine("БОЛЬШЕ 5000 мс => прерывание операции");
            Assert.AreEqual(false, completedOK);
        }

        private void Test_Handler_4000ms(object sender, EventArgs eventArgs)
        {
            Thread.Sleep(4000);
            Console.WriteLine($"Выполнился {nameof(Test_Handler_4000ms)}");
        }

        private void Test_Handler_6000ms(object sender, EventArgs eventArgs)
        {
            Thread.Sleep(6000);
            Console.WriteLine($"Выполнился {nameof(Test_Handler_6000ms)}");
        }
    }
}