using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using User = LisoScheduleBot.Models.User;
using LisoScheduleBot.Services;

namespace LisoScheduleBot.Handlers.Settings;

public class ChangingEmailHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    private readonly IEmailService _emailService;
    private readonly JsonCodeService _codeService;

    public ChangingEmailHandler(
        IMessageService messageService, 
        IUserService userService,
        IEmailService emailService,
        JsonCodeService codeService)
    {
        _messageService = messageService;
        _userService = userService;
        _emailService = emailService;
        _codeService = codeService;
    }

    public UserStep Step => UserStep.ChangingEmail;

    public async Task Handle(Message message, User user)
    {
        var email = message.Text;

        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.LockWithKey} Верифікаційний код відправлено на введений Email.",
            replyMarkup: KeyboardFactory.InputCancel()
        );

        user.Email = email;
        user.Step = UserStep.VerifyEmail;
        await _userService.SaveUser(user);

        var code = await _codeService.GetOrCreateLastCode(user.UserId);
        await _codeService.SaveEntity(code);
        await _emailService.SendMessage(
            email: email!,
            subject: $"{Emoji.LockWithKey} Верифікаційний код",
            body: $"{Emoji.Key} Код: <strong>{code.Code}</strong>"
        );
    }
}
