﻿using CommunityService.Core.Models;
using CommunityService.Persistence.Contexts;

namespace CommunityService.Persistence.Repositories;

public class PostsRepository(ForumContext context) : RepositoryBase<Post, ForumContext>(context) { }