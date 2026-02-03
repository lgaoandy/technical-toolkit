using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class AuditLogger : IAudioLogger
{
    private static readonly Dictionary<string, List<ActivityEntry>> _activityEntries = new(); 
}