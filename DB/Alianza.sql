USE [master]
GO
/****** Object:  Database [alianza]    Script Date: 31/8/2024 14:56:24 ******/
CREATE DATABASE [alianza]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'alianza', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\alianza.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'alianza_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\alianza_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [alianza] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [alianza].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [alianza] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [alianza] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [alianza] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [alianza] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [alianza] SET ARITHABORT OFF 
GO
ALTER DATABASE [alianza] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [alianza] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [alianza] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [alianza] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [alianza] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [alianza] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [alianza] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [alianza] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [alianza] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [alianza] SET  ENABLE_BROKER 
GO
ALTER DATABASE [alianza] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [alianza] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [alianza] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [alianza] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [alianza] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [alianza] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [alianza] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [alianza] SET RECOVERY FULL 
GO
ALTER DATABASE [alianza] SET  MULTI_USER 
GO
ALTER DATABASE [alianza] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [alianza] SET DB_CHAINING OFF 
GO
ALTER DATABASE [alianza] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [alianza] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [alianza] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [alianza] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'alianza', N'ON'
GO
ALTER DATABASE [alianza] SET QUERY_STORE = ON
GO
ALTER DATABASE [alianza] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [alianza]
GO
/****** Object:  View [dbo].[vs_Reporte]    Script Date: 31/8/2024 14:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vs_Reporte]
AS
SELECT        u.id AS UserId, u.email AS UserEmail, u.name AS UserName, u.position AS UserPosition, u.profile AS UserProfile, u.branch AS UserBranch, r.id AS RequestId, r.date AS RequestDate, r.status AS RequestStatus, 
                         r.type AS RequestType, r.apply_date AS RequestApplyDate, r.end_date AS RequestEndDate, ed.id AS ExclusiveDataId, ed.description AS ExclusiveDataDescription, ac.id AS AccountingAccountId, 
                         ac.description AS AccountingAccountDescription, an.id AS AccountNumberId, an.description AS AccountNumberDescription
FROM            requests.Users AS u LEFT OUTER JOIN
                         requests.Requests AS r ON u.email = r.email LEFT OUTER JOIN
                         requests.ExclusiveData AS ed ON u.email = ed.email LEFT OUTER JOIN
                         requests.AccountingAccounts AS ac ON u.email = ac.email LEFT OUTER JOIN
                         requests.AccountNumber AS an ON u.email = an.email
GO
/****** Object:  Table [dbo].[AccountingAccounts]    Script Date: 31/8/2024 14:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountingAccounts](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](255) NULL,
	[authorize] [bit] NOT NULL,
	[email_new] [varchar](255) NOT NULL,
	[id_request] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AccountNumber]    Script Date: 31/8/2024 14:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountNumber](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](255) NULL,
	[authorize] [bit] NOT NULL,
	[email_new] [varchar](255) NOT NULL,
	[id_accounting_account] [int] NOT NULL,
	[id_request] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ExclusiveData]    Script Date: 31/8/2024 14:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ExclusiveData](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[description] [varchar](255) NULL,
	[authorize] [bit] NOT NULL,
	[email_new] [varchar](255) NOT NULL,
	[id_request] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Requests]    Script Date: 31/8/2024 14:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Requests](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[date] [date] NOT NULL,
	[status] [bit] NOT NULL,
	[type] [varchar](255) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[apply_date] [date] NULL,
	[end_date] [date] NULL,
	[undefined] [bit] NOT NULL,
	[shift] [varchar](255) NULL,
	[operational_manager] [varchar](255) NULL,
	[authorize_operational] [bit] NOT NULL,
	[hr] [varchar](255) NULL,
	[authorize_hr] [bit] NOT NULL,
	[it_manager] [varchar](255) NULL,
	[authorize_it] [bit] NOT NULL,
	[general_manager] [varchar](255) NULL,
	[authorize_general] [bit] NOT NULL,
	[email_new] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 31/8/2024 14:56:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](255) NOT NULL,
	[password] [varchar](255) NOT NULL,
	[name] [varchar](255) NOT NULL,
	[position] [varchar](255) NULL,
	[profile] [varchar](255) NULL,
	[branch] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AccountingAccounts]  WITH CHECK ADD FOREIGN KEY([email_new])
REFERENCES [dbo].[Users] ([email])
GO
ALTER TABLE [dbo].[AccountNumber]  WITH CHECK ADD FOREIGN KEY([email_new])
REFERENCES [dbo].[Users] ([email])
GO
ALTER TABLE [dbo].[AccountNumber]  WITH CHECK ADD FOREIGN KEY([id_accounting_account])
REFERENCES [dbo].[AccountingAccounts] ([id])
GO
ALTER TABLE [dbo].[ExclusiveData]  WITH CHECK ADD FOREIGN KEY([email_new])
REFERENCES [dbo].[Users] ([email])
GO
ALTER TABLE [dbo].[Requests]  WITH CHECK ADD FOREIGN KEY([email])
REFERENCES [dbo].[Users] ([email])
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "u"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 208
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "r"
            Begin Extent = 
               Top = 6
               Left = 246
               Bottom = 136
               Right = 449
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ed"
            Begin Extent = 
               Top = 6
               Left = 487
               Bottom = 136
               Right = 657
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "ac"
            Begin Extent = 
               Top = 6
               Left = 695
               Bottom = 136
               Right = 865
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "an"
            Begin Extent = 
               Top = 138
               Left = 38
               Bottom = 268
               Right = 250
            End
            DisplayFlags = 280
            TopColumn = 2
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
      Begin ColumnWidths = 9
         Width = 284
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
         Width = 1500
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vs_Reporte'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vs_Reporte'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'vs_Reporte'
GO
USE [master]
GO
ALTER DATABASE [alianza] SET  READ_WRITE 
GO
