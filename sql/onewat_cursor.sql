--select object_id from uds join osm on osm.oneway=1 where buffer.STIntersection(osm.DATA).STLength() > SEGLENGTH 

--select DATA from osm where oneway=1
--update uds set oneway=0;
go
declare @buf nvarchar(max),@cnt int;
declare c cursor for
select data.ToString() from osm where oneway=1;
open c;
set @cnt=1;
FETCH NEXT FROM c  
INTO  @buf  ;
 while @@FETCH_STATUS =0
 begin 

 if @cnt % 50 =0 begin
select @cnt;
end;
set @cnt=@cnt+1;
update uds set oneway=1 where buffer.STIntersection(@buf).STLength() > seglength;

FETCH NEXT FROM c  
INTO @buf ;

 end
 close c
 go