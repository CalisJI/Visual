USE [master]
GO
/****** Object:  Database [ComponentState]    Script Date: 1/25/2021 8:15:03 PM ******/
CREATE DATABASE [ComponentState]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ComponentState', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.WINCC\MSSQL\DATA\ComponentState.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ComponentState_log', FILENAME = N'C:\Program Files (x86)\Microsoft SQL Server\MSSQL12.WINCC\MSSQL\DATA\ComponentState_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ComponentState] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ComponentState].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ComponentState] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ComponentState] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ComponentState] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ComponentState] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ComponentState] SET ARITHABORT OFF 
GO
ALTER DATABASE [ComponentState] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ComponentState] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ComponentState] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ComponentState] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ComponentState] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ComponentState] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ComponentState] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ComponentState] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ComponentState] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ComponentState] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ComponentState] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ComponentState] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ComponentState] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ComponentState] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ComponentState] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ComponentState] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ComponentState] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ComponentState] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ComponentState] SET  MULTI_USER 
GO
ALTER DATABASE [ComponentState] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ComponentState] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ComponentState] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ComponentState] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ComponentState] SET DELAYED_DURABILITY = DISABLED 
GO
USE [ComponentState]
GO
/****** Object:  User [SIMATIC HMI VIEWER User]    Script Date: 1/25/2021 8:15:03 PM ******/
CREATE USER [SIMATIC HMI VIEWER User] FOR LOGIN [DESKTOP-E8GRUEM\SIMATIC HMI VIEWER]
GO
/****** Object:  User [SIMATIC HMI User]    Script Date: 1/25/2021 8:15:03 PM ******/
CREATE USER [SIMATIC HMI User] FOR LOGIN [DESKTOP-E8GRUEM\SIMATIC HMI]
GO
/****** Object:  DatabaseRole [SIMATIC HMI VIEWER role]    Script Date: 1/25/2021 8:15:03 PM ******/
CREATE ROLE [SIMATIC HMI VIEWER role]
GO
/****** Object:  DatabaseRole [SIMATIC HMI role]    Script Date: 1/25/2021 8:15:03 PM ******/
CREATE ROLE [SIMATIC HMI role]
GO
ALTER ROLE [SIMATIC HMI VIEWER role] ADD MEMBER [SIMATIC HMI VIEWER User]
GO
ALTER ROLE [db_datareader] ADD MEMBER [SIMATIC HMI VIEWER User]
GO
ALTER ROLE [SIMATIC HMI role] ADD MEMBER [SIMATIC HMI User]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [SIMATIC HMI User]
GO
ALTER ROLE [db_datareader] ADD MEMBER [SIMATIC HMI User]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [SIMATIC HMI User]
GO
ALTER ROLE [db_datareader] ADD MEMBER [SIMATIC HMI VIEWER role]
GO
ALTER ROLE [db_ddladmin] ADD MEMBER [SIMATIC HMI role]
GO
ALTER ROLE [db_datareader] ADD MEMBER [SIMATIC HMI role]
GO
ALTER ROLE [db_datawriter] ADD MEMBER [SIMATIC HMI role]
GO
/****** Object:  Table [dbo].[component_status]    Script Date: 1/25/2021 8:15:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[component_status](
	[Serial] [nchar](100) NULL,
	[Name] [nchar](100) NULL,
	[Date] [nchar](10) NULL,
	[Status] [nchar](10) NULL,
	[ErrorType] [nchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  DdlTrigger [OnTriggerDboSchema]    Script Date: 1/25/2021 8:15:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE trigger [OnTriggerDboSchema] ON database FOR create_table, create_view AS BEGIN   DECLARE @xmlEventData xml   SELECT    @xmlEventData = eventdata()   DECLARE @schemaName nvarchar(max)   DECLARE @objectName nvarchar(max)   DECLARE @DynSql nvarchar(max)      SET @schemaName    = convert(nvarchar(max), @xmlEventData.query('/EVENT_INSTANCE/SchemaName/text()'))   SET @objectName    = convert(nvarchar(max), @xmlEventData.query('/EVENT_INSTANCE/ObjectName/text()'))   IF(@schemaName='')   BEGIN     SET @DynSql = N'alter schema [dbo] transfer [' + @schemaName + N'].[' + @objectName + N']'     EXEC sp_executesql @statement=@DynSql   END END SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO
ENABLE TRIGGER [OnTriggerDboSchema] ON DATABASE
GO
USE [master]
GO
ALTER DATABASE [ComponentState] SET  READ_WRITE 
GO
