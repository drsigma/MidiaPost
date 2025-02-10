using MidiaPost.CQRS.Core.Lib.Commands;

namespace MidiaPost.Cmd.Api.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        public string Username { get; set; }
    }
}
