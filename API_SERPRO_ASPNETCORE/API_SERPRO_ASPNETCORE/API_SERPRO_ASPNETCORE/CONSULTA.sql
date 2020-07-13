DECLARE @retono [varchar](MAX) = ''

SELECT @retono = [dbo].[udf_SerproConsultaPF]()

print @retono

select * FROM [dbo].[split](@retono,',')