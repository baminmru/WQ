drop view v_drpt_wi
go
create view v_drpt_wi
as
select
	  uds.FID_GRAPH,
	  track2obj.object_id,
	  i.DIRECTION,
	  i.DAYTYPE,
	  i.DAYINTERVAL,
		uds.name,uds.utoch

		,avg(V) V, MLENGTH/1000/avg(V) T,count(*) CNT
		from track2obj join uds on  track2obj.object_id=uds.object_id 
		JOIN UDSSTDINTERVAL I 
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

		group by track2obj.object_id,uds.FID_GRAPH,
	  uds.name,uds.utoch, i.DIRECTION
	  ,uds.MLENGTH,i.DAYINTERVAL,i.DAYTYPE
		

		
go



drop view v_drpt_wi_min 
go
create view v_drpt_wi_min 
 as
--select min(T) minT,max(V) maxV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi group by OBJECT_ID,DAYTYPE,DIRECTION
select  avg(T) minT,avg(V) maxV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi where  DAYINTERVAL=4 group by OBJECT_ID,DAYTYPE,DIRECTION
go

drop view v_drpt_wi_2xls
go

create view v_drpt_wi_2xls
as
select v_drpt_wi.FID_GRAPH ,v_drpt_wi.object_id,T,V,CNT,
case v_drpt_wi.DAYTYPE
when 'H' then 'ВЫХ.'
when 'W' then 'РАБ.'
else ''
end DAYTYPE,
DAYINTERVAL,
v_drpt_wi.DIRECTION,
name,
utoch
,
 minT/T 
 It, 

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

from v_drpt_wi join  v_drpt_wi_min on v_drpt_wi.object_ID=v_drpt_wi_min.object_id and v_drpt_wi.DAYTYPE=v_drpt_wi_min.DAYTYPE and v_drpt_wi.DIRECTION=v_drpt_wi_min.DIRECTION
where cnt >=8 and DAYINTERVAL >0 and FID_GRAPH !=0
go 


--select * from  v_drpt_wi_2xls order by name,utoch,DAYTYPE,DAYINTERVAL,object_id,DIRECTION





--select top(100) avg(T) baseT,avg(V) baseV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi where  DAYINTERVAL=4 group by OBJECT_ID,DAYTYPE,DIRECTION



--select distinct object_id  into kritical_objects from  v_drpt_wi_2xls where ODM in ('F','E') or PPM in('F','E')