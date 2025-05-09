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

namespace Aras.OOTB.Tests.LoadTests;


public class LoadTests1 : OOTBTest
{
    private readonly ILogger _logger;
    public LoadTests1(DefaultArasSessionFixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _logger = OOTBTest.CreateLogger();
    }


    static List<QueryInfoDTO> queryResultInfos = new List<QueryInfoDTO>();
    static object lockObj = new object();

    private const int NumberOfThreadsToRun = 100;
    private const int MaxNumberOfQueriesPerThread = 100;
    private const int ProbabilityOnAddDocumentPercentage = 2;
    private const int ProbabilityOnAddPercentage = 4;
    private const int ProbabilityOnEditPercentage = 10;

    private const int MinSleepTimeBeforeAQueryMilliseconds = 2000;
    private const int MaxSleepTimeBeforeAQueryMilliseconds = 10000;
    
    [Fact]
    [Trait("Category", "LoadTest")]
    public void Load_on_find_Parts()
    {
        DateTime startTime = DateTime.Now;
        var stopwatch = new Stopwatch();
        stopwatch.Start();
                
        try
        {
            Thread[] threads = new Thread[NumberOfThreadsToRun];
            for (int i = 0; i < NumberOfThreadsToRun; i++)
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
            _logger.LogError(ex,"Hupp!");
            throw;
        }
        finally {
            stopwatch.Stop();
            LogResult(startTime, stopwatch);
        }
        
    }

    private void LoadTest(object data)
    {
        (int threadNumber, Innovator.Client.IOM.Innovator inn) = ((int, Innovator.Client.IOM.Innovator))data;
        Stopwatch stopwatch = new Stopwatch();
        
        var random = new Random();
        try
        {
                  
            int numberOfQueriesToDo = random.Next(1,MaxNumberOfQueriesPerThread+1);
            for (int i = 1; i <= numberOfQueriesToDo-1; i++)
            {         
                int sleepTime = random.Next(MinSleepTimeBeforeAQueryMilliseconds, MaxSleepTimeBeforeAQueryMilliseconds);
                Thread.Sleep(sleepTime); // Simulate work 
                
                stopwatch.Restart();

                IUserScenario userScenario = GetUserScenario();
                string action = userScenario.Description;
                Item item = userScenario.Run(inn);

                if (item == null) {
                    _logger.LogWarning("Item is null!"); // TODO: Check why this happens sometimes
                    continue;
                } 
                string itemType =  item.getType();
                string number = item.getProperty("item_number", "N/A");
                string message = $"'{itemType} Number = {number}', Query number for thread {threadNumber} : {i} of {numberOfQueriesToDo}";               
                
                stopwatch.Stop();
                
                var queryInfo = new QueryInfoDTO(action,stopwatch.ElapsedMilliseconds, threadNumber,sleepTime, message);
                lock (lockObj) {
                    queryResultInfos.Add(queryInfo);
                }
                _logger.LogInformation("{Action} in {ElapsedTimeMs}: {Message}", action, stopwatch.ElapsedMilliseconds, message);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"LoadTest: {ex.Message}");
            //throw;
        }
    }

    private IUserScenario GetUserScenario() {
        var random = new Random();
        int random100Number = random.Next(1, 101);
        // UGLY: These assumes that the of probabilities is in this specific order
        if (random100Number <= ProbabilityOnAddDocumentPercentage) {
            return new AddDocumentWithFile();
        }

        if (random100Number <= ProbabilityOnAddPercentage ) {
            return new AddPart();
        }

        if (random100Number <= ProbabilityOnEditPercentage) {
            return new UpdatePart();
        }
        return new GetPart();

    }

    private void LogResult(DateTime startTime, Stopwatch stopwatch)
    {
        string logPath = $@"C:\temp\load-test.log"; // TODO: Store in app-dir with timestamp in filename.
        using (var sw = new StreamWriter(logPath, true)) {
            sw.WriteLine("START SESSION");
            sw.WriteLine($"Session start time: {startTime.ToString("yyyy-MM-dd HH:mm:ss")}");
            string message = $"Total execution time: {stopwatch.ElapsedMilliseconds/1000} (s)";
            sw.WriteLine(message);
            message = $"Number of threads: {NumberOfThreadsToRun}";
            sw.WriteLine(message);
            message = $"Max number of queries per thread: {MaxNumberOfQueriesPerThread}";
            sw.WriteLine(message);
            message = $"Probability of add action  : {ProbabilityOnAddPercentage} %";
            sw.WriteLine(message);
            message = $"Min wait time before query  : {MinSleepTimeBeforeAQueryMilliseconds} (ms)";
            sw.WriteLine(message);
            message = $"Max wait time before query  : {MaxSleepTimeBeforeAQueryMilliseconds} (ms)";
            sw.WriteLine(message);

            int gotCount = 0;
            int addedCount = 0;

            foreach (var res in queryResultInfos)
            {
                sw.WriteLine(ConvertQueryInfoDtoToString(res));
                if (res.Action == "ADDED") addedCount++;
                if (res.Action == "GOT") gotCount++;
            }

            sw.WriteLine($"Query summary: GOT count = {gotCount}, ADDED count = {addedCount}");

            sw.WriteLine("END SESSION");
            sw.WriteLine();
        }
    }

    private string ConvertQueryInfoDtoToString(QueryInfoDTO queryInfo) {
        var sb = new StringBuilder();
        sb.Append($"[{queryInfo.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff")}]");
        sb.Append(", ");
        sb.Append($"[{queryInfo.Action}]");
        sb.Append(", ");
        sb.Append($"Execution time = {queryInfo.ExecutionTimeMilliseconds} (ms)" );
        sb.Append(", ");
        sb.Append($"Thread Number = {queryInfo.ThreadNumber}");
        sb.Append(", ");
        sb.Append($"Sleep time = {queryInfo.SleepTime} (ms)");
        sb.Append(", ");
        sb.Append($"{queryInfo.Note}");
        return sb.ToString();
    }

    private class QueryInfoDTO {

        public DateTime TimeStamp;
        public string Action;
        public long ExecutionTimeMilliseconds;
        public int ThreadNumber;
        public int SleepTime;
        public string Note;

        public QueryInfoDTO(string action, long executionTimeMilliseconds, int threadNumber, int sleepTime, string note) {
            TimeStamp = DateTime.Now;
            Action = action;
            ExecutionTimeMilliseconds = executionTimeMilliseconds;
            ThreadNumber = threadNumber;
            SleepTime = sleepTime;
            Note = note;
        }
    }
    

}

