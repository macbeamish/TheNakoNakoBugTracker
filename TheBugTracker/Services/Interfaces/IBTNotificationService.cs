using TheBugTracker.Models;

namespace TheBugTracker.Services
{
    public interface IBTNotificationService
    {
        public Task AddNotificationAsync(Notification notification);

        public Task<List<Notification>> GetReceivedNotificationsAsync(string userid);

        public Task<List<Notification>> GetSentNotificationsAsync(string userid);  
        
        public Task SendEmailNotificationsByRoleAsync(Notification notification, int companyId, string role);

        public Task SendMembersEmailNotificationsAsync(Notification notification, List<BTUser> members);

        public Task<bool> SendEmailNotificationAsync(Notification notification, string emailSubject); 

    }
}
