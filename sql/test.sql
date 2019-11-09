select object_id,daytype,dayinterval,V,Drives from v_quality 
 where  drives >=300 
order by object_id,daytype,dayinterval