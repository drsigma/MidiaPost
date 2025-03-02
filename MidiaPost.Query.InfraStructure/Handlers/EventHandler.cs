﻿using MidiaPost.Cmd.Domain.Entities;
using MidiaPost.Common.Events;
using MidiaPost.Query.Domain.Repositories;

namespace MidiaPost.Query.InfraStructure.Handlers
{
    public class EventHandler : IEventHandler
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public EventHandler(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepository = postRepository;
            _commentRepository = commentRepository;
        }

        public async Task On(PostCreatedEvent @event)
        {
            var post = new PostEntity
            {
                PostId = @event.Id,
                Author = @event.Author,
                DatePosted = @event.DatePosted,
                Message = @event.Message
            };

            await _postRepository.CreateAsync(post);
        }

        public async Task On(MessageUpdatedEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Message = @event.Message;

            await _postRepository.UpdateAsync(post);

        }

        public async Task On(PostLikedEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            post.Likes++;

            await _postRepository.UpdateAsync(post);
        }

        public async Task On(CommentAddedEvent @event)
        {
            var comment = new CommentEntity
            {
                PostId = @event.Id,
                Comment = @event.Comment,
                CommentDate = @event.CommentDate,
                CommentId = @event.CommentId,
                UserName = @event.Username,
                Edit = false
            };

            await _commentRepository.CreateAsync(comment);
        }

        public async Task On(CommentRemovedEvent @event)
        {
            await _commentRepository.DeleteAsync(@event.Id);
        }

        public async Task On(PostRemovedEvent @event)
        {
            var post = await _postRepository.GetByIdAsync(@event.Id);

            if (post == null) return;

            await _postRepository.DeleteAsync(@event.Id);
        }

        public async Task On(CommentUpdatedEvent @event)
        {
            var comment = await _commentRepository.GetByIdAsync(@event.Id);


            if (comment == null) return;

            comment.CommentDate = @event.EditDate;
            comment.Comment = @event.Comment;
            comment.Edit = true;

            await _commentRepository.UpdateAsync(comment);
        }
    }
}
