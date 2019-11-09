

SELECT DATA.STLength(),OBJECT_ID,DATA.ToString() FROM UDSG where object_id in (select object_id from UDS where name like '%Долгоозёрная%')

select object_id,geography::STGeomFromText(DATA.ToString(),4326).STLength(),UDS.DATA.ToString() from UDS where name like '%Долгоозёрная%'


select  geography::STGeomFromText('LINESTRING( 30.2342898 60.0122561, 30.2745698 60.0243014)',4326).STLength()


update UDS set MLENGTH=geography::STGeomFromText(DATA.ToString(),4326).STLength();
from UDS where name like '%Долгоозёрная%'

drop view v_avg
go

create view v_avg as 
select avg(V) V,   MLENGTH/1000/avg(V) T,  MLENGTH, count(*) CNT ,UDS.object_ID, datepart(hour,GPSTIME) H from track2obj join uds on track2obj.object_id=uds.object_id
group by UDS.object_id,MLENGTH, datepart(hour,GPSTIME)
go


select FID_GRAPH, sum(v_avg.MLENGTH) L,  sum(T)*60 T, sum(v_avg.MLENGTH)/1000/ sum(T) V ,sum(CNT) CNT ,H,name from v_avg join uds on v_avg.object_id=uds.object_id
group by name,H,FID_GRAPH


select avg(V) V,  MLENGTH/1000/avg(V) T ,MLENGTH ,UDS.object_ID from track2obj join uds on track2obj.object_id=uds.object_id
group by UDS.object_id,MLENGTH


select cast( 2616.0/1000.0/42.0*60.0*60 as time),  42 * 17.22222 /60