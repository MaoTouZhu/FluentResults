using System;
using System.Threading.Tasks;
using FluentResults.Samples.WebController;
using Hangfire;

namespace FluentResults.Samples.HangfireJobs
{
    public class CustomerJob
    {
        public static void AddOrUpdateJob()
        {
            RecurringJob.AddOrUpdate(() => Dispatch(), Cron.Daily);
        }

        public static Task<ResultDto> Dispatch()
        {
            // 一些业务逻辑
            Console.WriteLine("Fire-and-forget!");

            // 使用自定义 ResultDto 类，让序列化方式由你掌控
            // 尽可能晚地把 FluentResult 的 Result 对象转换成自定义 ResultDto 对象。
            return Task.FromResult(Result.Ok().ToResultDto());
        }
    }
}