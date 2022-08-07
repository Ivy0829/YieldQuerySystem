CREATE TABLE [dbo].[DailyYield] (
    [SerialNo]     BIGINT       IDENTITY (1, 1) NOT NULL,
    [Guid]         VARCHAR (64) NOT NULL,
    [YearCode]     VARCHAR (2)  NOT NULL,
    [Plant]        VARCHAR (5)  NOT NULL,
    [SubLotNo]     VARCHAR (10) NOT NULL,
    [LotNo]        VARCHAR (6)  NOT NULL,
    [StageCode]    VARCHAR (5)  NOT NULL,
    [Cust2Code]    VARCHAR (2)  NOT NULL,
    [Cust3Code]    VARCHAR (3)  NOT NULL,
    [PkgCode]      VARCHAR (4)  NOT NULL,
    [Device]       VARCHAR (50) NOT NULL,
    [TrackInTime]  DATETIME     NOT NULL,
    [TrackInQty]   INT          NOT NULL,
    [TrackOutTime] DATETIME     NOT NULL,
    [TrackOutQty]  INT          NOT NULL,
    [SumDefectQty] INT          NOT NULL,
    [RunType]      CHAR (1)     NOT NULL,
    [Yield]        VARCHAR (8)  NOT NULL
);

