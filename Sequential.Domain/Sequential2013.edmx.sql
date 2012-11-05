
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 11/04/2012 17:05:15
-- Generated from EDMX file: C:\Users\Carleton\Documents\GitHub\Sequential\Sequential.Domain\Sequential2013.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [Seq2013];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_SeqBookSeqChapter]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SeqChapters] DROP CONSTRAINT [FK_SeqBookSeqChapter];
GO
IF OBJECT_ID(N'[dbo].[FK_SeqChapterSeqPage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SeqPages] DROP CONSTRAINT [FK_SeqChapterSeqPage];
GO
IF OBJECT_ID(N'[dbo].[FK_SeqTagSeqPost_SeqTag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SeqTagSeqPost] DROP CONSTRAINT [FK_SeqTagSeqPost_SeqTag];
GO
IF OBJECT_ID(N'[dbo].[FK_SeqTagSeqPost_SeqPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SeqTagSeqPost] DROP CONSTRAINT [FK_SeqTagSeqPost_SeqPost];
GO
IF OBJECT_ID(N'[dbo].[FK_SeqCategorySeqPost]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SeqPosts] DROP CONSTRAINT [FK_SeqCategorySeqPost];
GO
IF OBJECT_ID(N'[dbo].[FK_SeqPageSeqTag_SeqPage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SeqPageSeqTag] DROP CONSTRAINT [FK_SeqPageSeqTag_SeqPage];
GO
IF OBJECT_ID(N'[dbo].[FK_SeqPageSeqTag_SeqTag]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SeqPageSeqTag] DROP CONSTRAINT [FK_SeqPageSeqTag_SeqTag];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[SeqPages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqPages];
GO
IF OBJECT_ID(N'[dbo].[SeqChapters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqChapters];
GO
IF OBJECT_ID(N'[dbo].[SeqBooks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqBooks];
GO
IF OBJECT_ID(N'[dbo].[SeqPosts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqPosts];
GO
IF OBJECT_ID(N'[dbo].[SeqTags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqTags];
GO
IF OBJECT_ID(N'[dbo].[SeqCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqCategories];
GO
IF OBJECT_ID(N'[dbo].[SeqTagSeqPost]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqTagSeqPost];
GO
IF OBJECT_ID(N'[dbo].[SeqPageSeqTag]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SeqPageSeqTag];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'SeqPages'
CREATE TABLE [dbo].[SeqPages] (
    [PageId] int IDENTITY(1,1) NOT NULL,
    [PageNum] smallint  NULL,
    [PubDate] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [SeqChapterChapterId] int  NOT NULL
);
GO

-- Creating table 'SeqChapters'
CREATE TABLE [dbo].[SeqChapters] (
    [ChapterId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(max)  NOT NULL,
    [ChapterNum] int  NULL,
    [PageCount] int  NOT NULL,
    [SeqBookBookId] int  NOT NULL
);
GO

-- Creating table 'SeqBooks'
CREATE TABLE [dbo].[SeqBooks] (
    [BookId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(256)  NOT NULL,
    [PageCount] int  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [UriContext] nvarchar(256)  NOT NULL,
    [CoverImage] nvarchar(max)  NULL,
    [Description] nvarchar(1024)  NULL
);
GO

-- Creating table 'SeqPosts'
CREATE TABLE [dbo].[SeqPosts] (
    [PostId] int IDENTITY(1,1) NOT NULL,
    [Title] nvarchar(256)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CreateDate] datetime  NOT NULL,
    [ModifiedDate] datetime  NULL,
    [Published] bit  NOT NULL,
    [Excerpt] nvarchar(max)  NULL,
    [ExtendedText] nvarchar(max)  NULL,
    [Permalink] nvarchar(256)  NULL,
    [BlogId] nvarchar(64)  NULL,
    [SeqCategory_CategoryId] int  NOT NULL
);
GO

-- Creating table 'SeqTags'
CREATE TABLE [dbo].[SeqTags] (
    [TagId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [Tally] int  NOT NULL,
    [BlogId] nvarchar(64)  NOT NULL
);
GO

-- Creating table 'SeqCategories'
CREATE TABLE [dbo].[SeqCategories] (
    [CategoryId] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(128)  NOT NULL,
    [Tally] int  NOT NULL,
    [LastUpdated] datetime  NOT NULL,
    [BlogId] nvarchar(64)  NOT NULL
);
GO

-- Creating table 'SeqTagSeqPost'
CREATE TABLE [dbo].[SeqTagSeqPost] (
    [SeqTags_TagId] int  NOT NULL,
    [SeqPosts_PostId] int  NOT NULL
);
GO

-- Creating table 'SeqPageSeqTag'
CREATE TABLE [dbo].[SeqPageSeqTag] (
    [SeqPage_PageId] int  NOT NULL,
    [SeqTags_TagId] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [PageId] in table 'SeqPages'
ALTER TABLE [dbo].[SeqPages]
ADD CONSTRAINT [PK_SeqPages]
    PRIMARY KEY CLUSTERED ([PageId] ASC);
GO

-- Creating primary key on [ChapterId] in table 'SeqChapters'
ALTER TABLE [dbo].[SeqChapters]
ADD CONSTRAINT [PK_SeqChapters]
    PRIMARY KEY CLUSTERED ([ChapterId] ASC);
GO

-- Creating primary key on [BookId] in table 'SeqBooks'
ALTER TABLE [dbo].[SeqBooks]
ADD CONSTRAINT [PK_SeqBooks]
    PRIMARY KEY CLUSTERED ([BookId] ASC);
GO

-- Creating primary key on [PostId] in table 'SeqPosts'
ALTER TABLE [dbo].[SeqPosts]
ADD CONSTRAINT [PK_SeqPosts]
    PRIMARY KEY CLUSTERED ([PostId] ASC);
GO

-- Creating primary key on [TagId] in table 'SeqTags'
ALTER TABLE [dbo].[SeqTags]
ADD CONSTRAINT [PK_SeqTags]
    PRIMARY KEY CLUSTERED ([TagId] ASC);
GO

-- Creating primary key on [CategoryId] in table 'SeqCategories'
ALTER TABLE [dbo].[SeqCategories]
ADD CONSTRAINT [PK_SeqCategories]
    PRIMARY KEY CLUSTERED ([CategoryId] ASC);
GO

-- Creating primary key on [SeqTags_TagId], [SeqPosts_PostId] in table 'SeqTagSeqPost'
ALTER TABLE [dbo].[SeqTagSeqPost]
ADD CONSTRAINT [PK_SeqTagSeqPost]
    PRIMARY KEY NONCLUSTERED ([SeqTags_TagId], [SeqPosts_PostId] ASC);
GO

-- Creating primary key on [SeqPage_PageId], [SeqTags_TagId] in table 'SeqPageSeqTag'
ALTER TABLE [dbo].[SeqPageSeqTag]
ADD CONSTRAINT [PK_SeqPageSeqTag]
    PRIMARY KEY NONCLUSTERED ([SeqPage_PageId], [SeqTags_TagId] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [SeqBookBookId] in table 'SeqChapters'
ALTER TABLE [dbo].[SeqChapters]
ADD CONSTRAINT [FK_SeqBookSeqChapter]
    FOREIGN KEY ([SeqBookBookId])
    REFERENCES [dbo].[SeqBooks]
        ([BookId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SeqBookSeqChapter'
CREATE INDEX [IX_FK_SeqBookSeqChapter]
ON [dbo].[SeqChapters]
    ([SeqBookBookId]);
GO

-- Creating foreign key on [SeqChapterChapterId] in table 'SeqPages'
ALTER TABLE [dbo].[SeqPages]
ADD CONSTRAINT [FK_SeqChapterSeqPage]
    FOREIGN KEY ([SeqChapterChapterId])
    REFERENCES [dbo].[SeqChapters]
        ([ChapterId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SeqChapterSeqPage'
CREATE INDEX [IX_FK_SeqChapterSeqPage]
ON [dbo].[SeqPages]
    ([SeqChapterChapterId]);
GO

-- Creating foreign key on [SeqTags_TagId] in table 'SeqTagSeqPost'
ALTER TABLE [dbo].[SeqTagSeqPost]
ADD CONSTRAINT [FK_SeqTagSeqPost_SeqTag]
    FOREIGN KEY ([SeqTags_TagId])
    REFERENCES [dbo].[SeqTags]
        ([TagId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SeqPosts_PostId] in table 'SeqTagSeqPost'
ALTER TABLE [dbo].[SeqTagSeqPost]
ADD CONSTRAINT [FK_SeqTagSeqPost_SeqPost]
    FOREIGN KEY ([SeqPosts_PostId])
    REFERENCES [dbo].[SeqPosts]
        ([PostId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SeqTagSeqPost_SeqPost'
CREATE INDEX [IX_FK_SeqTagSeqPost_SeqPost]
ON [dbo].[SeqTagSeqPost]
    ([SeqPosts_PostId]);
GO

-- Creating foreign key on [SeqCategory_CategoryId] in table 'SeqPosts'
ALTER TABLE [dbo].[SeqPosts]
ADD CONSTRAINT [FK_SeqCategorySeqPost]
    FOREIGN KEY ([SeqCategory_CategoryId])
    REFERENCES [dbo].[SeqCategories]
        ([CategoryId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SeqCategorySeqPost'
CREATE INDEX [IX_FK_SeqCategorySeqPost]
ON [dbo].[SeqPosts]
    ([SeqCategory_CategoryId]);
GO

-- Creating foreign key on [SeqPage_PageId] in table 'SeqPageSeqTag'
ALTER TABLE [dbo].[SeqPageSeqTag]
ADD CONSTRAINT [FK_SeqPageSeqTag_SeqPage]
    FOREIGN KEY ([SeqPage_PageId])
    REFERENCES [dbo].[SeqPages]
        ([PageId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [SeqTags_TagId] in table 'SeqPageSeqTag'
ALTER TABLE [dbo].[SeqPageSeqTag]
ADD CONSTRAINT [FK_SeqPageSeqTag_SeqTag]
    FOREIGN KEY ([SeqTags_TagId])
    REFERENCES [dbo].[SeqTags]
        ([TagId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SeqPageSeqTag_SeqTag'
CREATE INDEX [IX_FK_SeqPageSeqTag_SeqTag]
ON [dbo].[SeqPageSeqTag]
    ([SeqTags_TagId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------