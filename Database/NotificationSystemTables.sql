USE [NotificationSystem]
GO
/****** Object:  Table [dbo].[AttachementArchive]    Script Date: 8/18/2023 4:59:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AttachementArchive](
	[EmailId] [bigint] NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
	[ArchiveDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Attachment]    Script Date: 8/18/2023 4:59:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Attachment](
	[EmailId] [bigint] IDENTITY(1,1) NOT NULL,
	[Path] [nvarchar](max) NOT NULL,
	[FileName] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_Attachment] PRIMARY KEY CLUSTERED 
(
	[EmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailQueue]    Script Date: 8/18/2023 4:59:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailQueue](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Subject] [nvarchar](500) NULL,
	[Body] [nvarchar](max) NOT NULL,
	[Recipients] [nvarchar](500) NOT NULL,
	[CCRecipients] [nvarchar](500) NULL,
	[BCCRecipients] [nvarchar](500) NULL,
	[CreationDate] [datetime] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[RetryCount] [smallint] NULL,
	[LastRetryDate] [datetime] NULL,
	[SendingDate] [datetime] NOT NULL,
	[SourceId] [smallint] NOT NULL,
	[HasAttachment] [bit] NULL,
 CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[EmailQueueArchive]    Script Date: 8/18/2023 4:59:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailQueueArchive](
	[Id] [bigint] NOT NULL,
	[Subject] [nvarchar](500) NULL,
	[Body] [nvarchar](max) NOT NULL,
	[Recipients] [nvarchar](500) NOT NULL,
	[CCRecipients] [nvarchar](500) NULL,
	[BCCRecipients] [nvarchar](500) NULL,
	[CreationDate] [datetime] NOT NULL,
	[InsertedDate] [datetime] NOT NULL,
	[RetryCount] [smallint] NULL,
	[LastRetryDate] [datetime] NULL,
	[SendingDate] [datetime] NOT NULL,
	[SourceId] [smallint] NOT NULL,
	[HasAttachment] [bit] NULL,
	[ArchiveDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NotificationStatus]    Script Date: 8/18/2023 4:59:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationStatus](
	[Id] [smallint] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_NotificationStatus] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Source]    Script Date: 8/18/2023 4:59:42 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Source](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[SenderEmail] [nvarchar](100) NOT NULL,
	[AllowAllRecipients] [bit] NOT NULL,
	[DefaultRecipients] [nvarchar](100) NOT NULL,
	[RecipientsWhiteList] [nvarchar](max) NOT NULL,
	[EmailServerUser] [nvarchar](200) NOT NULL,
	[EmailServerPassword] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_Source] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
