
using InformationService.DataAccess;
using InformationService.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace InformationService.Background;

public class DeleteOldPosts(IServiceProvider serviceProvider, ILogger<DeleteOldPosts> logger) : BackgroundService
{
    private readonly DateTime _cutOffDate = DateTime.UtcNow.AddDays(-7);
    private readonly TimeSpan _sleepPeriod = TimeSpan.FromDays(1);
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationContext>();

            var toDelete = await context.Posts
                .Where(p => p.History.Any())
                .Select(p => new
                {
                    Post = p,
                    LastHistory = p.History
                        .OrderByDescending(h => h.UpdateTime)
                        .First()
                })
                .Where(x => x.LastHistory != null &&
                            x.LastHistory.EditType == EditType.Deleted &&
                            x.LastHistory.UpdateTime <= _cutOffDate)
                .Select(x => x.Post)
                .ToListAsync();

            logger.LogInformation("Приступаем к очистке...");
            if (toDelete.Any())
            {
                context.Posts.RemoveRange(toDelete);
                int count = await context.SaveChangesAsync();
                logger.LogInformation($"Удалено {count} статей");
            }

            await Task.Delay(_sleepPeriod);
        }
    }
}
