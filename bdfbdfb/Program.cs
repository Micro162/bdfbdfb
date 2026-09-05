using System;
using System.Collections.Generic;
using System.Threading;

class CollectionPrinter
{
    public void ProcessCollection(IEnumerable<object> collection)
    {
        Thread receiverThread = new Thread(() => HandleCollection(collection));
        receiverThread.Start();
        receiverThread.Join();
    }

    private void HandleCollection(IEnumerable<object> collection)
    {
        List<Thread> workerThreads = new List<Thread>();

        foreach (var item in collection)
        {
            var localItem = item; 
            Thread worker = new Thread(() =>
            {
                string result = localItem.ToString();
                Console.WriteLine($"[Потік {Thread.CurrentThread.ManagedThreadId}] {result}");
            });

            workerThreads.Add(worker);
            worker.Start();
        }

        foreach (var t in workerThreads)
            t.Join();
    }
}

class Program
{
    static void Main()
    {
        var items = new List<object> { 1, "Привіт", 3.14, DateTime.Now, true };

        var printer = new CollectionPrinter();
        printer.ProcessCollection(items);

        Console.WriteLine("Обробку завершено.");
        Console.ReadLine();
    }
}