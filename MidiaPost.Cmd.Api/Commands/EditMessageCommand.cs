using MidiaPost.CQRS.Core.Lib.Commands;

namespace MidiaPost.Cmd.Api.Commands
{
    public class EditMessageCommand : BaseCommand
    {
        public string Message { get; set; }
    }
}
