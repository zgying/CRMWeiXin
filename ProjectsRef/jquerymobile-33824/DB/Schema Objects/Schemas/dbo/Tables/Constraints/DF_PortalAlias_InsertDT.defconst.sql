ALTER TABLE [dbo].[PortalAlias]
    ADD CONSTRAINT [DF_PortalAlias_InsertDT] DEFAULT (getdate()) FOR [CreateDate];

