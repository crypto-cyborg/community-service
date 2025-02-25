﻿using CommunityService.Core.Exceptions;
using CommunityService.Core.Extensions;
using CommunityService.Core.Interfaces.Services;
using CommunityService.Core.Models;
using CommunityService.Infrastructure.ServiceClients;
using CommunityService.Persistence;
using CommunityService.Persistence.Contexts;
using LanguageExt;
using Microsoft.EntityFrameworkCore;

namespace CommunityService.Application.Services;

public class PostsService(UnitOfWork unitOfWork, ITagsService tagsService, UserService userService)
    : IPostsService
{
    public async Task<Fin<IEnumerable<Post>>> GetAllPosts()
    {
        var posts = await unitOfWork.PostsRepository.GetAsync().AsNoTracking().ToListAsync();
        foreach (var post in posts)
        {
            post.Comments = await unitOfWork.CommentsRepository.GetAsync(c => c.PostId == post.Id).ToListAsync();
            post.Reactions = await unitOfWork.ReactionRepository.GetAsync(r => r.PostId == post.Id).ToListAsync();
        }

        return Fin<IEnumerable<Post>>.Succ(posts);
    }

    public async Task<Fin<Post>> GetPostById(string id)
    {
        var post = await unitOfWork.PostsRepository.GetByIdAsync(id);

        return post ?? Fin<Post>.Fail(new PostNotFoundException());
    }

    public async Task<Fin<Post>> CreatePost(PostExtensions.CreatePostDto dto)
    {
        var user = await userService.Exists(dto.UserId.ToString());

        if (!user.Found)
        {
            return Fin<Post>.Fail(new UserNotFoundException($"User {dto.UserId} does not exist"));
        }

        var newUser = new User { Id = new Guid(user.Id), Username = user.Username };
        await unitOfWork.UserRepository.InsertAsync(newUser);

        var tags = await tagsService.EnsureCreated(dto.Tags);
        var post = PostExtensions.Create(newUser.Id, dto.Topic, dto.Text, dto.Tags);

        await unitOfWork.PostsRepository.InsertAsync(post);

        await unitOfWork.SaveCommunityChangesAsync();
        await unitOfWork.SaveForumChangesAsync();

        return post;
    }
}