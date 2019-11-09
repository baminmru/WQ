USE [wq3]
GO



/****** Object:  View [dbo].[v_track]    Script Date: 07.11.2019 21:54:39 ******/
DROP VIEW [dbo].[v_track_di]
GO

/****** Object:  View [dbo].[v_track]    Script Date: 07.11.2019 21:54:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[v_track_di] as
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



drop table t_track_di
go
select object_id,daytype,dayinterval,avg(V) V, count(*) CNT into t_track_di from v_track_di
group by object_id,daytype,dayinterval
order by object_id,daytype,dayinterval
go


/****** Object:  Index [ClusteredIndex-20191104-180236]    Script Date: 09.11.2019 10:21:10 ******/
CREATE UNIQUE CLUSTERED INDEX [t_track_di_index] ON [dbo].t_track_di
(
	[object_id] ASC,
	[DAYTYPE] ASC,
	[DAYINTERVAL] ASC
	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO



select t_track_di.object_id,case DAYTYPE
when 'H' then 'ВЫХ.'
when 'W' then 'РАБ.'
else ''
end DAYTYPE,DAYINTERVAL,V,CNT, name,utoch,fid_graph from t_track_di join uds on 
t_track_di.object_id= uds.object_id
order by name,utoch
go
