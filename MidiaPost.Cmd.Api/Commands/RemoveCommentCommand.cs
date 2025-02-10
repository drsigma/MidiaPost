﻿using MidiaPost.CQRS.Core.Lib.Commands;

namespace MidiaPost.Cmd.Api.Commands
{
    public class RemoveCommentCommand : BaseCommand
    {
        public Guid CommentId { get; set; }
        public string Username { get; set; }
    }
}
