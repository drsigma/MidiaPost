using MidiaPost.CQRS.Core.Lib.Commands;

namespace MidiaPost.Cmd.Api.Commands
{
    public class NewPostCommand : BaseCommand
    {
        public string Author { get; set; }
        public string Message { get; set; }
    }
}
