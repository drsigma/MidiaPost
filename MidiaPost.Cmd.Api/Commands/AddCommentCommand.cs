using MidiaPost.CQRS.Core.Lib.Commands;

namespace MidiaPost.Cmd.Api.Commands
{
    public class AddCommentCommand : BaseCommand
    {
        public string Comment { get; set; }
        public string Username { get; set; }
    }
}

