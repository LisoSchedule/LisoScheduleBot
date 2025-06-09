using Telegram.Bot.Types;
using LisoScheduleBot.Enums;
using LisoScheduleBot.Interfaces;
using LisoScheduleBot.Utils;
using LisoScheduleBot.Services;
using User = LisoScheduleBot.Models.User;

namespace LisoScheduleBot.Handlers.Settings;

public class VerifyingEmailHandler : IUserStepHandler
{
    private readonly IMessageService _messageService;
    private readonly IUserService _userService;
    private readonly JsonCodeService _codeService;

    public VerifyingEmailHandler(
        IMessageService messageService, 
        IUserService userService,
        JsonCodeService codeService)
    {
        _messageService = messageService;
        _userService = userService;
        _codeService = codeService;
    }

    public UserStep Step => UserStep.VerifyingEmail;

    public async Task Handle(Message message, User user)
    {
        var code = message.Text;
        var lastCode = await _codeService.GetOrCreateLastCode(user.UserId);

        if (!code!.Equals(lastCode.Code))
        {
            await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.CrossMark} Верифікаційний код введено невірно.\n\n" +
                $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
            replyMarkup: KeyboardFactory.Settings(user.Settings)
            );

            lastCode.IsUsed = true;
            await _codeService.SaveEntity(lastCode);

            user.Email = string.Empty;
            user.Step = UserStep.ChangeSettings;
            await _userService.SaveUser(user);
            return;
        }

        await _messageService.SendMessage(
            chatId: user.ChatId,
            text: $"{Emoji.CheckMark} Чудово, Email вказано.\n\n" +
                $"{Emoji.PhoneWithArrow} Обирай, що забажаєш.",
            replyMarkup: KeyboardFactory.Settings(user.Settings)
        );

        lastCode.IsUsed = true;
        await _codeService.SaveEntity(lastCode);

        user.Step = UserStep.ChangeSettings;
        await _userService.SaveUser(user);
    }
}
