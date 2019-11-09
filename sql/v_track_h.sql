USE [wq3]
GO





/****** Object:  View [dbo].[v_track]    Script Date: 07.11.2019 21:54:39 ******/
DROP VIEW [dbo].[v_track_h]
GO

/****** Object:  View [dbo].[v_track]    Script Date: 07.11.2019 21:54:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE view [dbo].[v_track_h] as
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
		 convert(varchar,GPSTIME,102) YD
		,datepart(hour,GPSTIME) hour,
		  
		object_id,V
		
		from track2obj 
		


		go

		drop table t_track_h
go

select object_id,daytype,yd, hour,avg(V) V, count(*) CNT into t_track_h from v_track_h
group by object_id,daytype,yd,hour
order by object_id,daytype,yd,hour
go


/****** Object:  Index [ClusteredIndex-20191104-180236]    Script Date: 09.11.2019 10:21:10 ******/
CREATE UNIQUE CLUSTERED INDEX t_track_h_index ON [dbo].t_track_h
(
	[object_id] ASC,
	[DAYTYPE] ASC,
	[YD] ASC,
	hour ASC
	
	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO




drop table WETHER_H
go
select distinct  YD thedate ,  hour ,isnull(wether.WTYPE,'БЕЗ ОСАДКОВ') wether into WETHER_H from t_track_h
 left join wether
on YD = convert(varchar(30),thedate,102) 
and (hour =datepart(hour,thedate ) or hour =datepart(hour,thedate )+1 or hour =datepart(hour,thedate )+2)
go


drop table track_wether
go

select t_track_h.object_id, DAYTYPE,t_track_h.hour,V,CNT, name,utoch,fid_graph,wether  into track_wether from t_track_h join uds on 
t_track_h.object_id= uds.object_id
join wether_H on thedate=YD and wether_H.hour =t_track_h.hour
order by name,utoch
go


CREATE  INDEX track_wether_index ON [dbo].track_wether
(
	[object_id] ASC,
	[DAYTYPE] ASC,
	[WETHER] ASC
	
	
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO


select track_wether.object_id,case track_wether.daytype
when 'H' then 'ВЫХ.'
when 'W' then 'РАБ.'
else ''
end daytype ,wether,dayinterval,name,utoch,FID_GRAPH,
sum(V*CNT) /sum(cnt) V, sum(CNT) CNT

from track_wether
JOIN UDSSTDINTERVAL I 
		ON track_wether.OBJECT_ID=I.OBJECT_ID 
		and i.hour=track_wether.hour
		AND i.DAYTYPE
=track_wether.daytype
		and i.DIRECTION= 'G'

group by  track_wether.object_id,track_wether.daytype,wether,dayinterval,name,utoch,FID_GRAPH

order by name,utoch,track_wether.daytype,wether
