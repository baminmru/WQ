/****** Script for SelectTopNRows command from SSMS  ******/
SELECT  [V]
      ,[Drives]
      ,[object_id]
      ,[DAYINTERVAL]
  FROM [WQ].[dbo].[v_quality5]
  
  -- where drives >25
  order by object_id,dayinterval 
  
  
  /****** Script for SelectTopNRows command from SSMS  ******/
SELECT  [V]
      ,[Drives]
      ,[object_id]
      ,[DAYTYPE]
      ,[DAYINTERVAL]
  FROM [WQ].[dbo].[v_quality]
  -- where drives >25
  order by object_id,daytype,dayinterval
  
  SELECT  [V]
      ,[Drives]
      ,[object_id]
      ,[hour]
      ,[qinterval]
      ,[DAYTYPE]
  FROM [WQ].[dbo].[v_quality15] 
 where drives >25
  order by object_id,daytype,hour,qinterval 
  
  
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
count(*) Trips,
t.hour,
t.qINTERVAL
,uds.FID_GRAPH,uds.name,uds.utoch 
from v_track t join uds on t.object_id=uds.object_id
where name !='' -- and dayinterval>0
group by 
t.hour,
t.qINTERVAL,uds.FID_GRAPH,uds.name,uds.utoch
order by utoch,name,
t.hour,
t.qINTERVAL
go



