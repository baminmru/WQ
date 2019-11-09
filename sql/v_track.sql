USE [WQ]
GO

/****** Object:  View [dbo].[v_track]    Script Date: 23.10.2019 11:51:47 ******/
DROP VIEW [dbo].[v_track]
GO

/****** Object:  View [dbo].[v_track]    Script Date: 23.10.2019 11:51:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[v_track] as
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
		END ) DAYINTERVAL,
		object_id,V
		from track2obj 
GO


