use wq3
go
truncate table t_rpt_wi
go

INSERT INTO [dbo].[t_rpt_wi]
           ([FID_GRAPH]
           ,[object_id]
           ,[DAYTYPE]
           ,[DAYINTERVAL]
           ,[YD]
           ,[name]
           ,[utoch]
           ,[V]
           ,[T]
           ,[CNT])
     select [FID_GRAPH]
           ,[object_id]
           ,[DAYTYPE]
           ,[DAYINTERVAL]
           ,[YD]
           ,[name]
           ,[utoch]
           ,[V]
           ,[T]
           ,[CNT] from v_rpt_wi
GO




truncate table trands
go

INSERT INTO [dbo].[trands]
           ([FID_GRAPH]
           ,[object_id]
           ,[T]
           ,[V]
           ,[CNT]
           ,[DAYTYPE]
           ,[DAYINTERVAL]
           ,[YD]
           ,[name]
           ,[utoch]
           ,[It]
		   ,delay
           ,[ODM]
           ,[PPM])  select  [FID_GRAPH]
           ,[object_id]
           ,[T]
           ,[V]
           ,[CNT]
           ,[DAYTYPE]
           ,[DAYINTERVAL]
           ,[YD]
           ,[name]
           ,[utoch]
           ,[It]
		   ,delay
           ,[ODM]
           ,[PPM]   from  v_rpt_wi_2xls ;
go


