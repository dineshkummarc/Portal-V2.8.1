-- Use database
use [RiversideInternetForums]
GO

-- Make table modifications
ALTER TABLE [dbo].[WS_Users] DROP CONSTRAINT [DF_Users_UseAvatar]
GO

ALTER TABLE [dbo].[WS_Users] DROP COLUMN [UseAvatar]
GO

ALTER TABLE [dbo].[WS_Threads] DROP COLUMN [IsPinned]
GO

ALTER TABLE [dbo].[WS_Users] ADD [Roles] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO

-- Resend all stored procedures
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_AddThread]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_AddThread]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_UpdateThread]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_UpdateThread]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_IncrementUserPostCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_IncrementUserPostCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_AddPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_AddPost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetPost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetForumSearchResults]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetForumSearchResults]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetThreads]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetThreads]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetThreadCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetThreadCount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetThreadFromPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetThreadFromPost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetSortOrderFromPost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetSortOrderFromPost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetPostFromThreadAndPage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetPostFromThreadAndPage]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_IncrementThreadViews]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_IncrementThreadViews]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetThread]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetThread]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetRepliesFromThread]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetRepliesFromThread]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetUserIDFromEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetUserIDFromEmail]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_UpdateUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_UpdateUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_AddUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_AddUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_GetWebIDAndFolder]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_GetWebIDAndFolder]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_AliasExists]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_AliasExists]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_EmailExists]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_EmailExists]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_UpdatePost]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_UpdatePost]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[WS_UpdatePostPinned]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[WS_UpdatePostPinned]
GO

CREATE PROCEDURE WS_GetUser
(
	@UserID int
)
AS

SELECT
	Alias,
	Email,
	Password,
	WebID,
	Avatar,
	PostCount,
	Roles
FROM
	WS_Users
WHERE
	UserID = @UserID
GO

CREATE PROCEDURE WS_AddThread
(
	@ThreadID		int,
	@ForumID		int,
	@PinnedDate		datetime
)
AS

INSERT INTO WS_Threads
(
	ThreadID,
	Replies,
	Views,
	LastPostedPostID,
	ForumID,
	PinnedDate
)

VALUES
(
	@ThreadID,
	0,
	0,
	@ThreadID,
	@ForumID,
	@PinnedDate
)
GO

CREATE PROCEDURE WS_UpdateThread
(
	@ThreadID			int,
	@LastPostedPostID	int,
	@PinnedDate			datetime
)
AS

UPDATE
	WS_Threads
SET
	LastPostedPostID	= @LastPostedPostID,
	Replies				= ((SELECT Replies FROM WS_Threads WHERE ThreadID = @ThreadID) + 1),
	PinnedDate			= @PinnedDate
WHERE
	ThreadID = @ThreadID
GO

CREATE PROCEDURE WS_IncrementUserPostCount
(
	@UserID int
)
AS

UPDATE
	WS_Users
SET
	PostCount = ((SELECT PostCount FROM WS_Users WHERE UserID = @UserID) + 1)
WHERE
	UserID = @UserID
GO

CREATE PROCEDURE WS_AddPost
(
	@ParentPostID	int,
	@ForumID		int,
	@UserID			int,
	@RemoteAddr		nvarchar(100),
	@Notify			bit,
	@Subject		nvarchar(255),
	@Body			text,
	@PinnedDate		datetime,
	@PostDate		datetime,
	@PostID			int OUTPUT
)
AS

DECLARE @MaxTreeSortOrder int
DECLARE @MaxFlatSortOrder int
DECLARE @ParentLevel int
DECLARE @ThreadID int
DECLARE @ParentTreeSortOrder int
DECLARE @NextTreeSortOrder int
DECLARE @ThreadPinnedDate datetime

IF @ParentPostID = 0 -- New Post
	BEGIN
		-- Do INSERT into Posts table
		INSERT
			WS_Posts (ParentPostID, UserID, RemoteAddr, Notify, Subject, Body, PostDate, ThreadID, PostLevel, TreeSortOrder, FlatSortOrder)
		VALUES 
			(@ParentPostID, @UserID, @RemoteAddr, @Notify, @Subject, @Body, @PostDate, 0, 0, 0, 0)

		-- Get the new Post ID
		SELECT @PostID = @@IDENTITY

		-- Update Posts with the new post id
		UPDATE 
			WS_Posts
		SET 
			ThreadID = @PostID
		WHERE 
			PostID = @PostID
	END
ELSE -- @ParentPostID <> 0 means reply to an existing post
	BEGIN
		-- Get post information for what we are replying to
		SELECT 
			@ParentLevel = PostLevel,
			@ThreadID = ThreadID,
			@ParentTreeSortOrder = TreeSortOrder
		FROM 
			WS_Posts
		WHERE 
			PostID = @ParentPostID

		-- Calculate maximum flat sort order
		SELECT
			@MaxFlatSortOrder = Max(FlatSortOrder)
		FROM
			WS_Posts
		WHERE
			ThreadID = @ThreadID

		-- Is there another post at the same level or higher
		IF EXISTS (	SELECT * 
					FROM WS_Posts 
					WHERE PostLevel <= @ParentLevel 
					AND TreeSortOrder > @ParentTreeSortOrder
					AND ThreadID = @ThreadID )
			BEGIN
				-- Find the next post at the same level or higher
				SELECT 
					@NextTreeSortOrder = Min(TreeSortOrder)
				FROM 
					WS_Posts
				WHERE 
					PostLevel <= @ParentLevel 
					AND TreeSortOrder > @ParentTreeSortOrder
					AND ThreadID = @ThreadID

				-- Move the existing posts down
				UPDATE 
					WS_Posts
				SET 
					TreeSortOrder = TreeSortOrder + 1
				WHERE 
					ThreadID = @ThreadID
					AND TreeSortOrder >= @NextTreeSortOrder

				--  And put this one into place
				INSERT 
					WS_Posts (ParentPostID, UserID, RemoteAddr, Notify, Subject, Body, PostDate, ThreadID, PostLevel, TreeSortOrder, FlatSortOrder)
				VALUES 
					(@ParentPostID, @UserID, @RemoteAddr, @Notify, @Subject, @Body, @PostDate, @ThreadID, @ParentLevel + 1, @NextTreeSortOrder, @MaxFlatSortOrder + 1)
					
				-- Get the new post ID
				SELECT @PostID = @@IDENTITY
			END
		ELSE -- There are no posts at this level or above
			BEGIN
				-- Find the highest sort order for this parent
				SELECT 
					@MaxTreeSortOrder = MAX(TreeSortOrder)
				FROM 
					WS_Posts
				WHERE 
					ThreadID = @ThreadID

				-- Insert the new post
				INSERT 
					WS_Posts (ParentPostID, UserID, RemoteAddr, Notify, Subject, Body, PostDate, ThreadID, PostLevel, TreeSortOrder, FlatSortOrder)
				VALUES 
					(@ParentPostID, @UserID, @RemoteAddr, @Notify, @Subject, @Body, @PostDate, @ThreadID, @ParentLevel + 1, @MaxTreeSortOrder + 1, @MaxFlatSortOrder + 1)
					
				-- Get the new post ID
				SELECT @PostID = @@IDENTITY
			END
	END
	
IF (@ParentPostID = 0)
	BEGIN
		IF (@PinnedDate > @PostDate)
			EXEC WS_AddThread @PostID, @ForumID, @PinnedDate
		ELSE
			EXEC WS_AddThread @PostID, @ForumID, @PostDate
	END
ELSE
	BEGIN
		SELECT
			@ThreadPinnedDate = PinnedDate
		FROM
			WS_Threads
		WHERE
			ThreadID = @ThreadID
			
		IF (@ThreadPinnedDate > @PostDate)
			EXEC WS_UpdateThread @ThreadID, @PostID, @ThreadPinnedDate
		ELSE
			EXEC WS_UpdateThread @ThreadID, @PostID, @PostDate
	END
	
EXEC WS_IncrementUserPostCount @UserID
GO

CREATE PROCEDURE WS_GetPost
(
	@PostID int
)
AS

SELECT
	ParentPostID,	-- Post fields
	WS_Posts.UserID,
	RemoteAddr,
	Notify,
	Subject,
	Body,
	PostDate,
	ThreadID,
	PostLevel,
	TreeSortOrder,
	FlatSortOrder,
	Alias,				-- User fields
	Email,
	Password,
	WebID,
	Avatar,
	PostCount
	
FROM
	WS_Posts, WS_Users
WHERE
	PostID = @PostID AND
	WS_Posts.UserID = WS_Users.UserID
GO

CREATE PROCEDURE WS_GetForumSearchResults
(
	@WhereClause	nvarchar(500),
	@PageIndex		int,
	@PageSize		int
)
AS

-- Create a temp table to store the select results
CREATE TABLE #PageIndex 
(
	IndexID	int IDENTITY (1, 1) NOT NULL,
	PostID	int
)

-- Create dynamic SQL to populate temporary table
DECLARE @sql nvarchar(1000)
SELECT  @sql =	'INSERT INTO #PageIndex(PostID) SELECT WS_Posts.PostID ' + 
				'FROM WS_Posts (nolock), WS_Threads (nolock) ' +
				'WHERE WS_Posts.ThreadID = WS_Threads.ThreadID ' +
				@WhereClause + ' ORDER BY PostDate DESC'
EXEC(@sql)

-- All of the rows are inserted into the table - now select the correct subset
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int
DECLARE @RecordCount	int

-- Set the page bounds
SET @PageLowerBound = @PageSize * @PageIndex
SET @PageUpperBound = @PageLowerBound + @PageSize + 1
SELECT @RecordCount = COUNT(*) FROM #PageIndex

-- Select the data out of the temporary table
SELECT
	PageIndex.PostID,
	WS_Posts.Subject,
	WS_Posts.PostDate,
	WS_Users.Alias,
	RecordCount = @RecordCount
FROM
	WS_Users, WS_Threads, WS_Posts, #PageIndex PageIndex
WHERE
	WS_Users.UserID			= WS_Posts.UserID	AND
	WS_Threads.ThreadID		= WS_Posts.ThreadID	AND
	PageIndex.PostID		= WS_Posts.PostID	AND
	PageIndex.IndexID		> @PageLowerBound		AND
	PageIndex.IndexID		< @PageUpperBound
ORDER BY
	PageIndex.IndexID
GO

CREATE PROCEDURE WS_GetThreads
(
	@ForumID	int,
	@PageSize	int,
	@PageIndex	int
)
AS

DECLARE @PageLowerBound int
DECLARE @PageUpperBound int

-- Set the page bounds
SET @PageLowerBound = @PageSize * @PageIndex
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

-- Create a temp table to store the select results
CREATE TABLE #PageIndex 
(
	IndexID		int IDENTITY (1, 1) NOT NULL,
	ThreadID	int
)

-- INSERT into the temp table
INSERT INTO #PageIndex (ThreadID)

SELECT 
	ThreadID
FROM 
	WS_Threads
WHERE 
	ForumID = @ForumID
ORDER BY 
	PinnedDate DESC

SELECT
	C.Subject AS Subject,
	A.Alias AS StartedByAlias,
	Replies,
	Views,
	B.Alias AS LastPostAlias,
	D.PostDate AS DateLastPost,
	WS_Threads.ThreadID AS ThreadID,
	WS_Threads.PinnedDate AS PinnedDate,
	D.PostID As LastPostID
FROM
	WS_Threads, WS_Posts C, WS_Posts D, WS_Users A, WS_Users B, #PageIndex PageIndex
WHERE
	WS_Threads.ThreadID				= PageIndex.ThreadID	AND
	PageIndex.IndexID				> @PageLowerBound		AND
	PageIndex.IndexID				< @PageUpperBound		AND
	WS_Threads.ThreadID				= C.PostID				AND
	WS_Threads.ForumID				= @ForumID				AND
	A.UserID						= C.UserID				AND	
	WS_Threads.LastPostedPostID		= D.PostID				AND
	C.ThreadID						= D.ThreadID			AND
	B.UserID						= D.UserID
ORDER BY
	PageIndex.IndexID
GO

CREATE PROCEDURE WS_GetThreadCount
(
	@ForumID	int,
	@Count		int OUTPUT
)
AS

SELECT @Count = (
	SELECT
		COUNT(ThreadID)
	FROM
		WS_Threads
	WHERE
		ForumID = @ForumID)
GO

CREATE PROCEDURE WS_GetThreadFromPost
(
	@PostID		int,
	@ThreadID	int OUTPUT
)
AS

SELECT @ThreadID = (SELECT ThreadID FROM WS_Posts WHERE PostID=@PostID)
GO

CREATE PROCEDURE WS_GetSortOrderFromPost
(
	@PostID		int,
	@FlatView	bit,
	@SortOrder	int OUTPUT
)
AS

IF @FlatView = 1
	SELECT @SortOrder = (SELECT FlatSortOrder FROM WS_Posts WHERE PostID=@PostID)
ELSE
	SELECT @SortOrder = (SELECT TreeSortOrder FROM WS_Posts WHERE PostID=@PostID)
GO

CREATE PROCEDURE WS_GetPostFromThreadAndPage
(
	@ThreadID		int,
	@ThreadPage		int,
	@PostsPerPage	int,
	@FlatView		bit,
	@PostID			int OUTPUT
)
AS

DECLARE @SortOrder int
SELECT @SortOrder = @ThreadPage*@PostsPerPage

IF @FlatView = 1
	SELECT @PostID = (SELECT PostID FROM WS_Posts WHERE ThreadID=@ThreadID AND FlatSortOrder=@SortOrder)
ELSE
	SELECT @PostID = (SELECT PostID FROM WS_Posts WHERE ThreadID=@ThreadID AND TreeSortOrder=@SortOrder)
GO

CREATE PROCEDURE WS_IncrementThreadViews
(
	@ThreadID int
)
AS

UPDATE
	WS_Threads
SET
	Views = ((SELECT Views FROM WS_Threads WHERE ThreadID = @ThreadID) + 1)
WHERE
	ThreadID = @ThreadID
GO

CREATE PROCEDURE WS_GetThread
(
	@ThreadID		int,
	@ThreadPage		int,
	@PostsPerPage	int,
	@FlatView		bit
)
AS

DECLARE @StartSortOrder int
DECLARE @StopSortOrder int
SELECT @StartSortOrder = @ThreadPage*@PostsPerPage
SELECT @StopSortOrder = @StartSortOrder + @PostsPerPage - 1

IF (@FlatView = 1)
	BEGIN
		SELECT
			ParentPostID,	-- Post fields
			WS_Posts.UserID,
			RemoteAddr,
			Notify,
			Subject,
			Body,
			PostDate,
			ThreadID,
			PostLevel,
			TreeSortOrder,
			FlatSortOrder,
			PostID,
			Alias,				-- User fields
			Email,
			Password,
			WebID,
			Avatar,
			PostCount
		FROM
			WS_Posts, WS_Users
		WHERE
			ThreadID = @ThreadID AND
			WS_Users.UserID = WS_Posts.UserID AND
			FlatSortOrder >= @StartSortOrder AND
			FlatSortOrder <= @StopSortOrder
		ORDER BY
			FlatSortOrder
	END
ELSE
	BEGIN
		SELECT
			ParentPostID,	-- Post fields
			WS_Posts.UserID,
			RemoteAddr,
			Notify,
			Subject,
			Body,
			PostDate,
			ThreadID,
			PostLevel,
			TreeSortOrder,
			FlatSortOrder,
			PostID,
			Alias,				-- User fields
			Email,
			Password,
			WebID,
			Avatar,
			PostCount
		FROM
			WS_Posts, WS_Users
		WHERE
			ThreadID = @ThreadID AND
			WS_Users.UserID = WS_Posts.UserID AND
			TreeSortOrder >= @StartSortOrder AND
			TreeSortOrder <= @StopSortOrder
		ORDER BY
			TreeSortOrder
	END
GO

CREATE PROCEDURE WS_GetRepliesFromThread
(
	@ThreadID	int,
	@Replies	int OUTPUT
)
AS

SELECT @Replies = (SELECT Replies FROM WS_Threads WHERE ThreadID=@ThreadID)
GO

CREATE PROCEDURE WS_GetUserIDFromEmail
(
	@Email	nvarchar(100),
	@WebID	int,
	@UserID	int OUTPUT
)
AS

SELECT @UserID = (
	SELECT
		UserID
	FROM
		WS_Users
	WHERE
		Email = @Email AND
		WebID = @WebID)
GO

CREATE PROCEDURE WS_UpdateUser
(
	@UserID		int,
	@Alias		nvarchar(100),
	@Email		nvarchar(100),
	@Password	nvarchar(50),
	@Avatar		nvarchar(50)
)
AS

DECLARE @AvatarValue nvarchar(50)
SELECT @AvatarValue = ISNULL(@Avatar, (SELECT Avatar FROM WS_Users WHERE UserID = @UserID))

UPDATE
	WS_Users
SET
	Alias		= @Alias,
	Email		= @Email,
	Password	= @Password,
	Avatar		= @AvatarValue
WHERE
	UserID = @UserID
GO

CREATE PROCEDURE WS_AddUser
(
	@Alias		nvarchar(100),
	@Email		nvarchar(100),
	@Password	nvarchar(50),
	@WebID		int,
	@UserID	int OUTPUT
)
AS

INSERT INTO WS_Users
(
	Alias,
	Email,
	Password,
	WebID,
	Avatar,
	PostCount
)

VALUES
(
	@Alias,
	@Email,
	@Password,
	@WebID,
	NULL,
	0
)

SELECT
	@UserID = @@Identity
GO

CREATE PROCEDURE WS_GetWebIDAndFolder
(
	@WebDomain	nvarchar(50),
	@WebID		int OUTPUT,
	@Folder		nvarchar(50) OUTPUT
)
AS

SELECT @WebID = (SELECT WebID FROM WS_Webs WHERE WebDomain = @WebDomain)
SELECT @Folder = (SELECT Folder FROM WS_Webs WHERE WebDomain = @WebDomain)
GO

CREATE PROCEDURE WS_AliasExists
(
	@Alias		nvarchar(100),
	@WebID		int,
	@Exists		bit OUTPUT
)
AS

IF ((SELECT
		COUNT(*)
	FROM
		WS_Users
	WHERE
		Alias = @Alias AND WebID = @WebID) > 0)
	SELECT @Exists = 1
ELSE
	SELECT @Exists = 0
GO

CREATE PROCEDURE WS_EmailExists
(
	@Email		nvarchar(100),
	@WebID		int,
	@Exists		bit OUTPUT
)
AS

IF ((SELECT
		COUNT(*)
	FROM
		WS_Users
	WHERE
		Email = @Email AND WebID = @WebID) > 0)
	SELECT @Exists = 1
ELSE
	SELECT @Exists = 0
GO

CREATE PROCEDURE WS_UpdatePost
(
	@PostID		int,
	@Subject	nvarchar(255),
	@Body		text,
	@Notify		bit
)
AS

UPDATE
	WS_Posts
SET
	Subject	= @Subject,
	Body	= @Body,
	Notify	= @Notify
WHERE
	PostID = @PostID
GO

CREATE PROCEDURE WS_UpdatePostPinned
(
	@PostID		int,
	@Subject	nvarchar(255),
	@Body		text,
	@Notify		bit,
	@PinnedDate datetime
)
AS

EXEC WS_UpdatePost @PostID, @Subject, @Body, @Notify

UPDATE
	WS_Threads
SET
	PinnedDate	= @PinnedDate
WHERE
	ThreadID = @PostID
GO
