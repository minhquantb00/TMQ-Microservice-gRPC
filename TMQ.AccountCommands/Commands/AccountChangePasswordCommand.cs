using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.AccountCommands.Commands
{
    [ProtoContract]
    public record AccountChangePasswordCommand : AccountBaseCommand
    {
        [ProtoMember(1)] public required string Password { get; set; }
        [ProtoMember(2)] public required string NewPassword { get; set; }
        [ProtoMember(3)] public required string VerifyNewPassword { get; set; }
        [ProtoMember(4)] public required string OTP { get; set; }
        [ProtoMember(5)] public required string IP { get; set; }
        [ProtoMember(6)] public required string OTPKey { get; set; }
        [ProtoMember(7)] public required bool VerifyOTP { get; set; }
    }

    [ProtoContract]
    public record AccountChangePhoneNumberCommand : AccountBaseCommand
    {
        [ProtoMember(1)] public required string OTP { get; set; }
        [ProtoMember(2)] public required string PhoneNumber { get; set; }
        [ProtoMember(3)] public required string IP { get; set; }
        [ProtoMember(4)] public required string OTPKey { get; set; }
    }
}
