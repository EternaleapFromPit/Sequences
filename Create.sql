USE [Sequences]
GO

/****** Object:  Table [dbo].[Sequences]    Script Date: 11/27/2022 7:22:22 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sequences](
	[Name] [nvarchar](50) NOT NULL,
	[Value] [int] NOT NULL,
	[Date] [varchar](4) NOT NULL
) ON [PRIMARY]
GO