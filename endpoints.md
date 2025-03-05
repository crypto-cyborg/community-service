`GET /tags` - get a list of available tags

`GET /reactions/types` - get a list of available reaction types

`GET /posts` - get all posts

`GET /posts/:id` - get post by id

#

`POST /posts` - create post

`POST /posts/:postId/comments` - leave a comment on post

`POST /posts/:postId/reactions/:userId` - leave a reaction on post

`POST /posts/comments/:commentId` - reply to a comment

#

`DELETE /posts/:postId` - delete post

`DELETE /posts/:postId/reactions/:userID` - delete user reaction from post
