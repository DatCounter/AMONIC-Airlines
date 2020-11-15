USE [master]
GO
/****** Object:  Database [AMONIC-Airlines]    Script Date: 11/15/2020 9:43:32 PM ******/
CREATE DATABASE [AMONIC-Airlines]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'AMONIC-Airlines', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AMONIC-Airlines.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'AMONIC-Airlines_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\AMONIC-Airlines_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [AMONIC-Airlines] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [AMONIC-Airlines].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [AMONIC-Airlines] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET ARITHABORT OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [AMONIC-Airlines] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [AMONIC-Airlines] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET  DISABLE_BROKER 
GO
ALTER DATABASE [AMONIC-Airlines] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [AMONIC-Airlines] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [AMONIC-Airlines] SET  MULTI_USER 
GO
ALTER DATABASE [AMONIC-Airlines] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [AMONIC-Airlines] SET DB_CHAINING OFF 
GO
ALTER DATABASE [AMONIC-Airlines] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [AMONIC-Airlines] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [AMONIC-Airlines] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [AMONIC-Airlines] SET QUERY_STORE = OFF
GO
USE [AMONIC-Airlines]
GO
/****** Object:  Table [dbo].[ActivityUser]    Script Date: 11/15/2020 9:43:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityUser](
	[Email] [varchar](100) NOT NULL,
	[LoginDate] [datetime] NOT NULL,
	[LogoutDate] [datetime] NULL,
	[Unsuccessful_logout_reason] [varchar](100) NULL,
	[Time_spent]  AS (CONVERT([time],[LogoutDate]-[LoginDate])),
 CONSTRAINT [ActivityUser_PK] PRIMARY KEY CLUSTERED 
(
	[Email] ASC,
	[LoginDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Airport]    Script Date: 11/15/2020 9:43:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Airport](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[ShortName] [varchar](10) NOT NULL,
 CONSTRAINT [PK__Airport__3213E83FE61FFCFF] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flight]    Script Date: 11/15/2020 9:43:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flight](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Flight_Schedules]    Script Date: 11/15/2020 9:43:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Flight_Schedules](
	[Flight_number] [int] NOT NULL,
	[DateTimeOfRace] [datetime] NOT NULL,
	[FromAir] [int] NOT NULL,
	[ToAir] [int] NOT NULL,
	[CodeOfFlight] [int] NOT NULL,
	[EconomyPrice] [decimal](10, 2) NOT NULL,
	[isCanceled] [bit] NOT NULL,
 CONSTRAINT [PK__Flight_S__5DD08D7872ECAEC2] PRIMARY KEY CLUSTERED 
(
	[Flight_number] ASC,
	[DateTimeOfRace] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Offices]    Script Date: 11/15/2020 9:43:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Offices](
	[Office_code] [int] NOT NULL,
	[Name] [varchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Office_code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/15/2020 9:43:33 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[isAdmin] [bit] NOT NULL,
	[First_name] [varchar](100) NOT NULL,
	[Second_name] [varchar](100) NOT NULL,
	[Office] [int] NOT NULL,
	[Birthdate] [date] NOT NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK__Users__A9D105353CDF422A] PRIMARY KEY CLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActivityUser]  WITH CHECK ADD  CONSTRAINT [FK__ActivityU__Email__3F466844] FOREIGN KEY([Email])
REFERENCES [dbo].[Users] ([Email])
GO
ALTER TABLE [dbo].[ActivityUser] CHECK CONSTRAINT [FK__ActivityU__Email__3F466844]
GO
ALTER TABLE [dbo].[Flight_Schedules]  WITH CHECK ADD  CONSTRAINT [FK__Flight_Sc__CodeO__4F7CD00D] FOREIGN KEY([CodeOfFlight])
REFERENCES [dbo].[Flight] ([id])
GO
ALTER TABLE [dbo].[Flight_Schedules] CHECK CONSTRAINT [FK__Flight_Sc__CodeO__4F7CD00D]
GO
ALTER TABLE [dbo].[Flight_Schedules]  WITH CHECK ADD  CONSTRAINT [FK__Flight_Sc__FromA__4D94879B] FOREIGN KEY([FromAir])
REFERENCES [dbo].[Airport] ([id])
GO
ALTER TABLE [dbo].[Flight_Schedules] CHECK CONSTRAINT [FK__Flight_Sc__FromA__4D94879B]
GO
ALTER TABLE [dbo].[Flight_Schedules]  WITH CHECK ADD  CONSTRAINT [FK__Flight_Sc__ToAir__4E88ABD4] FOREIGN KEY([ToAir])
REFERENCES [dbo].[Airport] ([id])
GO
ALTER TABLE [dbo].[Flight_Schedules] CHECK CONSTRAINT [FK__Flight_Sc__ToAir__4E88ABD4]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK__Users__Office__3B75D760] FOREIGN KEY([Office])
REFERENCES [dbo].[Offices] ([Office_code])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK__Users__Office__3B75D760]
GO
USE [master]
GO
ALTER DATABASE [AMONIC-Airlines] SET  READ_WRITE 
GO
