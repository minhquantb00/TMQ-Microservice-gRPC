using Microsoft.AspNetCore.Authentication;
using System.Web;
using TMQ.AccountCommands.Commands;
using TMQ.AccountCommands.Events;
using TMQ.AccountManager.Shared;
using TMQ.AccountReadModels;
using TMQ.BaseCommands;
using TMQ.Common;
using TMQ.BaseApplication.Services;
using TMQ.Config;
using TMQ.EsRepositories;
using TMQ.EnumDefine;
using TMQ.AccountDomains.Entities;

namespace TMQ.AccountManager.Services
{
    public class AccountService(
    ILogger<AuthenticationService> logger,
    ContextService contextService,
    UserRepositoryResolver userRepositoryResolver,
    ICommonService commonService,
    OtpUtility otpUtility,
    IESRepository esRepository
) : BaseService(logger, contextService), IAccountService
    {

        public async Task<BaseCommandResponse<string>> Add(AccountAddCommand command)
        {
            return await ProcessCommand<string>(async (response) =>
            {
                var results = AccountAddRequestValidator.ValidateModel(command);
                if (!results.IsValid)
                {
                    response.SetFail(results.Errors.Select(p => p.ToString()));
                    return;
                }

                var userRepository = userRepositoryResolver(command.AccountType);
                if (command.UserName?.Length > 0)
                {
                    var account =
                        await userRepository.GetByUserNameOrEmailOrPhoneNumber(command.UserName);
                    if (account != null && account.Status != AccountStatusEnum.Deleted)
                    {
                        response.SetFail("UserName already exists");
                        return;
                    }
                }

                if (command.Email?.Length > 0)
                {
                    var account =
                        await userRepository.GetByUserNameOrEmailOrPhoneNumber(command.Email);
                    if (account != null && account.Status != AccountStatusEnum.Deleted)
                    {
                        response.SetFail("Email address already exists");
                        return;
                    }
                }

                if (command.PhoneNumber?.Length > 0)
                {
                    string mobile = command.PhoneNumber;
                    if (CommonUtility.MobileValid(ref mobile))
                    {
                        command.PhoneNumber = mobile;
                    }
                    else
                    {
                        response.SetFail("PhoneNumber invalid");
                        return;
                    }

                    var account =
                        await userRepository.GetByUserNameOrEmailOrPhoneNumber(command.PhoneNumber);
                    if (account != null && account.Status != AccountStatusEnum.Deleted)
                    {
                        response.SetFail("Phone number already exists");
                        return;
                    }
                }

                var id = await commonService.GetNextCode(new GetNextCodeQuery()
                {
                    LoginUid = command.LoginUid.AsEmpty(),
                    ProcessUid = command.ProcessUid.AsEmpty(),
                    Prefix = "C",
                    IsDigit = true,
                    TypeName = typeof(RUser).FullName,
                });
                //var user = new User(command, id);
                //user.AddOtpTypeAndAuthenticatorSecretKey(command, otpUtility.GenerateRandomKey);
                await userRepository.Add(user);
                response.Data = user.Id;
                //EventAdd(user.ToAddEvent());

                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Change(AccountChangeCommand command)
        {
            return await ProcessCommand(command, async (response) =>
            {
                var results = AccountChangeRequestValidator.ValidateModel(command);
                if (!results.IsValid)
                {
                    response.SetFail(results.Errors.Select(p => p.ToString()));
                    return;
                }

                var userRepository = userRepositoryResolver(command.AccountType);
                var account = await userRepository.Get(command.ObjectId!);
                if (account == null)
                {
                    response.SetFail("account not exists");
                    return;
                }

                if (command.PhoneNumber?.Length > 0)
                {
                    string mobile = command.PhoneNumber;
                    if (CommonUtility.MobileValid(ref mobile))
                    {
                        command.PhoneNumber = mobile;
                    }
                    else
                    {
                        response.SetFail("PhoneNumber invalid");
                        return;
                    }

                    var accountPhone =
                        await userRepository.GetByUserNameOrEmailOrPhoneNumber(command.PhoneNumber);
                    if (accountPhone != null && accountPhone.Id != account.Id)
                    {
                        response.SetFail("phone number already exists");
                        return;
                    }
                }

                if (command.Email?.Length > 0)
                {
                    var accountEmail =
                        await userRepository.GetByUserNameOrEmailOrPhoneNumber(command.Email);
                    if (accountEmail != null && accountEmail.Id != account.Id)
                    {
                        response.SetFail("email address already exists");
                        return;
                    }
                }

                var user = new User(account);
                user.Change(command);
                await userRepository.Change(user);
                EventAdd(user.ToChangeEvent());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> SetPassword(SetPasswordCommand command)
        {
            return await ProcessCommand(command, async (response) =>
            {
                var results = ResetPasswordRequestValidator.ValidateModel(command);
                if (!results.IsValid)
                {
                    response.SetFail(results.Errors.Select(p => p.ToString()));
                    return;
                }

                var userRepository = userRepositoryResolver(command.Type);
                var account = await userRepository.Get(command.ObjectId!);
                if (account == null)
                {
                    response.SetFail("account not exists");
                    return;
                }

                var user = new User(account);
                user.SetPassword(command);
                await userRepository.PasswordChange(user);
                response.SetSuccess();
            });
        }


        public async Task Process(UserAddEvent @event)
        {
            await ProcessEvent(async () => { await UserSyncToES(@event); });
        }

        public async Task Process(UserChangeEvent @event)
        {
            await ProcessEvent(async () => { await UserSyncToES(@event); });
        }

        private async Task UserSyncToES(UserAddEvent @event)
        {
            if (@event.ObjectId is not { Length: > 0 })
            {
                return;
            }

            var userRepository = userRepositoryResolver(@event.AccountTypeEnum);
            RUser? user = await userRepository.Get(@event.ObjectId);
            if (user == null)
            {
                LogError($"user not found: {@event.ObjectId}");
                return;
            }

            string response = await esRepository.Add(Constant.ESUserIndexName, user.Id, user);
            var result = Serialize.JsonDeserializeObject<EsAddResult>(response);
            if (result?.errors == true)
            {
                LogError($"user SyncToEs Error: {response}");
            }
        }
    }
}
