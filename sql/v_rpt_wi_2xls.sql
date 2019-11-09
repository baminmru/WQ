drop view v_rpt_swi
go
create view v_rpt_swi
as
select
	  uds.FID_GRAPH,
	  i.DAYTYPE,
	  i.DAYINTERVAL,
		uds.name,uds.utoch

		,avg(V) V, MLENGTH/1000/avg(V) T,count(*) CNT
		from track2obj 
		join uds on  track2obj.object_id=uds.object_id 
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

		group by uds.FID_GRAPH,
	  uds.name,uds.utoch
	  ,uds.MLENGTH,i.DAYINTERVAL,i.DAYTYPE
		

		
go



drop view v_rpt_swi_min 
go
create view v_rpt_swi_min 
 as
--select min(T) minT,max(V) maxV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi group by OBJECT_ID,DAYTYPE,DIRECTION
select  avg(T) minT,avg(V) maxV,FID_GRAPH,DAYTYPE  from v_rpt_swi where  DAYINTERVAL=4 group by FID_GRAPH,DAYTYPE
go

drop view v_rpt_swi_2xls
go

create view v_rpt_swi_2xls
as
select v_drpt_wi.FID_GRAPH ,T,V,CNT,
case v_drpt_wi.DAYTYPE
when 'H' then 'ВЫХ.'
when 'W' then 'РАБ.'
else ''
end DAYTYPE,
DAYINTERVAL,
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

from v_rpt_swi v_drpt_wi join  v_rpt_swi_min v_drpt_wi_min on v_drpt_wi.FID_GRAPH=v_drpt_wi_min.FID_GRAPH and v_drpt_wi.DAYTYPE=v_drpt_wi_min.DAYTYPE 
where DAYINTERVAL >0 and v_drpt_wi.FID_GRAPH !=0
--group by name,utoch,v_drpt_wi.DAYTYPE,DAYINTERVAL,v_drpt_wi.FID_GRAPH

go 


--select * from  v_rpt_swi_2xls order by name,utoch,DAYTYPE,DAYINTERVAL,FID_GRAPH





--select top(100) avg(T) baseT,avg(V) baseV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi where  DAYINTERVAL=4 group by OBJECT_ID,DAYTYPE,DIRECTION



