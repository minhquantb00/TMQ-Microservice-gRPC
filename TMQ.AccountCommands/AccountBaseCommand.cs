using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountCommands.Commands;
using TMQ.BaseCommands;

namespace TMQ.AccountCommands
{
    [ProtoContract]
    [ProtoInclude(200, typeof(AccountAddCommand))]
    [ProtoInclude(300, typeof(AccountChangeCommand))]
    [ProtoInclude(400, typeof(AccountChangePasswordCommand))]
    [ProtoInclude(500, typeof(AccountChangePhoneNumberCommand))]
    [ProtoInclude(600, typeof(SetPasswordCommand))]
    [ProtoInclude(700, typeof(AccountSyncCommand))]
    public record AccountBaseCommand : BaseCommand
    {
        [ProtoMember(101)] public override string? ObjectId { get; set; }
        [ProtoMember(102)] public override string? ProcessUid { get; set; }
        [ProtoMember(103)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(104)] public override string? LoginUid { get; set; }
    }
}
