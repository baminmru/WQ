USE [wq3]
GO


drop table WETHER_H
go
select distinct  convert(varchar(30),gpstime,102) thedate , datepart(hour,gpstime) hour ,isnull(wether.WTYPE,'БЕЗ ОСАДКОВ') wether into WETHER_H from track2obj
 left join wether
on convert(varchar(30),gpstime,102) = convert(varchar(30),thedate,102) 
and (datepart(hour,gpstime) =datepart(hour,thedate ) or datepart(hour,gpstime) =datepart(hour,thedate )+1 or datepart(hour,gpstime) =datepart(hour,thedate )+2)
go


USE [wq3]
GO

SET ANSI_PADDING ON
GO

/****** Object:  Index [ClusteredIndex-20191107-222819]    Script Date: 07.11.2019 22:29:23 ******/
CREATE UNIQUE CLUSTERED INDEX [ClusteredIndex-20191107-222819] ON [dbo].[WETHER_H]
(
	[thedate] ASC,
	[hour] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



/****** Object:  View [dbo].[v_track]    Script Date: 07.11.2019 21:54:39 ******/
DROP VIEW [dbo].[v_track]
GO

/****** Object:  View [dbo].[v_track]    Script Date: 07.11.2019 21:54:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[v_track] as
select (case datename(dw, GPSTIME)
             when 'Saturday' then 'H'
             when 'Sunday' then 'H'
			 else
			 /* 2019 год */
			 case substring(convert(varchar,GPSTIME,102),6,5) 
				 when '01.01' then 'H'
				 when '01.02' then 'H'
				 when '01.03' then 'H'
				 when '01.04' then 'H'
				 when '01.05' then 'H'
				 when '01.06' then 'H'
				 when '01.07' then 'H'
				 when '01.08' then 'H'
				 when '02.23' then 'H'
				 when '03.08' then 'H'
				 when '05.01' then 'H'
				 when '05.02' then 'H'
				 when '05.03' then 'H'
				 when '05.09' then 'H'
				 when '05.10' then 'H'
				 when '06.12' then 'H'
				 when '11.04' then 'H'
				 else 'W'
			end
        end) DAYTYPE,
		(case datepart(hour,GPSTIME)
		WHEN 7 THEN 1
		WHEN 8 THEN 1
		WHEN 9 THEN 1
		WHEN 10 THEN 1
		WHEN 12 THEN 2
		WHEN 13 THEN 2
		WHEN 14 THEN 2
		WHEN 17 THEN 3
		WHEN 18 THEN 3
		WHEN 19 THEN 3
		WHEN 22 THEN 4
		WHEN 23 THEN 4
		WHEN 0 THEN 4
		ELSE 0
		END ) DAYINTERVAL
		,datepart(hour,GPSTIME) hour
		  ,case
		  when datepart(minute,GPSTIME) <15 then '0-14'
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then '15-29'
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then '30-44'
		  when datepart(minute,GPSTIME) >= 45 then '45-59'
		  end qinterval,
		uds.object_id,V,
		case uds.oneway
		when 1 then 'G'
		when 0 then DIRECTION
		end DIRECTION,
		wether_H.wether,
		FID_GRAPH,uds.name,utoch,MLENGTH
		from track2obj 
		join uds on track2obj.object_id=uds.object_id
		join WETHER_H on wether_H.thedate = convert(varchar(30),track2obj.GPSTIME,102) and datepart(hour,track2obj.GPSTIME) = wether_H.Hour 
GO


