-- =============================================
-- Author:		<Ivy>
-- Create date: <2022/09/14>
-- Description:	<Search data for dailyYield>
-- =============================================
Create PROCEDURE [dbo].[SP_SearchDataforDailyYield]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


SELECT distinct [Plant]
      ,[StageCode]
      ,[Cust2Code]
      ,[Cust3Code]
      ,[PkgCode]
FROM [YieldReportSystem].[dbo].[DailyYield]


END