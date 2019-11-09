

USE [WQ]
GO

/****** Object:  Table [dbo].[TRACKAVG]    Script Date: 28.10.2019 9:50:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TRACKAVG](
	[OBJECT_ID] [int] NOT NULL,
	[M] [int] NOT NULL,
	[D] [int] NOT NULL,
	[H] [int] NOT NULL,
	[Q] [int] NOT NULL,
	[V] [float] NULL,
	[T] [float] NULL,
	[CNT] [int] NOT NULL,
 CONSTRAINT [PK_TRACKAVG] PRIMARY KEY CLUSTERED 
(
	[OBJECT_ID] ASC,
	[M] ASC,
	[D] ASC,
	[H] ASC,
	[Q] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


delete from track2obj where seconds is null
go

insert into TRACKAVG(OBJECT_ID,M,D,H,Q,V,T,CNT)
select 
	  object_id,
	  datepart(month,GPSTIME) M,
	  datepart(day,GPSTIME) D,
  	  datepart(hour,GPSTIME) H
		  ,case
		  when datepart(minute,GPSTIME) <15 then 0
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then 1
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then 2
		  when datepart(minute,GPSTIME) >= 45 then 3
		  end Q,
	
		avg(V) V, avg(seconds) T,count(*) CNT
		from track2obj 
		group by object_id,
		datepart(month,GPSTIME),
		datepart(day,GPSTIME),
  		datepart(hour,GPSTIME)
		  ,case when datepart(minute,GPSTIME) <15 then 0
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then 1
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then 2
		  when datepart(minute,GPSTIME) >= 45 then 3
		  end
GO

select count(*) from TRACKAVG
