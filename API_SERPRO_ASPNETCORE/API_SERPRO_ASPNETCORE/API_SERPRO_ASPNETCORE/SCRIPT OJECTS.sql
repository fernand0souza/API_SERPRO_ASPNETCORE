sp_configure 'Advanced Options', 1
GO

RECONFIGURE
GO

sp_configure 'Ole Automation Procedures', 1
GO

RECONFIGURE
GO



/****** Object:  UserDefinedFunction [dbo].[udf_SerproConsultaPF]    Script Date: 13/07/2020 00:03:56 ******/
DROP FUNCTION [dbo].[udf_SerproConsultaPF]
GO

/****** Object:  UserDefinedFunction [dbo].[udf_SerproConsultaPF]    Script Date: 13/07/2020 00:03:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[udf_SerproConsultaPF] (@CPF VARCHAR(11))
RETURNS VARCHAR(MAX)
AS
BEGIN

    DECLARE @GETURL VARCHAR(200) = NULL
    
	SET @GETURL = 'http://localhost:52819'	-- CAMINHO DO PROJETO 

	SET @GETURL = CONCAT(@GETURL, '/api/Home/',@CPF)
    
    IF (LEN(RTRIM(ISNULL(@GETURL, ''))) > 0)    				
		BEGIN 
			DECLARE @win int = 0
			DECLARE @hr  int = 0
			DECLARE @response varchar(4000) = NULL

			EXEC @hr=sp_OACreate 'WinHttp.WinHttpRequest.5.1', @win OUTPUT 
			IF @hr <> 0 EXEC sp_OAGetErrorInfo @win
		
			EXEC @hr=sp_OAMethod @win, 'Open',NULL,'GET',@GETURL,'false'
			IF @hr <> 0 EXEC sp_OAGetErrorInfo @win
					
			EXEC @hr=sp_OAMethod @win, 'Send'
			IF @hr <> 0 EXEC sp_OAGetErrorInfo @win

			EXEC @hr=sp_OAGetProperty @win, 'ResponseText', @response OUTPUT
			IF @hr <> 0 EXEC sp_OAGetErrorInfo @win

			EXEC @hr=sp_OADestroy @win 
			IF @hr <> 0 EXEC sp_OAGetErrorInfo @win
			END
    RETURN @response 
END



GO




/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 12/07/2020 15:50:42 ******/
DROP FUNCTION [dbo].[Split]
GO

/****** Object:  UserDefinedFunction [dbo].[Split]    Script Date: 12/07/2020 15:50:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[Split](@String varchar(MAX), @Delimiter char(1))       
RETURNS @temptable TABLE (RowNumber int,items varchar(max))       
AS    
BEGIN       
    DECLARE @idx int = 0
    DECLARE @RowNumber [int] = 0 
    DECLARE @slice varchar(8000) = null       
        
    SET @RowNumber = 0;
    SET @idx = 1
        
    IF (LEN(ISNULL(@String,'')) > 0)
		WHILE (@idx != 0)       
		BEGIN       
			set @idx = charindex(@Delimiter, @String)       
			if (@idx != 0)       
				set @slice = left(@String,@idx - 1)       
			else       
				set @slice = @String       
	              
			if (len(@slice) > 0)  
				insert into @temptable(RowNumber,Items) values (@RowNumber, LTRIM(@slice))
	      
			set @String = right(@String,len(@String) - @idx)       
			if len(@String) = 0 BREAK     
	            
			SET @RowNumber = (@RowNumber + 1)
		END   
	--        
    RETURN
END


GO


