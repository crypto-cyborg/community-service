namespace CommunityService.API.Mapping;

public class Mapper
{
    public PostsMapping PostsMapper { get; private set; }
    public CommentMapping CommentsMapper { get; private set; }

    public Mapper(PostsMapping ps, CommentMapping cm)
    {
        PostsMapper = ps;
        CommentsMapper = cm;
    }
}