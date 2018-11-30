--All Release Related SQL Should go here.



---------------------ADD:Binuka Rnasinghe(26-02-2013) 
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GLRY_paObjectCategoryOrder]') AND type in (N'U'))
/****** Object:  Table [dbo].[GLRY_paObjectCategoryOrder]    Script Date: 02/26/2013 11:21:54 ******/
BEGIN
	CREATE TABLE [dbo].[GLRY_paObjectCategoryOrder](
		[Id] [int] IDENTITY(1,1) NOT NULL,
		[PaObjectCategoryId] [int] NOT NULL,
		[SortOrder] [int] NOT NULL,
		[IsVisible] [bit] NOT NULL,
	 CONSTRAINT [PK_GLRY_paObjectCategoriesOrder] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	) ,
	CONSTRAINT [FK_GLRY_paObjectCategoryOrder_paObjectCategories] FOREIGN KEY([PaObjectCategoryId])
	REFERENCES [dbo].[paObjectCategories] ([Id]) 
	)  
END
------------------------------------------------------------------------


USE [DEVGALR]
GO
/****** Object:  Table [dbo].[paObjectMockups]    Script Date: 02/26/2013 20:55:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[paObjectMockups]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[paObjectMockups](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[ObjectCategoryID] [int] NULL,
	[ObjectTypeID] [int] NULL,
	[Description] [varchar](4000) NULL,
	[ClientType] [varchar](20) NULL,
	[ObjectImage] [image] NULL,
	[ObjectThumbnail] [image] NULL,
	[PreRegisterPreView] [bit] NULL,
	[ViewCount] [int] NULL,
	[UploadedDate] [datetime] NULL,
	[UploadedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_paObjectMockups] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK_paObjectMockups_paObjectCategories]    Script Date: 02/26/2013 20:55:06 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_paObjectMockups_paObjectCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[paObjectMockups]'))
ALTER TABLE [dbo].[paObjectMockups]  WITH CHECK ADD  CONSTRAINT [FK_paObjectMockups_paObjectCategories] FOREIGN KEY([ObjectCategoryID])
REFERENCES [dbo].[paObjectCategories] ([ID])
GO
ALTER TABLE [dbo].[paObjectMockups] CHECK CONSTRAINT [FK_paObjectMockups_paObjectCategories]
GO
/****** Object:  ForeignKey [FK_paObjectMockups_paObjectTypes]    Script Date: 02/26/2013 20:55:07 ******/
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_paObjectMockups_paObjectTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[paObjectMockups]'))
ALTER TABLE [dbo].[paObjectMockups]  WITH CHECK ADD  CONSTRAINT [FK_paObjectMockups_paObjectTypes] FOREIGN KEY([ObjectTypeID])
REFERENCES [dbo].[paObjectTypes] ([ID])
GO
ALTER TABLE [dbo].[paObjectMockups] CHECK CONSTRAINT [FK_paObjectMockups_paObjectTypes]
GO

-------------------------------------------------------------

GO

IF not EXISTS( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = 'GLRY_ProspectsGalleryContents' 
           AND  COLUMN_NAME = 'TableType')
BEGIN

ALTER TABLE dbo.GLRY_ProspectsGalleryContents
ADD TableType [nchar](5)

update dbo.GLRY_ProspectsGalleryContents
set TableType = 'p'

END

-------------------------------------

GO

ALTER TABLE GLRY_ProspectsGalleryContents
DROP CONSTRAINT FK_GLRY_ProspectsGalleryContents_paObjects
