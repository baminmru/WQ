﻿drop view v_krit_trand
go
create view v_krit_trand
as
select
	  uds.FID_GRAPH,
	  uds.object_id,
	  i.DAYTYPE,
	  i.DAYINTERVAL,
	    -- convert(varchar(20),GPSTIME,102) YD, -- +'.'+convert(varchar(5),datepart(hour,GPSTIME) ) YD,
		datepart(week,GPSTIME) WEEK,

		/*
	    datepart(hour,GPSTIME) H,
		 case
		   when datepart(minute,GPSTIME) <15 then '0-14'
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then '15-29'
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then '30-45'
		  when datepart(minute,GPSTIME) >= 45 then '45-59'
		  end Q ,
		  */
		uds.name,uds.utoch

		,avg(V) V, MLENGTH/1000/avg(V) T ,count(*) CNT
		from track2obj join uds on  track2obj.object_id=uds.object_id 
		JOIN UDSINTERVAL I 
		ON track2obj.OBJECT_ID=I.OBJECT_ID 
		and i.hour=datepart(hour,track2obj.GPSTIME)
		AND i.daytype=
		(case datename(dw, GPSTIME)
             when 'Saturday' then 'H'
             when 'Sunday' then 'H'
			 else
			 /* 2019  */
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
        end)
		and i.DIRECTION=(case uds.ONEWAY
	  when 0 then track2obj.DIRECTION
	  when 1 then 'G'
	  end)
	   where track2obj.object_id 	in (select object_id from kritical_objects)
		group by uds.object_id,uds.FID_GRAPH,
	  uds.name,uds.utoch, 
	  uds.MLENGTH,i.DAYINTERVAL,i.DAYTYPE,  datepart(week,GPSTIME) --convert(varchar(20),GPSTIME,102) -- +'.'+convert(varchar(5),datepart(hour,GPSTIME))
	  /*
	  ,datepart(hour,GPSTIME) , case
		  when datepart(minute,GPSTIME) <15 then '0-14'
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then '15-29'
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then '30-45'
		  when datepart(minute,GPSTIME) >= 45 then '45-59'
		  end
		*/

		
go



drop view v_krit_trand_min 
go
create view v_krit_trand_min 
 as
--select min(T) minT,max(V) maxV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi group by OBJECT_ID,DAYTYPE,DIRECTION
select  avg(T) minT,avg(V) maxV,FID_GRAPH,DAYTYPE  from v_krit_trand where  DAYINTERVAL=4 group by object_id,FID_GRAPH,DAYTYPE
go

drop view v_krit_trand_2xls
go

create view v_krit_trand_2xls
as
select v_drpt_wi.FID_GRAPH ,object_id,T,V,CNT,
case v_drpt_wi.DAYTYPE
when 'H' then 'ВЫХ.'
when 'W' then 'РАБ.'
else ''
end DAYTYPE,
DAYINTERVAL,
WEEK,
name,
utoch
,
 minT/T  It, 

 case
 when T>minT  then (T-minT) * 60 *60
 else 0
 end delay, 

case 
when V/maxV >=0.9 then 'A'
when V/maxV >=0.7 and V/maxV <0.9 then 'B'
when V/maxV >=0.55 and V/maxV <0.7 then 'C'
when V/maxV >=0.4 and V/maxV <0.55 then 'D'
when V/maxV >0.3 and V/maxV <0.4 then 'E'
else 'F'
end ODM ,

case 
when V/maxV >=0.9 then 'A'
when V/maxV >=0.7 and V/maxV <0.9 then 'B'
when V/maxV >=0.5 and V/maxV <0.7 then 'C'
when V/maxV >=0.4 and V/maxV <0.5 then 'D'
when V/maxV >0.33 and V/maxV <0.4 then 'E'
else 'F'
end PPM 

from v_krit_trand v_drpt_wi join  v_krit_trand_min v_drpt_wi_min on v_drpt_wi.FID_GRAPH=v_drpt_wi_min.FID_GRAPH and v_drpt_wi.DAYTYPE=v_drpt_wi_min.DAYTYPE 
where DAYINTERVAL >0 and v_drpt_wi.FID_GRAPH !=0
go 


-- select * from  v_krit_trand_2xls order by name,utoch,WEEK,DAYTYPE,DAYINTERVAL





--select top(100) avg(T) baseT,avg(V) baseV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi where  DAYINTERVAL=4 group by OBJECT_ID,DAYTYPE,DIRECTION



