
using NCrontab;

namespace ContosoWorker;

public class LogTimes : BackgroundService, IHostedService
{
  private CrontabSchedule schedule;
  private DateTime nextRun;
  private readonly IServiceScopeFactory serviceScopeFactory;
  private string timer => "*/10 * * * * *";

  public LogTimes(IServiceScopeFactory serviceScopeFactory)
  {
    schedule = CrontabSchedule.Parse(timer, new CrontabSchedule.ParseOptions
    {
      IncludingSeconds = true
    });
    nextRun = schedule.GetNextOccurrence(DateTime.Now);
    this.serviceScopeFactory = serviceScopeFactory;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    do
    {
      var now = DateTime.Now;
      var nextrun = schedule.GetNextOccurrence(now);
      if(now > nextRun)
      {
        Process();
        nextRun = schedule.GetNextOccurrence(DateTime.Now);
      }
      await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
    }
    while(!stoppingToken.IsCancellationRequested);
  }

  private void Process()
  {
    try 
    {
      using (var scope = serviceScopeFactory.CreateScope())
      {
        Console.WriteLine("Waktu sekarang = " + DateTime.Now);
      }
    } 
    catch (Exception e)
    {
      Console.WriteLine(e.Message);
    }
  }
}