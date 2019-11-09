drop view v_drpt_whq
go
create view v_drpt_whq
as
select
	  uds.FID_GRAPH,
	  track2obj.object_id,
	  case uds.ONEWAY
	  when 0 then track2obj.DIRECTION
	  when 1 then 'G'
	  end DIRECTION,
	  
	  -- datepart(month,GPSTIME) M,
	  -- datepart(day,GPSTIME) D,
	  --datename(weekday,GPSTIME) WD,
  	
	
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

		  datepart(hour,GPSTIME) H,
		 case
		  when datepart(minute,GPSTIME) <15 then 0
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then 1
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then 2
		  when datepart(minute,GPSTIME) >= 45 then 3
		  end Q 
		  ,

		/*(case datepart(hour,GPSTIME)
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
		END ) DAYINTERVAL, */

		uds.name,uds.utoch

		,avg(V) V, MLENGTH/1000/avg(V) T,count(*) CNT
		from track2obj join uds on  track2obj.object_id=uds.object_id
		group by track2obj.object_id,uds.FID_GRAPH,
	  uds.name,uds.utoch,  case uds.ONEWAY
	  when 0 then track2obj.DIRECTION
	  when 1 then 'G'
	  end, uds.MLENGTH,
		--datepart(month,GPSTIME),
		--datepart(day,GPSTIME),
		 --datename(weekday,GPSTIME),
  		
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

		datepart(hour,GPSTIME),
		 case
		  when datepart(minute,GPSTIME) <15 then 0
		  when datepart(minute,GPSTIME) >= 15 and datepart(minute,GPSTIME) <30 then 1
		  when datepart(minute,GPSTIME) >= 30 and datepart(minute,GPSTIME) <45 then 2
		  when datepart(minute,GPSTIME) >= 45 then 3
		  end


		/*(case datepart(hour,GPSTIME)
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
		END ) */

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
go



drop view v_drpt_whq_min 
go
create view v_drpt_whq_min 
 as
select min(T) minT,max(V) maxV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi group by OBJECT_ID,DAYTYPE,DIRECTION

go



drop view v_drpt_whq_2xls
go
create view v_drpt_whq_2xls
as
select v_drpt_whq.FID_GRAPH ,v_drpt_whq.object_id,T,V,CNT,
case v_drpt_whq.DAYTYPE
when 'H' then 'Вых.'
when 'W' then 'Раб.'
else ''
end DAYTYPE,
H,
Q,
v_drpt_whq.DIRECTION,
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

from v_drpt_whq join  v_drpt_whq_min on v_drpt_whq.object_ID=v_drpt_whq_min.object_id and v_drpt_whq.DAYTYPE=v_drpt_whq_min.DAYTYPE and v_drpt_whq.DIRECTION=v_drpt_whq_min.DIRECTION
where cnt >=8  and FID_GRAPH !=0

go

 select * from v_drpt_whq_2xls order by name,utoch,DAYTYPE,H,Q,DIRECTION

 go




 