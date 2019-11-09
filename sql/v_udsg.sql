
 drop view v_USDG
 go
 create view v_USDG
 as 
 select object_id,Q,BUFFER DATA from UDSG
 go
 drop view v_USDG2
 go
  create view v_USDG2
 as 
 select object_id,Q,DATA from UDSG
 go

