-- =============================================
-- Author:		<Ivy>
-- Create date: <2022/07/21>
-- Description:	<Daily Yield by Stage Data>
-- =============================================
CREATE PROCEDURE [dbo].[SP_DailyYieldByStage]
	-- Add the parameters for the stored procedure here

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT StageCode,sum(TrackInQty) as InQty, sum(TrackOutQty) as OutQty,sum(TrackInQty)-sum(TrackOutQty) as DefectQty,Convert(varchar,CONVERT(decimal(18,4),sum(TrackOutQty)*1.0/sum(TrackInQty)*100))+ '%' as Yield,CONVERT(varchar(100),TrackOutTime,111) as OutTime  
FROM DailyYield as dy
group by StageCode,CONVERT(varchar(100),TrackOutTime,111)
order by OutTime

END