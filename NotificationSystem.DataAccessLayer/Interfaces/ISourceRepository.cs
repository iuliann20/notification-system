using NotificationSystem.Models.Source;

namespace NotificationSystem.DataAccessLayer.Interfaces
{
    public interface ISourceRepository
    {
        Task<SourceDto> GetSourceById(int sourceId);
    }
}
