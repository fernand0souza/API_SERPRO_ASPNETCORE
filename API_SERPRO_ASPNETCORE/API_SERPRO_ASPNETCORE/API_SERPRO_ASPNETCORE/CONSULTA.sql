DECLARE @retono [varchar](MAX) = ''
SELECT @retono = [dbo].[udf_SerproConsultaPF](00000000434) --CPF REGULAR/IRREGULAR
print @retono
select * FROM [dbo].[split](@retono,',')


SELECT @retono = [dbo].[udf_SerproConsultaPF](8864778911) --CPF INVALIDO!
print @retono
select * FROM [dbo].[split](@retono,',')


SELECT @retono = [dbo].[udf_SerproConsultaPF](61068664061) --CPF CPF NÃO ENCONTRADO !
print @retono
select * FROM [dbo].[split](@retono,',')
