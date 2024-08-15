using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Notifications.WebUI.Models;
using Notifications.WebUI.Services;

namespace Notifications.WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly INotificationService _notificationService;

        public IndexModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public IEnumerable<NotificationEvent> Notifications { get; private set; }

        public async Task OnGetAsync()
        {
            Notifications = await _notificationService.GetNotificationsAsync();
        }

        public async Task<IActionResult> OnPostAsync(string message)
        {
            var notificationEvent = new NotificationEvent
            {
                Id = Guid.NewGuid().ToString(),
                Message = message,
                Status = "Pendente",
                CreatedAt = DateTime.UtcNow
            };

            await _notificationService.SendNotificationAsync(notificationEvent);
            return RedirectToPage();
        }
    }
}