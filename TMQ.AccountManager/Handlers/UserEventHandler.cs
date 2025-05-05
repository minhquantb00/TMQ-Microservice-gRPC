using TMQ.AccountCommands.Events;
using TMQ.AccountManager.Shared;
using TMQ.EventBus;

namespace TMQ.AccountManager.Handlers
{
    public class UserEventHandler(ILogger<UserEventHandler> logger, IAccountService accountService)
    : IEventHandler<UserAddEvent>, IEventHandler<UserChangeEvent>
    {
        public string WorkerGroup => string.Empty;

        public async Task Handle(UserAddEvent message, string topic)
        {
            await accountService.Process(message);
        }
        public async Task Handle(UserChangeEvent message, string topic)
        {
            await accountService.Process(message);
        }
    }
}
