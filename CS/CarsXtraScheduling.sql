SET NOCOUNT ON
GO

USE master
GO
if exists (select * from sysdatabases where name='CarsXtraScheduling')
	drop database CarsXtraScheduling
GO

DECLARE @device_directory NVARCHAR(520)
SELECT @device_directory = SUBSTRING(filename, 1, CHARINDEX(N'master.mdf', LOWER(filename)) - 1)
FROM master.dbo.sysaltfiles WHERE dbid = 1 AND fileid = 1

EXECUTE (N'CREATE DATABASE CarsXtraScheduling
  ON PRIMARY (NAME = N''CarsXtraScheduling'', FILENAME = N''' + @device_directory + N'CarsXtraScheduling.mdf'')
  LOG ON (NAME = N''CarsXtraScheduling_log'',  FILENAME = N''' + @device_directory + N'CarsXtraScheduling.ldf'')')
GO

exec sp_dboption 'CarsXtraScheduling','trunc. log on chkpt.','true'
exec sp_dboption 'CarsXtraScheduling','select into/bulkcopy','true'
GO


USE [CarsXtraScheduling]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cars](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Trademark] [nvarchar](50) NULL,
	[Model] [nvarchar](50) NULL,
	[HP] [smallint] NULL DEFAULT ((0)),
	[Liter] [float] NULL DEFAULT ((0)),
	[Cyl] [smallint] NULL DEFAULT ((0)),
	[TransmissSpeedCount] [smallint] NULL DEFAULT ((0)),
	[TransmissAutomatic] [nvarchar](3) NULL,
	[MPG_City] [smallint] NULL DEFAULT ((0)),
	[MPG_Highway] [smallint] NULL DEFAULT ((0)),
	[Category] [nvarchar](7) NULL,
	[Description] [ntext] NULL,
	[Hyperlink] [nvarchar](50) NULL, NULL
	[Picture] [image] NULL,
	[Price] [money] NULL DEFAULT ((0)),
	[RtfContent] [ntext] NULL,
 CONSTRAINT [PK_Cars] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CarScheduling](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CarId] [nvarchar](max) NULL,
	[UserId] [int] NULL,
	[Status] [int] NULL,
	[Subject] [nvarchar](50) NULL,
	[Description] [ntext] NULL,
	[Label] [int] NULL,
	[StartTime] [datetime] NULL,
	[EndTime] [datetime] NULL,
	[Location] [nvarchar](50) NULL,
	[AllDay] [bit] NOT NULL,
	[EventType] [int] NULL,
	[RecurrenceInfo] [ntext] NULL,
	[ReminderInfo] [ntext] NULL,
	[Price] [money] NULL,
	[ContactInfo] [ntext] NULL,
 CONSTRAINT [PK_CarScheduling] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/***********************************************************************************************************
Cars table data
***********************************************************************************************************/
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('Mercedes-Benz','SL500 Roadster',302,4.966000000000000e+000,8,5,'Yes',16,23,'SPORTS','http://www.mercedes.com',83800.0000)
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('Mercedes-Benz','CLK55 AMG Cabriolet',342,5.439000000000000e+000,8,5,'Yes',17,24,'SPORTS','http://www.mercedes.com',79645.0000)
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('Mercedes-Benz','C230 Kompressor Sport Coupe',189,1.796000000000000e+000,4,5,'Yes',21,28,'SPORTS','http://www.mercedes.com',25600.0000)
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('BMW','530i',225,3.000000000000000e+000,6,5,'No',21,30,'SALOON','http://www.bmw.com',39450.0000)
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('Rolls-Royce','Corniche',325,6.750000000000000e+000,8,4,'Yes',11,16,'SALOON','http://www.rollsroyce.com',370485.0000)
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('Jaguar','S-Type 3.0',235,3.000000000000000e+000,6,5,'No',18,25,'SALOON','http://www.jaguar.com',44320.0000)
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('Cadillac','Seville',275,4.600000000000000e+000,8,4,'Yes',18,27,'SALOON','http://www.cadillac.com',49600.0000)
INSERT INTO [Cars] ([Trademark],[Model],[HP],[Liter],[Cyl],[TransmissSpeedCount],[TransmissAutomatic],[MPG_City],[MPG_Highway],[Category],[Hyperlink],[Price])VALUES('Cadillac','DeVille',275,4.600000000000000e+000,8,4,'Yes',18,27,'SALOON','http://www.cadillac.com',47780.0000)

/***********************************************************************************************************
CarScheduling table data
***********************************************************************************************************/

INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES('1',NULL,3,'Mr.Brown','Rent this car',2,'Jul 12 2008 11:00:00:000AM','Jul 12 2008  2:30:00:000PM','city',0,0,NULL,' ',8.0000,'cellular: +530145961202')
INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES('1',NULL,2,'Repair','Scheduled repair of this car',4,'Jul 14 2008  8:00:00:000AM','Jul 15 2008  4:30:00:000PM','Service Center',0,0,NULL,' ',90.0000,'Contact: Paula Wilson Address: OR Elgin City Center Plaza 516 Main St.')
INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES('1',NULL,3,'Mr.White','Rent this car',3,'Jul 13 2008 10:00:00:000AM','Jul 13 2008  5:00:00:000PM','city',0,0,NULL,' ',7.5000,'phone: (401) 349-4620')
INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES('<ResourceIds>  <ResourceId Type="System.Int32" Value="2" />  </ResourceIds>',NULL,1,'Wash','Wash this car in the garage',1,'Jul  5 2008  4:30:00:000PM','Jul  5 2008  6:00:00:000PM','Garage',0,1,'<RecurrenceInfo Start="07/05/2008 16:30:00" End="08/01/2008 00:00:00" WeekDays="62" Id="975889a8-ea37-4625-a1ec-0fb2806199e2" OccurrenceCount="20" Range="2" Type="1" />',NULL,7.5000,'7466 - Gas / Car Wash')
INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES(NULL,NULL,3,'Tune up','Check up after maintenance',5,'Jul 15 2008  7:30:00:000PM','Jul 15 2008 10:30:00:000PM','Service',0,0,NULL,NULL,45.0000,'Len Radde, 10564 W Woodward Ave, Wauwatosa WI 53444  Email: s_vanish1@servicec.com')
INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES('1',NULL,3,'Mr.Green','Rent this car for the all day',3,'Jul 11 2008 12:00:00:000AM','Jul 12 2008 12:00:00:000AM','city',1,0,NULL,' ',6.0000,'Phone: (414) 964-5861 (w); (414) 647-1231 (cell); (414) 965-5950 (fax)')
INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES(NULL,NULL,-1,'Wash','Wash this car in the garage',-1,'Jul 11 2008  7:00:00:000AM','Jul 11 2008  9:00:00:000AM','Garage',0,1,'<RecurrenceInfo Start="07/11/2008 07:00:00" End="12/03/2027 07:00:00" WeekDays="32" Id="51c81018-53fa-4d10-925f-2ed7f8408c75" Month="12" OccurrenceCount="20" Range="1" Type="3" />',NULL,10.0000,'<ResourceIds>  <ResourceId Type="System.Int32" Value="1" />  </ResourceIds>')
INSERT INTO [CarScheduling] ([CarId],[UserId],[Status],[Subject],[Description],[Label],[StartTime],[EndTime],[Location],[AllDay],[EventType],[RecurrenceInfo],[ReminderInfo],[Price],[ContactInfo])VALUES('3',NULL,4,'Mrs.Black','Rent this car',3,'Jul 11 2008 10:00:00:000AM','Jul 11 2008 11:30:00:000AM','out-of-town',0,0,NULL,' ',7.0000,'Phone: (262) 946-9474; w (222) 723-2678 x22, cell (253) 713-0563, fax (361) 733-2870')