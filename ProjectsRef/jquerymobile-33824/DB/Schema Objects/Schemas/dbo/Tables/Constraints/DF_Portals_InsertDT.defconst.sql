ALTER TABLE [dbo].[Portals]
    ADD CONSTRAINT [DF_Portals_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

