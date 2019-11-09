drop view v_udsintervals
go
create view v_udsintervals
as 
select object_id ,DAYINTERVAL,DIRECTION,DAYTYPE,

case 
when dayinterval!=4 then min(hour)
else max(hour)
end StartHour
,
case 
when dayinterval!=4 then max(hour)
else min(hour)
end EndHour
 from UDSInterval 
where dayinterval<4 and  object_id in (
select distinct object_id  from udsinterval 
where (dayinterval=1 and hour=11 )or
(dayinterval=2 and hour=15 ) or
( dayinterval=3 and hour=20 ) 
-- or( dayinterval=4 and hour=1)
) 
group by object_id ,DAYINTERVAL,DIRECTION,DAYTYPE
 
union
 select object_id ,4,DIRECTION,DAYTYPE,

23 StartHour
,
1 EndHour
 from UDSInterval 
where  dayinterval=4 and  object_id in (
select distinct object_id  from udsinterval 
where (
 dayinterval=4 and hour=1)
) 


go


select  FID_GRAPH, uds.object_id, name,utoch,DAYINTERVAL,case DAYTYPE
when 'H' then 'ВЫХ.'
when 'W' then 'РАБ.'
else ''
end DAYTYPE,DIRECTION,StartHour,EndHour from v_udsintervals
join uds on v_udsintervals.object_id=uds.object_id
where FID_GRAPH !=0
order by name,utoch,uds.object_id ,DAYTYPE,DAYINTERVAL,DIRECTION
