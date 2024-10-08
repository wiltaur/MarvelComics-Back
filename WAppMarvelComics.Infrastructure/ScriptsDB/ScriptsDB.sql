/****** Object:  Database [DBMarvelComic]    Script Date: 26/08/2024 11:14:27 a. m. ******/
CREATE DATABASE [DBMarvelComic]
GO
USE [DBMarvelComic]
GO
/****** Object:  Schema [Master]    Script Date: 26/08/2024 11:07:48 a. m. ******/
CREATE SCHEMA [Master]
GO
/****** Object:  Table [Master].[IdentificationType]    Script Date: 26/08/2024 11:07:48 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Master].[IdentificationType](
	[IdType] [varchar](4) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_IdType] PRIMARY KEY CLUSTERED 
(
	[IdType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Master].[User]    Script Date: 26/08/2024 11:07:48 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Master].[User](
	[IdUser] [varchar](50) NOT NULL,
	[IdType] [varchar](4) NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[IdUser] ASC,
	[IdType] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [Master].[Comic]    Script Date: 26/08/2024 11:07:48 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Master].[Comic](
	[Id] [int] NOT NULL,
	[Title] [varchar](250) NOT NULL,
	[Description] [varchar](max) NULL,
	[Image] [varchar](250) NULL,
 CONSTRAINT [PK_Comic] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [Master].[ComicFavorite]    Script Date: 26/08/2024 11:07:48 a. m. ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Master].[ComicFavorite](
	[Id] [int] NOT NULL,
	[IdUser] [varchar](50) NOT NULL,
	[IdTypeUser] [varchar](4) NOT NULL,
 CONSTRAINT [PK_ComicFavorite] PRIMARY KEY CLUSTERED 
(
	[Id] ASC,
	[IdUser] ASC,
	[IdTypeUser] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
INSERT [Master].[IdentificationType] ([IdType], [Name]) VALUES (N'CC', N'Cédula')
INSERT [Master].[IdentificationType] ([IdType], [Name]) VALUES (N'CE', N'Cédula de extranjería')
INSERT [Master].[IdentificationType] ([IdType], [Name]) VALUES (N'LIBR', N'Número de libreta')
INSERT [Master].[IdentificationType] ([IdType], [Name]) VALUES (N'PAS', N'Pasaporte')
INSERT [Master].[IdentificationType] ([IdType], [Name]) VALUES (N'TI', N'Tarjeta de Identidad')
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UC_Email]    Script Date: 26/08/2024 11:07:49 a. m. ******/
ALTER TABLE [Master].[User] ADD  CONSTRAINT [UC_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [Master].[ComicFavorite]  WITH CHECK ADD  CONSTRAINT [FK_ComicFavorite_Comic] FOREIGN KEY([Id])
REFERENCES [Master].[Comic] ([Id])
GO
ALTER TABLE [Master].[ComicFavorite] CHECK CONSTRAINT [FK_ComicFavorite_Comic]
GO
ALTER TABLE [Master].[ComicFavorite]  WITH CHECK ADD  CONSTRAINT [FK_ComicFavorite_User] FOREIGN KEY([IdUser], [IdTypeUser])
REFERENCES [Master].[User] ([IdUser], [IdType])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [Master].[ComicFavorite] CHECK CONSTRAINT [FK_ComicFavorite_User]
GO
ALTER TABLE [Master].[User]  WITH CHECK ADD  CONSTRAINT [FK_UserIdType] FOREIGN KEY([IdType])
REFERENCES [Master].[IdentificationType] ([IdType])
GO
ALTER TABLE [Master].[User] CHECK CONSTRAINT [FK_UserIdType]
GO