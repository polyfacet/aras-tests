using Xunit.Abstractions;
using Innovator.Client.IOM;
using Aras.OOTB.Tests.Fixture;
using System.Diagnostics;
using Aras.Core.Tests.ArasExtensions;
using Aras.OOTB.Tests.Models;
using System.Text;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Aras.Core.Tests;
using Aras.OOTB.Tests.LoadTests.UserScenario;
using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Aras.OOTB.Tests.LoadTests;

class LoadTestConfig
    {
        public const int NumberOfThreadsToRun = 100;
        public  const int MaxNumberOfQueriesPerThread  = 100;
        public const int ProbabilityOnAddDocumentPercentage  = 2;
        public const int ProbabilityOnAddPercentage = 4;
        public const int ProbabilityOnEditPercentage = 10;
        public const int MinSleepTimeBeforeAQueryMilliseconds = 2000;
        public const int MaxSleepTimeBeforeAQueryMilliseconds = 10000;
}

public class LoadTests1 : OOTBTest
{
    private readonly ILogger _logger;
    private static readonly ConcurrentBag<QueryInfoDTO> QueryResultInfos = new();
    private static readonly ThreadLocal<Random> ThreadRandom = new(() => new Random(Guid.NewGuid().GetHashCode()));


    public LoadTests1(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _logger = OOTBTest.CreateLogger();
    }

    [Fact]
    [Trait("Category", "LoadTest")]
    public void Load_on_find_Parts()
    {
        DateTime startTime = DateTime.Now;
        var stopwatch = Stopwatch.StartNew();

        try
        {
            Thread[] threads = new Thread[LoadTestConfig.NumberOfThreadsToRun];
            for (int i = 0; i < LoadTestConfig.NumberOfThreadsToRun; i++)
            {
                threads[i] = new Thread(LoadTest);
                threads[i].Start((i, AdminInn));
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Hupp!");
            throw;
        }
        finally
        {
            stopwatch.Stop();
            LogResult(startTime, stopwatch);
        }
    }

    private void LoadTest(object data)
    {
        (int threadNumber, Innovator.Client.IOM.Innovator inn) = ((int, Innovator.Client.IOM.Innovator))data;
        var random = ThreadRandom.Value!;
        try
        {
            int numberOfQueriesToDo = random.Next(1, LoadTestConfig.MaxNumberOfQueriesPerThread + 1);
            for (int i = 1; i < numberOfQueriesToDo; i++)
            {
                int sleepTime = random.Next(LoadTestConfig.MinSleepTimeBeforeAQueryMilliseconds, LoadTestConfig.MaxSleepTimeBeforeAQueryMilliseconds);
                Thread.Sleep(sleepTime);

                var stopwatch = Stopwatch.StartNew();

                IUserScenario userScenario = GetUserScenario(random);
                string action = userScenario.Description;
                Item item = userScenario.Run(inn);

                if (item == null)
                {
                    _logger.LogWarning("Item is null!"); // TODO: Check why this happens sometimes
                    continue;
                }
                string itemType = item.getType();
                string number = item.getProperty("item_number", "N/A");
                string message = $"'{itemType} Number = {number}', Query number for thread {threadNumber} : {i} of {numberOfQueriesToDo}";

                stopwatch.Stop();

                var queryInfo = new QueryInfoDTO(action, stopwatch.ElapsedMilliseconds, threadNumber, sleepTime, message);
                QueryResultInfos.Add(queryInfo);

                _logger.LogInformation("{Action} in {ElapsedTimeMs}: {Message}", action, stopwatch.ElapsedMilliseconds, message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"LoadTest: {ex.Message}");
        }
    }

    private IUserScenario GetUserScenario(Random random)
    {
        int random100Number = random.Next(1, 101);
        if (random100Number <= LoadTestConfig.ProbabilityOnAddDocumentPercentage)
            return new AddDocumentWithFile();
        if (random100Number <= LoadTestConfig.ProbabilityOnAddPercentage)
            return new AddPart();
        if (random100Number <= LoadTestConfig.ProbabilityOnEditPercentage)
            return new UpdatePart();
        return new GetPart();
    }

    private void LogResult(DateTime startTime, Stopwatch stopwatch)
    {
        string logPath = @"C:\temp\load-test.log"; // TODO: Store in app-dir with timestamp in filename.
        using var sw = new StreamWriter(logPath, true);
        sw.WriteLine("START SESSION");
        sw.WriteLine($"Session start time: {startTime:yyyy-MM-dd HH:mm:ss}");
        sw.WriteLine($"Total execution time: {stopwatch.ElapsedMilliseconds / 1000} (s)");
        sw.WriteLine($"Number of threads: {LoadTestConfig.NumberOfThreadsToRun}");
        sw.WriteLine($"Max number of queries per thread: {LoadTestConfig.MaxNumberOfQueriesPerThread}");
        sw.WriteLine($"Probability of add action  : {LoadTestConfig.ProbabilityOnAddPercentage} %");
        sw.WriteLine($"Min wait time before query  : {LoadTestConfig.MinSleepTimeBeforeAQueryMilliseconds} (ms)");
        sw.WriteLine($"Max wait time before query  : {LoadTestConfig.MaxSleepTimeBeforeAQueryMilliseconds} (ms)");

        int gotCount = 0, addedCount = 0;
        foreach (var res in QueryResultInfos)
        {
            sw.WriteLine(ConvertQueryInfoDtoToString(res));
            if (res.Action == "ADDED") addedCount++;
            if (res.Action == "GOT") gotCount++;
        }
        sw.WriteLine($"Query summary: GOT count = {gotCount}, ADDED count = {addedCount}");
        sw.WriteLine("END SESSION");
        sw.WriteLine();
    }

    private string ConvertQueryInfoDtoToString(QueryInfoDTO queryInfo)
    {
        var sb = new StringBuilder();
        sb.Append($"[{queryInfo.TimeStamp:yyyy-MM-dd HH:mm:ss.fff}]");
        sb.Append(", ");
        sb.Append($"[{queryInfo.Action}]");
        sb.Append(", ");
        sb.Append($"Execution time = {queryInfo.ExecutionTimeMilliseconds} (ms)");
        sb.Append(", ");
        sb.Append($"Thread Number = {queryInfo.ThreadNumber}");
        sb.Append(", ");
        sb.Append($"Sleep time = {queryInfo.SleepTime} (ms)");
        sb.Append(", ");
        sb.Append($"{queryInfo.Note}");
        return sb.ToString();
    }

    private class QueryInfoDTO
    {
        public DateTime TimeStamp { get; }
        public string Action { get; }
        public long ExecutionTimeMilliseconds { get; }
        public int ThreadNumber { get; }
        public int SleepTime { get; }
        public string Note { get; }

        public QueryInfoDTO(string action, long executionTimeMilliseconds, int threadNumber, int sleepTime, string note)
        {
            TimeStamp = DateTime.Now;
            Action = action;
            ExecutionTimeMilliseconds = executionTimeMilliseconds;
            ThreadNumber = threadNumber;
            SleepTime = sleepTime;
            Note = note;
        }
    }
 
}