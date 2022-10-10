-- =============================================
-- Author:		<Ivy>
-- Create date: <2022/09/21>
-- Description:	<Daily Yield Defect number Data>
-- =============================================
Create PROCEDURE [dbo].[SP_DailyYieldDefectData]
	-- Add the parameters for the stored procedure here
	(
	@Plant varchar(5),
	@Cust2Code varchar(2),
	@Cust3Code varchar(3),
	@PKGCode varchar(4),
	@StageCode varchar(4),
	@DeviceName varchar(50),
	@StartTime datetime,
	@EndTime datetime
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;


select dy.SubLotNo,dy.LotNo,Cust2Code,dy.Device,dy.StageCode,dy.TrackInTime,dy.TrackOutTime,dy.TrackInQty,dy.TrackOutQty,dy.Yield,dy.SumDefectQty,dyd.DefectName,dyd.DefectQty
from [YieldReportSystem].[dbo].[DailyYield] as dy
left join  [YieldReportSystem].[dbo].[DailyYieldDetail] as dyd on dy.Guid=dyd.Guid


where TrackOutTime between @StartTime and DATEADD(DAY, 1, @EndTime)
	and (dy.Plant = @Plant or @Plant is null)
	and (dy.Cust2Code = @Cust2Code or @Cust2Code  is null)
	and (dy.Cust3Code = @Cust3Code or @Cust3Code is null)
	and (dy.PKGCode = @PKGCode or @PKGCode is null)
	and (dy.StageCode = @StageCode or @StageCode is null)
	and (dy.Device = @DeviceName or @DeviceName is null)


order by dy.TrackOutTime

END