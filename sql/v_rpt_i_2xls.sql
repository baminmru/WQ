drop view v_rpt_i
go
create view v_rpt_i
as
select
	  uds.FID_GRAPH,
	  uds.OBJECT_ID ,
	  i.DAYINTERVAL,
	  uds.name,uds.utoch
		,avg(V) V, MLENGTH/1000/avg(V) T,count(*) CNT
		from track2obj 
		join uds on  track2obj.object_id=uds.object_id 
		JOIN UDSSTDINTERVAL I 
		ON track2obj.OBJECT_ID=I.OBJECT_ID 
		and i.hour=datepart(hour,track2obj.GPSTIME)
		AND i.daytype='W'
		and i.DIRECTION= 'G'
	  

		group by uds.FID_GRAPH,uds.object_id,
	  uds.name,uds.utoch
	  ,uds.MLENGTH,i.DAYINTERVAL
		

		
go



drop view v_rpt_i_min 
go
create view v_rpt_i_min 
 as
--select min(T) minT,max(V) maxV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi group by OBJECT_ID,DAYTYPE,DIRECTION
select  avg(T) minT,avg(V) maxV,FID_GRAPH  from v_rpt_i where  DAYINTERVAL=4 group by FID_GRAPH,OBJECT_ID
go

drop view v_rpt_i_2xls
go

create view v_rpt_i_2xls
as
select v_drpt_wi.FID_GRAPH, v_drpt_wi.OBJECT_ID ,T,V,CNT,

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

from v_rpt_i v_drpt_wi join  v_rpt_i_min v_drpt_wi_min on v_drpt_wi.OBJECT_ID=v_drpt_wi_min.OBJECT_ID 
where DAYINTERVAL >0 and v_drpt_wi.FID_GRAPH !=0
--group by name,utoch,v_drpt_wi.DAYTYPE,DAYINTERVAL,v_drpt_wi.FID_GRAPH

go 


--select * from  v_rpt_i_2xls  where  CNT >=8 order by name,utoch,DAYINTERVAL,FID_GRAPH


select * from  v_rpt_i_2xls  where (ODM in ('E','F') or PPM in ('E','F')) and CNT >=8
order by name,utoch,DAYINTERVAL,FID_GRAPH


--select top(100) avg(T) baseT,avg(V) baseV,OBJECT_ID,DAYTYPE,DIRECTION  from v_drpt_wi where  DAYINTERVAL=4 group by OBJECT_ID,DAYTYPE,DIRECTION



