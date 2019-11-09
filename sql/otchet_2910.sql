alter view v_rpt_ih
as
select
	  track2obj.object_id,
	  -- datepart(month,GPSTIME) M,
	  -- datepart(day,GPSTIME) D,
	  --datename(weekday,GPSTIME) WD,
  	  --datepart(hour,GPSTIME) H,
		/* case
		  when datepart(minute,GPSTIME) <15 then 0
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then 1
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then 2
		  when datepart(minute,GPSTIME) >= 45 then 3
		  end Q 
		  ,
	*/
	(case datename(dw, GPSTIME)
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


		,avg(V) V, MLENGTH/1000/avg(V) TimeInHour,count(*) CNT
		from track2obj join uds on  track2obj.object_id=uds.object_id
		group by track2obj.object_id,uds.MLENGTH,
		--datepart(month,GPSTIME),
		--datepart(day,GPSTIME),
		 --datename(weekday,GPSTIME),
  		--datepart(hour,GPSTIME),


		(case datename(dw, GPSTIME)
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
        end) ,
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
		END ) 

		/* ,case when datepart(minute,GPSTIME) <15 then 0
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then 1
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then 2
		  when datepart(minute,GPSTIME) >= 45 then 3
		  end
		  */
		  



	/*	  order by track2obj.object_id,datepart(hour,GPSTIME)
		  ,case when datepart(minute,GPSTIME) <15 then 0
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then 1
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then 2
		  when datepart(minute,GPSTIME) >= 45 then 3
		  end
		  */





	
select avg(V) V,   MLENGTH/1000/avg(V) T,  MLENGTH, sum(CNT) CNT ,UDS.object_ID, H from v_rpt_h join uds on v_rpt_h.object_id=uds.object_id
group by UDS.object_id,MLENGTH, H

create view v_street_ih
as
select FID_GRAPH,   sum(TimeInHour)*60 T, sum(MLENGTH)/1000/sum(TimeInHour) V ,sum(CNT) CNT ,DAYTYPE,DAYINTERVAL,name ,utoch from v_rpt_ih join uds on v_rpt_ih.object_id=uds.object_id
where DAYINTERVAL >0 and name !=''
group by name,DAYTYPE,DAYINTERVAL,utoch,FID_GRAPH

drop view v_street_ih_min 
go
create view v_street_ih_min 
 as
select min(T) minT,max(V) maxV,FID_GRAPH,DAYTYPE  from v_street_ih group by FID_GRAPH,DAYTYPE










select v_street_ih.FID_GRAPH ,T,V,CNT,
case v_street_ih.DAYTYPE
when 'H' then 'Вых.'
when 'W' then 'Раб.'
else ''
end DAYTYPE,
DAYINTERVAL,
name,
utoch
,
case 
when minT/T>0.999999999999999  then ''
else convert(varchar(20),minT/T )
end It, 

case 
when minT/T>0.999999999999999  then ''
when V/maxV >=0.9 then 'A'
when V/maxV >=0.7 and V/maxV <0.9 then 'B'
when V/maxV >=0.55 and V/maxV <0.7 then 'C'
when V/maxV >=0.4 and V/maxV <0.55 then 'D'
when V/maxV >0.3 and V/maxV <0.4 then 'E'
else 'F'
end ODM ,

case 
when  minT/T>0.999999999999999  then ''
when V/maxV >=0.9 then 'A'
when V/maxV >=0.7 and V/maxV <0.9 then 'B'
when V/maxV >=0.5 and V/maxV <0.7 then 'C'
when V/maxV >=0.4 and V/maxV <0.5 then 'D'
when V/maxV >0.33 and V/maxV <0.4 then 'E'
else 'F'
end PPM 

from v_street_ih join  v_street_ih_min on v_street_ih.FID_GRAPH=v_street_ih_min.FID_GRAPH and v_street_ih.DAYTYPE=v_street_ih_min.DAYTYPE
where cnt >=8
order by name,utoch,DAYTYPE,DAYINTERVAL