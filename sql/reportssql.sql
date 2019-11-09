select 
avg(V) V,
count(*) Trips,
DAYTYPE,
DAYINTERVAL
,uds.FID_GRAPH,uds.name,uds.utoch 
from v_track t join uds on t.object_id=uds.object_id
where name !=''
group by DAYTYPE,
DAYINTERVAL,uds.FID_GRAPH,uds.name,uds.utoch
order by utoch,name,DAYTYPE,
DAYINTERVAL
go

select 
avg(V) V,
count(*) Trips,
DAYINTERVAL
,uds.FID_GRAPH,uds.name,uds.utoch 
from v_track t join uds on t.object_id=uds.object_id
where name !='' -- and dayinterval>0
group by 
DAYINTERVAL,uds.FID_GRAPH,uds.name,uds.utoch

order by utoch,name,
DAYINTERVAL
go

select 
avg(V) V,
count(*) Trips,
DAYTYPE,
DAYINTERVAL
,uds.FID_GRAPH,uds.name,uds.utoch 
from v_track t join uds on t.object_id=uds.object_id
where name !='' -- and dayinterval>0
group by 
DAYTYPE,DAYINTERVAL,uds.FID_GRAPH,uds.name,uds.utoch
order by utoch,name,
DAYTYPE,DAYINTERVAL
go

select 
avg(V) V,
stdev(v) stdev_V,
count(*) Trips,
t.hour,
t.qINTERVAL
,uds.FID_GRAPH,uds.name,uds.utoch 
from v_track t join uds on t.object_id=uds.object_id
where name !='' -- and dayinterval>0
group by 
t.hour,
t.qINTERVAL,uds.FID_GRAPH,uds.name,uds.utoch
having count(*) >8
order by utoch,name,
t.hour,
t.qINTERVAL
go

select count(distinct FID_GRAPH)*24*4 from uds